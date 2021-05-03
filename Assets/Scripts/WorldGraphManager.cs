using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldGraphManager : MonoBehaviour
{
    public static WorldGraphManager _instance;
    public float edgeDistanceMax = 3.0f;
    public float edgeDistanceMin = 2.12f;
    public Graph graph;

    public int fromIndex;
    public int toIndex;

    public Monster testMonster;
    public Player testPlayer;

    List<Node> path;

    public bool showPath = false;
    private void Awake()
    {
        _instance = this;
    }
    private void Update()
    {

        //Test
        if (testMonster && testMonster.currentStandOnNode != null)
            fromIndex = testMonster.currentStandOnNode.index;
        if (testPlayer && testPlayer.curNode != null)
            toIndex = testPlayer.curNode.index;

        //Debug.Log(fromIndex+":"+ toIndex);

        if (Input.GetKeyDown(KeyCode.P))
        {
            //RandomIndex();
            GetPath(fromIndex, toIndex);
            showPath = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {

        }
    }
    private void Start()
    {
        // InitializeGraph();
    }

    private void RandomIndex()
    {

        fromIndex = Random.Range(0, graph.Nodes.Count);
        toIndex = Random.Range(0, graph.Nodes.Count);
        if (fromIndex == toIndex)
        {
            RandomIndex();
        }
        else
        {
            return;
        }
    }

    public void InitializeGraph()
    {
        graph = new Graph();
        //Transform room = WorldConfiguration._instance.bigRubikRooms[0];
        Transform[] rooms = WorldConfiguration._instance.bigRubikRooms;
        //rooms.childCout = 6
        foreach (Transform room in rooms)
        {
            Transform[] walls = new Transform[6];
            Transform[] bricks = new Transform[54];

            //rooms.childCount * walls[0].childCount=6*9=54
            int index = 0;
            for (int i = 0; i < walls.Length; i++)
            {
                walls[i] = room.GetChild(i);
                Transform curWall = walls[i];
                for (int j = 0; j < curWall.childCount; j++)
                {
                    bricks[index] = curWall.GetChild(j);
                    index++;
                }

            }

            for (int i = 0; i < bricks.Length; i++)
            {
                Transform brick = bricks[i];
                graph.AddNode(brick.position, brick);
                //Brick b = brick.gameObject.AddComponent(typeof(Brick)) as Brick;
                //b.index = graph.Nodes.Count;
            }
        }

        List<Node> allNodes = graph.Nodes;
        foreach (Node from in allNodes)
        {
            foreach (Node to in allNodes)
            {
                float distance = Vector3.Distance(from.worldPosition, to.worldPosition);
                //if (distance <= edgeDistanceMax && distance > edgeDistanceMin && from != to)
                //{
                //    graph.AddEdge(from, to);
                //}

                if (distance <= edgeDistanceMax)
                {
                    if (BrickInSameRoom(from.brickTransform, to.brickTransform))
                    {
                        graph.AddEdge(from, to);
                    }
                    else if (IsBothPassages(from.brickTransform, to.brickTransform) && distance < 1f)
                    {
                        graph.AddEdge(from, to);
                    }
                    //Debug.Log(GenerateLevel.passages.Contains(from.transform));
                    //if (GenerateLevel.passages.Contains(from.transform) && GenerateLevel.passages.Contains(to.transform))
                    //{
                    //    Debug.Log(true);
                    //    graph.AddEdge(from, to);
                    //}
                }
            }
        }
        AddNodeToBrick();
        //RandomIndex();

    }

    private bool IsBothPassages(Transform brickOne, Transform brickTwo)
    {
        if (GenerateLevel.bigRubikPassages.Contains(brickOne.transform) && GenerateLevel.bigRubikPassages.Contains(brickTwo.transform))
            return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (graph == null)
        {
            return;
        }
        List<Edge> allEdges = graph.Edges;
        foreach (Edge e in allEdges)
        {
            Debug.DrawLine(e.from.worldPosition, e.to.worldPosition, Color.white);
        }

        List<Node> allNodes = graph.Nodes;
        foreach (Node n in allNodes)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(n.worldPosition, 0.2f);
        }


        if (showPath)
        {
            if (fromIndex < allNodes.Count && toIndex < allNodes.Count)
            {

                if (path.Count > 1)
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(path[i].brickTransform.position, path[i + 1].brickTransform.position, Color.red);
                    }
                }
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(allNodes[fromIndex].brickTransform.position, 0.2f);
                Gizmos.DrawSphere(allNodes[toIndex].brickTransform.position, 0.2f);
                //showPath = false;
            }
        }
    }

    public void GetPath(int fromIndex, int toIndex)
    {
        List<Node> allNodes = graph.Nodes;
        path = GetPath(allNodes[fromIndex], allNodes[toIndex]);
    }

    public List<Node> GetPath(Node start, Node end)
    {
        List<Node> curPath = new List<Node>();

        StartCoroutine(GetPathCrt(curPath, start, end));
        return curPath;
    }

    public IEnumerator GetPathCrt(List<Node> path, Node start, Node end)
    {

        yield return new WaitForEndOfFrame();
        if (start == end)
        {
            path.Add(start);
        }

        List<Node> openList = new List<Node>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        Dictionary<Node, float> distances = new Dictionary<Node, float>();

        for (int i = 0; i < graph.Nodes.Count; i++)
        {
            openList.Add(graph.Nodes[i]);
            distances.Add(graph.Nodes[i], float.PositiveInfinity);
        }

        distances[start] = 0f;


        while (openList.Count > 0)
        {

            if (openList.Count % 10 == 0) yield return new WaitForEndOfFrame();

            openList = openList.OrderBy(x => distances[x]).ToList();
            Node current = openList[0];
            openList.Remove(current);

            if (current == end)
            {
                while (previous.ContainsKey(current))
                {
                    path.Insert(0, current);
                    current = previous[current];
                }

                path.Insert(0, current);
                break;
            }

            foreach (Node neighbor in graph.Neighbors(current))
            {

                float distance = graph.Distance(current, neighbor);

                float newDistance = distances[current] + distance;

                if (newDistance < distances[neighbor])
                {
                    distances[neighbor] = newDistance;
                    previous[neighbor] = current;
                }
            }
        }
    }


    public bool BrickInSameRoom(Transform brickOne, Transform brickTwo)
    {
        if (brickOne.parent.parent == brickTwo.parent.parent)
            return true;
        return false;
    }

    public void AddNodeToBrick()
    {
        List<Node> allNodes = graph.Nodes;
        Transform[] rooms = WorldConfiguration._instance.bigRubikRooms;
        foreach (Transform room in rooms)
        {
            foreach (Transform wall in room)
            {
                foreach (Transform brick in wall)
                {
                    foreach (Node node in allNodes)
                    {
                        if (brick == node.brickTransform)
                        {
                            Brick b = brick.gameObject.AddComponent(typeof(Brick)) as Brick;
                            b.index = node.index;
                        }
                    }
                }
            }
        }
    }
}



