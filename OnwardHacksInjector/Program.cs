﻿using AuthGG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnwardHacksInjector
{
    class Program
    {
        [DllImport("Injector.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int Inject(string processName, string dllPath);
        static void Main()
        {

            //This connects your file to the Auth.GG API, and sends back your application settings and such
            OnProgramStart.Initialize("Onward Hacks", "319519", "OkHghfTTgqSvvzEnou0ZVqnrUnllOqIGB1K", "1.0");
            PrintLogo();
            Console.WriteLine("[1] Register");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[3] Extend Subscription");
            string option = Console.ReadLine();
            if (option == "1")
            {
                if (!ApplicationSettings.Register)
                {
                    //Register is disabled in application settings, will show a messagebox that it is not enabled
                    //MessageBox.Show("Register is not enabled, please try again later!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    Process.GetCurrentProcess().Kill(); //closes the application
                }
                else
                {
                    Console.Clear();
                    PrintLogo();
                    Console.WriteLine();
                    Console.WriteLine("Username:");
                    string username = Console.ReadLine();
                    Console.WriteLine("Password:");
                    string password = Console.ReadLine();
                    Console.WriteLine("Email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("License:");
                    string license = Console.ReadLine();
                    if (API.Register(username, password, email, license))
                    {
                        Console.WriteLine("You have successfully registered!");
                        // Do code of what you want after successful register here!
                    }
                }
            }
            else if (option == "2")
            {
                if (!ApplicationSettings.Login)
                {
                    //Register is disabled in application settings, will show a messagebox that it is not enabled
                    //MessageBox.Show("Login is not enabled, please try again later!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    Process.GetCurrentProcess().Kill(); //closes the application
                }
                else
                {
                    Console.Clear();
                    PrintLogo();
                    Console.WriteLine();
                    Console.WriteLine("Username:");
                    Console.Write(">>> ");
                    string username = Console.ReadLine();
                    Console.WriteLine("Password:");
                    Console.Write(">>> ");
                    string password = Console.ReadLine();
                    if (API.Login(username, password))
                    {
                        Console.WriteLine("You have successfully logged in!");
                        Console.Clear();
                        PrintLogo();
                        if (User.Rank == "69")
                        {
                            Console.WriteLine("Welcome Devloper, Press 1 To Inject or 2 To Build A New Encrypted DLL");
                            int opt = Int32.Parse(Console.ReadLine());
                            switch (opt)
                            {
                                case 1:
                                    InjectDll();
                                    break;
                                case 2:
                                    AesCryptographyService aesManager = new AesCryptographyService();
                                    byte[] encrypt = aesManager.EncryptDll(File.ReadAllBytes("OnwardHacks.dll"), GetKey(), GetIV());
                                    File.WriteAllBytes("OnwardHacksEncrypted.dll", encrypt);
                                    break;
                            }
                        }
#if SILENT
                        else if (User.Rank == "2")
#elif BLATANT
                        else if (User.Rank == "1")
#endif
                        {
                            Console.WriteLine("Press Enter To Inject");
                            Console.ReadLine();
                            InjectDll();
                        }
                    }
                }
            }
            else if (option == "3")
            {
                Console.Clear();
                PrintLogo();
                Console.WriteLine();
                Console.WriteLine("Username:");
                string username = Console.ReadLine();
                Console.WriteLine("Password:");
                string password = Console.ReadLine();
                Console.WriteLine("Token:");
                string token = Console.ReadLine();
                if (API.ExtendSubscription(username, password, token))
                {
                    Console.WriteLine("You have successfully extended your subscription!");
                    //MessageBox.Show("You have successfully extended your subscription!", OnProgramStart.Name, MessageBoxButton.OK, MessageBoxImage.Information);
                    // Do code of what you want after successful extend here!
                }
            }
            Console.Read();
        }
        public static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("          ██████╗ ███╗   ██╗██╗    ██╗ █████╗ ██████╗ ██████╗     ██╗  ██╗ █████╗ ██╗  ██╗");
            Console.WriteLine("         ██╔═══██╗████╗  ██║██║    ██║██╔══██╗██╔══██╗██╔══██╗    ██║  ██║██╔══██╗╚██╗██╔╝");
            Console.WriteLine("         ██║   ██║██╔██╗ ██║██║ █╗ ██║███████║██████╔╝██║  ██║    ███████║███████║ ╚███╔╝ ");
            Console.WriteLine("         ██║   ██║██║╚██╗██║██║███╗██║██╔══██║██╔══██╗██║  ██║    ██╔══██║██╔══██║ ██╔██╗ ");
            Console.WriteLine("         ╚██████╔╝██║ ╚████║╚███╔███╔╝██║  ██║██║  ██║██████╔╝    ██║  ██║██║  ██║██╔╝ ██╗");
            Console.WriteLine("          ╚═════╝ ╚═╝  ╚═══╝ ╚══╝╚══╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═════╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝");
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void InjectDll()
        {
            //Aes aes = Aes.Create();
            //Console.WriteLine(Convert.ToBase64String(aes.Key));
            //Console.WriteLine(Convert.ToBase64String(aes.IV));
            //Inject Dll
            AesCryptographyService aesManager = new AesCryptographyService();
            string dllPath = aesManager.DecryptDll(File.ReadAllBytes("OnwardHacksEncrypted.dll"), GetKey(), GetIV());
            Inject("Onward.exe", dllPath);
        }

        private static byte[] GetKey()
        {
#if SILENT
            return Convert.FromBase64String(App.GrabVariable("4CB5jhFCzs98IEmUIIQBPFXffwmR6") + App.GrabVariable("LhFC6B9YyS9Nxq3PcJvfwaJRba4z0") + App.GrabVariable("SjXfrVXazsV2n7zU5ito3QkIlZ3xF"));
#elif BLATANT
            return Convert.FromBase64String(App.GrabVariable("MsF0GctAsrJB4GiQSFoRA452NEcKU") + App.GrabVariable("CyG5tnYEeHIKVBJ6iacp8uqRdvnSU") + App.GrabVariable("oMiE9GLEcMo4cZa5XVvEncptJl0SU"));
#endif
        }

        private static byte[] GetIV()
        {
#if SILENT
            return Convert.FromBase64String(App.GrabVariable("I2IKewsKi4mPpz1CPSMrML51dmJRg") + App.GrabVariable("ijNSm6kzyrLLkgdBxD8nFhWOoUuoy"));
#elif BLATANT
            return Convert.FromBase64String(App.GrabVariable("DgK40d6YS4MM116h2kdfJoRyaQK5F") + App.GrabVariable("CSIV6mvhdTrzvcVBiEvAWl6sgOTvb"));
#endif
        }
    }
}
