using System;
using System.Collections.Generic;


public class mixSet
{ 
	public mixSet()
	{
	}
    
    public class Node
    {
        public string ID { get; internal set; }
        public XmlElement xSource { get; set; }
        public List<Edge> Edges = new List<Edge>();

        public double gridX { get; internal set; }
        public double gridY { get; internal set; }

        public float longitude { get; set; }
        public float latitude { get; set; }

        //public readonly List<Edge> Incoming = new List<Edge>();
        //public readonly List<Edge> OutGoing = new List<Edge>();

        public Node(XmlElement xJunction)
        {
            xSource = xJunction;
            ID = xSource.GetAttribute("code");
            latitude = float.Parse(xSource.GetAttribute("latitude"));
            longitude = float.Parse(xSource.GetAttribute("latitude"));
        }
    }

    public class Edge
    {
        public Node n1 { get; internal set; }
        public Node n2 { get; internal set; }
        public XmlElement xOrigin { get; set; }

        public Edge(XmlElement xShoulder)
        {
            xOrigin = xShoulder;
            this.n1 = float.Parse(xOrigin.GetAttribute("junction2"));
            n1.Edges.Add(this);
            this.n2 = float.Parse(xOrigin.GetAttribute("junction1"));
            n2.Edges.Add(this);
        }
    }

    public class Graph
    {

        // hash таблица, позволяющая ключу находить вершину
        private Dictionary<string,Node> hashNodes = new Dictionary<string, Node>();
        public IEnumerable<Node> Nodes => hashNodes.Values;

        // Xml-document, в котором хранится граф.
        XmlDocument doc = new XmlDocument();
        // конструктор, считывающий граф из файла-сети
        public Graph(string netFileName)
        {
            doc.Load(netFileName);

            foreach (XmlElement xJunction in doc.DocumentElement.SelectNode("//junctions/junction"))
            {
                Node n = new Node(xJunction);
                hashNodes.Add(n.ID, n);
            }
            // далее считываете дуги
            foreach (XmlElement xShoulder in doc.DocumentElement.SelectEdge("//shoulders/shoulder"))
            {
                Edge n1n2 = new Edge(xShoulder);
            }
       }
    }
    public void Save(string netFileName)
    {
        foreach (Node n in hashNode.Values)
            n.xSource.SetAttribute("grid") = $"{n.gridX},{n.gridY}";            
        doc.Save(netFileName);
    }
}
