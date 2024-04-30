using System;
using System.Text;
using TestDemoApp.ElasticSearch;
using TestDemoApp.Office;
using UtilsSharp;

namespace TestDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExcelInit.All();
            //LoggerInit.LoggerTest();
            //Console.ReadKey();

            ElasticSearchInit.Init();
            Console.ReadKey();


            //new KafkaInit();
            //Console.ReadKey();

            //RabbitMqInit.Init();

            //RabbitMqInit.SendMessage();

            //RabbitMqInit.GetMessage();

            //Console.ReadKey();
        }
    }

}
