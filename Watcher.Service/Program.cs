using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Watcher.Service
{
    internal class Program
    {
        static async Task Main()
        {
            // Use camera index 0 (usually the default USB webcam)
            //using var capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            //if (!capture.IsOpened())
            //{
            //    Console.WriteLine("Camera not found.");
            //    return;
            //}

            // Load Haar cascade for face detection
            var faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");

            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7169") };

            using var frame = new Mat();
            while (true)
            {
                using var capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
                if (!capture.IsOpened())
                {
                    Console.WriteLine("Camera not found.");
                    return;
                }
                //capture.Read(frame);

                capture.Retrieve(frame);
                if (frame.Empty())
                    continue;

                // Convert to grayscale for detection
                using var gray = new Mat();
                Cv2.CvtColor(frame, gray, ColorConversionCodes.BGR2GRAY);

                // Detect faces
                var faces = faceCascade.DetectMultiScale(gray, scaleFactor: 1.1, minNeighbors: 5);

                foreach (var face in faces)
                {
                    Console.WriteLine("Found");
                    // Crop face region
                    var faceMat = new Mat(frame, face);

                    // Convert to Bitmap for HTTP transfer
                    using var bitmap = BitmapConverter.ToBitmap(faceMat);
                    byte[] imageBytes;
                    using (var ms = new MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imageBytes = ms.ToArray();
                    }

                    // Create HTTP content
                    var content = new MultipartFormDataContent
                    {
                        { new ByteArrayContent(imageBytes), "image", "face.jpg" }
                    };

                    File.WriteAllBytes($"./../../image-{DateTime.Now.ToString("hh-mm-ss")}.jpg", imageBytes);

                    //  var response = await client.PostAsync("/recognize", content);
                    //   var responseText = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine(responseText);
                }
                await Task.Delay(1000);

            }
        }
    }
}
