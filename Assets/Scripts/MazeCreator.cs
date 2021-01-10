using System.Collections.Generic;
using UnityEngine;

public class MazeCreator : MonoBehaviour
{
    [System.Serializable]
    public class Cell {
        public bool Visited=false;

        public GameObject up;
        public GameObject left;
        public GameObject right;
        public GameObject down;
    }


    public GameObject wall;
    private int xSize = 10;
    private int zSize = 10;
    private float wallLength = 1f;
    private Vector3 initialPos;
    GameObject wallHolder;
    public Cell[] cell;
    public int currentCell;
    private int visitedCells = 0;
    private bool startedBuilding=false;
    private int currentNeighbour;
    private List<int> lastCells;
    private int backTrack=0;
    private int wallToBreak = 0;

    // Start is called before the first frame update
    void Start()
    {
        createGrid();
    }

    private void createGrid()
    {
        initialPos = new Vector3(-(xSize / 2) + wallLength / 2, 0, -(zSize / 2) + wallLength / 2);
        Vector3 myPos = initialPos;
        wallHolder = GameObject.Find("Maze");


        for (int i = 0; i < zSize; i++) {
            for (int j = 0; j <= xSize; j++) {
                myPos = new Vector3(initialPos.x + (j * wallLength) - (wallLength / 2), 0, initialPos.z + (i * wallLength) - (wallLength / 2));
                var temp =  Instantiate(wall, myPos, Quaternion.identity);
                temp.transform.parent = wallHolder.transform;
            }
        }
        for (int i = 0; i <= zSize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                myPos = new Vector3(initialPos.x + (j * wallLength) , 0, initialPos.z + (i * wallLength)-wallLength);
                var temp = Instantiate(wall, myPos, Quaternion.Euler(0,90,0));
                temp.transform.parent = wallHolder.transform;
            }
        }
        createCells();
    }

    private void createCells()
    {
        lastCells = new List<int>();
        lastCells.Clear();
        GameObject[] allWalls;
        int children = wallHolder.transform.childCount;
        allWalls = new GameObject[children];
        cell = new Cell[xSize * zSize];
        int eastWest = 0;
        int childProcess = 0;
        int cellCount=0;

        //Get all walls
        for (int i = 0; i < children; i++) {
            allWalls[i] = wallHolder.transform.GetChild(i).gameObject;
        }

        //
        for (int i = 0; i < cell.Length; i++) {
            if (cellCount == xSize)
            {
                eastWest ++;
                cellCount = 0;
            }

            cell[i] = new Cell();
            cell[i].left = allWalls[eastWest];
            cell[i].down = allWalls[childProcess + (xSize + 1) * zSize];

          
            eastWest++;
            cellCount++;
            childProcess++;
            cell[i].right = allWalls[eastWest];
            cell[i].up = allWalls[(childProcess + (xSize + 1) * zSize) + xSize - 1];

           
        }

        CreateMaze();


    }

    private void CreateMaze()
    {
        while (visitedCells < cell.Length)
        {
            if (startedBuilding)
            {
                GetNeighbours();
                if (!cell[currentNeighbour].Visited && cell[currentCell].Visited) {
                    BreakWall();
                    cell[currentNeighbour].Visited = true;
                    visitedCells++;
                    lastCells.Add(currentCell);
                    currentCell = currentNeighbour;
                    if (lastCells.Count > 0) {
                        backTrack = lastCells.Count - 1;
                    }
                }
            }
            else
            {
                currentCell = Random.Range(0, cell.Length);
                cell[currentCell].Visited = true;
                visitedCells++;
                startedBuilding = true;
            }
        }
        Debug.Log("Finished");
        Destroy(cell[0].left);
        Destroy(cell[99].right);
    }

    private void BreakWall()
    {
        switch (wallToBreak) {
            case 1:
                Destroy(cell[currentCell].left);
                break;

            case 2:
                Destroy(cell[currentCell].right);
                break;

            case 3:
                Destroy(cell[currentCell].up);
                break;

            case 4:
                Destroy(cell[currentCell].down);
                break;
        }
    }

    private void GetNeighbours()
    {
        List<int> Neighbours = new List<int>();
        int[] connectingWall = new int[4];
        //left
        if (currentCell - 1 >= 0 && (currentCell)%xSize!=0)
        {
            if (!cell[currentCell - 1].Visited)
            {
                Neighbours.Add(currentCell - 1);
                connectingWall[Neighbours.Count-1] = 1;
            }
        }

        //right
        if (currentCell + 1 <= cell.Length && (currentCell + 1) % xSize != 0)
        {
            if (!cell[currentCell + 1].Visited)
            {
                Neighbours.Add(currentCell + 1);
                connectingWall[Neighbours.Count-1]=2;
            }
        }

        //up
        if (currentCell + zSize < cell.Length)
        {
            if (!cell[currentCell + zSize].Visited)
            {
                Neighbours.Add(currentCell + zSize);
                connectingWall[Neighbours.Count-1] = 3;
            }
        }

        //down
        if (currentCell - zSize >= 0)
        {
            if (!cell[currentCell - zSize].Visited)
            {
                Neighbours.Add(currentCell - zSize);
                connectingWall[Neighbours.Count-1] = 4;
            }
        }
        if (Neighbours.Count != 0)
        {
            int nextNeighbour = Random.Range(0, Neighbours.Count);
            currentNeighbour = Neighbours[nextNeighbour];
            wallToBreak = connectingWall[nextNeighbour];
        }

        else {
            if (backTrack > 0){
                currentCell = lastCells[backTrack];
                backTrack--;
                
            }    
                
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
