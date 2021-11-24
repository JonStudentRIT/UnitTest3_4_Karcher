using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest3_4
{
    public class Node : IComparable<Node>
    {
        // any node-specific data here
        public int nState;
        public Node(int nState)
        {
            this.nState = nState;
            this.minCostToStart = int.MaxValue;
        }
        // fields needed for Dijkstra algorithm
        public List<Edge> edges = new List<Edge>();
        public int minCostToStart;
        public Node nearestToStart;
        public bool visited;
        public void AddEdge(int cost, Node connection)
        {
            Edge e = new Edge(cost, connection);
            edges.Add(e);
        }
        public int CompareTo(Node n)
        {
            return this.minCostToStart.CompareTo(n.minCostToStart);
        }
    }
    public class Edge : IComparable<Edge>
    {
        public int cost;
        public Node connectedNode;
        public Edge(int cost, Node connectedNode)
        {
            this.cost = cost;
            this.connectedNode = connectedNode;
        }
        public int CompareTo(Edge e)
        {
            return this.cost.CompareTo(e.cost);
        }
    }
    static class Program
    {
        // node connection graph
        static int[][] lGraph = new int[][]
        {
             /* 0 Red      */ new int[] { 1, 5 },
             /* 1 DarkBlue */ new int[] { 2, 3 },
             /* 2 Yellow   */ new int[] { 7 },
             /* 3 LightBlue*/ new int[] { 1, 5 },
             /* 4 Purple   */ new int[] { 2 },
             /* 5 grey     */ new int[] { 3, 6 },
             /* 6 Orange   */ new int[] { 4 },
             /* 7 Green    */ new int[] { }
        };
        // node edge cost graph
        static int[][] cGraph = new int[][]
        {
             /* 0 Red      */ new int[] { 1, 5 }, 
             /* 1 DarkBlue */ new int[] { 8, 1 }, 
             /* 2 Yellow   */ new int[] { 6 },    
             /* 3 LightBlue*/ new int[] { 1, 0 }, 
             /* 4 Purple   */ new int[] { 1 }, 
             /* 5 grey     */ new int[] { 0, 1 },    
             /* 6 Orange   */ new int[] { 1 }, 
             /* 7 Green    */ new int[] {  }
        };
        // a list to contain all of the color nodes
        static List<Node> nodes = new List<Node>();
        static void Main(string[] args)
        {
            // a reference to a color node
            Node node;
            // build the list of nodes
            for (int i = 0; i < lGraph.Length; ++i)
            {
                node = new Node(i);
                nodes.Add(node);
            }
            // add the edges to the nodes
            for (int i = 0; i < lGraph.Length; ++i)
            {
                int[] thisState = lGraph[i];
                int[] thisCosts = cGraph[i];
                for (int cCntr = 0; cCntr < thisState.Length; ++cCntr)
                {
                    nodes[i].AddEdge(thisCosts[cCntr], nodes[thisState[cCntr]]);
                }
                // sort the edges
                nodes[i].edges.Sort();
            }
            // build the shortest path list from GetShortestPathDijkstra
            List<Node> shortestPath = GetShortestPathDijkstra();
            // output the color names of the states
            foreach (Node n in shortestPath)
            {
                if (n.nState == 0)
                {
                    Console.WriteLine("Red");
                }
                if (n.nState == 1)
                {
                    Console.WriteLine("DarkBlue");
                }
                if (n.nState == 2)
                {
                    Console.WriteLine("Yellow");
                }
                if (n.nState == 3)
                {
                    Console.WriteLine("Light Blue");
                }
                if (n.nState == 4)
                {
                    Console.WriteLine("Purple");
                }
                if (n.nState == 5)
                {
                    Console.WriteLine("Grey");
                }
                if (n.nState == 6)
                {
                    Console.WriteLine("Orange");
                }
                if (n.nState == 7)
                {
                    Console.WriteLine("Green");
                }
            }
        }
        static public List<Node> GetShortestPathDijkstra()
        {
            // call the search
            DijstraSearch();
            // create a list of each node based on the shortest path
            List<Node> shortestPath = new List<Node>();
            // add the destination node
            shortestPath.Add(nodes[7]);// <-- destination
            // build the shortest path up to the destination
            BuildShortestPath(shortestPath, nodes[7]);
            // reverse the path since building the path results in green to red and we want red to green
            shortestPath.Reverse();
            return (shortestPath);
        }
        static public void BuildShortestPath(List<Node> list, Node node)
        {
            // if we are at the start node
            if (node.nearestToStart == null)
            {
                return;
            }
            // add this node
            list.Add(node.nearestToStart);
            // recursivly add the next nearest node
            BuildShortestPath(list, node.nearestToStart);
        }
        static public void DijstraSearch()
        {
            // starting at node Red
            Node start = nodes[0];// <-- startPoint
            // initialize the cost to 0
            start.minCostToStart = 0;
            // create a list of nodes to work with
            List<Node> prioQueue = new List<Node>();
            // add the starting node
            prioQueue.Add(start);
            do
            {
                // sort the list of nodes
                prioQueue.Sort();
                // set a node reference to the first element in the list
                Node node = prioQueue.First();
                // remove the node referenced
                prioQueue.Remove(node);
                // search each node's edge's connectedNode organized by the cost
                foreach (Edge cnn in node.edges.OrderBy(e => e.cost).ToList())
                {
                    Node childNode = cnn.connectedNode;
                    // if we have already visited this node
                    if (childNode.visited)
                    {
                        continue;
                    }
                    // if the node has not been checked yet or we have found a new minimum value
                    if (childNode.minCostToStart == int.MaxValue ||
                        node.minCostToStart + cnn.cost < childNode.minCostToStart)
                    {
                        childNode.minCostToStart = node.minCostToStart + cnn.cost;
                        childNode.nearestToStart = node;
                        // if we dont already have the connected node in the list
                        if (!prioQueue.Contains(childNode))
                        {
                            prioQueue.Add(childNode);
                        }
                    }
                }
                // set the node to visited
                node.visited = true;
                // if we are at the destination
                if (node == nodes[7])
                {
                    return;
                }
            } while (prioQueue.Any());
        }
    }
}