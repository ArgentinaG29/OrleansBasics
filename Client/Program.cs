using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using GrainInterfaces;

namespace Client
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    while (Console.ReadKey().Key != ConsoleKey.Enter)
                    {
                        
                        await DoClientWork(client);
                    }
                    
                    
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var friend = client.GetGrain<IHello>(0);
            var response = await friend.SayHello("Good morning, HelloGrain!");
            Console.WriteLine("\n\n{0}\n\n", response);
            var response1 = await friend.SayHello("Good afternoon, HelloGrain!");
            Console.WriteLine("\n\n{0}\n\n", response1);

            //Se realiza una transacción seleccionando el número de cuenta el cual servira para elegir el grano
            Console.WriteLine("\n Ingrese el numero de cuenta origen: ");
            int no_cuentaO = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("\n Ingrese el numero de cuenta destino: ");
            int no_cuentaD = Convert.ToInt32(Console.ReadLine());

            Random random = new Random();
            var origen = client.GetGrain<IAccount>(no_cuentaO);
            var destino = client.GetGrain<IAccount>(no_cuentaD);
            int cantidad = random.Next(100);
            var response2 = await origen.Debito(cantidad);
            var response3 = await destino.Deposito(cantidad);
            Console.WriteLine("\n Cuenta origen:");
            Console.WriteLine("\n{0}\n", response2);
            Console.WriteLine("\n Cuenta destino:");
            Console.WriteLine("\n{0}\n", response3);
            Console.WriteLine("Presione Enter para salir y otra tecla para continuar");
        }
    }
}
