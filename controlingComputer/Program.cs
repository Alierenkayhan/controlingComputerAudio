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

                AdjustVolume(device, keyInfo.Key);

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
}
