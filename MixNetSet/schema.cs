using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using static MixNetSet.mixSet;
using System.Collections;
using System.Security.Policy;
using System.Windows.Forms.VisualStyles;

namespace MixNetSet
{
    class schema
    {
        private Graph graph;
        public schema(Graph g)
        {
            this.graph = g;
            this.schemvert();
        }

        //размещение вершин
        double maxYGRID = 0;
        double minXGRID = 2147483646; //2.5 для 2 схемы
        double epsilon1 = 2.5; //0.2 или 0.27 или 0.5 для области 4 для городов 2.5 для  второй схемы //0.007 для метро 0.3 для районов  0.3 для городов 0.01 для районов
        double epsilon2 = 2.5;
        double koef = 6; // 0.05 для областей 6 для городов
        int NUM = 0;
        int YlayersNum = 0;
        int XlayersNum = 0;
        int n = 50; // макс кол-во уровней
        ArrayList nodesToRemove = new ArrayList();
        public static Dictionary<string, Node> freeNodes = new Dictionary<string, Node>(mixSet.Graph.hashNodes);
        public static Dictionary<string, Node> freeNodesX = new Dictionary<string, Node>(mixSet.Graph.hashNodes);
        List<List<Node>> layerX = new List<List<Node>>(); //горизонтально
        List<List<Node>> layerY = new List<List<Node>>(); //вертикально

        public void schemvert()
        {

            for (int j = 0; j < n; j++)
            {
                layerX.Add(new List<Node>());
            }
            for (int j = 0; j < n; j++)
            {
                layerY.Add(new List<Node>());
            }
            // упорядочивание по строкам
            while (freeNodes.Count != 0)
            {
                foreach (mixSet.Node m in freeNodes.Values)
                {
                    if (m.gridY >= maxYGRID) maxYGRID = m.gridY;
                }
                foreach (mixSet.Node m in freeNodes.Values)
                {
                    if (m.gridY >= (maxYGRID - epsilon2))
                    {
                        mixSet.Graph.hashNodes[m.ID].gridY = maxYGRID;
                        mixSet.Graph.hashNodes[m.ID].levelNUMY = NUM;
                        nodesToRemove.Add(m.ID);
                    }
                }
                foreach (string k in nodesToRemove)
                {
                    layerX[NUM].Add(mixSet.Graph.hashNodes[k]);
                    freeNodes.Remove(k);
                }
                nodesToRemove.Clear();
                maxYGRID = 0;
                NUM += 1;
            }
            YlayersNum = NUM - 1;
            NUM = 0;
            //упорядочивание по столбцам
            while (freeNodesX.Count != 0)
            {
                foreach (mixSet.Node m in freeNodesX.Values)
                {
                    if (m.gridX <= minXGRID) minXGRID = m.gridX;
                }
                foreach (mixSet.Node m in freeNodesX.Values)
                {
                    if (m.gridX <= (minXGRID + epsilon1))
                    {
                        mixSet.Graph.hashNodes[m.ID].gridX = minXGRID;
                        mixSet.Graph.hashNodes[m.ID].levelNUMX = NUM;
                        nodesToRemove.Add(m.ID);
                    }
                }
                foreach (string m in nodesToRemove)
                {
                    layerY[NUM].Add(mixSet.Graph.hashNodes[m]);
                    freeNodesX.Remove(m);
                }
                nodesToRemove.Clear();
                minXGRID = 2147483646;
                NUM += 1;
            }
            XlayersNum = NUM - 1;
       
            //стянем по x 
            for (int i = 0; i < XlayersNum; i++)
            {
                if (!(layerY[i][0].gridX - layerY[i + 1][0].gridX == koef)) //6 для городов, 0,005\\ 0.01 метро 0.05 или 0.03 для районов
                {
                    foreach (Node m in layerY[i + 1])
                    {
                        m.gridX = layerY[i][0].gridX + koef;
                    }
                }
            }
            //стянем по у
            for (int i = YlayersNum - 1; i > -1; i--)
            {
                if (!(layerX[i + 1][0].gridY - layerX[i][0].gridY == koef))
                {
                    foreach (Node m in layerX[i])
                    {
                        m.gridY = layerX[i + 1][0].gridY + koef;
                    }
                }
            }

            for (int i = 0; i <= YlayersNum; i++) 
            {
                foreach (Node m in layerX[i])
                {
                    foreach (Node n in layerX[i])
                    {
                        if (m.ID != n.ID)
                        {
                            if (n.gridX == m.gridX && n.gridY == m.gridY)
                            {
                                double Xdist = m.gridXCONST - n.gridYCONST;
                                double Ydist = m.gridYCONST - n.gridYCONST;
                                int nomer = 0;
                                if (Math.Abs(Xdist) > Math.Abs(Ydist))
                                {
                                    if (Xdist < Math.Abs(Xdist))
                                    {
                                        List<Node> curr = new List<Node>();
                                        curr = layerY.Find(x => x.Contains(m));
                                        for (int h = 0; h <= XlayersNum; h++) 
                                        {
                                            if (curr == layerY[h])
                                            {
                                                nomer = h;
                                            }
                                        }
                                        if (!(layerY[nomer + 1].Exists(x => x.gridY == m.gridY)))
                                        {
                                            m.gridX = layerY[nomer + 1][0].gridX;
                                            layerY[nomer].Remove(m);
                                            layerY[nomer + 1].Add(m);
                                        }
                                        else
                                        {
                                            if (!(layerY[nomer - 1].Exists((x => x.gridY == m.gridY))))
                                            {
                                                m.gridX = layerY[nomer - 1][0].gridX;
                                                layerY[nomer].Remove(m);
                                                layerY[nomer - 1].Add(m);
                                            }
                                            else
                                            {
                                                curr = layerX.Find(x => x.Contains(m));
                                                for (int h = 0; h <= YlayersNum; h++)
                                                {
                                                    if (curr == layerX[h])
                                                    {
                                                        nomer = h;
                                                    }
                                                }
                                                if (!(layerX[nomer + 1].Exists(x => x.gridX == m.gridX)))
                                                {
                                                    m.gridY = layerX[nomer + 1][0].gridY;
                                                    
                                                }
                                                else
                                                {
                                                    if (!(layerX[nomer - 1].Exists(x => x.gridX == m.gridX)))
                                                    {
                                                         m.gridY = layerX[nomer - 1][0].gridY;
                                                     
                                                    }
                                                }

                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (Xdist == Math.Abs(Xdist))
                                        {
                                            List<Node> curr = new List<Node>();
                                            curr = layerY.Find(x => x.Contains(n));
                                            for (int h = 0; h <= XlayersNum; h++)
                                            {
                                                if (curr == layerY[h])
                                                {
                                                    nomer = h;
                                                }

                                            }
                                            if (!(layerY[nomer + 1].Exists(x => x.gridY == n.gridY)))
                                            {
                                                if (layerY[nomer + 1].Count != 0) 
                                                {
                                                    n.gridX = layerY[nomer + 1][0].gridX;
                                                    layerY[nomer].Remove(n);
                                                    layerY[nomer + 1].Add(n);
                                                }
                                            }
                                            else
                                            {
                                                if (!(layerY[nomer - 1].Exists((x => x.gridY == n.gridY))))
                                                {
                                                    n.gridX = layerY[nomer - 1][0].gridX;
                                                    layerY[nomer].Remove(n);
                                                    layerY[nomer - 1].Add(n);
                                                }
                                                else
                                                {
                                                    curr = layerX.Find(x => x.Contains(n));
                                                    for (int h = 0; h <= YlayersNum; h++)
                                                    {
                                                        if (curr == layerX[h])
                                                        {
                                                            nomer = h;
                                                        }
                                                    }
                                                    if (!(layerX[nomer + 1].Exists(x => x.gridX == n.gridX)))
                                                    {
                                                        n.gridY = layerX[nomer + 1][0].gridY;
                                                   
                                                    }
                                                    else
                                                    {
                                                        if (!(layerX[nomer - 1].Exists(x => x.gridX == n.gridX)))
                                                        {
                                                            n.gridY = layerX[nomer - 1][0].gridY;
                                                          
                                                        }

                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    /**/
                                    if (Math.Abs(Ydist) > Math.Abs(Xdist)) 
                                    {
                                        if (Ydist == Math.Abs(Ydist))
                                        {
                                            List<Node> curr = new List<Node>();
                                            curr = layerX.Find(x => x.Contains(n));
                                            for (int h = 0; h <= YlayersNum; h++)
                                            {
                                                if (curr == layerX[h])
                                                {
                                                    nomer = h;
                                                }

                                            }
                                            if (!(layerX[nomer + 1].Exists(x => x.gridX == n.gridX)))
                                            {
                                                if (layerX[nomer + 1].Count != 0) //добавить везде такие условия
                                                {
                                                    n.gridY = layerX[nomer + 1][0].gridY;
                                                    layerX[nomer].Remove(n);
                                                    layerX[nomer + 1].Add(n);
                                                }
                                            }
                                            else
                                            {
                                                if (!(layerY[nomer - 1].Exists((x => x.gridY == n.gridY))))
                                                {
                                                    n.gridX = layerY[nomer - 1][0].gridX;
                                                    layerY[nomer].Remove(n);
                                                    layerY[nomer - 1].Add(n);
                                                }
                                                else
                                                {
                                                    curr = layerY.Find(x => x.Contains(n));
                                                    for (int h = 0; h <= XlayersNum; h++)
                                                    {
                                                        if (curr == layerY[h])
                                                        {
                                                            nomer = h;
                                                        }
                                                    }
                                                    if (!(layerY[nomer + 1].Exists(x => x.gridY == n.gridY)))
                                                    {
                                                        n.gridX = layerY[nomer + 1][0].gridX;
                                                        ///////////////////////////////////////layerX[nomer].Remove(n);
                                                        ///////////////////////////////////////layerX[nomer + 1].Add(n);
                                                    }
                                                    else
                                                    {
                                                        if (!(layerY[nomer - 1].Exists(x => x.gridY == n.gridY)))
                                                        {
                                                            n.gridX = layerY[nomer - 1][0].gridX;
                                                           
                                                        }

                                                    }

                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Ydist < Math.Abs(Ydist))
                                            {
                                                List<Node> curr = new List<Node>();
                                                curr = layerX.Find(x => x.Contains(m));
                                                for (int h = 0; h <= YlayersNum; h++) 
                                                {
                                                    if (curr == layerX[h])
                                                    {
                                                        nomer = h;
                                                    }
                                                }
                                                if (!(layerX[nomer + 1].Exists(x => x.gridX == m.gridX)))
                                                {
                                                    m.gridY = layerX[nomer + 1][0].gridY;
                                                    layerX[nomer].Remove(m);
                                                    layerX[nomer + 1].Add(m);
                                                }
                                                else
                                                {
                                                    if (!(layerX[nomer - 1].Exists((x => x.gridX == m.gridX))))
                                                    {
                                                        m.gridY = layerX[nomer - 1][0].gridY;
                                                        layerX[nomer].Remove(m);
                                                        layerX[nomer - 1].Add(m);
                                                    }
                                                    else
                                                    {
                                                        curr = layerY.Find(x => x.Contains(m));
                                                        for (int h = 0; h <= XlayersNum; h++)
                                                        {
                                                            if (curr == layerY[h])
                                                            {
                                                                nomer = h;
                                                            }
                                                        }
                                                        if (!(layerY[nomer + 1].Exists(x => x.gridY == m.gridY)))
                                                        {
                                                            m.gridX = layerY[nomer + 1][0].gridX;
                                                           
                                                        }
                                                        else
                                                        {
                                                            if (!(layerY[nomer - 1].Exists(x => x.gridY == m.gridY)))
                                                            {
                                                                m.gridX = layerY[nomer - 1][0].gridX;
                                                              
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i <= XlayersNum; i++)
            {
                foreach (Node m in layerY[i])
                {
                    foreach (Node n in layerY[i])
                    {
                        if (m.ID != n.ID)
                        {
                            if (n.gridX == m.gridX && n.gridY == m.gridY)
                            {
                                double Xdist = m.gridXCONST - n.gridYCONST;
                                double Ydist = m.gridYCONST - n.gridYCONST;
                                int nomer = 0;

                                if (Ydist < Math.Abs(Ydist))
                                {
                                    List<Node> curr = new List<Node>();
                                    curr = layerX.Find(x => x.Contains(m));
                                    for (int h = 0; h <= YlayersNum; h++)
                                    {
                                        if (curr == layerX[h])
                                        {
                                            nomer = h;
                                        }
                                    }
                                    if (!(layerX[nomer + 1].Exists(x => x.gridX == m.gridX)))
                                    {
                                        m.gridY = layerX[nomer + 1][0].gridY;
                                        layerX[nomer].Remove(m);
                                        layerX[nomer + 1].Add(m);
                                    }
                                    else
                                    {
                                        if (!(layerX[nomer - 1].Exists((x => x.gridX == m.gridX))))
                                        {
                                            m.gridY = layerX[nomer - 1][0].gridY;
                                            layerX[nomer].Remove(m);
                                            layerX[nomer - 1].Add(m);
                                        }
                                    }
                                }
                                else
                                {
                                    if (Ydist == Math.Abs(Ydist))
                                    {
                                        List<Node> curr = new List<Node>();
                                        curr = layerX.Find(x => x.Contains(n));
                                        for (int h = 0; h <= YlayersNum; h++)
                                        {
                                            if (curr == layerX[h])
                                            {
                                                nomer = h;
                                            }
                                        }
                                        if (!(layerX[nomer + 1].Exists(x => x.gridX == n.gridX)))
                                        {
                                            n.gridY = layerX[nomer + 1][0].gridY;
                                            layerX[nomer].Remove(n);
                                            layerX[nomer + 1].Add(n);
                                        }
                                        else
                                        {
                                            if (!(layerX[nomer - 1].Exists((x => x.gridX == n.gridX))))
                                            {
                                                n.gridY = layerX[nomer - 1][0].gridY;
                                                layerX[nomer].Remove(n);
                                                layerX[nomer - 1].Add(n);
                                            }
                                            else
                                            {
                                                for (int j = nomer + 1; j <= XlayersNum; j++)
                                                {
                                                    if (!layerX[j + 1].Exists(x => x.gridX == n.gridX))
                                                    {
                                                        for (int k = nomer + 1; k <= j + 1; k++)
                                                        {
                                                           //
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }           
        }                           
    }
}
 