using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VR;

public class Graph
{

    private List<Node> nodes;
    private List<Edge> edges;
    public List<Node> Nodes { get => nodes; }
    public List<Edge> Edges { get => edges; }

    public Graph()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();
    }

    public bool Adjacent(Node from, Node to)
    {
        foreach (Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return true;
        }
        return false;
    }

    public List<Node> Neighbors(Node from)
    {
        List<Node> result = new List<Node>();
        foreach (Edge e in edges)
        {
            if (e.from == from)
            {
                result.Add(e.to);
            }
        }
        return result;
    }

    public float Distance(Node from, Node to)
    {
        foreach (Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return e.GetWeight();
        }
        return Mathf.Infinity;
    }

    public void AddNode(Vector3 worldPosition, Transform transform)
    {
        nodes.Add(new Node(nodes.Count, worldPosition, transform));
    }

    public void AddEdge(Node from, Node to)
    {
        edges.Add(new Edge(from, to, 1));
    }


    //public List<Node> GetPath(Node start, Node end)
    //{
    //    List<Node> path = new List<Node>();

    //    StartCoroutine(GetPathCrt(path, start, end));
    //    return path;

    //}

    //public IEnumerator GetPathCrt(List<Node> path, Node start, Node end)
    //{

    //    yield return new WaitForEndOfFrame();
    //    if (start == end)
    //    {
    //        path.Add(start);
    //    }

    //    List<Node> openList = new List<Node>();
    //    Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
    //    Dictionary<Node, float> distances = new Dictionary<Node, float>();

    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        openList.Add(nodes[i]);
    //        distances.Add(nodes[i], float.PositiveInfinity);
    //    }

    //    distances[start] = 0f;


    //    while (openList.Count > 0)
    //    {

    //        // if (i % 10)  yield return new WaitForEndOfFrame(); 

    //        openList = openList.OrderBy(x => distances[x]).ToList();
    //        Node current = openList[0];
    //        openList.Remove(current);

    //        if (current == end)
    //        {
    //            while (previous.ContainsKey(current))
    //            {
    //                path.Insert(0, current);
    //                current = previous[current];
    //            }

    //            path.Insert(0, current);
    //            break;
    //        }

    //        foreach (Node neighbor in Neighbors(current))
    //        {

    //            float distance = Distance(current, neighbor);

    //            float newDistance = distances[current] + distance;

    //            if (newDistance < distances[neighbor])
    //            {
    //                distances[neighbor] = newDistance;
    //                previous[neighbor] = current;
    //            }
    //        }
    //    }
    //}
}

public class Node
{
    public int index;
    public Vector3 worldPosition;
    public Transform brickTransform;

    public Node(int index, Vector3 worldPosition, Transform transform)
    {
        this.index = index;
        this.worldPosition = worldPosition;
        this.brickTransform = transform;
    }
}

public class Edge
{
    public Node from;
    public Node to;

    private float weight;

    public Edge(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public float GetWeight()
    {
        return weight;
    }
}


