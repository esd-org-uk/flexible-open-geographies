using Esd.FlexibleOpenGeographies.Service.Functionality;

namespace Esd.FlexibleOpenGeographies.Service.Console
{
    class Program
    {
        static void Main()
        {
            var controller = new Controller();
            controller.Start();
            System.Console.ReadKey();
        }
    }
}
