using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Timer time;
        board brd = new board();
        Random ran = new Random();
        List<Zone> free;
        int seconds = 0, minutes = 0;
        


        public Form1()
        {
            InitializeComponent();
            this.KeyUp -=Form1_KeyUp;
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
            this.KeyPreview = true;
            panel1.Width = 350;
            panel1.Height = 350;
            brd.elWidth = 110;
            initFree();
            startRan();
            time = new Timer();
            time.Interval = 1000;
            time.Tick += second;
           

        }

        private void second(object sender, EventArgs e)
        {
            seconds++;
            if(seconds + 1 >= 60)
            {
                minutes++;
                seconds = 0;
            }
            label3.Text = String.Format(" {0}{1}:{2}{3}" , (minutes/10).ToString(), (minutes % 10).ToString(), (seconds / 10).ToString(), (seconds % 10).ToString());
            

        }

        private void initFree()
        {
            free = new List<Zone>();
            foreach (Zone zone in brd.zones)
            {
                free.Add(zone);
            }
           

        }
        private void startRan()
        {
            int a = 1, b = 1;
            while(a == b)
            {
                a = ran.Next(1, 5) * 10 + ran.Next(1, 5);
                b = ran.Next(1, 5) * 10 + ran.Next(1, 5);
            }
            foreach(Zone zone in brd.zones)
            {
                if(zone.zID == a)
                {
                    zone.value += 2;
                    free.Remove(zone);
                }else if(zone.zID == b)
                {
                    zone.value += 2;
                    free.Remove(zone);
                }
            }
            
            
            panel1.Invalidate();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
            foreach(Zone z in brd.zones) { z.calcColor(); }
            Graphics g = panel1.CreateGraphics();
            brd.drawBack(g);
            g.Dispose();
            AddZoneLabels();
        }

        
        private void AddZoneLabels()
        {
            brd.calcScore();
            lblScore.Text = brd.score.ToString();
            lblBest.Text = brd.bScore.ToString();
            for (int i = 0; i < brd.zones.Count; i++)
            {
                Zone zone = brd.zones[i];
                Label label = new Label();
                label.Text = zone.value.ToString();
                label.Width = zone.width;
                label.Height = zone.width;
                label.Location = new Point(zone.zX, zone.zY);
                label.BackColor = zone.color;
                label.TextAlign = ContentAlignment.MiddleCenter;
                panel1.Controls.Add(label);
            }
            gameOver();

        }



        private void btnRestart_Click(object sender, EventArgs e)
        {
            foreach (Zone zone in brd.zones)
            {
                zone.value = 0;
            }
            initFree();
            panel1.Controls.Clear();
            startRan();
            time.Stop();
            seconds = 0;
            minutes = 0;
            label3.Text = String.Format(" 00:00");
            

        }
        public void addRandom()
        {
            if (free.Count > 0)
            {
                int index = ran.Next(free.Count);
                Zone randomZone = free[index];
                randomZone.value = 2;
                free.Remove(randomZone);
                
            }
            
        }
       
        private void btnLeft_Click(object sender, EventArgs e)
        {
            bool moved = false;
                foreach (Zone zone in brd.zones)
                {
                    zone.isMerged = false; 
                }
                int d = 0;
                do
                {
                    for (int row = 0; row < 4; row++)
                    {
                        for (int col = 1; col < 4; col++)
                        {
                            Zone currentZone = null;
                            Zone leftNeighbor = null;
                            int zID = (row + 1) * 10 + (col + 1);
                            foreach (Zone zone in brd.zones)
                            {
                                if (zone.zID == zID)
                                { currentZone = zone; break; }
                            }

                            zID = (row + 1) * 10 + (col);
                            foreach (Zone zone in brd.zones)
                            {
                                if (zone.zID == zID)
                                {
                                    leftNeighbor = zone; break;
                                }
                            }
                            if (leftNeighbor.value == 0 && currentZone.value >= 2)
                            {
                                leftNeighbor.value = currentZone.value;
                                currentZone.value = 0;
                                free.Add(currentZone);
                                free.Remove(leftNeighbor);
                                moved = true;
                            }
                            else if (leftNeighbor.value == currentZone.value && !leftNeighbor.isMerged && !currentZone.isMerged && currentZone.value >= 2)
                            {
                                leftNeighbor.value += currentZone.value;
                                currentZone.value = 0;
                                leftNeighbor.isMerged = true;
                                free.Add(currentZone);
                                currentZone.isMerged = true;
                                moved = true;
                        }


                        }
                    }
                d++;
                } while (d < 3);

            if(moved)
            {
                addRandom();
                panel1.Controls.Clear();
                panel1.Invalidate();
            }
            

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            bool moved = false;
            foreach (Zone zone in brd.zones)
            {
                zone.isMerged = false;
            }
            int d = 0;
            do
            {
                for (int col = 0; col < 4; col++)
                {
                    for (int row = 1; row < 4; row++)
                    {
                        Zone currentZone = null;
                        Zone upNeighbor = null;
                        int zID = (row + 1) * 10 + (col + 1);
                        foreach (Zone zone in brd.zones)
                        {
                            if (zone.zID == zID)
                            {
                                currentZone = zone;
                            }
                        }

                        zID = (row) * 10 + (col + 1);
                        foreach (Zone zone in brd.zones)
                        {
                            if (zone.zID == zID)
                            {
                                upNeighbor = zone;
                            }
                        }

                        if (upNeighbor.value == 0 && currentZone.value >= 2)
                        {
                            upNeighbor.value = currentZone.value;
                            currentZone.value = 0;
                            free.Add(currentZone);
                            free.Remove(upNeighbor);
                            moved = true;
                        }
                        else if (upNeighbor.value == currentZone.value && !upNeighbor.isMerged && !currentZone.isMerged && currentZone.value >= 2)
                        {
                            upNeighbor.value += currentZone.value;
                            currentZone.value = 0;
                            upNeighbor.isMerged = true;
                            free.Add(currentZone);
                            currentZone.isMerged = true;
                            moved = true;
                        }
                    }
                }

                d++;
            } while (d < 3);
            if (moved)
            {
                addRandom();
                panel1.Controls.Clear();
                panel1.Invalidate();
            }
            
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            bool moved = false;
            foreach (Zone zone in brd.zones)
            {
                zone.isMerged = false;
            }
            int d = 0;
            do
            {
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 2; col >= 0; col--)
                    {
                        Zone currentZone = null;
                        Zone rightNeighbor = null;
                        int zID = (row + 1) * 10 + (col + 1);
                        foreach (Zone zone in brd.zones)
                        {
                            if (zone.zID == zID)
                            {
                                currentZone = zone;
                            }
                        }

                        zID = (row + 1) * 10 + (col + 2);
                        foreach (Zone zone in brd.zones)
                        {
                            if (zone.zID == zID)
                            {
                                rightNeighbor = zone;
                            }
                        }

                        if (rightNeighbor.value == 0 && currentZone.value >= 2)
                        {
                            rightNeighbor.value = currentZone.value;
                            currentZone.value = 0;
                            free.Add(currentZone);
                            free.Remove(rightNeighbor);
                            moved = true;
                        }
                        else if (rightNeighbor.value == currentZone.value && !rightNeighbor.isMerged && !currentZone.isMerged && currentZone.value >= 2)
                        {
                            rightNeighbor.value += currentZone.value;
                            currentZone.value = 0;
                            rightNeighbor.isMerged = true;
                            free.Add(currentZone);
                            currentZone.isMerged = true;
                            moved = true;
                        }
                    }
                }
                d++;
            } while (d < 3);
            if (moved)
            {
                addRandom();
                panel1.Controls.Clear();
                panel1.Invalidate();
            }
            
        }


        private void btnDown_Click(object sender, EventArgs e)
        {
            bool moved = false;
            foreach (Zone zone in brd.zones)
            {
                zone.isMerged = false;
            }
            int d = 0;
            do
            {
                for (int col = 0; col < 4; col++)
                {
                    for (int row = 2; row >= 0; row--)
                    {
                        Zone currentZone = null;
                        Zone downNeighbor = null;
                        int zID = (row + 1) * 10 + (col + 1);
                        foreach (Zone zone in brd.zones)
                        {
                            if (zone.zID == zID)
                            {
                                currentZone = zone;
                            }
                        }

                        zID = (row + 2) * 10 + (col + 1);
                        foreach (Zone zone in brd.zones)
                        {
                            if (zone.zID == zID)
                            {
                                downNeighbor = zone;
                            }
                        }

                        if (downNeighbor.value == 0 && currentZone.value >= 2)
                        {
                            downNeighbor.value = currentZone.value;
                            currentZone.value = 0;
                            free.Add(currentZone);
                            free.Remove(downNeighbor);
                            moved = true;
                        }
                        else if (downNeighbor.value == currentZone.value && !downNeighbor.isMerged && !currentZone.isMerged && currentZone.value >= 2)
                        {
                            downNeighbor.value += currentZone.value;
                            currentZone.value = 0;
                            downNeighbor.isMerged = true;
                            free.Add(currentZone);
                            currentZone.isMerged = true;
                            moved = true;
                        }
                    }
                }
                d++;
            } while (d < 3);
            if (moved)
            {
                addRandom();
                panel1.Controls.Clear();
                panel1.Invalidate();
            }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            time.Start();

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
            {
                btnUp_Click(sender, e);
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                btnLeft_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
            {
                btnDown_Click(sender, e);
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                btnRight_Click(sender, e);
            }
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.R)
            {
                btnRestart_Click(sender, e);
            }
  

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private bool CanMove()
        {

            
            if (free.Count > 0) return true;

            
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    Zone currentZone = brd.zones[row * 4 + col];

                    
                    if (col < 3)
                    {
                        Zone rightNeighbor = brd.zones[row * 4 + col + 1];
                        if (currentZone.value == rightNeighbor.value) return true;
                    }

                    
                    if (row < 3)
                    {
                        Zone bottomNeighbor = brd.zones[(row + 1) * 4 + col];
                        if (currentZone.value == bottomNeighbor.value) return true;
                    }

                    
                    if (col > 0)
                    {
                        Zone leftNeighbor = brd.zones[row * 4 + col - 1];
                        if (currentZone.value == leftNeighbor.value) return true;
                    }

                    
                    if (row > 0)
                    {
                        Zone topNeighbor = brd.zones[(row - 1) * 4 + col];
                        if (currentZone.value == topNeighbor.value) return true;
                    }
                }
            }
            return false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnRestart_Click(sender, e);
            brd.bScore = 0;
        }

        public void gameOver()
        {
            if(!CanMove())
            {
                time.Stop();
                GameOver go = new GameOver();
                go.score = brd.score;
                go.s();
                DialogResult result = go.ShowDialog();
                if (result == DialogResult.OK)
                {
                    foreach (Zone zone in brd.zones)
                    {
                        zone.value = 0;
                    }
                    initFree();
                    panel1.Controls.Clear();
                    startRan();
                    time.Stop();
                    seconds = 0;
                    minutes = 0;
                    label3.Text = String.Format(" 00:00");
                    
                }
            }
        }

    }
}
