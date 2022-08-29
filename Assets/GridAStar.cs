using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAStar : MonoBehaviour
{

    public Transform StartPosition;
    public LayerMask WallMask;
    public Vector2 GridSizeW;
    public float nRadius;
    public float fDistanceBetweenNodes;

    Node[,] ArrayNodes;
    public List<Node> FinalPath;


    public float fNodeDiameter;
    public int a_GridSizeX, a_GridSizeY;


    private void Start()//Ran once the program starts
    {
        fNodeDiameter = nRadius * 2;
        a_GridSizeX = Mathf.RoundToInt(GridSizeW.x / fNodeDiameter);
        a_GridSizeY = Mathf.RoundToInt(GridSizeW.y / fNodeDiameter);
        CreateGrid();
    }

    public void CreateGrid()
    {
        ArrayNodes = new Node[a_GridSizeX, a_GridSizeY];
        Vector3 botLeft = transform.position - Vector3.right * GridSizeW.x / 2 - Vector3.forward * GridSizeW.y / 2;
        for (int x = 0; x < a_GridSizeX; x++)
        {
            for (int y = 0; y < a_GridSizeY; y++)//Loop through the array of nodes
            {
                Vector3 worldPoint = botLeft + Vector3.right * (x * fNodeDiameter + nRadius) + Vector3.forward * (y * fNodeDiameter + nRadius);
                bool Wall = true;

                if (Physics.CheckSphere(worldPoint, nRadius, WallMask)) 
                {
                    Wall = false;
                }

                ArrayNodes[x, y] = new Node(Wall, worldPoint, x, y);
            }
        }
    }

    public List<Node> GetNeighbors(Node neighbor)
    {
        List<Node> ListOfNeighbors = new List<Node>();
        int checkX;
        int checkY;
        
        //right
        checkX = neighbor.gridX + 1;
         checkY = neighbor.gridY;
         if (checkX >= 0 && checkX < a_GridSizeX)
         {
             if (checkY >= 0 && checkY < a_GridSizeY)
             {
                 ListOfNeighbors.Add(ArrayNodes[checkX, checkY]);
             }
         }
       //left
         checkX = neighbor.gridX - 1;
         checkY = neighbor.gridY;
         if (checkX >= 0 && checkX < a_GridSizeX)
         {
             if (checkY >= 0 && checkY < a_GridSizeY)
             {
                 ListOfNeighbors.Add(ArrayNodes[checkX, checkY]);
             }
         }

        //top
         checkX = neighbor.gridX;
         checkY = neighbor.gridY + 1;
         if (checkX >= 0 && checkX < a_GridSizeX)
         {
             if (checkY >= 0 && checkY < a_GridSizeY)
             {
                 ListOfNeighbors.Add(ArrayNodes[checkX, checkY]);
             }
         }
         //bottom
         checkX = neighbor.gridX;
         checkY = neighbor.gridY - 1;
         if (checkX >= 0 && checkX < a_GridSizeX)
         {
             if (checkY >= 0 && checkY < a_GridSizeY)
             {
                 ListOfNeighbors.Add(ArrayNodes[checkX, checkY]);
             }
         }
        
        return ListOfNeighbors;
    }

    
    public Node NodeFromWorldPoint(Vector3 a_vWorldPos) //closest node in wposition
    {
        float ixPos = ((a_vWorldPos.x + GridSizeW.x / 2) / GridSizeW.x);
        float yPos = ((a_vWorldPos.z + GridSizeW.y / 2) / GridSizeW.y);

        int x = Mathf.FloorToInt(Mathf.Clamp((a_GridSizeX) * ixPos, 0, a_GridSizeX - 1));
        int ix = Mathf.RoundToInt((a_GridSizeX) * ixPos); //if %x = 1 (far right) gridsize x will be * 1 and we still need to -1 so we're not outside of the array.
        int iy = Mathf.RoundToInt((a_GridSizeY) * yPos);

        return ArrayNodes[ix, iy];
    }





    
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(GridSizeW.x, 1, GridSizeW.y));

        if (ArrayNodes != null)
        {
            foreach (Node n in ArrayNodes)
            {
                if (n.isAWall)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }


                if (FinalPath != null)
                {
                    if (FinalPath.Contains(n))
                    {
                        Gizmos.color = Color.red;
                    }

                }


                Gizmos.DrawCube(n.wPosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));
            }
        }
    }
}