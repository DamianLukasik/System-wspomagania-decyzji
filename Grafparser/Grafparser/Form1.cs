using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grafparser
{
    public partial class Form1 : Form
    {
        Dictionary<Guid, Neuron> neurony;
        Dictionary<int, Guid> lista;
        public Form1()
        {
            InitializeComponent();
        }      

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lista = new Dictionary<int, Guid>();
            neurony = new Dictionary<Guid, Neuron>();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Plik tekstowy|*.txt";
            openFileDialog1.Title = "Wybierz plik";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                Tbx_zawartość_pliku.Text = sr.ReadToEnd();
                sr.Close();
            }
            
            if (Generuj_Graf(Tbx_zawartość_pliku.Text))
            {
                MessageBox.Show("Graf został wygenerowany");                
                Pbx_graf.Image = new System.Drawing.Bitmap(Pbx_graf.Width, Pbx_graf.Height);
                using (Graphics G = Graphics.FromImage(Pbx_graf.Image))
                {
                    foreach (KeyValuePair<Guid, Neuron> neo in neurony)
                    {
                        //MessageBox.Show(neo.Value.show());
                        Point p = neo.Value.get_pkt();
                        System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(p.X,p.Y,10,10);
                        G.DrawEllipse(new Pen(neo.Value.get_kolor()), rectangle);
                        
                        List<Guid> krawędzie = neo.Value.get_edge();
                        foreach (Guid kraw in krawędzie)
                        {
                            Point o = neurony[kraw].get_pkt();
                            G.DrawLine(new Pen(neo.Value.get_kolor()), p, o);
                        }                        
                    }                                        
                }
                Pbx_graf.Refresh();
            }
        }
        private Boolean Generuj_Graf(String text)
        {
            String[] węzły = text.Split('|');
            int leng = węzły.Count() - 1;
            for (int i = 0; i < leng; i+=2)
            {
                //MessageBox.Show(węzły[i]+"  "+węzły[i+1]);
                Neuron neuron = new Neuron(węzły[i], węzły[i+1]);
                neurony.Add(neuron.get_idx(),neuron);
                lista.Add(lista.Count(), neuron.get_idx());
            }
            Dictionary<int, Guid> lista2 = lista.Reverse().ToDictionary(x => x.Key, x => x.Value);
            int k = 0;
            lista.Clear();
            foreach (KeyValuePair<int, Guid> l in lista2)
            {
                lista.Add(k,l.Value);
                k++;
            }
            Random rnd = new Random();
            foreach (KeyValuePair<Guid, Neuron> neo in neurony)
            {
                //  MessageBox.Show(neo.Key + "   " + neo.Value);
                List<String> bin_list = neo.Value.get_krawędz_odkodowaną();
                String bin = string.Join("", bin_list.ToArray());
                bin = Reverse(bin);
                int i = 0;
                foreach (char b in bin)
                {
                    if (b == '1')
                    {
                     //   MessageBox.Show(lista[i].ToString() + "  " + bin);
                        neo.Value.add_guid(lista[i]);
                    }
                    i++;
                }               
                Point p = new Point(rnd.Next(0, Pbx_graf.Width), rnd.Next(0, Pbx_graf.Height));
                neo.Value.set_pkt(p);
                neo.Value.set_kolor(Color.FromArgb(rnd.Next(60, 255), rnd.Next(60, 255), rnd.Next(60, 255)));
            }
            return true;
        }
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }   
}
