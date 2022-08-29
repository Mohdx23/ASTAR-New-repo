using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{

    GridAStar GridReference;
    public Transform StartPosition;
    public Transform TargetPosition;

    private void Awake()
    {
        GridReference = GetComponent<GridAStar>();
    }

    private void Update()//Every frame
    {
        LocatePath(StartPosition.position, TargetPosition.position);
    }

    public void LocatePath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);
        Node EndNode = GridReference.NodeFromWorldPoint(a_TargetPos);

        List<Node> OpenList = new List<Node>();
        List<Node> ClosedList = new List<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].nodeHCost < CurrentNode.nodeHCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == EndNode)
            {
                GetFoundPath(StartNode, EndNode);
            }

            foreach (Node NeighborNode in GridReference.GetNeighbors(CurrentNode))
            {
                if (!NeighborNode.isAWall || ClosedList.Contains(NeighborNode))
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.nodeGCost + taxiCabDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.nodeGCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.nodeGCost = MoveCost;
                    NeighborNode.nodeHCost = taxiCabDistance(NeighborNode, EndNode);
                    NeighborNode.ParentNode = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }

        }
    }



   public void GetFoundPath(Node startNode, Node targetNode)
    {
        List<Node> endPath = new List<Node>();
        Node currentN = targetNode;

        while (currentN != startNode)
        {
            endPath.Add(currentN);
            currentN = currentN.ParentNode;
        }

        endPath.Reverse();

        GridReference.FinalPath = endPath;

    }

    public int taxiCabDistance(Node b_nodeA, Node b_nodeB)
    {
        int ix = Mathf.Abs(b_nodeA.gridX - b_nodeB.gridX);
        int iy = Mathf.Abs(b_nodeA.gridY - b_nodeB.gridY);

        return ix + iy;
    }
}