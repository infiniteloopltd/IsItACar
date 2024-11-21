// You may need to add
using System.Diagnostics;

namespace CarClassifierCore
{


    public class Program
    {
        static void Main(string[] args)
        {

             var car = CarDetection.IsItACar("C:\\Users\\fiach\\source\\repos\\CarClassifierCore\\CAR.png");

  
            Debug.Assert(car);
        }




    }


}
