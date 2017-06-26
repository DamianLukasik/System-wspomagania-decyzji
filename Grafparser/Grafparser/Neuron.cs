using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafparser
{    
    class Neuron
    {
        //Z pliku tekstowego
        String dane;
        String krawędzie_zakodowane;
        //Struktura logiczna
        Guid idx = Guid.NewGuid();
        List<Guid> edge = new List<Guid>();
        Point pkt = new Point();
        Color kolor = new Color();
        public Neuron(string dane, string krawędzie_zakodowane)
        {
            this.dane = dane;
            this.krawędzie_zakodowane = krawędzie_zakodowane;
        }
        public void set_kolor(Color k)
        {
            kolor = k;
        }
        public Color get_kolor()
        {
            return kolor;
        }
        public void set_pkt(Point p)
        {
            pkt = p;
        }
        public Point get_pkt()
        {
            return pkt;
        }
        public List<Guid> get_edge()
        {
            return edge;
        }
        public Guid get_idx() { return idx; }       

        private void wyczysć_wszystko()
        {        
            dane = null;
            krawędzie_zakodowane = null;
        }
        private void wyczysć_(int i)
        {
            switch (i)
            {
                case 1:
                    dane = null;
                    break;
                case 2:
                    krawędzie_zakodowane = null;
                    break;
                default:
                    break;
            }
        }
        public List<String> get_krawędz_odkodowaną()
        {
            List<String> List_bin = new List<String>();
            foreach (string letter in krawędzie_zakodowane.Select(c => Convert.ToString(c, 2)))
            {
                List_bin.Add(letter.Substring(2));
            }
            return List_bin;
        }

        internal string show()
        {
            string str = "Neuron:\n"+idx+ "\n==========\n"+ 
                dane+"\n"+krawędzie_zakodowane+" - "+string.Join("",get_krawędz_odkodowaną().ToArray())+ "\n\nKrawędź:\n";
            foreach(Guid v in edge)
            {
                str += v.ToString() + "\n";
            }
            return str;
        }

        internal void add_guid(Guid key)
        {
            edge.Add(key);
        }
    }
}
