using System;
using NLPService;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            NLPController.Instance.StartService();
            Console.Write("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
