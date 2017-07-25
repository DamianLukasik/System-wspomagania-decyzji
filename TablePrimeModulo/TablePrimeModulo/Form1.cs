using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TablePrimeModulo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            string[] primes = { "", "2", "3", "5", "7", "11", "13", "17", "19", "23",
                "29", "31", "37", "41", "43", "47", "53", "59", "61", "67", "71", "73",
                "79", "83", "89", "97", "101", "103", "107", "109", "113" };
            int i = 0;

            GridResult.ColumnCount = primes.Length;
            foreach (string prime in primes)
            {
                GridResult.Columns[i].Name = prime;
                i++;
            }

            string[] research = { "112", "113", "114" };
            foreach (string res in research)
            {
                string[] row = new string[primes.Length];
                int j = 0;
                foreach (string prime in primes)
                {
                    int a = Int32.Parse(res);
                    int b = prime.Length == 0 ? a : a % Int32.Parse(prime);
                   // b = a % b;

                    row[j] = b.ToString();
                    j++;
                }
                GridResult.Rows.Add(row);
            }
            

        }        
    }
}
