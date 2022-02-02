using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace MixNetSet
{
    class mixSet
    {
        public class Node
        {
            public string ID { get; internal set; }
            public XmlElement xSource { get; set; }
            public List<Edge> Edges = new List<Edge>();

            public double gridX { get; internal set; }
            public double gridY { get; internal set; }

            public double gridXCONST { get; internal set; }
            public double gridYCONST { get; internal set; }

            public float longitude { get; set; }
            public float latitude { get; set; }

            public double attForceX { get; set; }
            public double attForceY { get; set; }
            public double repForceX { get; set; }
            public double repForceY { get; set; }
            public double n_eForceX { get; set; }
            public double n_eForceY { get; set; }

            public double magForceX { get; set; }
            public double magForceY { get; set; }

            public double[] dispRest = new double[8];
            public double nodeDispX { get; set; }
            public double nodeDispY { get; set; }

            public double levelNUMY { get; set; }
            public double levelNUMX { get; set; }
            public List<List<Node>> sides { get; set; }

            public Node(XmlElement xJunction)
            {
                xSource = xJunction;
                ID = xSource.GetAttribute("code");
                CultureInfo temp_culture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                gridX = double.Parse(xSource.GetAttribute("longitude"));
                gridY = double.Parse(xSource.GetAttribute("latitude"));
                gridXCONST = gridX;
                gridYCONST = gridY;
                Thread.CurrentThread.CurrentCulture = temp_culture;
                levelNUMY = 0;
                levelNUMX = 0;
                for (int i = 0; i <= 7; i++) { dispRest[i] = 2147483646; }
                sides = new List<List<Node>>();
                for (int i = 0; i < 4; i++) { sides.Add(new List<Node>()); }
            }
        }

        public class Edge
        {
            public Node n1 { get; internal set; }
            public Node n2 { get; internal set; }
            public string vehicle { get; internal set; }
            public List<double> e1Coord { get; set; } //х и у координата первой вершины
            public List<double> e2Coord { get; set; } //x и у координата второй вершины
            public List<double> bendCoord { get; set; } // координаты излома, по дефолту =0;
            public int h = 0;// { get; set; } //отвечает за изгиб, 1 - вверх, 2 - вниз, 0 - нет изгиба
            /// <summary>
            /// </summary>
            public List<double> teste1Coord { get; set; } //х и у координаа
            public List<double> teste2Coord { get; set; } //x и у координата второй вершины
            public List<double> FtestbendCoord { get; set; }         
            public List<double> StestbendCoord { get; set; }
            /// <summary>
            /// </summary>
            public XmlElement xOrigin { get; set; }

            public Edge(XmlElement xShoulder)
            {
                xOrigin = xShoulder;
                string xID2 = xOrigin.GetAttribute("junction1");
                n1 = Graph.hashNodes[xID2];
                n1.Edges.Add(this);

                string xID1 = xOrigin.GetAttribute("junction2");
                n2 = Graph.hashNodes[xID1];
                n2.Edges.Add(this);
                vehicle = xOrigin.GetAttribute("vehicle");
                e1Coord = new List<double>();
                e2Coord = new List<double>();
                bendCoord = new List<double>();
                teste1Coord = new List<double>();
                teste2Coord = new List<double>();
                FtestbendCoord = new List<double>();
                StestbendCoord = new List<double>();
                FtestbendCoord.Add(0);
                StestbendCoord.Add(0);
                FtestbendCoord.Add(0);
                StestbendCoord.Add(0);
                bendCoord.Add(0); 
                bendCoord.Add(0);
                h = 0;
            }
        }

        public class Graph
        {
            public static Dictionary<string, Node> hashNodes = new Dictionary<string, Node>();
            //public IEnumerable<Node> Nodes => hashNodes.Values;

            public static Dictionary<string, Edge> hashEdges = new Dictionary<string, Edge>();
            // Xml-document, в котором хранится граф.
            XmlDocument xDoc = new XmlDocument();
            // конструктор, считывающий граф из файла-сети
            public Graph(string netFileName)
            {
                xDoc.Load(netFileName);
                foreach (XmlElement xJunction in xDoc.DocumentElement.SelectNodes("//junctions/junction"))
                {
                    Node n = new Node(xJunction);
                    hashNodes.Add(n.ID, n);
                }
                foreach (XmlElement xShoulder in xDoc.DocumentElement.SelectNodes("//shoulders/shoulder"))
                {
                    Edge e = new Edge(xShoulder);
                    if (!hashEdges.ContainsKey(e.n1.ID + e.n2.ID))
                    {
                        hashEdges.Add(e.n1.ID + e.n2.ID, e);
                    }
                }
            }
            public void xSave(string netFileName)
            {
                CultureInfo temp_culture = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                foreach (Node n in Graph.hashNodes.Values)         
                    n.xSource.SetAttribute("grid", $"{n.longitude},{n.latitude}");
                Thread.CurrentThread.CurrentCulture = temp_culture;
                xDoc.Save(netFileName);
            }
        }
    }

}
