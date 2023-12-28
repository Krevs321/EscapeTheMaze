using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    // Dimensions of the maze
    [HideInInspector] public int mazeWidth, mazeHeight;
    // Position where our algorithm will start from. Always (0,0).
    [HideInInspector] public int startX, startY;
    
    // An array of maze cells representing the grid.
    MazeCell[,] maze;
    // The maze cell we are currently looking at.
    Vector2Int currentCell;

    public MazeCell[,] GetMaze()
    {
        mazeWidth = Random.Range(15, 20);
        mazeHeight = Random.Range(15, 20);

        maze = new MazeCell[mazeWidth, mazeHeight];

        for (int x = 0; x < mazeWidth; x++) 
        {
            for (int y = 0; y < mazeHeight; y++) 
            {
                maze[x, y] = new MazeCell(x, y);
            }
        }

        CarvePath(startX, startY);

        return maze;
    }

    List<Direction> directions = new List<Direction>
    { 
        Direction.Up, Direction.Down, Direction.Left, Direction.Right,
    };

    List<Direction> GetRandomDirections()
    {
        // Make a copy of the above list that we can mess around with.
        // We will use above list again.
        List<Direction> dir = new List<Direction>(directions);

        // List of random directions.
        List<Direction> rndDir = new List<Direction>();

        while(dir.Count > 0)                            // Loop unitl the original list is empty.
        {
            int rnd = Random.Range(0, dir.Count);       // Get random index in list.
            rndDir.Add(dir[rnd]);                       // Add the random direction to our new list.
            dir.RemoveAt(rnd);                          // Remove that direction so we can't choose it again.
        }

        return rndDir;
    }

    bool isCellValid(int x, int y)
    {
        // If the cell is outside the map or it has been already visited, we considered it not valid.
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1 || maze[x, y].visited) return false;
        else return true;
    }

    Vector2Int CheckNeighbours ()
    {
        List<Direction> rndDir = GetRandomDirections();

        for (int i = 0; i < rndDir.Count; i++)
        {
            // Set neighbour coordinates to current cell for now.
            Vector2Int neighbour = currentCell;

            switch (rndDir[i])
            {
                case Direction.Up:
                    neighbour.y++;
                    break;
                case Direction.Down:
                    neighbour.y--;
                    break;
                case Direction.Left:
                    neighbour.x--;
                    break;
                case Direction.Right:
                    neighbour.x++;
                    break;
            }

            // If the neighbour we just tried is VALID, we can return that neighbour. If not, we go to this  function again.
            if (isCellValid(neighbour.x, neighbour.y)) { return neighbour; }
        }

        return currentCell;
    }

    // Takes in too maze positions and sets the cells acordingly.
    void BreakWalls (Vector2Int primaryCell, Vector2Int secondaryCell)
    {
        // We can only go in one direction at a time so we can handle this using if else statments.
        
        if(primaryCell.x > secondaryCell.x) //Primary Cells Left Wall
        {
            maze[primaryCell.x, primaryCell.y].leftWall = false;
        } 
        else if (primaryCell.x < secondaryCell.x) // Secondary Cells Left Wall
        {
            maze[secondaryCell.x, secondaryCell.y].leftWall = false;
        }
        else if (primaryCell.y < secondaryCell.y) // Primary Cells Top Wall
        {
            maze[primaryCell.x, primaryCell.y].topWall = false;
        }
        else if (primaryCell.y > secondaryCell.y) // Secondary Cells Top Wall
        {
            maze[secondaryCell.x, secondaryCell.y].topWall = false;
        }
    }

    // Starting at the x,y passed in, carves a path through the maze until it encounters a "dead end"
    // (a dead end is a cell with no valid neighbours).
    void CarvePath(int x, int y)
    {
        // Performs a quick check to make sure our start position is wintin the boundaries of the map,
        // if not, set them to a default and throw a warning.
        if (x < 0 || y < 0 || x > mazeWidth - 1 || y > mazeHeight - 1)
        {
            x = y = 0;
            Debug.LogWarning("Starting position is out of bounds, defaulting to 0,0");
        }

        // Set current cell to starting position.
        currentCell = new Vector2Int(x, y);
        // List to keep track of our current path.
        List<Vector2Int> path = new List<Vector2Int> ();

        bool deadEnd = false;

        while (!deadEnd) 
        {
            Vector2Int nextCell = CheckNeighbours();

            //If that cell has no valid neighbours, set deadend to true so we bereak out of the loop.
            if (nextCell == currentCell)
            {
                // If there are no valid neighbours in the current cell, we go BACK on our path to the first cell that has valid neighbour.
                for(int i = path.Count - 1; i >= 0; i--)
                {
                    currentCell = path[i];                              // Set current cell to the next step back along our path.
                    path.RemoveAt(i);                                   //Remove this step form the path.
                    nextCell = CheckNeighbours();                       // Check the new cell to see if any other neighbours are valid.

                    // If we find a valid neighbour, break out of the loop.
                    if (nextCell != currentCell) break;
                }

                if (nextCell == currentCell)
                    deadEnd = true;
            }
            else
            {
                BreakWalls(currentCell, nextCell);                      // Set wall flags on these two cells.
                maze[currentCell.x, currentCell.y].visited = true;      // Set cell to visited before moving on.
                currentCell = nextCell;                                 // Set the current cell to the valid neighbour we found.
                path.Add(currentCell);                                  // Add this cell to out path.
            }
        }
    }
}

// Directions that algorithm will take at random.
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MazeCell
{
    // If algorithm has been to current cell or not.
    public bool visited;
    public int x, y;
    public bool topWall;
    public bool leftWall;

    // Return x and y as a Vector2Int.
    public Vector2Int position
    {
        get 
        {
            return new Vector2Int(x, y);
        }
    }

    // Constructor for maze cells.
    public MazeCell (int x, int y)
    {
        // The coordinates of this cell in the maze grid.
        this.x = x;
        this.y = y;

        // Wheter the algorithm has visited this cell or not - false to start.
        visited = false;

        // All walls present untill algorithm removes them.
        topWall = leftWall = true;
    }
}