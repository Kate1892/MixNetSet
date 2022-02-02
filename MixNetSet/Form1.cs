using System;
using System.Drawing;
using System.Windows.Forms;

namespace MixNetSet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mixSet.Graph g = new mixSet.Graph("net2.XML");
            stackAlg set = new stackAlg(g);
            schema ortSet = new schema(g);
            edges ed = new edges(g);
            g.xSave("net2.XML");

            pbCanvas.SetBounds(0, 0, 3000, 5000);
        }

        private void pictureBox3_Paint(object sender, PaintEventArgs e)
        {
            Pen p = Pens.Green;
            Graphics g = e.Graphics;
            float recWidt = 25;
            float recHeight = 20;
            foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
            {
                n.gridX = n.longitude;
                n.gridY = n.latitude;

                g.DrawRectangle(p, n.longitude, n.latitude, recWidt, recHeight);
                g.DrawString(n.ID, Font, Brushes.Red, n.longitude + 5, n.latitude + 5);
            }

           
            int lk = 0;
            string lf = "";
            foreach (mixSet.Edge r in mixSet.Graph.hashEdges.Values)
            {               
                
                if (r.h == 0)
                {
                    if (r.e1Coord.Count != 0) 
                    {
                        g.DrawLine(p, (float)r.e1Coord[0], (float)r.e1Coord[1], (float)r.e2Coord[0], (float)r.e2Coord[1]);
                    }
                    else
                    {
                        lk += 1;
                        lf += r.n1.ID + r.n2.ID;
                    }
                }
                else
                {               
                    g.DrawLine(p, (float)r.e1Coord[0], (float)r.e1Coord[1], (float)r.bendCoord[0], (float)r.bendCoord[1]);
                    g.DrawLine(p, (float)r.e2Coord[0], (float)r.e2Coord[1], (float)r.bendCoord[0], (float)r.bendCoord[1]);      
                }
               // g.DrawLine(p, r.n1.longitude+10, r.n1.latitude+10, r.n2.longitude+10, r.n2.latitude+10);
            }
            lf += "";
            lk += 0;
            
        }
    }
}
