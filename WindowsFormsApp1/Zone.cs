using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class Zone
    {
        public int zID {  get; set; }
        public int width { get; set; }
        public bool isMerged {  get; set; } = false;
        public int zX { get; set; }
        public int zY { get; set; }
        
        public int value { get; set; }
        public Zone(int zid)
        {
            zID = zid;
            zID = zid;
            margin = 10;
            width = 75;
            calcPosition();
        }
        public Color color { get; set; }
        public int margin { get;  set; }

        public void calcColor()
        {
            int v = value;
            double r = v * (v * 0.2), g = v * (v * 1.2), b = v * (v*0.01); 

            if(v == 0)
            {
                color = Color.FromArgb(220, 220, 220);
            }else if(g < 200)
            {
                color = Color.FromArgb((int)r, 255 - (int)g, 100);
            }else if(r < 255)
            {
                color = Color.FromArgb((int) r, 55, 100);
            }else if(b < 100)
            {
                color = Color.FromArgb(255, 55, 100 + (int) b);
            }
                
           
        }
        public override string ToString()
        {
            return zID.ToString();
        }

        public void calcPosition()
        {
            
            int mod = zID % 10;
            int div = zID / 10;
            zX = (mod - 1) * width + mod * margin;
            zY = (div - 1) * width + div * margin;
        }
        
        public void draw(Graphics g)
        {
            calcColor();
            SolidBrush br = new SolidBrush(color);
            
            g.FillRectangle(br, zX, zY, width, width);
            br.Dispose();
        }
    }
}
