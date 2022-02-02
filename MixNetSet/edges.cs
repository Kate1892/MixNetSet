using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static MixNetSet.mixSet;

namespace MixNetSet
{
    class edges
    {
        private Graph graph;
        public edges(Graph g)
        {
            this.graph = g;
            this.nodeL();
            this.ortEdges();


        }
        public void nodeL()
        {
            double minX = 2147483646;
            double maxY = -2147483646;
            foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
            {
                if (minX > n.gridX) { minX = n.gridX; }
                if (maxY < Math.Abs(n.gridY)) { maxY = Math.Abs(n.gridY); }
            }
            if (minX < 0)
            {
                foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
                {
                    n.gridX -= minX;
                }
                minX = 0;
            }
            foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
            {
                if (Math.Abs(n.gridY) == maxY)
                {
                    if (n.gridX < 0)
                    {
                        foreach (mixSet.Node m in mixSet.Graph.hashNodes.Values)
                        {
                            m.gridY += maxY;
                        }
                    }
                }
            }
            maxY = -2147483646;
            foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
            {
                if (minX > n.gridX) { minX = n.gridX; }
                if (maxY < n.gridY) { maxY = n.gridY; }
            }
            int k = mixSet.Graph.hashEdges.Count();
            int l = mixSet.Graph.hashNodes.Count();
            foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
            {

                n.longitude = (float)(Math.Round((float)((n.gridX - minX) * 6 + 10), 1));  // на 6 для городов, на 1000 для областей, на 4000 для метро
                n.latitude = (float)(Math.Round((float)(maxY - n.gridY) * 6 + 10, 1));
             
                n.gridX = n.longitude;
                n.gridY = n.latitude;
            }
        }
        public int routeSel(Node n1, Node n2, int inPos, float recWidt, float recHeight)
        {
            string edgeID = "null";
            if (mixSet.Graph.hashEdges.ContainsKey(n1.ID + n2.ID))
            {
                mixSet.Graph.hashEdges[n1.ID + n2.ID].teste1Coord.Add(n1.gridX + recWidt/2);
                mixSet.Graph.hashEdges[n1.ID + n2.ID].teste1Coord.Add(n1.gridY + recHeight / 2);

                mixSet.Graph.hashEdges[n1.ID + n2.ID].teste2Coord.Add(n2.gridX + recWidt / 2);
                mixSet.Graph.hashEdges[n1.ID + n2.ID].teste2Coord.Add(n2.gridY + recHeight/2);
                edgeID = n1.ID + n2.ID;
            }
            if (mixSet.Graph.hashEdges.ContainsKey(n2.ID + n1.ID))
            {
                mixSet.Graph.hashEdges[n2.ID + n1.ID].teste1Coord.Add(n1.gridX + recWidt/2);
                mixSet.Graph.hashEdges[n2.ID + n1.ID].teste1Coord.Add(n1.gridY + recHeight / 2);

                mixSet.Graph.hashEdges[n2.ID + n1.ID].teste2Coord.Add(n2.gridX + recWidt / 2);
                mixSet.Graph.hashEdges[n2.ID + n1.ID].teste2Coord.Add(n2.gridY + recHeight / 2);
                edgeID = n2.ID + n1.ID;
            }
            if (inPos == 1)
            {
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste1Coord[0]; //h=1
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste2Coord[1];

                mixSet.Graph.hashEdges[edgeID].StestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste2Coord[0]; //h=2
                mixSet.Graph.hashEdges[edgeID].StestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste1Coord[1];
            }
            if (inPos == 2)
            {
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste2Coord[0]; //h=1
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste1Coord[1]; 

                mixSet.Graph.hashEdges[edgeID].StestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste1Coord[0]; //h=2
                mixSet.Graph.hashEdges[edgeID].StestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste2Coord[1];
            }
            if (inPos == 3)
            {
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste2Coord[0]; //h=1
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste1Coord[1];

                mixSet.Graph.hashEdges[edgeID].StestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste1Coord[0]; //h=2
                mixSet.Graph.hashEdges[edgeID].StestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste2Coord[1];
            }
            if (inPos == 4)
            {
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste1Coord[0]; //h=1
                mixSet.Graph.hashEdges[edgeID].FtestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste2Coord[1];

                mixSet.Graph.hashEdges[edgeID].StestbendCoord[0] = mixSet.Graph.hashEdges[edgeID].teste2Coord[0]; //h=2
                mixSet.Graph.hashEdges[edgeID].StestbendCoord[1] = mixSet.Graph.hashEdges[edgeID].teste1Coord[1];
            }
            int intersecCount1 = 0;
            int intersecCount2 = 0;
            string hhh = "";
            foreach(Node m in mixSet.Graph.hashNodes.Values)
            {
                if (intersection(m, mixSet.Graph.hashEdges[edgeID].teste1Coord[0], mixSet.Graph.hashEdges[edgeID].teste1Coord[1],
                    mixSet.Graph.hashEdges[edgeID].FtestbendCoord[0], mixSet.Graph.hashEdges[edgeID].FtestbendCoord[1], recWidt, recHeight) == 1 ||
                     intersection(m, mixSet.Graph.hashEdges[edgeID].teste2Coord[0], mixSet.Graph.hashEdges[edgeID].teste2Coord[1],
                     mixSet.Graph.hashEdges[edgeID].FtestbendCoord[0], mixSet.Graph.hashEdges[edgeID].FtestbendCoord[1], recWidt, recHeight) == 1)
                {
                    intersecCount1 += 1;
                    hhh += m.ID;
                }
               
                if (intersection(m, mixSet.Graph.hashEdges[edgeID].teste1Coord[0], mixSet.Graph.hashEdges[edgeID].teste1Coord[1],
                     mixSet.Graph.hashEdges[edgeID].StestbendCoord[0], mixSet.Graph.hashEdges[edgeID].StestbendCoord[1], recWidt, recHeight) ==1 ||
                     intersection(m, mixSet.Graph.hashEdges[edgeID].teste2Coord[0], mixSet.Graph.hashEdges[edgeID].teste2Coord[1],
                     mixSet.Graph.hashEdges[edgeID].StestbendCoord[0], mixSet.Graph.hashEdges[edgeID].StestbendCoord[1], recWidt, recHeight) == 1)
                {
                    intersecCount2 += 1;
                   
                }
            }
            if(intersecCount1 >= intersecCount2)
            {
                return 2; //те h=1;
            }
            else
            {
                return 1; //те h=2;
            }       
        }
        public int intersection(Node m, double p1x,double p1y, double p2x, double p2y, float recWeight, float recHeight) 
        {
            double r1x = m.gridX;
            double r1y = m.gridY;
            double r2x = m.gridX + recWeight;
            double r2y = m.gridY;
            double r3x = m.gridX + recWeight;
            double r3y = m.gridY + recHeight;
            double r4x = m.gridX;
            double r4y = m.gridY + recHeight;

            if (p1x > r1x && p1x > r2x && p1x > r3x && p1x > r4x && p2x > r1x && p2x > r2x && p2x > r3x && p2x > r4x) return 0;
            if (p1x < r1x && p1x < r2x && p1x < r3x && p1x < r4x && p2x < r1x && p2x < r2x && p2x < r3x && p2x < r4x) return 0;
            if (p1y > r1y && p1y > r2y && p1y > r3y && p1y > r4y && p2y > r1y && p2y > r2y && p2y > r3y && p2y > r4y) return 0;
            if (p1y < r1y && p1y < r2y && p1y < r3y && p1y < r4y && p2y < r1y && p2y < r2y && p2y < r3y && p2y < r4y) return 0;


            double f1 = (p2y - p1y) * r1x + (p1x - p2x) * r1y + (p2x * p1y - p1x * p2y);
            double f2 = (p2y - p1y) * r2x + (p1x - p2x) * r2y + (p2x * p1y - p1x * p2y);
            double f3 = (p2y - p1y) * r3x + (p1x - p2x) * r3y + (p2x * p1y - p1x * p2y);
            double f4 = (p2y - p1y) * r4x + (p1x - p2x) * r4y + (p2x * p1y - p1x * p2y);

            if (f1 < 0 && f2 < 0 && f3 < 0 && f4 < 0) return 0;
            if (f1 > 0 && f2 > 0 && f3 > 0 && f4 > 0) return 0;

            return 1;
        }
        public int straightEdges(int l, Node m)
        {
            for (int j = 0; j < m.sides[l].Count; j++)
            {
                if (mixSet.Graph.hashEdges.ContainsKey(m.ID + m.sides[l][j].ID))
                {
                    if (mixSet.Graph.hashEdges[m.ID + m.sides[l][j].ID].h == 0)
                    {
                        return 1;
                    }
                }
                if (mixSet.Graph.hashEdges.ContainsKey(m.sides[l][j].ID + m.ID))
                {
                    if (mixSet.Graph.hashEdges[m.sides[l][j].ID + m.ID].h == 0)
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
        int interposition = 0;
        public void ortEdges()
        {
            float recWidt = 25;
            float recHeight = 20; // нужны параметры
            int identifier = 0;
            // распределяем ребра по сторонам учитывая взаиморасположение и сохраняя выпуклость излома
            foreach (mixSet.Edge el in mixSet.Graph.hashEdges.Values)
            {
                if (el.n1.ID == "ХМ" && el.n2.ID == "СГ") { 
                    int jk = 0; }

                if (el.n1.gridX == el.n2.gridX)
                {
                    if (el.n1.gridY >= el.n2.gridY) {
                        if (!el.n1.sides[0].Exists(m => m.ID == el.n2.ID)) { el.n1.sides[0].Add(el.n2); }
                        if (!el.n2.sides[2].Exists(m => m.ID == el.n1.ID)) { el.n2.sides[2].Add(el.n1); } 
                    }
                    if (el.n1.gridY < el.n2.gridY) {
                        if (!el.n2.sides[0].Exists(m => m.ID == el.n1.ID)) { el.n2.sides[0].Add(el.n1); }
                        if (!el.n1.sides[2].Exists(m => m.ID == el.n2.ID)) { el.n1.sides[2].Add(el.n2); } 
                    }
                }
                if (el.n1.gridY == el.n2.gridY)
                {
                    if (el.n1.gridX <= el.n2.gridX){
                        if (!el.n1.sides[1].Exists(m => m.ID == el.n2.ID)) { el.n1.sides[1].Add(el.n2); }
                        if (!el.n2.sides[3].Exists(m => m.ID == el.n1.ID)) { el.n2.sides[3].Add(el.n1); }
                    }
                    if (el.n2.gridX < el.n1.gridX) {
                        if (!el.n2.sides[1].Exists(m => m.ID == el.n1.ID)) { el.n2.sides[1].Add(el.n1); }
                        if (!el.n1.sides[3].Exists(m => m.ID == el.n2.ID)) { el.n1.sides[3].Add(el.n2); } 
                    }
                }
                ///////////////////////////////////////////////////////// как вариант (не исп)
                //if (el.n1.gridX < el.n2.gridX)
                //{
                //    if (el.n1.gridY > el.n2.gridY)
                //    {
                //        if ((el.n1.sides[0].Count <= el.n1.sides[1].Count) || (el.n2.sides[3].Count <= el.n2.sides[2].Count))
                //        {
                //            el.n1.sides[0].Add(el.n2);
                //            el.n2.sides[3].Add(el.n1);
                //            el.h = 2;
                //        }
                //        else
                //        {
                //            if ((el.n1.sides[0].Count > el.n1.sides[1].Count) && (el.n2.sides[3].Count > el.n2.sides[2].Count))
                //            {
                //                el.n1.sides[1].Add(el.n2);
                //                el.n2.sides[2].Add(el.n1);
                //                el.h = 1;
                //            }
                //        }
                //    }
                //    if (el.n2.gridY > el.n1.gridY)
                //    {
                //        if ((el.n1.sides[2].Count <= el.n1.sides[1].Count) || (el.n2.sides[3].Count <= el.n2.sides[0].Count))
                //        {
                //            el.n1.sides[2].Add(el.n2);
                //            el.n2.sides[3].Add(el.n1);
                //            el.h = 2;
                //        }
                //        else
                //        {
                //            if ((el.n1.sides[1].Count < el.n1.sides[2].Count) && (el.n2.sides[0].Count < el.n2.sides[3].Count))
                //            {
                //                el.n1.sides[1].Add(el.n2);
                //                el.n2.sides[0].Add(el.n1);
                //                el.h = 1;
                //            }
                //        }
                //    }
                //}
                ////
                //if (el.n1.gridX > el.n2.gridX)
                //{
                //    if (el.n1.gridY < el.n2.gridY)
                //    {
                //        if ((el.n2.sides[1].Count <= el.n2.sides[0].Count) || (el.n1.sides[2].Count <= el.n1.sides[3].Count))
                //        {
                //            el.n2.sides[1].Add(el.n1);
                //            el.n1.sides[2].Add(el.n2);
                //            el.h = 2;
                //        }
                //        else
                //        {
                //            if ((el.n2.sides[1].Count > el.n2.sides[0].Count) && (el.n1.sides[2].Count > el.n1.sides[3].Count))
                //            {
                //                el.n2.sides[0].Add(el.n1);
                //                el.n1.sides[3].Add(el.n2);
                //                el.h = 1;
                //            }
                //        }
                //    }
                //    if (el.n2.gridY < el.n1.gridY)
                //    {
                //        if ((el.n2.sides[2].Count <= el.n2.sides[1].Count) || (el.n1.sides[3].Count <= el.n1.sides[0].Count))
                //        {
                //            el.n2.sides[2].Add(el.n1);
                //            el.n1.sides[3].Add(el.n2);
                //            el.h = 2;
                //        }
                //        else
                //        {
                //            if ((el.n2.sides[1].Count < el.n2.sides[2].Count) && (el.n1.sides[0].Count < el.n1.sides[3].Count))
                //            {
                //                el.n2.sides[1].Add(el.n1);
                //                el.n1.sides[0].Add(el.n2);
                //                el.h = 1;
                //            }
                //        }
                //    }
                //}
                /////////////////////////////////////////////////////////
                ////if (el.n1.gridX < el.n2.gridX)
                ////{
                ////    if (el.n1.gridY > el.n2.gridY)
                ////    {
                ////        interposition = 1;

                ////        if ((el.n1.sides[1].Count <= el.n1.sides[0].Count) || (el.n2.sides[2].Count <= el.n2.sides[3].Count))
                ////        {
                ////            el.n1.sides[1].Add(el.n2);
                ////            el.n2.sides[2].Add(el.n1);
                ////            el.h = 2;
                ////        }
                ////        else
                ////        {
                ////            if ((el.n1.sides[1].Count > el.n1.sides[0].Count) && (el.n2.sides[2].Count > el.n2.sides[3].Count))
                ////            {
                ////                el.n1.sides[0].Add(el.n2);
                ////                el.n2.sides[3].Add(el.n1);
                ////                el.h = 1;
                ////            }
                ////        }
                ////    }
                ////    if (el.n2.gridY > el.n1.gridY)
                ////    {
                ////        interposition = 2;
                ////        if ((el.n1.sides[1].Count <= el.n1.sides[2].Count) || (el.n2.sides[0].Count <= el.n2.sides[3].Count))
                ////        {
                ////            el.n1.sides[1].Add(el.n2);
                ////            el.n2.sides[0].Add(el.n1);
                ////            el.h = 1;
                ////        }
                ////        else
                ////        {
                ////            if ((el.n1.sides[2].Count < el.n1.sides[1].Count) && (el.n2.sides[3].Count < el.n2.sides[0].Count))
                ////            {
                ////                el.n1.sides[2].Add(el.n2);
                ////                el.n2.sides[3].Add(el.n1);
                ////                el.h = 2;
                ////            }
                ////        }
                ////    }
                ////}

                ////if (el.n1.gridX > el.n2.gridX)
                ////{
                ////    if (el.n1.gridY < el.n2.gridY)
                ////    {
                ////        interposition = 3;
                ////        if ((el.n2.sides[0].Count <= el.n2.sides[1].Count) || (el.n1.sides[3].Count <= el.n1.sides[2].Count))
                ////        {
                ////            el.n2.sides[0].Add(el.n1);
                ////            el.n1.sides[3].Add(el.n2);
                ////            el.h = 1;
                ////        }
                ////        else
                ////        {
                ////            if ((el.n2.sides[0].Count > el.n2.sides[1].Count) && (el.n1.sides[3].Count > el.n1.sides[2].Count))
                ////            {
                ////                el.n2.sides[1].Add(el.n1);
                ////                el.n1.sides[2].Add(el.n2);
                ////                el.h = 2;
                ////            }
                ////        }
                ////    }
                ////    if (el.n2.gridY < el.n1.gridY)
                ////    {
                ////        interposition = 4;
                ////        if ((el.n2.sides[1].Count <= el.n2.sides[2].Count) || (el.n1.sides[0].Count <= el.n1.sides[3].Count))
                ////        {
                ////            el.n2.sides[1].Add(el.n1);
                ////            el.n1.sides[0].Add(el.n2);
                ////            el.h = 1;
                ////        }
                ////        else
                ////        {
                ////            if ((el.n2.sides[2].Count < el.n2.sides[1].Count) && (el.n1.sides[3].Count < el.n1.sides[0].Count))
                ////            {
                ////                el.n2.sides[2].Add(el.n1);
                ////                el.n1.sides[3].Add(el.n2);
                ////                el.h = 2;
                ////            }
                ////        }
                ////    }
                ////}
                //////////////////
                /////////////
                ////
                ///
                if (el.n1.gridX < el.n2.gridX)
                {
                    if (el.n1.gridY > el.n2.gridY)
                    {
                        interposition = 1;
                        int cur = routeSel(el.n1, el.n2, interposition, recWidt, recHeight);

                        if (cur == 2)
                        {
                            el.n1.sides[1].Add(el.n2);
                            el.n2.sides[2].Add(el.n1);
                            el.h = 2;
                        }
                        else
                        {
                            if (cur == 1)
                            {
                                el.n1.sides[0].Add(el.n2);
                                el.n2.sides[3].Add(el.n1);
                                el.h = 1;
                            }
                        }
                    }
                    if (el.n2.gridY > el.n1.gridY)
                    {
                        interposition = 2;
                        int cur = routeSel(el.n1, el.n2, interposition, recWidt, recHeight);
                        if (cur == 1)
                        {
                            el.n1.sides[1].Add(el.n2);
                            el.n2.sides[0].Add(el.n1);
                            el.h = 1;
                        }
                        else
                        {
                            if (cur == 2)
                            {
                                el.n1.sides[2].Add(el.n2);
                                el.n2.sides[3].Add(el.n1);
                                el.h = 2;
                            }
                        }
                    }
                }

                if (el.n1.gridX > el.n2.gridX)
                {
                    if (el.n1.gridY < el.n2.gridY)
                    {
                        interposition = 3;
                        int cur = routeSel(el.n1, el.n2, interposition, recWidt, recHeight);
                        if (cur == 1)
                        {
                            el.n2.sides[0].Add(el.n1);
                            el.n1.sides[3].Add(el.n2);
                            el.h = 1;
                        }
                        else
                        {
                            if (cur == 2)
                            {
                                el.n2.sides[1].Add(el.n1);
                                el.n1.sides[2].Add(el.n2);
                                el.h = 2;
                            }
                        }
                    }
                    if (el.n2.gridY < el.n1.gridY)
                    {
                        interposition = 4;
                        int cur = routeSel(el.n1, el.n2, interposition, recWidt, recHeight);
                        if (cur == 1)
                        {
                            el.n2.sides[1].Add(el.n1);
                            el.n1.sides[0].Add(el.n2);
                            el.h = 1;
                        }
                        else
                        {
                            if (cur == 2)
                            {
                                el.n2.sides[2].Add(el.n1);
                                el.n1.sides[3].Add(el.n2);
                                el.h = 2;
                            }
                        }
                    }
                }
                ///
            }

            // записываем ребрам координаты портов
            double portDist = 0;
            double k = 0;
            foreach (mixSet.Node m in mixSet.Graph.hashNodes.Values)
            {               
                identifier = 0;
                //////////////////////////////////////////////////////////////////////1////////////////////////////////////////////////////
                if (m.sides[1].Count != 0)
                {
                    if (m.ID == "КЛ")
                    {
                        int kgh = 0;
                    }
                    List<Node> topVert = new List<Node>();
                    List<Node> lowerVert = new List<Node>();
                    List<Node> sameLevelVert = new List<Node>();
                    for (int i = 0; i < m.sides[1].Count; i++)
                    {
                        if (m.gridY > m.sides[1][i].gridY) { topVert.Add(m.sides[1][i]); }
                        if (m.gridY < m.sides[1][i].gridY) { lowerVert.Add(m.sides[1][i]); }
                        if (m.gridY == m.sides[1][i].gridY) { sameLevelVert.Add(m.sides[1][i]); }
                    }
                    if (topVert.Count != 0) { topVert = topVert.OrderBy(n => n.gridX).ToList(); }
                    if (lowerVert.Count != 0) { lowerVert = lowerVert.OrderBy(n => n.gridX).ToList(); }
                    if (sameLevelVert.Count != 0) { sameLevelVert = sameLevelVert.OrderBy(n => n.gridX).ToList(); }
                    ///////////////////////////////////
                    if (straightEdges(1, m) == 0)
                    {
                        identifier = 1;
                        portDist = recHeight / (m.sides[1].Count + 1);
                        k = portDist;
                        for (int p = 0; p < topVert.Count; p++)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridX + recWidt);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridY + k); //добавляем у координату

                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(topVert[p].ID + m.ID))
                            {
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridY + k);
                            }
                            k += portDist;
                        }
                        k = portDist;
                        for (int p = 0; p < lowerVert.Count; p++)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridX + recWidt);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridY + recHeight - k); //добавляем у координату

                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[p].ID + m.ID))
                            {
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);
                            }
                            k += portDist;
                        }
                    }
                    else      //если есть прямые ребра
                    {
                        for (int d = 0; d < 1; d++)
                        {
                            if (sameLevelVert.Count > 1)
                            {
                                int hjk = 0;
                            }
                            List<Node> topVertN = new List<Node>();
                            List<Node> lowerVertN = new List<Node>();
                            List<Node> sameLevelVertN = new List<Node>();
                            for (int h = 0; h < sameLevelVert[d].sides[3].Count; h++)
                            {
                                if (sameLevelVert[d].gridY > sameLevelVert[d].sides[3][h].gridY) { topVertN.Add(sameLevelVert[d].sides[3][h]); }
                                if (sameLevelVert[d].gridY < sameLevelVert[d].sides[3][h].gridY) { lowerVertN.Add(sameLevelVert[d].sides[3][h]); }
                                if (sameLevelVert[d].gridY == sameLevelVert[d].sides[3][h].gridY) { sameLevelVertN.Add(sameLevelVert[d].sides[3][h]); }

                            }
                            if (topVertN.Count != 0) { topVertN = topVertN.OrderBy(n => n.gridX).ToList(); }
                            if (lowerVertN.Count != 0) { lowerVertN = lowerVertN.OrderBy(n => n.gridX).ToList(); }
                            if (sameLevelVertN.Count != 0) { sameLevelVertN = sameLevelVertN.OrderBy(n => n.gridX).ToList(); }
                            
                            /////////////////////////////////////////////////
                            if ((m.sides[1].Count == 1) && (sameLevelVert[d].sides[3].Count == 1))
                            { //если каждой стороне принадлежит по 1 вершине
                                identifier = 1;

                                if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d].ID))
                                {
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridX + recWidt);//добавляем х координату
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridY + recHeight / 2); //тип посередине

                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight / 2); // посередине
                                }
                                if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + m.ID))
                                {
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridX + recWidt);//добавляем х координату
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridY + recHeight / 2); //тип посередине

                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridX);
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight / 2); // посередине
                                }
                            }
                            else
                            {
                                /////////////////////////////////////////////////
                                if (m.sides[1].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recHeight / (sameLevelVert[d].sides[3].Count + 1);
                                    k = portDist;
                                    for (int i = topVertN.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    for (int i = 0; i < sameLevelVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + sameLevelVertN[i].ID)) 
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                            // те sameLevelVertN[1] это и есть m
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVertN[i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVertN[i].ID + sameLevelVert[d].ID)) 
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVertN[i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    for (int i = 0; i < lowerVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                }
                                ////////////////////////////////////////////////////////////
                                if (sameLevelVert[d].sides[3].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recHeight / (m.sides[1].Count + 1);
                                    k = portDist;
                                    for (int i = 0; i < topVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    for (int i = 0; i < sameLevelVert.Count; i++) 
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    // }
                                    for (int i = lowerVert.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + k);

                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            /////////////////////// ////////////////////////                                   
                            if ((topVert.Count == 0) && (topVertN.Count == 0) && (identifier == 0))
                            {
                                identifier = 1;
                                if (m.sides[1].Count >= sameLevelVert[d].sides[3].Count)
                                {
                                    portDist = recHeight / (m.sides[1].Count + 1);
                                    k = portDist;
                                    for (int i = 0; i < lowerVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    /* */
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist; 
                                    portDist = (recHeight - k*sameLevelVert.Count) / sameLevelVert[d].sides[3].Count; 
                                    k = portDist;
                                    for (int i = lowerVertN.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recHeight / (sameLevelVert[d].sides[3].Count + 1);
                                    k = portDist;
                                    for (int i = lowerVertN.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    //размещаем прямое ребро
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist; ;
                                    portDist = (recHeight - k*sameLevelVert.Count) / m.sides[1].Count;
                                    k = portDist;
                                    for (int i = 0; i < lowerVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            ////////////////////////////////////////////////
                            if ((lowerVert.Count == 0) && (lowerVertN.Count == 0) && (identifier == 0))
                            {
                                identifier = 1;
                                if (m.sides[1].Count >= sameLevelVert[d].sides[3].Count)
                                {
                                    portDist = recHeight / (m.sides[1].Count + 1);
                                    k = portDist;
                                    for (int i = 0; i < topVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recHeight - k*sameLevelVert.Count) / sameLevelVert[d].sides[3].Count;
                                    k = portDist;
                                    for (int i = topVertN.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recHeight / (sameLevelVert[d].sides[3].Count + 1);
                                    k = portDist;
                                    for (int i = topVertN.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(sameLevelVert[d].gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist; ;
                                    portDist = (recHeight - k*sameLevelVert.Count) / m.sides[1].Count;
                                    k = portDist;
                                    for (int i = 0; i < topVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            /////////////////////// ////////////////////////////////
                            if (identifier == 0) // если не зашли никуда раньше
                            {
                                identifier = 1;
                                /**/////
                                for (int i = 0; i < sameLevelVert.Count; i++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridX + recWidt);//добавляем х координату 
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridY + recHeight / 2); //тип посередине

                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX);
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight / 2);
                                    }// посередине
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);//добавляем х координату
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight / 2); //тип посередине

                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(m.gridX);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(m.gridY + recHeight / 2);
                                    }
                                    k += portDist;
                                }
                                portDist = (recHeight / 2) / (topVert.Count + 1);
                                k = portDist;
                                for (int i = 0; i < topVert.Count; i++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                    }
                                    k += portDist;
                                }
                                portDist = (recHeight / 2) / (lowerVert.Count + 1);
                                k = portDist;
                                for (int i = 0; i < lowerVert.Count; i++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + recHeight - k); 
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k); 
                                    }
                                    k += portDist;
                                }
                                portDist = (recHeight / 2) / (topVertN.Count + 1);
                                k = portDist;
                                for (int i = topVertN.Count - 1; i > -1; i--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(m.gridY + k);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(m.gridY + k);
                                    }
                                    k += portDist;
                                }
                                portDist = (recHeight / 2) / (lowerVertN.Count + 1);
                                k = portDist;
                                for (int i = lowerVertN.Count - 1; i > -1; i--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(m.gridY + recHeight - k);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(m.gridX + recWidt);
                                        mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(m.gridY + recHeight - k);
                                    }
                                    k += portDist;
                                }
                            }
                        }
                    }
                }
                ///////////////////////////////////////////////////////////2//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                if (m.sides[2].Count != 0)
                {
                    if (m.ID == "УУ")
                    {
                        int kgрh = 0;
                    }
                    identifier = 0;
                    List<Node> topVert = new List<Node>();
                    List<Node> lowerVert = new List<Node>();
                    List<Node> sameLevelVert = new List<Node>();
                    for (int i = 0; i < m.sides[2].Count; i++)
                    {
                        if (m.gridX > m.sides[2][i].gridX) { topVert.Add(m.sides[2][i]); } //слева
                        if (m.gridX < m.sides[2][i].gridX) { lowerVert.Add(m.sides[2][i]); } // справа
                        if (m.gridX == m.sides[2][i].gridX) { sameLevelVert.Add(m.sides[2][i]); }

                    }
                    if (topVert.Count != 0) { topVert = topVert.OrderBy(n => n.gridY).ToList(); }
                    if (lowerVert.Count != 0) { lowerVert = lowerVert.OrderBy(n => n.gridY).ToList(); }
                    if (sameLevelVert.Count != 0) { sameLevelVert = sameLevelVert.OrderBy(n => n.gridY).ToList(); }
                    if (straightEdges(2, m) == 0)
                    {
                        portDist = recWidt / (m.sides[2].Count + 1);
                        k = portDist;
                        for (int p = 0; p < topVert.Count; p++)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridX + k);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridY + recHeight);
                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(topVert[p].ID + m.ID))
                            {
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridX + k);//добавляем х координату
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridY + recHeight); //
                            }
                            k += portDist;
                        }
                        k = portDist;

                        for (int p = 0; p < lowerVert.Count; p++)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridX + recWidt - k);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridY + recHeight); //добавляем у координату

                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[p].ID + m.ID))
                            {
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                            }
                            k += portDist;
                        }
                    }
                    else //есть прямые ребра
                    {
                        for (int d = 0; d < 1; d++) 
                        {

                            if (sameLevelVert.Count > 1)
                            {
                                int hjk = 0;
                            }
                            List<Node> topVertN = new List<Node>();
                            List<Node> lowerVertN = new List<Node>();
                            List<Node> sameLevelVertN = new List<Node>();
                            for (int h = 0; h < sameLevelVert[d].sides[0].Count; h++)
                            {
                                if (sameLevelVert[d].gridX > sameLevelVert[d].sides[0][h].gridX) { topVertN.Add(sameLevelVert[d].sides[0][h]); }
                                if (sameLevelVert[d].gridX < sameLevelVert[d].sides[0][h].gridX) { lowerVertN.Add(sameLevelVert[d].sides[0][h]); }
                                if (sameLevelVert[d].gridX == sameLevelVert[d].sides[0][h].gridX) { sameLevelVertN.Add(sameLevelVert[d].sides[0][h]); }
                            }
                            if (topVertN.Count != 0) { topVertN = topVertN.OrderBy(n => n.gridY).ToList(); }
                            if (lowerVertN.Count != 0) { lowerVertN = lowerVertN.OrderBy(n => n.gridY).ToList(); }
                            if (sameLevelVertN.Count != 0) { sameLevelVertN = sameLevelVertN.OrderBy(n => n.gridY).ToList(); }
                            ////////////////////////////////////////////
                            if (((m.sides[2].Count == 1) && (sameLevelVert[d].sides[0].Count == 1)))
                            {
                                identifier = 1;
                                if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d].ID))
                                {
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridX + recWidt / 2);//добавляем х координату
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridY + recHeight); //тип посередине

                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt / 2);
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY); // посередине
                                }
                                if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + m.ID))
                                {
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridX + recWidt / 2);//добавляем х координату
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridY + recHeight); //тип посередине

                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt / 2);
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridY); // посередине
                                }
                            }
                            else
                            {
                                //////////////////////////////////////////////////
                                if (m.sides[2].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recWidt / (sameLevelVert[d].sides[0].Count + 1);
                                    k = portDist;
                                    for (int j = topVertN.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        k += portDist;
                                    }
                                    //прямое ребро 
                                    /**/
                                    for (int i = 0; i < sameLevelVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + sameLevelVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY);

                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVert[i].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVertN[d].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);

                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVertN[d].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    
                                    for (int j = 0; j < lowerVertN.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        k += portDist;
                                    }
                                }
                                //////////////////////////////////////////////// 
                                if (sameLevelVert[d].sides[0].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recWidt / (m.sides[2].Count + 1);
                                    k = portDist;
                                    for (int j = 0; j < topVert.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    //размещаем прямые ребра
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY);
                                        }
                                        k += portDist;
                                    }
                                    for (int j = lowerVert.Count - 1; j > -1; j--)
                                    {
                                        
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY + recHeight);

                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            /////////////////////////////////
                            if (topVert.Count == 0 && topVertN.Count == 0 && identifier == 0)
                            {
                                identifier = 1;
                                if (m.sides[2].Count >= sameLevelVert[d].sides[0].Count)
                                {
                                    portDist = recWidt / (m.sides[2].Count + 1);
                                    k = portDist;
                                    for (int j = 0; j < lowerVert.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recWidt - k*sameLevelVert.Count) / sameLevelVert[d].sides[0].Count;
                                    k = portDist;
                                    for (int j = lowerVertN.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recWidt / (sameLevelVert[d].sides[0].Count + 1);
                                    k = portDist;
                                    for (int j = lowerVertN.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        k += portDist;
                                    }
                                    //размещаем прямое ребро
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recWidt - k*sameLevelVert.Count) / m.sides[2].Count;
                                    k = portDist;
                                    for (int j = 0; j < lowerVert.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            //////////////////////////////////// 
                            if ((lowerVert.Count == 0) && (lowerVertN.Count == 0) && (identifier == 0))
                            {
                                identifier = 1;
                                if (m.sides[2].Count >= sameLevelVert[d].sides[0].Count)
                                {
                                    portDist = recWidt / (m.sides[2].Count + 1);
                                    k = portDist;
                                    for (int j = 0; j < topVert.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recWidt - k*sameLevelVert.Count) / sameLevelVert[d].sides[0].Count;
                                    k = portDist;
                                    for (int j = topVertN.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recWidt / (sameLevelVert[d].sides[0].Count + 1);
                                    k = portDist;
                                    for (int j = topVertN.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recWidt - k*sameLevelVert.Count) / m.sides[2].Count;
                                    k = portDist;
                                    for (int j = 0; j < topVert.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            /////////////////////////////////////////////////////
                            if (identifier == 0)
                            {
                                identifier = 1;
                              /**/  for (int i = 0; i < sameLevelVert.Count; i++)
                              {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt / 2);//добавляем х координату
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight); //тип посередине

                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt / 2);
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY);
                                    }// посередине
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt / 2);//добавляем х координату
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight); //тип посередине

                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt / 2);
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY);
                                    }
                                    k += portDist;
                              }

                                portDist = (recWidt / 2) / (topVert.Count + 1);
                                k = portDist;
                                for (int j = 0; j < topVert.Count; j++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                        mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY + recHeight);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                        mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                    }
                                    k += portDist;
                                }
                                portDist = (recWidt / 2) / (lowerVert.Count + 1);
                                k = portDist;
                                for (int j = 0; j < lowerVert.Count; j++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY + recHeight);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY + recHeight);
                                    }
                                    k += portDist;
                                }
                                portDist = (recWidt / 2) / (lowerVertN.Count + 1);
                                k = portDist;
                                for (int j = lowerVertN.Count - 1; j > -1; j--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                    }
                                    k += portDist;
                                }
                                portDist = (recWidt / 2) / (topVertN.Count + 1);
                                k = portDist;
                                for (int j = topVertN.Count - 1; j > -1; j--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                        mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                    }
                                    k += portDist;
                                }
                            }
                        }
                    }
                }
                /////////////////////////////////////////3//////////////////////////////////////////////////////////////////////////////////////
                if (m.sides[3].Count != 0)
                {
                    if (m.ID == "ЗД")
                    {
                        int rrrr = 6;
                    }
                    identifier = 0;
                    List<Node> topVert = new List<Node>();
                    List<Node> lowerVert = new List<Node>();
                    List<Node> sameLevelVert = new List<Node>();
                    for (int i = 0; i < m.sides[3].Count; i++)
                    {
                        if (m.gridY > m.sides[3][i].gridY) { topVert.Add(m.sides[3][i]); } //слева
                        if (m.gridY < m.sides[3][i].gridY) { lowerVert.Add(m.sides[3][i]); } // справа
                        if (m.gridY == m.sides[3][i].gridY) { sameLevelVert.Add(m.sides[3][i]); }
                    }
                    if (topVert.Count != 0) { topVert = topVert.OrderBy(n => n.gridX).ToList(); }
                    if (lowerVert.Count != 0) { lowerVert = lowerVert.OrderBy(n => n.gridX).ToList(); }
                    if (sameLevelVert.Count != 0) { sameLevelVert = sameLevelVert.OrderBy(n => n.gridX).ToList(); }
                    /////////////////////////////////////////////////////////
                    if (straightEdges(3, m) == 0)
                    {
                        identifier = 1;
                        portDist = recHeight / (m.sides[3].Count + 1);
                        k = portDist;
                        for (int p = topVert.Count - 1; p > -1; p--)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridX);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridY + k); //добавляем у координату

                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(topVert[p].ID  + m.ID))
                            {
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridX);
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridY + k);
                            }
                            k += portDist;
                        }
                        k = portDist;
                        for (int p = lowerVert.Count - 1; p > -1; p--)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridX);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridY + recHeight - k); //добавляем у координату

                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[p].ID + m.ID))
                            {
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridX);
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);
                            }
                            k += portDist;
                        }
                    }
                    else //если есть прямые ребра
                    {
                        for (int d = 0; d < 1; d++) 
                        {
                            if (sameLevelVert.Count > 1)
                            {
                                int hjk = 0;
                            }
                            List<Node> topVertN = new List<Node>();
                            List<Node> lowerVertN = new List<Node>();
                            List<Node> sameLevelVertN = new List<Node>();
                            for (int h = 0; h < sameLevelVert[d].sides[1].Count; h++)
                            {
                                if (sameLevelVert[d].gridY > sameLevelVert[d].sides[1][h].gridY) { topVertN.Add(sameLevelVert[d].sides[1][h]); }
                                if (sameLevelVert[d].gridY < sameLevelVert[d].sides[1][h].gridY) { lowerVertN.Add(sameLevelVert[d].sides[1][h]); }
                                if (sameLevelVert[d].gridY == sameLevelVert[d].sides[1][h].gridY) { sameLevelVertN.Add(sameLevelVert[d].sides[1][h]); }

                            }
                            if (topVertN.Count != 0) { topVertN = topVertN.OrderBy(n => n.gridX).ToList(); }
                            if (lowerVertN.Count != 0) { lowerVertN = lowerVertN.OrderBy(n => n.gridX).ToList(); }
                            if (sameLevelVertN.Count != 0) { sameLevelVertN = sameLevelVertN.OrderBy(n => n.gridX).ToList(); }
                            ///////////////////////////////////////////////
                            if ((m.sides[3].Count == 1) && (sameLevelVert[d].sides[1].Count == 1))
                            {
                                identifier = 1;
                                if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d].ID))
                                {
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridX);//добавляем х координату
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridY + recHeight / 2); //тип посередине

                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight / 2); // посередине
                                }
                                if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + m.ID))
                                {
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridX);//добавляем х координату
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridY + recHeight / 2); //тип посередине

                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight / 2); // посередине
                                }
                            }
                            else
                            {
                                /////////////////
                                if (m.sides[3].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recHeight / (sameLevelVert[d].sides[1].Count + 1);
                                    k = portDist;
                                    for (int i = 0; i < topVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + sameLevelVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                            
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVertN[i].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVertN[i].gridX);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                   
                                    for (int i = 0; i < lowerVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                }
                                ////////////////////////////////
                                if (sameLevelVert[d].sides[1].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recHeight / (m.sides[3].Count + 1);
                                    k = portDist;
                                    for (int i = topVert.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    for (int i = 0; i < lowerVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + k);

                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            ///////////////
                            if ((topVert.Count == 0) && (topVertN.Count == 0) && (identifier == 0))
                            {
                                identifier = 1;
                                if (m.sides[3].Count >= sameLevelVert[d].sides[1].Count)
                                {
                                    portDist = recHeight / (m.sides[3].Count + 1);
                                    k = portDist;
                                    for (int i = lowerVert.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    //k = portDist;
                                    portDist = (recHeight - k*sameLevelVert.Count) / sameLevelVert[d].sides[1].Count;
                                    k = portDist;
                                    for (int i = 0; i < lowerVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recHeight / (sameLevelVert[d].sides[1].Count + 1);
                                    k = portDist;
                                    for (int i = 0; i < lowerVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recHeight - k*sameLevelVert.Count) / m.sides[3].Count;
                                    k = portDist;
                                    for (int i = lowerVert.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + recHeight - k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            ///////////////
                            if ((lowerVert.Count == 0) && (lowerVertN.Count == 0) && (identifier == 0))
                            {
                                identifier = 1;
                                if (m.sides[3].Count >= sameLevelVert[d].sides[1].Count)
                                {
                                    portDist = recHeight / (m.sides[3].Count + 1);
                                    k = portDist;
                                    for (int i = topVert.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recHeight - k*sameLevelVert.Count) / sameLevelVert[d].sides[1].Count;
                                    k = portDist;
                                    for (int i = 0; i < topVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recHeight / (sameLevelVert[d].sides[1].Count + 1);
                                    k = portDist;
                                    for (int i = 0; i < topVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                            mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + k);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(sameLevelVert[d+i].gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + k);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(sameLevelVert[d+i].gridY + k);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recHeight - k*sameLevelVert.Count) / m.sides[3].Count;
                                    k = portDist;
                                    for (int i = topVert.Count - 1; i > -1; i--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                            mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                        }
                                        k += portDist;
                                    }
                                }
                            }
                            //////////////////////////////////////////
                            if (identifier == 0)
                            {
                                identifier = 1;
                                //////////////////////////////////////////////////////////////////////////
                                /**/
                                for (int i = 0; i < sameLevelVert.Count; i++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt);//добавляем х координату
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY + recHeight / 2); //тип посередине

                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX);
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight / 2);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt);//добавляем х координату
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight / 2); //тип посередине

                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX);
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY + recHeight / 2);
                                    }
                                    k += portDist;
                                }

                                portDist = (recHeight / 2) / (topVert.Count + 1);
                                k = portDist;
                                for (int i = topVert.Count - 1; i > -1; i--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridX);
                                        mixSet.Graph.hashEdges[m.ID + topVert[i].ID].e1Coord.Add(m.gridY + k);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVert[i].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                        mixSet.Graph.hashEdges[topVert[i].ID + m.ID].e2Coord.Add(m.gridY + k);
                                    }
                                    k += portDist;
                                }
                                portDist = (recHeight / 2) / (lowerVert.Count + 1);
                                k = portDist;
                                for (int i = lowerVert.Count - 1; i > -1; i--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridX);
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[i].ID].e1Coord.Add(m.gridY + recHeight - k);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[i].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridX);
                                        mixSet.Graph.hashEdges[lowerVert[i].ID + m.ID].e2Coord.Add(m.gridY + recHeight - k);
                                    }
                                    k += portDist;
                                }
                                portDist = (recHeight / 2) / (topVertN.Count + 1);
                                k = portDist;
                                for (int i = 0; i < topVertN.Count; i++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[i].ID].e1Coord.Add(m.gridY + k);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVertN[i].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                        mixSet.Graph.hashEdges[topVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(m.gridY + k);
                                    }
                                    k += portDist;
                                }
                                portDist = (recHeight / 2) / (lowerVertN.Count + 1);
                                k = portDist;
                                for (int i = 0; i < lowerVertN.Count; i++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[i].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[i].ID].e1Coord.Add(m.gridY + recHeight - k);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[i].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt);
                                        mixSet.Graph.hashEdges[lowerVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(m.gridY + recHeight - k);
                                    }
                                    k += portDist;
                                }
                            }
                        }

                    }



                }
                ////////////////////////////////////////////////0/////////////////////////////////////////////////////////////////////////////
                if (m.sides[0].Count != 0)
                {
                    if (m.ID == "ВГ")
                    {
                        int kgрh = 0;
                    }
                    identifier = 0;
                    List<Node> topVert = new List<Node>();
                    List<Node> lowerVert = new List<Node>();
                    List<Node> sameLevelVert = new List<Node>();
                    for (int i = 0; i < m.sides[0].Count; i++)
                    {
                        if (m.gridX > m.sides[0][i].gridX) { topVert.Add(m.sides[0][i]); } //слева
                        if (m.gridX < m.sides[0][i].gridX) { lowerVert.Add(m.sides[0][i]); } // справа
                        if (m.gridX == m.sides[0][i].gridX) { sameLevelVert.Add(m.sides[0][i]); }
                    }
                    if (topVert.Count != 0) { topVert = topVert.OrderBy(n => n.gridY).ToList(); }                 
                    if (lowerVert.Count != 0) { lowerVert = lowerVert.OrderBy(n => n.gridY).ToList(); }
                    if (sameLevelVert.Count != 0) { sameLevelVert = sameLevelVert.OrderBy(n => n.gridY).ToList(); }
                    if (straightEdges(0, m) == 0)
                    {
                        portDist = recWidt / (m.sides[0].Count + 1);
                        k = portDist;
                        for (int p = topVert.Count - 1; p > -1; p--)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridX + k);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + topVert[p].ID].e1Coord.Add(m.gridY);
                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(topVert[p].ID + m.ID))
                            {
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridX + k);//добавляем х координату 
                                mixSet.Graph.hashEdges[topVert[p].ID + m.ID].e2Coord.Add(m.gridY);
                            }
                            k += portDist;
                        }
                        k = portDist;
                        for (int p = lowerVert.Count - 1; p > -1; p--)
                        {
                            if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[p].ID))
                            {
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridX + recWidt - k);//добавляем х координату
                                mixSet.Graph.hashEdges[m.ID + lowerVert[p].ID].e1Coord.Add(m.gridY); //добавляем у координату

                            }
                            if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[p].ID + m.ID))
                            {
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                mixSet.Graph.hashEdges[lowerVert[p].ID + m.ID].e2Coord.Add(m.gridY);
                            }
                            k += portDist;
                        }

                    }
                    else
                    {
                        for (int d = 0; d < 1; d++) 
                        {
                             if (sameLevelVert.Count > 1)
                             {
                                int hjk = 0;
                             }
                            List<Node> topVertN = new List<Node>();
                            List<Node> lowerVertN = new List<Node>();
                            List<Node> sameLevelVertN = new List<Node>();
                            for (int h = 0; h < sameLevelVert[d].sides[2].Count; h++)
                            {
                                if (sameLevelVert[d].gridX > sameLevelVert[d].sides[2][h].gridX) { topVertN.Add(sameLevelVert[d].sides[2][h]); }
                                if (sameLevelVert[d].gridX < sameLevelVert[d].sides[2][h].gridX) { lowerVertN.Add(sameLevelVert[d].sides[2][h]); }
                                if (sameLevelVert[d].gridX == sameLevelVert[d].sides[2][h].gridX) { sameLevelVertN.Add(sameLevelVert[d].sides[2][h]); }
                            }
                            if (topVertN.Count != 0) { topVertN = topVertN.OrderBy(n => n.gridY).ToList(); }
                            if (lowerVertN.Count != 0) { lowerVertN = lowerVertN.OrderBy(n => n.gridY).ToList(); }
                            if (sameLevelVertN.Count != 0) { sameLevelVertN = sameLevelVertN.OrderBy(n => n.gridY).ToList(); }
                            /////////////////////////////////
                            if ((m.sides[0].Count == 1) && (sameLevelVert[d].sides[2].Count == 1))
                            {
                                identifier = 1;
                                if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d].ID))
                                {
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridX + recWidt / 2);//добавляем х координату
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e1Coord.Add(m.gridY); //тип посередине

                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt / 2);
                                    mixSet.Graph.hashEdges[m.ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight); // посередине
                                }
                                if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + m.ID))
                                {
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridX + recWidt / 2);//добавляем х координату
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e1Coord.Add(m.gridY); //тип посередине

                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt / 2);
                                    mixSet.Graph.hashEdges[sameLevelVert[d].ID + m.ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight); // посередине
                                }
                            }
                            else
                            {
                                ////////////////////////////////////
                                if (m.sides[0].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recWidt / (sameLevelVert[d].sides[2].Count + 1);
                                    k = portDist;
                                    for (int j = 0; j < topVertN.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVertN.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + sameLevelVertN[i].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                            
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + sameLevelVertN[i].ID].e2Coord.Add(sameLevelVertN[i].gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVertN[i].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);

                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVertN[i].ID + sameLevelVert[d].ID].e1Coord.Add(sameLevelVertN[i].gridY);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    for (int j = 0; j < lowerVertN.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        if ((mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID)))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY);
                                        }
                                        k += portDist;
                                    }

                                }
                                //////////////////////////////////////////
                                if (sameLevelVert[d].sides[2].Count == 1 && identifier == 0)
                                {
                                    identifier = 1;
                                    portDist = recWidt / (m.sides[0].Count + 1);
                                    k = portDist;
                                    for (int j = topVert.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    for (int j = 0; j < lowerVert.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY);

                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        k += portDist; //добавить это выше
                                    }
                                }
                            }
                            if (topVert.Count == 0 && topVertN.Count == 0 && identifier == 0)
                            {
                                identifier = 1;
                                if (m.sides[0].Count >= sameLevelVert[d].sides[2].Count)
                                {
                                    portDist = recWidt / (m.sides[0].Count + 1);
                                    k = portDist;
                                    for (int j = lowerVert.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recWidt - k*sameLevelVert.Count) / sameLevelVert[d].sides[2].Count;
                                    k = portDist;
                                    for (int j = 0; j < lowerVertN.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recWidt / (sameLevelVert[d].sides[2].Count + 1);
                                    k = portDist;
                                    for (int j = 0; j < lowerVertN.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist; 
                                    portDist = (recWidt - k*sameLevelVert.Count) / m.sides[0].Count;
                                    k = portDist;
                                    for (int j = lowerVert.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                            mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        k += portDist; 
                                    }
                                }
                            }
                            //////////////////////////////////////////////////////
                            if ((lowerVert.Count == 0) && (lowerVertN.Count == 0) && (identifier == 0))
                            {
                                identifier = 1;
                                if (m.sides[0].Count >= sameLevelVert[d].sides[2].Count)
                                {
                                    portDist = recWidt / (m.sides[0].Count + 1);
                                    k = portDist;
                                    for (int j = topVert.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recWidt - k*sameLevelVert.Count) / sameLevelVert[d].sides[2].Count;
                                    k = portDist;
                                    for (int j = 0; j < topVertN.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                }
                                else
                                {
                                    portDist = recWidt / (sameLevelVert[d].sides[2].Count + 1);
                                    k = portDist;
                                    for (int j = 0; j < topVertN.Count; j++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID))
                                        {
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                            mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);
                                        }
                                        k += portDist;
                                    }
                                    /**/
                                    for (int i = 0; i < sameLevelVert.Count; i++)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight);

                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight);

                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(sameLevelVert[d+i].gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY);
                                        }
                                        k += portDist;
                                    }
                                    k = portDist;
                                    portDist = (recWidt - k*sameLevelVert.Count) / m.sides[0].Count;
                                    k = portDist;
                                    for (int j = topVertN.Count - 1; j > -1; j--)
                                    {
                                        if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                        {
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY);
                                        }
                                        if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                        {
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                            mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                        }
                                        k += portDist;
                                    }

                                }
                            }
                            /////////////////
                            if (identifier == 0)
                            {
                                identifier = 1;
                                /**/
                                for (int i = 0; i < sameLevelVert.Count; i++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + sameLevelVert[d+i].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridX + recWidt / 2);//добавляем х координату
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e1Coord.Add(m.gridY); //тип посередине

                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridX + recWidt / 2);
                                        mixSet.Graph.hashEdges[m.ID + sameLevelVert[d+i].ID].e2Coord.Add(sameLevelVert[d+i].gridY + recHeight); // посередине
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d+i].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridX + recWidt / 2);//добавляем х координату
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e1Coord.Add(sameLevelVert[d+i].gridY + recHeight); //тип посередине

                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridX + recWidt / 2);
                                        mixSet.Graph.hashEdges[sameLevelVert[d+i].ID + m.ID].e2Coord.Add(m.gridY); // посередине 
                                    }
                                    k += portDist;
                                }
                                portDist = (recWidt / 2) / (topVert.Count + 1);
                                k = portDist;
                                for (int j = topVert.Count - 1; j > -1; j--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + topVert[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridX + k);
                                        mixSet.Graph.hashEdges[m.ID + topVert[j].ID].e1Coord.Add(m.gridY);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVert[j].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridX + k);
                                        mixSet.Graph.hashEdges[topVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                    }
                                    k += portDist;
                                }
                                portDist = (recWidt / 2) / (lowerVert.Count + 1);
                                k = portDist;
                                for (int j = lowerVert.Count - 1; j > -1; j--)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(m.ID + lowerVert[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[m.ID + lowerVert[j].ID].e1Coord.Add(m.gridY);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVert[j].ID + m.ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[lowerVert[j].ID + m.ID].e2Coord.Add(m.gridY);
                                    }
                                    k += portDist;
                                }
                                portDist = (recWidt / 2) / (topVertN.Count + 1);
                                k = portDist;
                                for (int j = 0; j < lowerVertN.Count; j++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + lowerVertN[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + lowerVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(lowerVertN[j].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + recWidt - k);
                                        mixSet.Graph.hashEdges[lowerVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);
                                    }
                                    k += portDist; 
                                }
                                portDist = (recWidt / 2) / (topVertN.Count + 1);
                                k = portDist;
                                for (int j = 0; j < topVertN.Count; j++)
                                {
                                    if (mixSet.Graph.hashEdges.ContainsKey(sameLevelVert[d].ID + topVertN[j].ID))
                                    {
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridX + k);
                                        mixSet.Graph.hashEdges[sameLevelVert[d].ID + topVertN[j].ID].e1Coord.Add(sameLevelVert[d].gridY + recHeight);
                                    }
                                    if (mixSet.Graph.hashEdges.ContainsKey(topVertN[j].ID + sameLevelVert[d].ID))
                                    {
                                        mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridX + k);
                                        mixSet.Graph.hashEdges[topVertN[j].ID + sameLevelVert[d].ID].e2Coord.Add(sameLevelVert[d].gridY + recHeight);
                                    }
                                    k += portDist;
                                }
                            }
                        }
                    }

                }
            }

            //координаты излома
            foreach (mixSet.Edge el in mixSet.Graph.hashEdges.Values)
            {
                if (el.n1.ID == "ВР" || el.n2.ID == "УХ")
                {
                    int rrrпрr = 6;
                }
                if (el.h != 0)
                {
                    if (el.n2.gridX > el.n1.gridX)
                    {
                        if (el.n1.gridY > el.n2.gridY)
                        {
                            if (el.h == 1) { el.bendCoord[0] = el.e1Coord[0]; el.bendCoord[1] = el.e2Coord[1]; }
                            if (el.h == 2) { el.bendCoord[0] = el.e2Coord[0]; el.bendCoord[1] = el.e1Coord[1]; } 
                        }
                        if (el.n1.gridY < el.n2.gridY)
                        {
                            if (el.h == 1) { el.bendCoord[0] = el.e2Coord[0]; el.bendCoord[1] = el.e1Coord[1]; }
                            if (el.h == 2) { el.bendCoord[0] = el.e1Coord[0]; el.bendCoord[1] = el.e2Coord[1]; }
                        }

                    }
                    if (el.n1.gridX > el.n2.gridX)
                    {
                        if (el.n1.gridY < el.n2.gridY)
                        {
                            if (el.h == 1) { el.bendCoord[0] = el.e2Coord[0]; el.bendCoord[1] = el.e1Coord[1]; }
                            if (el.h == 2) { el.bendCoord[0] = el.e1Coord[0]; el.bendCoord[1] = el.e2Coord[1]; }
                        }
                        if (el.n1.gridY > el.n2.gridY)
                        {
                            if (el.h == 1) { el.bendCoord[0] = el.e1Coord[0]; el.bendCoord[1] = el.e2Coord[1]; }
                            if (el.h == 2) { el.bendCoord[0] = el.e2Coord[0]; el.bendCoord[1] = el.e1Coord[1]; }
                        }
                    }
                }
            }
        }
    }
}
