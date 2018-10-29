using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace ISET2018_ProcessTheards
{
    class Program
    {
        const int N = 500;
        static int commun = 0;
        static ReaderWriterLock Verrou = new ReaderWriterLock();
        static Mutex autreVerrou = new Mutex();
        static void a()
        {
            int n = commun;
            for (int i = 0; i < N; i++)
            {
                //lock (typeof(Program))
                {
                    //Verrou.AcquireWriterLock(-1);
                    autreVerrou.WaitOne();
                    n = commun;
                     n++;
                    Console.Write("A {0,-7} ", commun);
                    commun = n;
                    //Verrou.ReleaseWriterLock();
                    autreVerrou.ReleaseMutex();
                }
            }
        }
        static void b()
        {
            int n = commun;
            for (int i = 0; i < N; i++)
            {
                //lock (typeof(Program))
                {
                    //Verrou.AcquireWriterLock(-1);
                    autreVerrou.WaitOne();

                    n = commun;
                    n++;
                    Console.Write("B {0,-7} ", commun);
                    commun = n;
                    //Verrou.ReleaseWriterLock();
                    autreVerrou.ReleaseMutex();

                }
            }
        }
    
        static void Main(string[] args)
        {
            if (ExisteInstance())
            {
                Console.WriteLine("Autre instance active => Bye");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Lancement des threads");
                Thread ta = new Thread(new ThreadStart(a));
                Thread tb = new Thread(new ThreadStart(b));
                ta.Start(); tb.Start();
                ta.Join(); tb.Join(); // j'arrive ici seulement quand  ta.Start(); tb.Start(); ont fini le processus 
                Console.WriteLine("Fin des threads ");

                Console.ReadKey();
                ////Console.WriteLine("Ouverture du bloc-note");
                ////Process p = Process.Start("notepad");
                //Process p = new Process();
                ////p.StartInfo.FileName = "notepad";
                //p.StartInfo.FileName = "Iset2018_Secondaire.exe";
                //p.StartInfo.UseShellExecute = false;
                //p.StartInfo.RedirectStandardInput = true;
                //p.StartInfo.RedirectStandardOutput = true;
                //try
                //{
                //p.Start();
                //    p.StandardInput.WriteLine("Largo");
                //    p.StandardInput.WriteLine("Winch");
                //    Console.WriteLine("Resultat : {0}", p.StandardOutput.ReadToEnd());
                //DateTime deb = p.StartTime;
                //Console.ReadLine();
                //}
                //catch { Console.WriteLine("Problème avec Iset2018_secondaire.exe"); }
                //if (!p.HasExited)
                //{
                //    p.Kill();
                //    p.WaitForExit();
                //}
                //    //Console.WriteLine("Debut : {0} ", deb);
                //    //Console.WriteLine("Fin   : {0} ", p.ExitTime);
                //    //Console.WriteLine("Durée : {0} ", p.ExitTime - deb);
                //    //Console.ReadLine();
                //}
            }
        }
        static bool ExisteInstance()
        {
            Process actu = Process.GetCurrentProcess();
            Process[] acti = Process.GetProcesses();
            foreach (Process p in acti)
                if (p.Id != actu.Id)
                    if (actu.ProcessName == p.ProcessName)
                        return true;
            return false;
        }
    }
}
