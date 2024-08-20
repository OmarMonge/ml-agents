/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public int gridSize = 8;
    public float wallProbability = 0.3f;

    private GameObject[,] grid;


    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    //Create the maze with the selected amount of cells (8x8)
    void GenerateGrid()
    {
        for( int x = 0; x < gridSize; x++ )
        {
            for( int z = 0; z < gridSize; z++ ) {
                //place cell prefab in position of grid
                Vector3 position = new Vector3(x, 0, z);
                GameObject cell;
                if (Random.value < wallProbability)
                {
                    cell = Instantiate(wallPrefab, position +  new Vector3(0,.25f,0), Quaternion.identity);
                }
                else 
                {
                    cell = Instantiate(cellPrefab, position, Quaternion.identity);
                }
                
                //add cell copy position to grid
                //grid[x, z] = cell;

                

            }
        }
    }


}
*/
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public int gridSize = 8;
    public float wallProbability = 0.7f; // Higher probability since walls are only on the edges

    private GameObject[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Create the maze with the selected amount of cells (8x8)
    void GenerateGrid()
    {
        grid = new GameObject[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // Place cell prefab in position of grid
                Vector3 position = new Vector3(x, 0, z);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);

                // Add cell copy position to grid
                grid[x, z] = cell;

                // Randomly decide if walls should be created on each side
                if (Random.value < wallProbability)
                {
                    CreateWall(cell, position, "north", x, z);
                }
                if (Random.value < wallProbability)
                {
                    CreateWall(cell, position, "south", x, z);
                }
                if (Random.value < wallProbability)
                {
                    CreateWall(cell, position, "east", x, z);
                }
                if (Random.value < wallProbability)
                {
                    CreateWall(cell, position, "west", x, z);
                }
            }
        }
    }

    // Method to create a wall on a specific side
    void CreateWall(GameObject cell, Vector3 position, string direction, int x, int z)
    {
        Vector3 wallPosition = position;
        Quaternion wallRotation = Quaternion.identity;

        switch (direction)
        {
            case "north":
                wallPosition += new Vector3(0, 0.25f, 0.5f);
                wallRotation = Quaternion.Euler(0, 90, 0);
                break;
            case "south":
                wallPosition += new Vector3(0, 0.25f, -0.5f);
                wallRotation = Quaternion.Euler(0, 90, 0);
                break;
            case "east":
                wallPosition += new Vector3(0.5f, 0.25f, 0);
                break;
            case "west":
                wallPosition += new Vector3(-0.5f, 0.25f, 0);
                break;
        }

        Instantiate(wallPrefab, wallPosition, wallRotation);
    }
}
*/
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public int gridSize = 8;

    private GameObject[,] grid;
    private bool[,] visited;

    // Directions in the form of (x, z) offsets
    private Vector2Int[] directions = {
        new Vector2Int(0, 1), // North
        new Vector2Int(1, 0), // East
        new Vector2Int(0, -1), // South
        new Vector2Int(-1, 0)  // West
    };

    void Start()
    {
        GenerateGrid();
        GenerateMaze(new Vector2Int(0, 0));
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        visited = new bool[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                grid[x, z] = cell;
                visited[x, z] = false;
            }
        }
    }

    void GenerateMaze(Vector2Int currentCell)
    {
        visited[currentCell.x, currentCell.y] = true;

        // Shuffle directions to get random paths
        directions = ShuffleDirections(directions);

        foreach (var direction in directions)
        {
            Vector2Int neighbor = currentCell + direction;

            // Check if neighbor is within grid bounds
            if (IsWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y])
            {
                // Remove wall between current cell and neighbor
                RemoveWall(currentCell, neighbor);

                // Recursively generate the maze for the neighboring cell
                GenerateMaze(neighbor);
            }
        }
    }

    void RemoveWall(Vector2Int current, Vector2Int neighbor)
    {
        Vector2Int wallDirection = neighbor - current;

        Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);
        Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;

        // Remove the wall by not placing one
        Instantiate(wallPrefab, wallPosition, wallRotation);
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
    }

    Vector2Int[] ShuffleDirections(Vector2Int[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int temp = directions[i];
            int randomIndex = Random.Range(i, directions.Length);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
        return directions;
    }
}*/
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public GameObject startPrefab; // Prefab for the start position
    public GameObject goalPrefab;  // Prefab for the goal position
    public int gridSize = 8;

    private GameObject[,] grid;
    private bool[,] visited;

    // Directions in the form of (x, z) offsets
    private Vector2Int[] directions = {
        new Vector2Int(0, 1), // North
        new Vector2Int(1, 0), // East
        new Vector2Int(0, -1), // South
        new Vector2Int(-1, 0)  // West
    };

    void Start()
    {
        GenerateGrid();
        GenerateMaze(new Vector2Int(0, 0));
        PlaceStartAndGoal();
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        visited = new bool[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                grid[x, z] = cell;
                visited[x, z] = false;
            }
        }
    }

    void GenerateMaze(Vector2Int currentCell)
    {
        visited[currentCell.x, currentCell.y] = true;

        // Shuffle directions to get random paths
        directions = ShuffleDirections(directions);

        foreach (var direction in directions)
        {
            Vector2Int neighbor = currentCell + direction;

            // Check if neighbor is within grid bounds
            if (IsWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y])
            {
                // Remove wall between current cell and neighbor
                RemoveWall(currentCell, neighbor);

                // Recursively generate the maze for the neighboring cell
                GenerateMaze(neighbor);
            }
        }
    }

    void RemoveWall(Vector2Int current, Vector2Int neighbor)
    {
        Vector2Int wallDirection = neighbor - current;

        Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);
        Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;

        // Remove the wall by not placing one
        Instantiate(wallPrefab, wallPosition, wallRotation);
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
    }

    Vector2Int[] ShuffleDirections(Vector2Int[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int temp = directions[i];
            int randomIndex = Random.Range(i, directions.Length);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
        return directions;
    }

    void PlaceStartAndGoal()
    {
        // Place start prefab at the starting position (0, 0)
        Vector3 startPosition = new Vector3(0, 0, 0);
        Instantiate(startPrefab, startPosition, Quaternion.identity);

        // Place goal prefab at the end position (gridSize-1, gridSize-1)
        Vector3 goalPosition = new Vector3(gridSize - 1, 0, gridSize - 1);
        Instantiate(goalPrefab, goalPosition, Quaternion.identity);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class MazeGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public GameObject startPrefab; // Prefab for the start position
    public GameObject goalPrefab;  // Prefab for the goal position
    public int gridSize = 8;

    private GameObject[,] grid;
    private bool[,] visited;

    // Directions in the form of (x, z) offsets
    private Vector2Int[] directions = {
        new Vector2Int(0, 1), // North
        new Vector2Int(1, 0), // East
        new Vector2Int(0, -1), // South
        new Vector2Int(-1, 0)  // West
    };

    private Vector2Int startCell = new Vector2Int(0, 0);
    private Vector2Int goalCell;

    void Start()
    {
        GenerateGrid();
        GenerateMaze(startCell);
        PlaceStartAndGoal();
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        visited = new bool[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                grid[x, z] = cell;
                visited[x, z] = false;
            }
        }
    }

    void GenerateMaze(Vector2Int currentCell)
    {
        visited[currentCell.x, currentCell.y] = true;

        // Shuffle directions to get random paths
        directions = ShuffleDirections(directions);

        foreach (var direction in directions)
        {
            Vector2Int neighbor = currentCell + direction;

            // Check if neighbor is within grid bounds
            if (IsWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y])
            {
                // Remove wall between current cell and neighbor
                RemoveWall(currentCell, neighbor);

                // Recursively generate the maze for the neighboring cell
                GenerateMaze(neighbor);
            }
        }

        // Track the farthest cell as the goal
        if (currentCell != startCell && Vector2Int.Distance(startCell, currentCell) > Vector2Int.Distance(startCell, goalCell))
        {
            goalCell = currentCell;
        }
    }

    void RemoveWall(Vector2Int current, Vector2Int neighbor)
    {
        Vector2Int wallDirection = neighbor - current;

        Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);
        Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;

        // Remove the wall by not placing one
        Instantiate(wallPrefab, wallPosition, wallRotation);
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
    }

    Vector2Int[] ShuffleDirections(Vector2Int[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int temp = directions[i];
            int randomIndex = Random.Range(i, directions.Length);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
        return directions;
    }

    void PlaceStartAndGoal()
    {
        // Place start prefab at the starting position
        Vector3 startPosition = new Vector3(startCell.x, 0, startCell.y);
        Instantiate(startPrefab, startPosition, Quaternion.identity);

        // Place goal prefab at the goal position
        Vector3 goalPosition = new Vector3(goalCell.x, 0, goalCell.y);
        Instantiate(goalPrefab, goalPosition, Quaternion.identity);
    }
}*/

/*

public class MazeGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public GameObject startPrefab; // Prefab for the start position
    public GameObject goalPrefab;  // Prefab for the goal position
    public int gridSize = 8;

    private GameObject[,] grid;
    private bool[,] visited;

    // Directions in the form of (x, z) offsets
    private Vector2Int[] directions = {
        new Vector2Int(0, 1), // North
        new Vector2Int(1, 0), // East
        new Vector2Int(0, -1), // South
        new Vector2Int(-1, 0)  // West
    };

    private Vector2Int startCell = new Vector2Int(0, 0);
    private Vector2Int goalCell;
    public GameObject pathPrefab;
    private List<Vector2Int> solutionPath = new List<Vector2Int>();

    void VisualizeSolution()
    {
        Vector2Int current = startCell;
        while (current != goalCell)
        {
            solutionPath.Add(current);
            Vector2Int direction = goalCell - current;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                current.x += (direction.x > 0) ? 1 : -1;
            }
            else
            {
                current.y += (direction.y > 0) ? 1 : -1;
            }
        }
        solutionPath.Add(goalCell);

        foreach (Vector2Int cell in solutionPath)
        {
            Vector3 pathPosition = new Vector3(cell.x, 0.1f, cell.y);
            Instantiate(pathPrefab, pathPosition, Quaternion.identity);
        }
    }
    void Start()
    {
        GenerateGrid();
        GenerateMaze(startCell);
        CarvePathToGoal();
        PlaceStartAndGoal();
    }

    void GenerateGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        visited = new bool[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x, 0, z);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                grid[x, z] = cell;
                visited[x, z] = false;

                // Place walls around the cell
                PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z + 1)); // North
                PlaceWall(new Vector2Int(x, z), new Vector2Int(x + 1, z)); // East
                if (z == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z - 1)); // South (only for bottom row)
                if (x == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x - 1, z)); // West (only for leftmost column)
            }
        }
    }

    void PlaceWall(Vector2Int current, Vector2Int neighbor)
    {
        Vector2Int wallDirection = neighbor - current;
        Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);
        Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
        Instantiate(wallPrefab, wallPosition, wallRotation);
    }

    void GenerateMaze(Vector2Int currentCell)
    {
        visited[currentCell.x, currentCell.y] = true;

        // Shuffle directions to get random paths
        directions = ShuffleDirections(directions);

        foreach (var direction in directions)
        {
            Vector2Int neighbor = currentCell + direction;

            // Check if neighbor is within grid bounds
            if (IsWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y])
            {
                // Carve a path between the current cell and the neighbor
                RemoveWall(currentCell, neighbor);

                // Recursively generate the maze for the neighboring cell
                GenerateMaze(neighbor);
            }
        }

        // Track the farthest cell as the goal
        if (currentCell != startCell && Vector2Int.Distance(startCell, currentCell) > Vector2Int.Distance(startCell, goalCell))
        {
            goalCell = currentCell;
        }
    }
    void RemoveWall(Vector2Int current, Vector2Int neighbor)
    {
        Vector2Int wallDirection = neighbor - current;
        Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);

        // Increase the overlap box size to ensure we catch the wall
        Collider[] colliders = Physics.OverlapBox(wallPosition, new Vector3(0.4f, 0.4f, 0.4f));

        bool wallRemoved = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Wall"))
            {
                Destroy(collider.gameObject);
                wallRemoved = true;
                Debug.Log($"Wall removed at position {wallPosition}");
                break;
            }
        }

        if (!wallRemoved)
        {
            Debug.LogWarning($"No wall found to remove at position {wallPosition}");
        }
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
    }

    Vector2Int[] ShuffleDirections(Vector2Int[] directions)
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int temp = directions[i];
            int randomIndex = Random.Range(i, directions.Length);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }
        return directions;
    }

    void CarvePathToGoal()
    {
        Vector2Int currentCell = startCell;

        while (currentCell != goalCell)
        {
            Vector2Int direction = goalCell - currentCell;
            Vector2Int nextStep = currentCell;

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                nextStep.x += (direction.x > 0) ? 1 : -1;
            }
            else
            {
                nextStep.y += (direction.y > 0) ? 1 : -1;
            }

            RemoveWall(currentCell, nextStep);
            currentCell = nextStep;
        }
    }

    void PlaceStartAndGoal()
    {
        // Place start prefab at the starting position
        Vector3 startPosition = new Vector3(startCell.x, 0, startCell.y);
        Instantiate(startPrefab, startPosition, Quaternion.identity);

        // Place goal prefab at the goal position
        Vector3 goalPosition = new Vector3(goalCell.x, 0, goalCell.y);
        Instantiate(goalPrefab, goalPosition, Quaternion.identity);
    }
}
*/


// public class MazeGenerator : MonoBehaviour
// {
//     public GameObject cellPrefab;
//     public GameObject wallPrefab;
//     public GameObject startPrefab;
//     public GameObject goalPrefab;
//     public int gridSize = 8;

//     private GameObject[,] grid;
//     private bool[,] visited;
//     private List<Vector2Int> frontiers = new List<Vector2Int>();

//     private Vector2Int[] directions = {
//         new Vector2Int(0, 1), // North
//         new Vector2Int(1, 0), // East
//         new Vector2Int(0, -1), // South
//         new Vector2Int(-1, 0)  // West
//     };

//     public Vector2Int startCell;
//     public Vector2Int goalCell;

//     void Start()
//     {
//         GenerateGrid();
//         GenerateMaze();
//         PlaceStartAndGoal();
//     }

//     void GenerateGrid()
//     {
//         grid = new GameObject[gridSize, gridSize];
//         visited = new bool[gridSize, gridSize];

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int z = 0; z < gridSize; z++)
//             {
//                 Vector3 position = new Vector3(x, 0, z);
//                 GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
//                 grid[x, z] = cell;
//                 visited[x, z] = false;

//                 // Place walls around the cell
//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z + 1)); // North
//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x + 1, z)); // East
//                 if (z == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z - 1)); // South (only for bottom row)
//                 if (x == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x - 1, z)); // West (only for leftmost column)
//             }
//         }
//     }

//     void PlaceWall(Vector2Int current, Vector2Int neighbor)
//     {
//         Vector2Int wallDirection = neighbor - current;
//         Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);
//         Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
//         Instantiate(wallPrefab, wallPosition, wallRotation);
//     }

//     public void GenerateMaze()
//     {
//         // Start with a random cell
//         startCell = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
//         AddToMaze(startCell);

//         while (frontiers.Count > 0)
//         {
//             int randomIndex = Random.Range(0, frontiers.Count);
//             Vector2Int current = frontiers[randomIndex];
//             frontiers.RemoveAt(randomIndex);

//             List<Vector2Int> neighbors = GetVisitedNeighbors(current);
//             if (neighbors.Count > 0)
//             {
//                 Vector2Int neighbor = neighbors[Random.Range(0, neighbors.Count)];
//                 RemoveWall(current, neighbor);
//                 AddToMaze(current);
//             }
//         }

//         // Set the goal as the cell furthest from the start
//         goalCell = FindFurthestCell(startCell);
//     }

//     void AddToMaze(Vector2Int cell)
//     {
//         visited[cell.x, cell.y] = true;
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y] && !frontiers.Contains(neighbor))
//             {
//                 frontiers.Add(neighbor);
//             }
//         }
//     }

//     List<Vector2Int> GetVisitedNeighbors(Vector2Int cell)
//     {
//         List<Vector2Int> neighbors = new List<Vector2Int>();
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && visited[neighbor.x, neighbor.y])
//             {
//                 neighbors.Add(neighbor);
//             }
//         }
//         return neighbors;
//     }

//     void RemoveWall(Vector2Int current, Vector2Int neighbor)
//     {
//         Vector2Int wallDirection = neighbor - current;
//         Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);

//         Collider[] colliders = Physics.OverlapBox(wallPosition, new Vector3(0.4f, 0.4f, 0.4f));
//         foreach (Collider collider in colliders)
//         {
//             if (collider.gameObject.CompareTag("Wall"))
//             {
//                 Destroy(collider.gameObject);
//                 break;
//             }
//         }
//     }

//     bool IsWithinBounds(Vector2Int position)
//     {
//         return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
//     }

//     Vector2Int FindFurthestCell(Vector2Int start)
//     {
//         Vector2Int furthest = start;
//         float maxDistance = 0;

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int y = 0; y < gridSize; y++)
//             {
//                 Vector2Int current = new Vector2Int(x, y);
//                 float distance = Vector2Int.Distance(start, current);
//                 if (distance > maxDistance)
//                 {
//                     maxDistance = distance;
//                     furthest = current;
//                 }
//             }
//         }

//         return furthest;
//     }

//     void PlaceStartAndGoal()
//     {
//         Vector3 startPosition = new Vector3(startCell.x, 0, startCell.y);
//         Instantiate(startPrefab, startPosition, Quaternion.identity);

//         Vector3 goalPosition = new Vector3(goalCell.x, 0, goalCell.y);
//         Instantiate(goalPrefab, goalPosition, Quaternion.identity);
//     }
//     public bool IsValidMove(Vector2Int from, Vector2Int to)
// {
//     return false;
//     // Check if the move is within bounds and there's no wall
//     // You'll need to implement this based on your maze representation
// }
// }


// public class MazeGenerator : MonoBehaviour
// {
//     public GameObject cellPrefab;
//     public GameObject wallPrefab;
//     public GameObject startPrefab;
//     public GameObject goalPrefab;
//     public GameObject agentPrefab; // Reference to the agent prefab
//     public int gridSize = 8;

//     private GameObject[,] grid;
//     private bool[,] visited;
//     private List<Vector2Int> frontiers = new List<Vector2Int>();
//     private GameObject agentInstance;

//     private Vector2Int[] directions = {
//         new Vector2Int(0, 1), // North
//         new Vector2Int(1, 0), // East
//         new Vector2Int(0, -1), // South
//         new Vector2Int(-1, 0)  // West
//     };

//     public Vector2Int startCell;
//     public Vector2Int goalCell;

//     void Start()
//     {
//         GenerateGrid();
//         GenerateMaze();
//         PlaceStartAndGoal();
//         InstantiateAgent();
//     }

//     void GenerateGrid()
//     {
//         grid = new GameObject[gridSize, gridSize];
//         visited = new bool[gridSize, gridSize];

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int z = 0; z < gridSize; z++)
//             {
//                 Vector3 position = new Vector3(x, 0, z);
//                 GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
//                 grid[x, z] = cell;
//                 visited[x, z] = false;

//                 // Place walls around the cell
//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z + 1)); // North
//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x + 1, z)); // East
//                 if (z == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z - 1)); // South (only for bottom row)
//                 if (x == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x - 1, z)); // West (only for leftmost column)
//             }
//         }
//     }

//     void PlaceWall(Vector2Int current, Vector2Int neighbor)
//     {
//         Vector2Int wallDirection = neighbor - current;
//         Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);
//         Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
//         Instantiate(wallPrefab, wallPosition, wallRotation);
//     }

//     public void GenerateMaze()
//     {
//         // Start with a random cell
//         startCell = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
//         AddToMaze(startCell);

//         while (frontiers.Count > 0)
//         {
//             int randomIndex = Random.Range(0, frontiers.Count);
//             Vector2Int current = frontiers[randomIndex];
//             frontiers.RemoveAt(randomIndex);

//             List<Vector2Int> neighbors = GetVisitedNeighbors(current);
//             if (neighbors.Count > 0)
//             {
//                 Vector2Int neighbor = neighbors[Random.Range(0, neighbors.Count)];
//                 RemoveWall(current, neighbor);
//                 AddToMaze(current);
//             }
//         }

//         // Set the goal as the cell furthest from the start
//         goalCell = FindFurthestCell(startCell);
//     }

//     void AddToMaze(Vector2Int cell)
//     {
//         visited[cell.x, cell.y] = true;
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y] && !frontiers.Contains(neighbor))
//             {
//                 frontiers.Add(neighbor);
//             }
//         }
//     }

//     List<Vector2Int> GetVisitedNeighbors(Vector2Int cell)
//     {
//         List<Vector2Int> neighbors = new List<Vector2Int>();
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && visited[neighbor.x, neighbor.y])
//             {
//                 neighbors.Add(neighbor);
//             }
//         }
//         return neighbors;
//     }

//     void RemoveWall(Vector2Int current, Vector2Int neighbor)
//     {
//         Vector2Int wallDirection = neighbor - current;
//         Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f, 0.25f, current.y + wallDirection.y * 0.5f);

//         Collider[] colliders = Physics.OverlapBox(wallPosition, new Vector3(0.4f, 0.4f, 0.4f));
//         foreach (Collider collider in colliders)
//         {
//             if (collider.gameObject.CompareTag("Wall"))
//             {
//                 Destroy(collider.gameObject);
//                 break;
//             }
//         }
//     }

//     bool IsWithinBounds(Vector2Int position)
//     {
//         return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
//     }

//     Vector2Int FindFurthestCell(Vector2Int start)
//     {
//         Vector2Int furthest = start;
//         float maxDistance = 0;

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int y = 0; y < gridSize; y++)
//             {
//                 Vector2Int current = new Vector2Int(x, y);
//                 float distance = Vector2Int.Distance(start, current);
//                 if (distance > maxDistance)
//                 {
//                     maxDistance = distance;
//                     furthest = current;
//                 }
//             }
//         }

//         return furthest;
//     }

//     void PlaceStartAndGoal()
//     {
//         Vector3 startPosition = new Vector3(startCell.x, 0, startCell.y);
//         Instantiate(startPrefab, startPosition, Quaternion.identity);

//         Vector3 goalPosition = new Vector3(goalCell.x, 0, goalCell.y);
//         Instantiate(goalPrefab, goalPosition, Quaternion.identity);
//     }

//     void InstantiateAgent()
//     {
//         Vector3 agentStartPosition = new Vector3(startCell.x, 0.5f, startCell.y); // Adjust y if necessary
//         agentInstance = Instantiate(agentPrefab, agentStartPosition, Quaternion.identity);
        
//         // Optionally set up the agent, e.g., resetting its velocity or configuring other parameters
//         Rigidbody agentRb = agentInstance.GetComponent<Rigidbody>();
//         if (agentRb != null)
//         {
//             agentRb.velocity = Vector3.zero;
//             agentRb.angularVelocity = Vector3.zero;
//         }

//         // You may also want to set the initial rotation of the agent
//         agentInstance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
//     }

//     public bool IsValidMove(Vector2Int from, Vector2Int to)
//     {
//         return false;
//         // Check if the move is within bounds and there's no wall
//         // You'll need to implement this based on your maze representation
//     }
// }
// public class MazeGenerator : MonoBehaviour
// {
//     public GameObject cellPrefab;
//     public GameObject wallPrefab;
//     public GameObject startPrefab;
//     public GameObject goalPrefab;
//     public GameObject agentPrefab;
//     public int gridSize = 8;
//     public int numberOfMazes = 1; // New variable to control number of mazes

//     private GameObject[,] grid;
//     private bool[,] visited;
//     private List<Vector2Int> frontiers = new List<Vector2Int>();
//     private GameObject agentInstance;

//     private Vector2Int[] directions = {
//         new Vector2Int(0, 1), // North
//         new Vector2Int(1, 0), // East
//         new Vector2Int(0, -1), // South
//         new Vector2Int(-1, 0)  // West
//     };

//     public List<Vector2Int> startCells = new List<Vector2Int>();
//     public List<Vector2Int> goalCells = new List<Vector2Int>();

//     void Start()
//     {
//         for (int i = 0; i < numberOfMazes; i++)
//         {
//             GenerateMazeInstance(i);
//         }
//     }

    
//     void GenerateMazeInstance(int mazeIndex)
//     {
//         GenerateGrid(mazeIndex);
//         GenerateMaze(mazeIndex);
//         PlaceStartAndGoal(mazeIndex);

//             InstantiateAgent(mazeIndex);

//     }

//     void GenerateGrid(int mazeIndex)
//     {
//         grid = new GameObject[gridSize, gridSize];
//         visited = new bool[gridSize, gridSize];

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int z = 0; z < gridSize; z++)
//             {
//                 Vector3 position = new Vector3(x + (mazeIndex * gridSize), 0, z);
//                 GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
//                 grid[x, z] = cell;
//                 visited[x, z] = false;

//                 // Place walls around the cell
//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z + 1), mazeIndex); // North
//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x + 1, z), mazeIndex); // East
//                 if (z == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z - 1), mazeIndex); // South (only for bottom row)
//                 if (x == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x - 1, z), mazeIndex); // West (only for leftmost column)
//             }
//         }
//     }


//     void PlaceWall(Vector2Int current, Vector2Int neighbor, int mazeIndex)
//     {
//         Vector2Int wallDirection = neighbor - current;
//         Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f + (mazeIndex * gridSize), 0.25f, current.y + wallDirection.y * 0.5f);
//         Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
//         Instantiate(wallPrefab, wallPosition, wallRotation);
//     }

//     public void GenerateMaze(int mazeIndex)
//     {
//         frontiers.Clear();
//         Vector2Int startCell = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
//         startCells.Add(startCell);
//         AddToMaze(startCell);

//         while (frontiers.Count > 0)
//         {
//             int randomIndex = Random.Range(0, frontiers.Count);
//             Vector2Int current = frontiers[randomIndex];
//             frontiers.RemoveAt(randomIndex);

//             List<Vector2Int> neighbors = GetVisitedNeighbors(current);
//             if (neighbors.Count > 0)
//             {
//                 Vector2Int neighbor = neighbors[Random.Range(0, neighbors.Count)];
//                 RemoveWall(current, neighbor, mazeIndex);
//                 AddToMaze(current);
//             }
//         }

//         // Set the goal as the cell furthest from the start
//         Vector2Int goalCell = FindFurthestCell(startCell);
//         goalCells.Add(goalCell);
//     }

//     void AddToMaze(Vector2Int cell)
//     {
//         visited[cell.x, cell.y] = true;
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && !visited[neighbor.x, neighbor.y] && !frontiers.Contains(neighbor))
//             {
//                 frontiers.Add(neighbor);
//             }
//         }
//     }

//     List<Vector2Int> GetVisitedNeighbors(Vector2Int cell)
//     {
//         List<Vector2Int> neighbors = new List<Vector2Int>();
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && visited[neighbor.x, neighbor.y])
//             {
//                 neighbors.Add(neighbor);
//             }
//         }
//         return neighbors;
//     }

//     void RemoveWall(Vector2Int current, Vector2Int neighbor, int mazeIndex)
//     {
//         Vector2Int wallDirection = neighbor - current;
//         Vector3 wallPosition = new Vector3(current.x + wallDirection.x * 0.5f + (mazeIndex * gridSize), 0.25f, current.y + wallDirection.y * 0.5f);

//         Collider[] colliders = Physics.OverlapBox(wallPosition, new Vector3(0.4f, 0.4f, 0.4f));
//         foreach (Collider collider in colliders)
//         {
//             if (collider.gameObject.CompareTag("Wall"))
//             {
//                 Destroy(collider.gameObject);
//                 break;
//             }
//         }
//     }

//     bool IsWithinBounds(Vector2Int position)
//     {
//         return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
//     }

//     Vector2Int FindFurthestCell(Vector2Int start)
//     {
//         Vector2Int furthest = start;
//         float maxDistance = 0;

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int y = 0; y < gridSize; y++)
//             {
//                 Vector2Int current = new Vector2Int(x, y);
//                 float distance = Vector2Int.Distance(start, current);
//                 if (distance > maxDistance)
//                 {
//                     maxDistance = distance;
//                     furthest = current;
//                 }
//             }
//         }

//         return furthest;
//     }

//     void PlaceStartAndGoal(int mazeIndex)
//     {
//         Vector3 startPosition = new Vector3(startCells[mazeIndex].x + (mazeIndex * gridSize), 0, startCells[mazeIndex].y);
//         Instantiate(startPrefab, startPosition, Quaternion.identity);

//         Vector3 goalPosition = new Vector3(goalCells[mazeIndex].x + (mazeIndex * gridSize), 0, goalCells[mazeIndex].y);
//         Instantiate(goalPrefab, goalPosition, Quaternion.identity);
//     }

//     void InstantiateAgent(int mazeIndex)
//     {
//         Vector3 agentStartPosition = new Vector3(startCells[mazeIndex].x + (mazeIndex * gridSize), 0.5f, startCells[mazeIndex].y);
//         agentInstance = Instantiate(agentPrefab, agentStartPosition, Quaternion.identity);
        
//         Rigidbody agentRb = agentInstance.GetComponent<Rigidbody>();
//         if (agentRb != null)
//         {
//             agentRb.velocity = Vector3.zero;
//             agentRb.angularVelocity = Vector3.zero;
//         }

//         agentInstance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
//     }

//     public bool IsValidMove(Vector2Int from, Vector2Int to, int mazeIndex)
//     {
//         // Implement this based on your maze representation
//         // You'll need to check if there's a wall between 'from' and 'to'
//         // and if 'to' is within the bounds of the specific maze
//         return false;
//     }
// }

// public class MazeGenerator : MonoBehaviour
// {
//     public GameObject cellPrefab;
//     public GameObject wallPrefab;
//     public GameObject startPrefab;
//     public GameObject goalPrefab;
//     public GameObject agentPrefab;
//     public int gridSize = 8;
//     public int numberOfMazes = 1;

//     private List<GameObject[,]> grids = new List<GameObject[,]>();
//     private List<bool[,]> visitedCells = new List<bool[,]>();
//     private List<Vector2Int> frontiers = new List<Vector2Int>();
//     private List<GameObject> agentInstances = new List<GameObject>();

//     private Vector2Int[] directions = {
//         new Vector2Int(0, 1), // North
//         new Vector2Int(1, 0), // East
//         new Vector2Int(0, -1), // South
//         new Vector2Int(-1, 0)  // West
//     };

//     public List<Vector2Int> startCells = new List<Vector2Int>();
//     public List<Vector2Int> goalCells = new List<Vector2Int>();

//     void Start()
//     {
//         for (int i = 0; i < numberOfMazes; i++)
//         {
//             GenerateMazeInstance(i);
//         }
//     }
//     public void GenerateNewMaze(int mazeIndex)
//     {
//         if (mazeIndex < 0 || mazeIndex >= numberOfMazes)
//         {
//             Debug.LogError($"Invalid maze index: {mazeIndex}");
//             return;
//         }

//         ClearMaze(mazeIndex);
//         GenerateMazeInstance(mazeIndex);
//     }

//     void ClearMaze(int mazeIndex)
//     {
//         // Destroy all objects for this maze
//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int z = 0; z < gridSize; z++)
//             {
//                 if (grids[mazeIndex][x, z] != null)
//                 {
//                     Destroy(grids[mazeIndex][x, z]);
//                 }
//             }
//         }

//         // Clear lists
//         grids[mazeIndex] = new GameObject[gridSize, gridSize];
//         visitedCells[mazeIndex] = new bool[gridSize, gridSize];
//     }
//     void GenerateMazeInstance(int mazeIndex)
//     {
//         GenerateGrid(mazeIndex);
//         GenerateMaze(mazeIndex);
//         PlaceStartAndGoal(mazeIndex);
//         InstantiateAgent(mazeIndex);
//     }

//     void GenerateGrid(int mazeIndex)
//     {
//         GameObject[,] grid = new GameObject[gridSize, gridSize];
//         bool[,] visited = new bool[gridSize, gridSize];
//         grids.Add(grid);
//         visitedCells.Add(visited);

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int z = 0; z < gridSize; z++)
//             {
//                 Vector3 position = new Vector3(x + (mazeIndex * (gridSize + 1)), 0, z);
//                 GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
//                 grid[x, z] = cell;
//                 visited[x, z] = false;

//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z + 1), mazeIndex);
//                 PlaceWall(new Vector2Int(x, z), new Vector2Int(x + 1, z), mazeIndex);
//                 if (z == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z - 1), mazeIndex);
//                 if (x == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x - 1, z), mazeIndex);
//             }
//         }
//     }

// void PlaceWall(Vector2Int current, Vector2Int neighbor, int mazeIndex)
// {
//     Vector2Int wallDirection = neighbor - current;
//     Vector3 wallPosition = new Vector3(
//         current.x + wallDirection.x * 0.5f + (mazeIndex * (gridSize + 1)),
//         0.25f,
//         current.y + wallDirection.y * 0.5f
//     );
//     Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
//     GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
//     wall.layer = LayerMask.NameToLayer("Wall");
// }

//     public void GenerateMaze(int mazeIndex)
// {
//     frontiers.Clear();
//     Vector2Int startCell = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
    
//     // Ensure we're not overwriting existing start cells
//     if (mazeIndex >= startCells.Count)
//         startCells.Add(startCell);
//     else
//         startCells[mazeIndex] = startCell;

//     AddToMaze(startCell, mazeIndex);

//     while (frontiers.Count > 0)
//     {
//         int randomIndex = Random.Range(0, frontiers.Count);
//         Vector2Int current = frontiers[randomIndex];
//         frontiers.RemoveAt(randomIndex);

//         List<Vector2Int> neighbors = GetVisitedNeighbors(current, mazeIndex);
//         if (neighbors.Count > 0)
//         {
//             Vector2Int neighbor = neighbors[Random.Range(0, neighbors.Count)];
//             RemoveWall(current, neighbor, mazeIndex);
//             AddToMaze(current, mazeIndex);
//         }
//     }

//     Vector2Int goalCell = FindFurthestCell(startCell, mazeIndex);
    
//     // Ensure we're not overwriting existing goal cells
//     if (mazeIndex >= goalCells.Count)
//         goalCells.Add(goalCell);
//     else
//         goalCells[mazeIndex] = goalCell;
// }

//     void AddToMaze(Vector2Int cell, int mazeIndex)
//     {
//         visitedCells[mazeIndex][cell.x, cell.y] = true;
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && !visitedCells[mazeIndex][neighbor.x, neighbor.y] && !frontiers.Contains(neighbor))
//             {
//                 frontiers.Add(neighbor);
//             }
//         }
//     }

//     List<Vector2Int> GetVisitedNeighbors(Vector2Int cell, int mazeIndex)
//     {
//         List<Vector2Int> neighbors = new List<Vector2Int>();
//         foreach (Vector2Int direction in directions)
//         {
//             Vector2Int neighbor = cell + direction;
//             if (IsWithinBounds(neighbor) && visitedCells[mazeIndex][neighbor.x, neighbor.y])
//             {
//                 neighbors.Add(neighbor);
//             }
//         }
//         return neighbors;
//     }

//     void RemoveWall(Vector2Int current, Vector2Int neighbor, int mazeIndex)
//     {
//         Vector2Int wallDirection = neighbor - current;
//         Vector3 wallPosition = new Vector3(
//             current.x + wallDirection.x * 0.5f + (mazeIndex * (gridSize + 1)),
//             0.25f,
//             current.y + wallDirection.y * 0.5f
//         );

//         Collider[] colliders = Physics.OverlapBox(wallPosition, new Vector3(0.4f, 0.4f, 0.4f));
//         foreach (Collider collider in colliders)
//         {
//             if (collider.gameObject.CompareTag("Wall"))
//             {
//                 Destroy(collider.gameObject);
//                 break;
//             }
//         }
//     }

//     bool IsWithinBounds(Vector2Int position)
//     {
//         return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
//     }

//     Vector2Int FindFurthestCell(Vector2Int start, int mazeIndex)
//     {
//         Vector2Int furthest = start;
//         float maxDistance = 0;

//         for (int x = 0; x < gridSize; x++)
//         {
//             for (int y = 0; y < gridSize; y++)
//             {
//                 Vector2Int current = new Vector2Int(x, y);
//                 float distance = Vector2Int.Distance(start, current);
//                 if (distance > maxDistance)
//                 {
//                     maxDistance = distance;
//                     furthest = current;
//                 }
//             }
//         }

//         return furthest;
//     }


// void InstantiateAgent(int mazeIndex)
// {
//     if (mazeIndex >= startCells.Count)
//     {
//         Debug.LogError($"Invalid mazeIndex: {mazeIndex}. Not enough start cells defined.");
//         return;
//     }

//     Vector3 agentStartPosition = new Vector3(startCells[mazeIndex].x + (mazeIndex * (gridSize + 1)), 0.5f, startCells[mazeIndex].y);
//     GameObject agentInstance = Instantiate(agentPrefab, agentStartPosition, Quaternion.identity);
//     agentInstances.Add(agentInstance);
    
//     if (agentInstance.TryGetComponent(out Rigidbody agentRb))
//     {
//         agentRb.velocity = Vector3.zero;
//         agentRb.angularVelocity = Vector3.zero;
//     }
//     else
//     {
//         Debug.LogWarning("Rigidbody component not found on the agent prefab.");
//     }

//     agentInstance.transform.rotation = Quaternion.identity;

//     if (agentInstance.TryGetComponent(out MazeAgent mazeAgent))
//     {
//         mazeAgent.SetupAgent(mazeIndex, this);
//         Debug.Log($"Agent instantiated for maze {mazeIndex} at position {agentStartPosition}");
//     }
//     else
//     {
//         Debug.LogError("MazeAgent component not found on the instantiated agent prefab.");
//     }
// }

// void PlaceStartAndGoal(int mazeIndex)
// {
//     Vector3 startPosition = new Vector3(startCells[mazeIndex].x + (mazeIndex * (gridSize + 1)), 0, startCells[mazeIndex].y);
//     Vector3 goalPosition = new Vector3(goalCells[mazeIndex].x + (mazeIndex * (gridSize + 1)), 0, goalCells[mazeIndex].y);

//     // Destroy existing start and goal objects if they exist
//     GameObject existingStart = GameObject.FindGameObjectWithTag("Start" + mazeIndex);
//     GameObject existingGoal = GameObject.FindGameObjectWithTag("Goal" + mazeIndex);
    
//     if (existingStart != null) Destroy(existingStart);
//     if (existingGoal != null) Destroy(existingGoal);

//     // Create new start and goal objects
//     GameObject start = Instantiate(startPrefab, startPosition, Quaternion.identity);
//     GameObject goal = Instantiate(goalPrefab, goalPosition, Quaternion.identity);

//     // Tag them for easy finding later
//     start.tag = "Start" + mazeIndex;
//     goal.tag = "Goal" + mazeIndex;
// }
// bool WallExists(Vector2Int from, Vector2Int to, int mazeIndex)
// {
//     Vector2Int difference = to - from;
//     Vector3 wallCenter = new Vector3(
//         from.x + difference.x * 0.5f + (mazeIndex * (gridSize + 1)),
//         0.25f,
//         from.y + difference.y * 0.5f
//     );

//     Collider[] colliders = Physics.OverlapBox(wallCenter, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, LayerMask.GetMask("Wall"));
//     return colliders.Length > 0;
// }
//     void UpdateAgent(int mazeIndex)
//     {
//         if (mazeIndex >= agentInstances.Count)
//         {
//             InstantiateAgent(mazeIndex);
//         }
//         else
//         {
//             GameObject agentInstance = agentInstances[mazeIndex];
//             Vector3 agentStartPosition = new Vector3(startCells[mazeIndex].x + (mazeIndex * (gridSize + 1)), 0.5f, startCells[mazeIndex].y);
//             agentInstance.transform.position = agentStartPosition;
//             agentInstance.transform.rotation = Quaternion.identity;

//             if (agentInstance.TryGetComponent(out Rigidbody agentRb))
//             {
//                 agentRb.velocity = Vector3.zero;
//                 agentRb.angularVelocity = Vector3.zero;
//             }

//             if (agentInstance.TryGetComponent(out MazeAgent mazeAgent))
//             {
//                 mazeAgent.ResetAgent();
//             }
//         }
//     }

// public bool IsValidMove(Vector2Int from, Vector2Int to, int mazeIndex)
// {
//     if (!IsWithinBounds(to))
//         return false;

//     Vector2Int difference = to - from;
//     if (Mathf.Abs(difference.x) + Mathf.Abs(difference.y) != 1)
//         return false; // Can only move to adjacent cells

//     return !WallExists(from, to, mazeIndex);
// }//ds
//     public Vector2Int GetStartCell(int mazeIndex)
//     {
//         return startCells[mazeIndex];
//     }

//     public Vector2Int GetGoalCell(int mazeIndex)
//     {
//         return goalCells[mazeIndex];
//     }
// }
public class MazeGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject wallPrefab;
    public GameObject startPrefab;
    public GameObject goalPrefab;
    public GameObject agentPrefab;
    public int gridSize = 8;
    public int numberOfMazes = 1;

    private List<GameObject[,]> grids = new List<GameObject[,]>();
    private List<bool[,]> visitedCells = new List<bool[,]>();
    private List<Vector2Int> frontiers = new List<Vector2Int>();
    private List<GameObject> agentInstances = new List<GameObject>();

    private Vector2Int[] directions = {
        new Vector2Int(0, 1), // North
        new Vector2Int(1, 0), // East
        new Vector2Int(0, -1), // South
        new Vector2Int(-1, 0)  // West
    };

    public List<Vector2Int> startCells = new List<Vector2Int>();
    public List<Vector2Int> goalCells = new List<Vector2Int>();

    void Start()
    {
        for (int i = 0; i < numberOfMazes; i++)
        {
            GenerateMazeInstance(i);
        }
    }

    void GenerateMazeInstance(int mazeIndex)
    {
        GenerateGrid(mazeIndex);
        GenerateMaze(mazeIndex);
        PlaceStartAndGoal(mazeIndex);
        InstantiateAgent(mazeIndex);
    }

    void GenerateGrid(int mazeIndex)
    {
        GameObject[,] grid = new GameObject[gridSize, gridSize];
        bool[,] visited = new bool[gridSize, gridSize];
        grids.Add(grid);
        visitedCells.Add(visited);

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x + (mazeIndex * (gridSize + 1)), 0, z);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                grid[x, z] = cell;
                visited[x, z] = false;

                PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z + 1), mazeIndex);
                PlaceWall(new Vector2Int(x, z), new Vector2Int(x + 1, z), mazeIndex);
                if (z == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x, z - 1), mazeIndex);
                if (x == 0) PlaceWall(new Vector2Int(x, z), new Vector2Int(x - 1, z), mazeIndex);
            }
        }
    }

void PlaceWall(Vector2Int current, Vector2Int neighbor, int mazeIndex)
{
    Vector2Int wallDirection = neighbor - current;
    Vector3 wallPosition = new Vector3(
        current.x + wallDirection.x * 0.5f + (mazeIndex * (gridSize + 1)),
        0.25f,
        current.y + wallDirection.y * 0.5f
    );
    Quaternion wallRotation = (wallDirection.x != 0) ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
    GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation);
    wall.layer = LayerMask.NameToLayer("Wall");
}

    public void GenerateMaze(int mazeIndex)
    {
        frontiers.Clear();
        Vector2Int startCell = new Vector2Int(Random.Range(0, gridSize), Random.Range(0, gridSize));
        startCells.Add(startCell);
        AddToMaze(startCell, mazeIndex);

        while (frontiers.Count > 0)
        {
            int randomIndex = Random.Range(0, frontiers.Count);
            Vector2Int current = frontiers[randomIndex];
            frontiers.RemoveAt(randomIndex);

            List<Vector2Int> neighbors = GetVisitedNeighbors(current, mazeIndex);
            if (neighbors.Count > 0)
            {
                Vector2Int neighbor = neighbors[Random.Range(0, neighbors.Count)];
                RemoveWall(current, neighbor, mazeIndex);
                AddToMaze(current, mazeIndex);
            }
        }

        Vector2Int goalCell = FindFurthestCell(startCell, mazeIndex);
        goalCells.Add(goalCell);
    }

    void AddToMaze(Vector2Int cell, int mazeIndex)
    {
        visitedCells[mazeIndex][cell.x, cell.y] = true;
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = cell + direction;
            if (IsWithinBounds(neighbor) && !visitedCells[mazeIndex][neighbor.x, neighbor.y] && !frontiers.Contains(neighbor))
            {
                frontiers.Add(neighbor);
            }
        }
    }

    List<Vector2Int> GetVisitedNeighbors(Vector2Int cell, int mazeIndex)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = cell + direction;
            if (IsWithinBounds(neighbor) && visitedCells[mazeIndex][neighbor.x, neighbor.y])
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    void RemoveWall(Vector2Int current, Vector2Int neighbor, int mazeIndex)
    {
        Vector2Int wallDirection = neighbor - current;
        Vector3 wallPosition = new Vector3(
            current.x + wallDirection.x * 0.5f + (mazeIndex * (gridSize + 1)),
            0.25f,
            current.y + wallDirection.y * 0.5f
        );

        Collider[] colliders = Physics.OverlapBox(wallPosition, new Vector3(0.4f, 0.4f, 0.4f));
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Wall"))
            {
                Destroy(collider.gameObject);
                break;
            }
        }
    }

    bool IsWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < gridSize && position.y >= 0 && position.y < gridSize;
    }

    Vector2Int FindFurthestCell(Vector2Int start, int mazeIndex)
    {
        Vector2Int furthest = start;
        float maxDistance = 0;

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector2Int current = new Vector2Int(x, y);
                float distance = Vector2Int.Distance(start, current);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    furthest = current;
                }
            }
        }

        return furthest;
    }

    void PlaceStartAndGoal(int mazeIndex)
    {
        Vector3 startPosition = new Vector3(startCells[mazeIndex].x + (mazeIndex * (gridSize + 1)), 0, startCells[mazeIndex].y);
        Instantiate(startPrefab, startPosition, Quaternion.identity);

        Vector3 goalPosition = new Vector3(goalCells[mazeIndex].x + (mazeIndex * (gridSize + 1)), 0.5f, goalCells[mazeIndex].y);
        Instantiate(goalPrefab, goalPosition, Quaternion.identity);
    }

void InstantiateAgent(int mazeIndex)
{
    if (mazeIndex >= startCells.Count)
    {
        Debug.LogError($"Invalid mazeIndex: {mazeIndex}. Not enough start cells defined.");
        return;
    }

    Vector3 agentStartPosition = new Vector3(startCells[mazeIndex].x + (mazeIndex * (gridSize + 1)), 0.5f, startCells[mazeIndex].y);
    GameObject agentInstance = Instantiate(agentPrefab, agentStartPosition, Quaternion.identity);
    agentInstances.Add(agentInstance);
    
    if (agentInstance.TryGetComponent(out Rigidbody agentRb))
    {
        agentRb.velocity = Vector3.zero;
        agentRb.angularVelocity = Vector3.zero;
    }
    else
    {
        Debug.LogWarning("Rigidbody component not found on the agent prefab.");
    }

    agentInstance.transform.rotation = Quaternion.identity;

    if (agentInstance.TryGetComponent(out MazeAgent mazeAgent))
    {
        mazeAgent.SetupAgent(mazeIndex, this);
        Debug.Log($"Agent instantiated for maze {mazeIndex} at position {agentStartPosition}");
    }
    else
    {
        Debug.LogError("MazeAgent component not found on the instantiated agent prefab.");
    }
}

public bool IsValidMove(Vector2Int from, Vector2Int to, int mazeIndex)
{
    if (!IsWithinBounds(to))
        return false;

    Vector2Int difference = to - from;
    if (Mathf.Abs(difference.x) + Mathf.Abs(difference.y) != 1)
        return false; // Can only move to adjacent cells

    Vector3 wallCenter = new Vector3(
        from.x + difference.x * 0.5f + (mazeIndex * (gridSize + 1)),
        0.25f, // Adjust this height based on your wall height
        from.y + difference.y * 0.5f
    );

    // Use a smaller radius for more precise detection
    Collider[] colliders = Physics.OverlapSphere(wallCenter, 0.1f, LayerMask.GetMask("Wall"));
    return colliders.Length == 0;
}
    public Vector2Int GetStartCell(int mazeIndex)
    {
        return startCells[mazeIndex];
    }

    public Vector2Int GetGoalCell(int mazeIndex)
    {
        return goalCells[mazeIndex];
    }
}