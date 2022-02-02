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

namespace MixNetSet
{
    class stackAlg
    {

        private Graph graph;
        public stackAlg(Graph g)
        {
            this.graph = g;
            this.setAlg();
        }

        private static int Mod(int a, int b)
        {
            int j = a;
            if (a > b || a < 0)
            {
                if (a < 0) {
                    if (-a<=b) {j = b + a;}
                    else {j = b - (-a - b * (-a / b));}
                }
                else { j = a - b * (a / b); }
            } return j;
        }

        public double nodeDISTANCE (mixSet.Node n1, mixSet.Node n2)
        {
             return Math.Sqrt(Math.Pow(n1.gridX - n2.gridX,2) + Math.Pow(n1.gridY - n2.gridY,2));
        }
        public static Dictionary<string, int> pairNodes = new Dictionary<string, int>();

        private Double angleBetween(double x, double y, int p1, int p2)
        {
            Vector vector1 = new Vector(x, y);
            Vector vector2 = new Vector(p1, p2);
            Double angleBetween;

            angleBetween = Vector.AngleBetween(vector1, vector2);

            return angleBetween;
        }
  
        public void setAlg()
        {            
            double W = 25.0; //размеры области 
            double L = 0.16; // не используется
            double area = W*L;
            double currForceX;
            double currForceY;
            double nodeDist;
            int itterNUM =10; 
            if (mixSet.Graph.hashNodes.Count() == 0)
                return;
            //double idEdLenght = Math.Sqrt(area / mixSet.Graph.hashNodes.Count());
            double idEdLenght = 0.01; //0.0001 для районов
            //double idEdLenght = 0.0005; //0.01 для городов 0.0005 Метро 
            // силы отталкивания
            for (int l = 1; l < itterNUM; l++)
            {
                foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
                {
                   n.nodeDispX = 0;
                   foreach (mixSet.Node m in mixSet.Graph.hashNodes.Values)
                    {
                        if (!pairNodes.ContainsKey(n.ID + m.ID) && (!pairNodes.ContainsKey(m.ID + n.ID)))
                        {
                            pairNodes.Add(n.ID + m.ID, 1);

                            if (n.ID != m.ID)
                            {
                                nodeDist = nodeDISTANCE(n, m);
                                currForceX = Math.Abs((Math.Pow(idEdLenght, 2) / Math.Pow(nodeDist, 2)) * (m.gridX - n.gridX));
                                currForceY = Math.Abs((Math.Pow(idEdLenght, 2) / Math.Pow(nodeDist, 2)) * (m.gridY - n.gridY));
                                if (n.gridX <= m.gridX) {
                                    if(n.gridY >= m.gridY)
                                    {                      
                                        n.repForceX -= currForceX;
                                        m.repForceX += currForceX;
                                        n.repForceY += currForceY;
                                        m.repForceY -= currForceY;

                                    }
                                    if(n.gridY <= m.gridY)
                                    {
                                        n.repForceX -= currForceX;
                                        m.repForceX += currForceX;
                                        n.repForceY -= currForceY;
                                        m.repForceY += currForceY;
                                    }

                                }
                                if (n.gridX >= m.gridX) {
                                    if(m.gridY >= n.gridY)
                                    {
                                        n.repForceX += currForceX;
                                        m.repForceX -= currForceX;
                                        n.repForceY -= currForceY;
                                        m.repForceY += currForceY;
                                    }
                                    if (m.gridY <= n.gridY)
                                    {
                                        n.repForceX += currForceX;
                                        m.repForceX -= currForceX;
                                        n.repForceY += currForceY;
                                        m.repForceY -= currForceY;
                                    }
                                }
                            }
                        }
                   }
                }

                // силы притяжения
                foreach (mixSet.Edge e in mixSet.Graph.hashEdges.Values)
                {
                    nodeDist = nodeDISTANCE(e.n1, e.n2);
                    currForceX = Math.Abs((nodeDist / idEdLenght) * (e.n1.gridX - e.n2.gridX));
                    currForceY = Math.Abs((nodeDist / idEdLenght) * (e.n1.gridY - e.n2.gridY));
                    if (e.n1.gridX <= e.n2.gridX)
                    {
                        if (e.n1.gridY >= e.n2.gridY)
                        {
                            e.n1.attForceX += currForceX;
                            e.n2.attForceX -= currForceX;
                            e.n1.attForceY -= currForceY;
                            e.n2.attForceY += currForceY;

                        }
                        if (e.n1.gridY <= e.n2.gridY)
                        {
                            e.n1.attForceX += currForceX;
                            e.n2.attForceX -= currForceX;
                            e.n1.attForceY += currForceY;
                            e.n2.attForceY -= currForceY;
                        }
                    }
                    if (e.n1.gridX >= e.n2.gridX)
                    {
                        if (e.n2.gridY >= e.n1.gridY)
                        {
                            e.n1.attForceX -= currForceX;
                            e.n2.attForceX += currForceX;
                            e.n1.attForceY += currForceY;
                            e.n2.attForceY -= currForceY;
                        }
                        if (e.n2.gridY <= e.n1.gridY)
                        {
                            e.n1.attForceX -= currForceX;
                            e.n2.attForceX += currForceX;
                            e.n1.attForceY -= currForceY;
                            e.n2.attForceY += currForceY;
                        }
                    }
                }

                //силы отталкивания вершина-ребро
                double gamma = 16.0; ///////////////////////////////////////16 для городов
               // double gamma = 0.005; //для области 0.005
             //  double gamma = 0.001;  //для метро 0.001
                double node_edgeDist;
                double x;
                double y;
                foreach (mixSet.Edge e in mixSet.Graph.hashEdges.Values)
                {
                    foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
                    {
                        if (n.ID != e.n1.ID && n.ID != e.n2.ID)
                        { // найти координаты виртуального узла
                            node_edgeDist = Math.Abs(((e.n2.gridY - e.n1.gridY) * n.gridX - (e.n2.gridX - e.n1.gridX) * n.gridY
                                + e.n2.gridX * e.n1.gridY - e.n2.gridY * e.n1.gridX) / Math.Sqrt(Math.Pow((e.n2.gridY - e.n1.gridY), 2) + Math.Pow((e.n2.gridX - e.n1.gridX), 2)));
                            if (node_edgeDist > gamma)
                            {
                                currForceX = currForceY = 0;
                            }
                            else {                          
                            double det = -Math.Pow((e.n2.gridX - e.n1.gridX), 2) - Math.Pow((e.n2.gridY - e.n1.gridY), 2);
                            double detX = (e.n2.gridX - e.n1.gridX) * (n.gridX * (e.n1.gridX - e.n2.gridX) + n.gridY * (e.n1.gridY - e.n2.gridY)) - (e.n2.gridY - e.n1.gridY) * (e.n1.gridX * (e.n2.gridY - e.n1.gridY) - e.n1.gridY * (e.n2.gridX - e.n1.gridX));
                            x = detX / det;
                            double detY = (e.n2.gridX - e.n1.gridX) * (e.n1.gridX * (e.n2.gridY - e.n1.gridY) - e.n1.gridY * (e.n2.gridX - e.n1.gridX)) + (e.n2.gridY - e.n1.gridY) * (n.gridX * (e.n1.gridX - e.n2.gridX) + n.gridY * (e.n1.gridY - e.n2.gridY));
                            y = detY / det;

                                if ((e.n1.gridX <= x && x <= e.n2.gridX && e.n2.gridY <= y && y <= e.n1.gridY) ||
                                   (e.n2.gridX <= x && x <= e.n1.gridX && e.n1.gridY <= y && y <= e.n2.gridY) ||
                                   (e.n2.gridX <= x && x <= e.n1.gridX && e.n2.gridY <= y && y <= e.n1.gridY) ||
                                   (e.n1.gridX <= x && x <= e.n2.gridX && e.n2.gridY <= y && y <= e.n1.gridY))
                                {

                                    nodeDist = Math.Sqrt(Math.Pow((n.gridX - x), 2) + Math.Pow((n.gridY - y), 2));
                                    double D = 0;

                                    currForceX = Math.Abs((Math.Pow((gamma - nodeDist), 2) / nodeDist) * (n.gridX - x));///
                                    currForceY = Math.Abs((Math.Pow((gamma - nodeDist), 2) / nodeDist) * (n.gridY - y));// 
                                                                                                                        // определяем наклон ребра
                                    if (((e.n1.gridX <= e.n2.gridX) && (e.n1.gridY <= e.n2.gridY)) || ((e.n2.gridX <= e.n1.gridX) && (e.n2.gridY <= e.n1.gridY)))
                                    {
                                        D = (n.gridX - e.n1.gridX) * (e.n2.gridY - e.n1.gridY) - (n.gridY - e.n1.gridY) * (e.n2.gridX - e.n1.gridX);
                                        if (D < 0) // справа от ребра
                                        {
                                            n.n_eForceX += currForceX;
                                            n.n_eForceY -= currForceY;
                                        }
                                        if (D > 0) //слева от ребра
                                        {
                                            n.n_eForceX -= currForceX;
                                            n.n_eForceY += currForceY;
                                        }
                                    }
                                    if (((e.n1.gridX >= e.n2.gridX) && (e.n1.gridY <= e.n2.gridY)) || ((e.n2.gridX >= e.n1.gridX) && (e.n2.gridY <= e.n1.gridY)))
                                    {
                                        D = (n.gridX - e.n1.gridX) * (e.n2.gridY - e.n1.gridY) - (n.gridY - e.n1.gridY) * (e.n2.gridX - e.n1.gridX);
                                        if (D < 0) // справа от ребра
                                        {
                                            n.n_eForceX += currForceX;
                                            n.n_eForceY += currForceY;
                                        }
                                        if (D > 0) //слева от ребра
                                        {
                                            n.n_eForceX -= currForceX;
                                            n.n_eForceY -= currForceY;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // аплитуды
                foreach (mixSet.Edge e in mixSet.Graph.hashEdges.Values)
                {
                    foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
                    {
                        if (n.ID != e.n1.ID && n.ID != e.n2.ID)
                        { // найти координаты виртуального узла
                            node_edgeDist = Math.Abs(((e.n2.gridY - e.n1.gridY) * n.gridX - (e.n2.gridX - e.n1.gridX) * n.gridY
                                + e.n2.gridX * e.n1.gridY - e.n2.gridY * e.n1.gridX) / Math.Sqrt(Math.Pow((e.n2.gridY - e.n1.gridY), 2) + Math.Pow((e.n2.gridX - e.n1.gridX), 2)));
                            if (node_edgeDist > gamma)
                            {
                                currForceX = currForceY = 0;
                            }
                            // если координаты лежат в нужных интервалах, считаем силу
                            double det = -Math.Pow((e.n2.gridX - e.n1.gridX), 2) - Math.Pow((e.n2.gridY - e.n1.gridY), 2);
                            double detX = (e.n2.gridX - e.n1.gridX) * (n.gridX * (e.n1.gridX - e.n2.gridX) + n.gridY * (e.n1.gridY - e.n2.gridY)) - (e.n2.gridY - e.n1.gridY) * (e.n1.gridX * (e.n2.gridY - e.n1.gridY) - e.n1.gridY * (e.n2.gridX - e.n1.gridX));
                            x = detX / det;
                            double detY = (e.n2.gridX - e.n1.gridX) * (e.n1.gridX * (e.n2.gridY - e.n1.gridY) - e.n1.gridY * (e.n2.gridX - e.n1.gridX)) + (e.n2.gridY - e.n1.gridY) * (n.gridX * (e.n1.gridX - e.n2.gridX) + n.gridY * (e.n1.gridY - e.n2.gridY));
                            y = detY / det;
                            int s = 0;
                            // если принадлежит
                            if ((e.n1.gridX <= x && x <= e.n2.gridX && e.n2.gridY <= y && y <= e.n1.gridY) ||
                               (e.n2.gridX <= x && x <= e.n1.gridX && e.n1.gridY <= y && y <= e.n2.gridY)  ||
                               (e.n2.gridX <= x && x <= e.n1.gridX && e.n2.gridY <= y && y <= e.n1.gridY)  ||
                               (e.n1.gridX <= x && x <= e.n2.gridX && e.n2.gridY <= y && y <= e.n1.gridY))
                            {
                                nodeDist = Math.Sqrt(Math.Pow((n.gridX - x), 2) + Math.Pow((n.gridY - y), 2));
                                double angRAD;
                                if ((y - n.gridY) > 0)
                                {
                                    angRAD = Math.Abs(angleBetween(n.gridX - x, n.gridY - y, 1, 0));
                                }
                                else { angRAD = Math.Abs(angleBetween(n.gridX - x, n.gridY - y, -1, 0)) + 180; }
                                                        
                                if (0 <= angRAD && angRAD < 45) { s = 0; }
                                else if (45 <= angRAD && angRAD < 90)   { s = 1; }
                                else if (90 <= angRAD && angRAD < 135)  { s = 2; }
                                else if (135 <= angRAD && angRAD < 180) { s = 3; }
                                else if (180 <= angRAD && angRAD < 225) { s = 4; }
                                else if (225 <= angRAD && angRAD < 270) { s = 5; }
                                else if (270 <= angRAD && angRAD < 315) { s = 6; }
                                else if (315 <= angRAD && angRAD < 360) { s = 7; }

                                n.dispRest[Mod((s - 2), 7)] = Math.Min(n.dispRest[Mod((s - 2), 7)], nodeDist / 3);
                                n.dispRest[Mod((s - 1), 7)] = Math.Min(n.dispRest[Mod((s - 1), 7)], nodeDist / 3);
                                n.dispRest[Mod(s, 7)] = Math.Min(n.dispRest[Mod(s, 7)], nodeDist / 3);
                                n.dispRest[Mod((s + 1), 7)] = Math.Min(n.dispRest[Mod((s + 1), 7)], nodeDist / 3);
                                n.dispRest[Mod((s + 2), 7)] = Math.Min(n.dispRest[Mod((s + 2), 7)], nodeDist / 3);

                                e.n1.dispRest[Mod((s + 2), 7)] = Math.Min(e.n1.dispRest[Mod((s + 2), 7)], nodeDist / 3);
                                e.n2.dispRest[Mod((s + 2), 7)] = Math.Min(e.n2.dispRest[Mod((s + 2), 7)], nodeDist / 3);
                                e.n1.dispRest[Mod((s + 3), 7)] = Math.Min(e.n1.dispRest[Mod((s + 3), 7)], nodeDist / 3);
                                e.n2.dispRest[Mod((s + 3), 7)] = Math.Min(e.n2.dispRest[Mod((s + 3), 7)], nodeDist / 3);
                                e.n1.dispRest[Mod((s + 4), 7)] = Math.Min(e.n1.dispRest[Mod((s + 4), 7)], nodeDist / 3);
                                e.n2.dispRest[Mod((s + 4), 7)] = Math.Min(e.n2.dispRest[Mod((s + 4), 7)], nodeDist / 3);
                                e.n1.dispRest[Mod((s + 5), 7)] = Math.Min(e.n1.dispRest[Mod((s + 5), 7)], nodeDist / 3);
                                e.n2.dispRest[Mod((s + 5), 7)] = Math.Min(e.n2.dispRest[Mod((s + 5), 7)], nodeDist / 3);
                                e.n1.dispRest[Mod((s + 6), 7)] = Math.Min(e.n1.dispRest[Mod((s + 6), 7)], nodeDist / 3);
                                e.n2.dispRest[Mod((s + 6), 7)] = Math.Min(e.n2.dispRest[Mod((s + 6), 7)], nodeDist / 3);

                            }
                            else
                            {
                                for (int i = 0; i <= 7; i++)
                                {
                                    n.dispRest[i] = Math.Min(n.dispRest[i], Math.Min(nodeDISTANCE(e.n1, n), nodeDISTANCE(e.n2, n)) / 3);
                                    e.n1.dispRest[i] = Math.Min(e.n1.dispRest[i], nodeDISTANCE(e.n1, n) / 3);
                                    e.n2.dispRest[i] = Math.Min(e.n2.dispRest[i], nodeDISTANCE(e.n2, n) / 3);
                                }
                            }
                        }

                    }

                }
                // магнитные силы 
                double Cm = 1.00;
                double B = 30.00;
                double alpha = 1.00;
                double betta = 0.5;
                double tetta;

                foreach (mixSet.Edge e in mixSet.Graph.hashEdges.Values)
                {
                    nodeDist = nodeDISTANCE(e.n1, e.n2);
                    if (((e.n1.gridX < e.n2.gridX) && (e.n1.gridY < e.n2.gridY))) // определяем наклон
                    {
                        double uptoX = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, 1, 0));
                        double uptoY = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, 0, 1));
                        if (uptoX >= uptoY) { 
                            tetta = uptoX;
                            currForceX = currForceY = Math.Abs(Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta));
                            e.n2.magForceX += currForceX;
                            e.n2.magForceY -= currForceY;
                            e.n1.magForceX -= currForceX;
                            e.n1.magForceY += currForceY;
                        }
                        else {
                            tetta = uptoY;
                            currForceX = currForceY = Math.Abs(Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta));
                            e.n2.magForceX -= currForceX;
                            e.n2.magForceY += currForceY;
                            e.n1.magForceX += currForceX;
                            e.n1.magForceY -= currForceY;
                        }
                    }
                    if (((e.n2.gridX < e.n1.gridX) && (e.n2.gridY < e.n1.gridY))) // определяем наклон
                    {
                        double uptoX = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, 1, 0));
                        double uptoY = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, 0, 1));
                        if (uptoX >= uptoY)
                        {
                            tetta = uptoX;
                            currForceX = currForceY = Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta);
                            e.n1.magForceX += currForceX;
                            e.n1.magForceY -= currForceY;
                            e.n2.magForceX -= currForceX;
                            e.n2.magForceY += currForceY;
                        }
                        else
                        {
                            tetta = uptoY;
                            currForceX = currForceY = Math.Abs(Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta));
                            e.n1.magForceX -= currForceX;
                            e.n1.magForceY += currForceY;
                            e.n2.magForceX += currForceX;
                            e.n2.magForceY -= currForceY;
                        }
                    }
                    if (((e.n1.gridX > e.n2.gridX) && (e.n1.gridY < e.n2.gridY))) // определяем наклон
                    {
                        double uptoX = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, -1, 0));
                        double uptoY = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, 0, 1));
                        if (uptoX >= uptoY)
                        {
                            tetta = uptoX;
                            currForceX = currForceY = Math.Abs(Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta));
                            e.n2.magForceX -= currForceX;
                            e.n2.magForceY -= currForceY;
                            e.n1.magForceX += currForceX;
                            e.n1.magForceY += currForceY;
                        }
                        else
                        {
                            tetta = uptoY;
                            currForceX = currForceY = Math.Abs(Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta));
                            e.n1.magForceX += currForceX;
                            e.n1.magForceY += currForceY;
                            e.n2.magForceX -= currForceX;
                            e.n2.magForceY -= currForceY;
                        }
                    }
                    if (((e.n2.gridX >= e.n1.gridX) && (e.n2.gridY <= e.n1.gridY))) // определяем наклон
                    {
                        double uptoX = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, -1, 0));
                        double uptoY = Math.Abs(angleBetween(e.n2.gridY - e.n1.gridY, e.n2.gridX - e.n1.gridY, 0, 1));
                        if (uptoX >= uptoY)
                        {
                            tetta = uptoX;
                            currForceX = currForceY = Math.Abs(Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta));
                            e.n1.magForceX -= currForceX;
                            e.n1.magForceY -= currForceY;
                            e.n2.magForceX += currForceX;
                            e.n2.magForceY += currForceY;
                        }
                        else
                        {
                            tetta = uptoY;
                            currForceX = currForceY = Math.Abs(Cm * B * Math.Pow(nodeDist, alpha) * Math.Pow(tetta, betta));
                            e.n2.magForceX += currForceX;
                            e.n2.magForceY += currForceY;
                            e.n1.magForceX -= currForceX;
                            e.n1.magForceY -= currForceY;
                        }
                    }
                }


                //итоговое смещение
                foreach (mixSet.Node n in mixSet.Graph.hashNodes.Values)
                {
                    n.nodeDispX = n.magForceX + n.n_eForceX + n.repForceX + n.attForceX; 
                    n.nodeDispY = n.magForceY + n.n_eForceY + n.repForceY + n.attForceY;
                    double newX = n.gridX + n.nodeDispX;
                    double newY = n.gridY + n.nodeDispY;

                    double distance = Math.Sqrt(Math.Pow(newX - n.gridX, 2) + Math.Pow(newY - n.gridY, 2));

                    double angRAD;
                    if ((newY - n.gridY) > 0)
                    {
                        angRAD = Math.Abs(angleBetween(n.gridY - newY, n.gridX - newX, 1, 0));
                    }
                    else { angRAD  = Math.Abs(angleBetween(n.gridY - newY, n.gridX - newX, 1, 0)) + 180; } 
                   
                    int s = 0;
                    if (0 <= angRAD && angRAD < 45) { s = 0; }
                    else if (45 <= angRAD && angRAD < 90)   { s = 1; }
                    else if (90 <= angRAD && angRAD < 135)  { s = 2; }
                    else if (135 <= angRAD && angRAD < 180) { s = 3; }
                    else if (180 <= angRAD && angRAD < 225) { s = 4; }
                    else if (225 <= angRAD && angRAD < 270) { s = 5; }
                    else if (270 <= angRAD && angRAD < 315) { s = 6; }
                    else if (315 <= angRAD && angRAD < 360) { s = 7; }

                    if (distance >= n.dispRest[s])
                    {
                        distance = n.dispRest[s];
                        double prX = distance * Math.Cos(angRAD);
                        double prY = distance * Math.Sin(angRAD);
                        newX = n.gridX + prX;
                        newY = n.gridY + prY;
                    }else
                    {
                        int k = 1;
                    }
                    n.gridX = newX;
                    n.gridY = newY;


                }               
            }
        }       
    }    
}
