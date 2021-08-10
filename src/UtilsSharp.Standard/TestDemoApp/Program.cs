using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using DotNetCore.Autofac;
using Newtonsoft.Json;
using OptionConfig;
using TestDemoApp.ElasticSearch;
using TestDemoApp.RabbitMQ;

namespace TestDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMqInit.Init();

            RabbitMqInit.SendMessage();

            RabbitMqInit.GetMessage();

            Console.ReadKey();
        }

    }
}
