using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{

    public int gridX;
    public int gridY;

    public bool isAWall;
    public Vector3 wPosition;

    public Node ParentNode;

    public int nodeGCost;
    public int nodeHCost;

    public int FCost { get { return nodeGCost + nodeHCost; } }

    public Node(bool a_isAWall, Vector3 wposition, int a_gridX, int a_gridY)
    {
        isAWall = a_isAWall;
        wPosition = wposition;
        gridX = a_gridX;
        gridY = a_gridY;
    }

}