using NAudio.CoreAudioApi;
using System.Diagnostics;
using System.IO.Ports;

class Program
{
    static void Main()
    {
        // Arduino'ya ait COM portunu belirtin
        using (SerialPort port = new SerialPort("COM6", 9600))
        {
            port.Open();

            while (true)
            {
                if (port.BytesToRead > 0)
                {
                    string receivedData = port.ReadExisting();
                    Console.WriteLine($"ReceivedData: {receivedData}");
                    if (receivedData == "1" || receivedData == "2")
                    {
                        AdjustVolume(receivedData);
                    }
                    else if (receivedData == "3" || receivedData == "4")
                    {
                        OpenVideo(receivedData);
                    }
                }
            }
        }
    }

    static void AdjustVolume(string type)
    {
        float step = 0.1f;

        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        switch (type)
        {
            case "1":
                device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Min(1.0f, device.AudioEndpointVolume.MasterVolumeLevelScalar + step);
                break;
            case "2":
                device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Max(0.0f, device.AudioEndpointVolume.MasterVolumeLevelScalar - step);
                break;
            default:
                break;
        }

        Console.WriteLine($"Updated Volume: {device.AudioEndpointVolume.MasterVolumeLevelScalar * 100}%");
    }

    static void OpenVideo(string type)
    {
        string videoPath = string.Empty;

        switch (type)
        {
            case "3":
                videoPath = @"C:\Users\ali_e\Downloads\video1.mp4";
                break;
            case "4":
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
