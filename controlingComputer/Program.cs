using System.Diagnostics;
using NAudio.CoreAudioApi;

class Program
{
    static void Main()
    {
        // Get the default audio endpoint device
        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        Console.WriteLine($"Current Volume: {device.AudioEndpointVolume.MasterVolumeLevelScalar * 100}%");

        while (true)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.M)
                    break;

                AdjustVolume(device, keyInfo.Key);
                OpenVideo(keyInfo.Key);

                Console.WriteLine($"Updated Volume: {device.AudioEndpointVolume.MasterVolumeLevelScalar * 100}%");
            }

            Thread.Sleep(100);
        }
    }

    static void AdjustVolume(MMDevice device, ConsoleKey key)
    {
        float step = 0.1f;

        switch (key)
        {
            case ConsoleKey.UpArrow:
                device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Min(1.0f, device.AudioEndpointVolume.MasterVolumeLevelScalar + step);
                break;
            case ConsoleKey.DownArrow:
                device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Max(0.0f, device.AudioEndpointVolume.MasterVolumeLevelScalar - step);
                break;
            default:
                break;
        }
    }

    static void OpenVideo(ConsoleKey key)
    {
        string videoPath = string.Empty;

        switch (key)
        {
            case ConsoleKey.A:
                videoPath = @"C:\Users\ali_e\Downloads\video1.mp4";
                break;
            case ConsoleKey.B:
                videoPath = @"C:\Users\ali_e\Downloads\video2.mp4";
                break;
            default:
                break;
        }

        if (!string.IsNullOrEmpty(videoPath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = videoPath,
                UseShellExecute = true
            });
        }
    }
}
