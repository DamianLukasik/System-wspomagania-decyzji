using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolvePayment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dzisiaj mamy {0:D}\n", DateTime.Now);
            ObliczWypłatę(-1.0, 20, 1);
            ObliczWypłatę(0.05, 20, 1);
            ObliczWypłatę(0.1, 20, 1);
            ObliczWypłatę(0.5, 20, 1);
            Console.ReadLine();
        }

        static void ObliczWypłatę(double procent, int dni, double początkowa)
        {
            double suma = 0.0;
            if (procent != -1.0)
            {                
                for (int i = 1; i <= dni; i++)
                {
                    Console.WriteLine("Dzień: {0}\t= {1} zł", i, początkowa);
                    suma += początkowa;
                    początkowa += (początkowa * procent);
                }
            }
            else
            {
                for (int i = 1; i <= dni; i++)
                {                    
                    Console.WriteLine("Dzień: {0}\t= {1} zł", i,początkowa);
                    suma += początkowa;
                    początkowa *= 2;
                }               
            }
            Console.WriteLine("\nWypłata:  \t: {0:0.00} zł", suma);
            Console.WriteLine("\n\n");
        }
    }
}
