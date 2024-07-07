using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace WindowsFormsApp1
{
    internal class board
    {
        public List<Zone> zones;

        public int zcount {get { return zones.Count;} }
        public int elWidth {  get; set; }
        public int score { get; set; }

        public int bScore { get; set; } = 0;

        
        public void calcScore()
        {
            double s = 0, coef = 0.1;
            foreach(Zone z in zones)
            {
                if (z.value >= 2048)
                {
                    coef = 0.03;
                }
                else if (z.value >= 1024)
                {   
                    coef = 0.05;
                }
                else if(z.value >=512)
                {
                    coef = 0.07;
                }
                s += z.value * (z.value * coef);
                score = (int)s;
            }
            if(score > bScore)
            {
                bScore = score;
            }
        }
        public board() { 
            zones = new List<Zone>();
          
            zones.Add(new Zone(11));
            zones.Add(new Zone(12));
            zones.Add(new Zone(13));
            zones.Add(new Zone(14));
            zones.Add(new Zone(21));
            zones.Add(new Zone(22));
            zones.Add(new Zone(23));
            zones.Add(new Zone(24));
            zones.Add(new Zone(31));
            zones.Add(new Zone(32));
            zones.Add(new Zone(33));
            zones.Add(new Zone(34));
            zones.Add(new Zone(41));
            zones.Add(new Zone(42));
            zones.Add(new Zone(43));
            zones.Add(new Zone(44));

        }
      

        public void drawBack(Graphics g)
        {
            SolidBrush br = new SolidBrush(Color.FromArgb(100, 100, 100));
            g.FillRectangle(br, 0, 0, 350, 350);

            br = new SolidBrush(Color.FromArgb(200, 200, 200));
            g.FillRectangle(br,  2, 2 , 346, 346);
            br.Dispose();
        }

        public void drawAll(Graphics g)
        {

            foreach (Zone frm in zones)
                frm.draw(g);
        }
    }
}
