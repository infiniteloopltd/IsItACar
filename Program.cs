using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;
using System.Drawing;

namespace CarClassifierCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var duckBytes = File.ReadAllBytes("DUCK.png");
            var carBytes = File.ReadAllBytes("CAR.png");

            var duck = IsItACar(duckBytes);
            var car = IsItACar(carBytes);
            
            Debug.Assert(!duck);
            Debug.Assert(car);
        }

        public static bool IsItACar(byte[] imageBytes)
        {
            var mat = new Mat();
            CvInvoke.Imdecode(imageBytes, ImreadModes.Color, mat);
            var image = mat.ToImage<Bgr, byte>();
            return IsItACar(image);
        }

        public static bool IsItACar(string imagePath)
        {
            var image = new Image<Bgr, byte>(imagePath);
            return IsItACar(image);
        }

        public static bool IsItACar(Image<Bgr, byte> image)
        {
            // source: https://raw.githubusercontent.com/andrewssobral/vehicle_detection_haarcascades/refs/heads/master/cars.xml
            const string cascadeFilePath = "cars.xml"; 
            var carClassifier = new CascadeClassifier(cascadeFilePath);
            using var grayImage = image.Convert<Gray, byte>();
            var cars = carClassifier.DetectMultiScale(
                grayImage,
                scaleFactor: 1.1,
                minNeighbors: 5,
                minSize: new Size(30, 30));
            // Only one car in the image is allowed.
            return cars.Length == 1;
        }

    }
}
