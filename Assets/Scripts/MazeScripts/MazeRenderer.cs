using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;
using UnityEditor;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField] MazeGenerator mazeGenerator;
    [SerializeField] GameObject mazeCellCompact;
    [SerializeField] GameObject enemy;

    // This is the physical size of the maze cells. If incorrect there could be overlapping or visible gaps.
    public float CellSize = 2f;
    public float noFloorProb = 0.05f;

    public NavMeshSurface surface;
    private bool spawnPoints = false;
    private bool enemySpawned = false;


    private void Start()
    {
        // Creating new maze.
        MazeCell[,] maze = mazeGenerator.GetMaze();

        float rndTableWidth = Random.Range(0, mazeGenerator.mazeWidth);
        float rndTableHeight = Random.Range(0, mazeGenerator.mazeHeight);

        //Loop through every cell of the maze.
        for (int x = 0; x < mazeGenerator.mazeWidth; x++)
        {
            for (int y = 0; y < mazeGenerator.mazeHeight; y++)
            {
                float randomNumber = Random.Range(0f, 1f);

                BuildGivenCell(maze, mazeCellCompact, x, y, randomNumber, rndTableWidth, rndTableHeight);                      
            }
        }
        // Build NavMeshSurface on the floor so that AI object can walk on it.
        surface.BuildNavMesh();
    }

    public void BuildGivenCell(MazeCell[,] maze, GameObject cell, int x, int y, double num, float rndTableWidth, float rndTableHeight)
    {
        // Instantiate a new maze cell prefab as a child of the MazeRenderer Object.
        GameObject newCell = Instantiate(cell, new Vector3((float)x * CellSize, 0f, (float)y * CellSize), Quaternion.identity, transform);

        // Get a reference to the cells MazeCellPrefab script.
        MazeCellObject mazeCell = newCell.GetComponent<MazeCellObject>();

        // Determine which walls need to be active.
        bool top = maze[x, y].topWall;
        bool left = maze[x, y].leftWall;

        // Bottom and right walls are deactivated by default unless we are at the bottom or right
        // edge of the maze.
        bool right = false;
        bool bottom = false;
        bool floor = true;
        bool holdTopWall = false;
        bool holdBotWall = false;
        bool holdLeftWall = false;
        bool holdRightWall = false;
        bool table = false;
        bool gun = false;

        bool gunSpawned = false;

        if (x == mazeGenerator.mazeWidth - 1) right = true;
        if (y == 0) bottom = true;

        if (!spawnPoints)
        {
            SpawnPointPlayer();
            SpawnPointAI();

            spawnPoints = true;
        }

        if (num > noFloorProb)
        {
            mazeCell.Init(top, bottom, left, right, floor, holdTopWall, holdBotWall, holdLeftWall, holdRightWall, table, gun);

            if (!gunSpawned && x == rndTableWidth && y == rndTableHeight)
            {
                table = true;
                gun = true;
                mazeCell.Init(top, bottom, left, right, floor, holdTopWall, holdBotWall, holdLeftWall, holdRightWall, table, gun);

                gunSpawned = true;
            }
        }
        else
        {
            floor = false;
            holdTopWall = true;
            holdBotWall = true;
            holdLeftWall = true;
            holdRightWall = true;
            mazeCell.Init(top, bottom, left, right, floor, holdTopWall, holdBotWall, holdLeftWall, holdRightWall, table, gun);
        }
    }

    void SpawnPointPlayer()
    {
        // Spawn Point for player. We create floor that is part of the NavMesh Navigation so in case that the player spawns with 
        // MazeCell prefab - MazeCellNoFloor, AI can still follow it.
        GameObject spawnPointPlayer = GameObject.CreatePrimitive(PrimitiveType.Plane);
        spawnPointPlayer.transform.position = new Vector3(0f, 0.01f, 0f);
        spawnPointPlayer.transform.localScale = new Vector3(0.2f, 0f, 0.2f);
        MeshRenderer meshRendererPlayer = spawnPointPlayer.GetComponent<MeshRenderer>();
        meshRendererPlayer.material.color = Color.green;
    }
    
    void  SpawnPointAI()
    {
        // Spawn Point for AI. We create floor that is part of the NavMesh Navigation so in case that the AI spawns with 
        // MazeCell prefab - MazeCellNoFloor, AI can still follow it.
        GameObject spawnPointAI = GameObject.CreatePrimitive(PrimitiveType.Plane);
        spawnPointAI.transform.position = new Vector3((mazeGenerator.mazeWidth * CellSize) - 2, 0.01f, (mazeGenerator.mazeHeight * CellSize) - 2);
        spawnPointAI.transform.localScale = new Vector3(0.2f, 0f, 0.2f);
        MeshRenderer meshRendererAI = spawnPointAI.GetComponent<MeshRenderer>();
        meshRendererAI.material.color = Color.red;

        Instantiate(enemy, new Vector3((mazeGenerator.mazeWidth * CellSize) - 2, 0f, (mazeGenerator.mazeHeight * CellSize) - 2), Quaternion.identity);
    }
}