using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Net.Sockets;

namespace Pinger
{
    internal class Program
    {
        public static void PingP(string hostname, int port, string[] args)
        {
            Console.Title = "Pinger.exe | -TOXIC-#1835 (CFT Development) | Port Ping";
            Console.WriteLine("PortPing is a testing tool!");
            using (var client = new TcpClient())
            {
                try
                {
                    client.Connect(hostname, port);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("[+] ");
                    Console.ResetColor();
                    Console.WriteLine($"{hostname}:{port} is open");
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("[-] ");
                    Console.ResetColor();
                    Console.WriteLine($"{hostname}:{port} is not open");
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Environment.NewLine + "Press any key to continue" + Environment.NewLine);
                Console.ResetColor();
                Console.ReadKey();
                Start(args);
            }
        }
        public static void Ping(string hostname, string[] args)
        {
            Console.Title = "Pinger.exe | -TOXIC-#1835 (CFT Development) | Pinging > " + hostname;
            using (var ping = new Ping())
            {
                while (true)
                {
                    try
                    {

                        var reply = ping.Send(hostname);
                        if (reply.Status == IPStatus.Success)
                        {
                            int pingMs = (int)reply.RoundtripTime;
                            Console.Write($"{hostname} > ");
                            if (pingMs < 100)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            }
                            else if (pingMs > 99 && pingMs < 181)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            }
                            Console.Write($"{pingMs}");
                            Console.ResetColor();
                            Console.WriteLine("ms");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("request not resloved | connection timed out");
                        }
                    }
                    catch (PingException) 
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("ping failed");
                    }
                    Console.ResetColor();
                    Thread.Sleep(1000);
                }
                Start(args);
            }
        }
        
        public static void Start(string[] args)
        {
            string hostname;
            int port;
            Console.Clear();
            Console.Title = "Pinger.exe | -TOXIC-#1835 (CFT Development) | Main Menu";
            if (args.Length > 0)
            {
                hostname = args[0];
                if (args.Length >= 2)
                {
                    port = int.Parse(args[1]);
                }
            }
            else
            {
                Console.Write("Enter Domain or IP> ");
                hostname = Console.ReadLine();
            }
            if (hostname.Contains(":"))
            {
                string[] parts = hostname.Split(':');
                hostname = parts[0];
                port = int.Parse(parts[1]);
                PingP(hostname, port, args);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Environment.NewLine + "Press [c] to stop the ping!" + Environment.NewLine);
                Console.ResetColor();
                Thread pingThread = new Thread(() => Ping(hostname, args));
                pingThread.Start();
                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.C)
                    {
                        pingThread.Abort();
                        break;
                    }
                }
                Start(args);
            }
        }
        static void Main(string[] args)
        {
            Start(args);
        }
    }
}
