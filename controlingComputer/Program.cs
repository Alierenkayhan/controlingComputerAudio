using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using NAudio.CoreAudioApi;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Program
{
    static void Main()
    {
        var connectionHost = "";
        var username = "";
        var password = "";
        var audioQueueName = "FromArduino";

        var factory = new ConnectionFactory
        {
            HostName = connectionHost,
            UserName = username,
            Password = password,
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(audioQueueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, e) =>
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body).Trim('\"');

            if (message == "audioincrease" || message == "audiodecrease")
            {
                AdjustVolume(message);
            }
            else if (message == "video1" || message == "video2")
            {
                OpenVideo(message);
            }

            Thread.Sleep(100);
        };

        channel.BasicConsume(audioQueueName, true, consumer);

        Console.WriteLine("Listening for messages. Press Ctrl+C to exit.");

        var exitEvent = new ManualResetEvent(false);
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            exitEvent.Set();
        };

        while (true)
        {
            if (exitEvent.WaitOne(0))
                break;
            Thread.Sleep(1000);
        }

        Console.WriteLine("Exiting...");
    }

    static void AdjustVolume(string type)
    {
        float step = 0.1f;

        MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
        MMDevice device = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        switch (type)
        {
            case "audioincrease":
                device.AudioEndpointVolume.MasterVolumeLevelScalar = Math.Min(1.0f, device.AudioEndpointVolume.MasterVolumeLevelScalar + step);
                break;
            case "audiodecrease":
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
            case "video1":
                videoPath = @"C:\Users\ali_e\Downloads\video1.mp4";
                break;
            case "video2":
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
