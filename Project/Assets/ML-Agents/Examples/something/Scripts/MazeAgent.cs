/*using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MazeAgent : Agent
{
    public MazeGenerator mazeGenerator;
    private Vector2Int currentPosition;

    public override void OnEpisodeBegin()
    {
        // Generate a new maze or reset the existing one
        mazeGenerator.GenerateMaze();
        currentPosition = mazeGenerator.startCell;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe current position
        sensor.AddObservation(currentPosition.x);
        sensor.AddObservation(currentPosition.y);

        // Observe goal position
        sensor.AddObservation(mazeGenerator.goalCell.x);
        sensor.AddObservation(mazeGenerator.goalCell.y);

        // Observe surrounding walls (optional, depends on your maze representation)
        // You might want to observe the state of neighboring cells here
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the action (0 = up, 1 = right, 2 = down, 3 = left)
        int action = actions.DiscreteActions[0];

        Vector2Int newPosition = currentPosition;
        switch (action)
        {
            case 0: newPosition += Vector2Int.up; break;
            case 1: newPosition += Vector2Int.right; break;
            case 2: newPosition += Vector2Int.down; break;
            case 3: newPosition += Vector2Int.left; break;
        }

        // Check if the move is valid and update position
        if (mazeGenerator.IsValidMove(currentPosition, newPosition))
        {
            currentPosition = newPosition;
        }
        else
        {
            // Penalize invalid moves
            AddReward(-0.1f);
        }

        // Check if reached the goal
        if (currentPosition == mazeGenerator.goalCell)
        {
            AddReward(1.0f);
            EndEpisode();
        }

        // Small penalty for each step to encourage finding shortest path
        AddReward(-0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Implement keyboard controls for manual testing
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.UpArrow)) discreteActionsOut[0] = 0;
        else if (Input.GetKey(KeyCode.RightArrow)) discreteActionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.DownArrow)) discreteActionsOut[0] = 2;
        else if (Input.GetKey(KeyCode.LeftArrow)) discreteActionsOut[0] = 3;
    }
}
*/
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MazeAgent : Agent
{
    public GameObject ground;
    public GameObject area;
    public GameObject goal;
    public bool useVectorObs;
    Rigidbody m_AgentRb;
    Material m_GroundMaterial;
    Renderer m_GroundRenderer;
    MazeSettings m_MazeSettings;
    StatsRecorder m_statsRecorder;
    public float moveSpeed = 5f; // Speed of the agent's movement
    public float rotateSpeed = 180f; // Speed of the agent's rotation

    public override void Initialize()
    {
        m_MazeSettings = FindObjectOfType<MazeSettings>();
        m_AgentRb = GetComponent<Rigidbody>();
        m_GroundRenderer = ground.GetComponent<Renderer>();
        m_GroundMaterial = m_GroundRenderer.material;
        m_statsRecorder = Academy.Instance.StatsRecorder;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            // Agent's position and rotation
            Vector3 normalizedPosition = (transform.position - ground.transform.position) / m_MazeSettings.mazeSize;
            sensor.AddObservation(normalizedPosition);
            sensor.AddObservation(transform.rotation.eulerAngles.y / 360f);

            // Direction and distance to goal
            Vector3 directionToGoal = (goal.transform.position - transform.position).normalized;
            float distanceToGoal = Vector3.Distance(transform.position, goal.transform.position) / m_MazeSettings.mazeSize;
            sensor.AddObservation(directionToGoal);
            sensor.AddObservation(distanceToGoal);

            // Agent's velocity
            sensor.AddObservation(m_AgentRb.velocity / moveSpeed);

            // Wall detection
            AddWallObservations(sensor);
        }
    }

    IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
    {
        m_GroundRenderer.material = mat;
        yield return new WaitForSeconds(time);
        m_GroundRenderer.material = m_GroundMaterial;
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1:
                dirToGo = transform.forward * 1f;
                break;
            case 2:
                dirToGo = transform.forward * -1f;
                break;
            case 3:
                rotateDir = transform.up * 1f;
                break;
            case 4:
                rotateDir = transform.up * -1f;
                break;
        }
        transform.Rotate(rotateDir, Time.deltaTime * 150f);
        m_AgentRb.AddForce(dirToGo * m_MazeSettings.agentRunSpeed, ForceMode.VelocityChange);
    }

    private void AddWallObservations(VectorSensor sensor)
{
    float rayDistance = 2f;
    int rayCount = 8;

    for (int i = 0; i < rayCount; i++)
    {
        float angle = i * (360f / rayCount);
        Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            sensor.AddObservation(hit.distance / rayDistance);
        }
        else
        {
            sensor.AddObservation(1.0f);
        }
    }
}


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var action = actionBuffers.DiscreteActions[0];
        Vector3 moveDir = Vector3.zero;
        float rotateAmount = 0f;

        switch (action)
        {
            case 1: // Move forward
                moveDir = transform.forward;
                break;
            case 2: // Move backward
                moveDir = -transform.forward;
                break;
            case 3: // Rotate right
                rotateAmount = 1f;
                break;
            case 4: // Rotate left
                rotateAmount = -1f;
                break;
        }

        // Check for wall collision before moving
        bool hitWall = false;
        if (moveDir != Vector3.zero)
        {
            if (Physics.Raycast(transform.position, moveDir, moveSpeed * Time.fixedDeltaTime, LayerMask.GetMask("Wall")))
            {
                hitWall = true;
                AddReward(-0.5f); // Significant penalty for hitting a wall
                Debug.Log("Hit Wall!");
            }
            else
            {
                // Move only if not hitting a wall
                Vector3 newPosition = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;
                m_AgentRb.MovePosition(newPosition);
            }
        }

        // Apply rotation
        if (rotateAmount != 0f)
        {
            float rotationAngle = rotateAmount * rotateSpeed * Time.fixedDeltaTime;
            transform.Rotate(0f, rotationAngle, 0f);
            AddReward(0.01f); // Small reward for rotation to encourage exploration
        }

        // Calculate reward based on distance to goal
        float distanceToGoal = Vector3.Distance(transform.position, goal.transform.position);
        float previousDistanceToGoal = Vector3.Distance(transform.position - moveDir * moveSpeed * Time.fixedDeltaTime, goal.transform.position);
        float distanceReward = previousDistanceToGoal - distanceToGoal;

        // Add distance-based reward only if not hitting a wall
        if (!hitWall)
        {
            AddReward(distanceReward * 0.1f);
        }

        // Small penalty for each step to encourage efficiency
        AddReward(-0.001f);

        // Encourage exploration by giving a small reward for new positions
        if (!IsPositionVisited(transform.position))
        {
            visitedPositions.Add(transform.position);
            AddReward(0.05f);
            Debug.Log("Visited new position!");
        }

        // Debug logging
        Debug.Log($"Action: {action}, Move: {moveDir}, Rotate: {rotateAmount}, Hit Wall: {hitWall}, Reward: {GetCumulativeReward()}");
    }

    private HashSet<Vector3> visitedPositions = new HashSet<Vector3>();

    private bool IsPositionVisited(Vector3 position)
    {
        float threshold = 0.1f; // Adjust this value based on your maze scale
        foreach (Vector3 visitedPos in visitedPositions)
        {
            if (Vector3.Distance(position, visitedPos) < threshold)
            {
                return true;
            }
        }
        return false;
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Goal"))
        {
            SetReward(1f);
            StartCoroutine(GoalScoredSwapGroundMaterial(m_MazeSettings.goalScoredMaterial, 0.5f));
            m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
        else if (col.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.1f); // Penalize for hitting walls
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
    }

    public override void OnEpisodeBegin()
    {
        visitedPositions.Clear();
        // Randomize agent start position within the maze
        // transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f))
        // + ground.transform.position;
        //transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        // m_AgentRb.velocity *= 0f;

        // Randomize goal position within the maze
        //goal.transform.position = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f))
        //  + area.transform.position;

        //m_statsRecorder.Add("Goal/Reached", 0, StatAggregationMethod.Sum);
    }
    private void FixedUpdate()
    {
        // If the agent hasn't moved significantly in a while, nudge it
        if (m_AgentRb.velocity.magnitude < 0.01f)
        {
            stuckTime += Time.fixedDeltaTime;
            if (stuckTime > 3f) // If stuck for more than 3 seconds
            {
                Vector3 randomDirection = Random.insideUnitSphere;
                randomDirection.y = 0;
                m_AgentRb.AddForce(randomDirection.normalized * moveSpeed, ForceMode.VelocityChange);
                stuckTime = 0f;
            }
        }
        else
        {
            stuckTime = 0f;
        }
    }

    private float stuckTime = 0f;
}

// using System.Collections.Generic;
// using System.Collections;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MazeAgent : Agent
// {
//     public GameObject goal;
//     public bool useVectorObs;
//     private Rigidbody m_AgentRb;
//     private MazeGenerator m_MazeGenerator;
//     private StatsRecorder m_statsRecorder;
//     public float moveSpeed = 5f;
//     public float rotateSpeed = 180f;

//     private int mazeIndex;
//     private Vector2Int currentCell;
//     private HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();

//     public void SetupAgent(int index, MazeGenerator generator)
//     {
//         mazeIndex = index;
//         m_MazeGenerator = generator;
//         currentCell = m_MazeGenerator.GetStartCell(mazeIndex);
//         transform.position = CellToWorldPosition(currentCell);
//     }

//     public override void Initialize()
//     {
//         m_AgentRb = GetComponent<Rigidbody>();
//         m_statsRecorder = Academy.Instance.StatsRecorder;
//     }

// public override void CollectObservations(VectorSensor sensor)
// {
//     if (useVectorObs)
//     {
//         // Normalized position within the maze
//         sensor.AddObservation((float)currentCell.x / m_MazeGenerator.gridSize);
//         sensor.AddObservation((float)currentCell.y / m_MazeGenerator.gridSize);

//         // Normalized rotation
//         sensor.AddObservation(transform.rotation.eulerAngles.y / 360f);

//         // Direction and distance to goal
//         Vector2Int goalCell = m_MazeGenerator.GetGoalCell(mazeIndex);
//         Vector2 directionToGoal = ((Vector2)(goalCell - currentCell)).normalized;
//         float distanceToGoal = Vector2Int.Distance(currentCell, goalCell) / m_MazeGenerator.gridSize;
//         sensor.AddObservation(directionToGoal.x);
//         sensor.AddObservation(directionToGoal.y);
//         sensor.AddObservation(distanceToGoal);

//         // Wall detection
//         AddWallObservations(sensor);
//     }
// }
//     private void AddWallObservations(VectorSensor sensor)
//     {
//         foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
//         {
//             sensor.AddObservation(m_MazeGenerator.IsValidMove(currentCell, currentCell + direction, mazeIndex) ? 1 : 0);
//         }
//     }

//     public override void OnActionReceived(ActionBuffers actionBuffers)
//     {
//         var action = actionBuffers.DiscreteActions[0];
//         Vector2Int moveDir = Vector2Int.zero;
//         float rotateAmount = 0f;

//         switch (action)
//         {
//             case 0: moveDir = Vector2Int.up; break;
//             case 1: moveDir = Vector2Int.down; break;
//             case 2: moveDir = Vector2Int.left; break;
//             case 3: moveDir = Vector2Int.right; break;
//             case 4: rotateAmount = 1f; break;
//             case 5: rotateAmount = -1f; break;
//         }

//     // Move
//     if (moveDir != Vector2Int.zero)
//     {
//         Vector2Int newCell = currentCell + moveDir;
//         bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);
        
//         // Debug visualization
//         Debug.DrawLine(CellToWorldPosition(currentCell), CellToWorldPosition(newCell), isValidMove ? Color.green : Color.red, 1f);

//         if (isValidMove)
//         {
//             currentCell = newCell;
//             Vector3 newPosition = CellToWorldPosition(currentCell);
//             m_AgentRb.MovePosition(newPosition);
            
//             if (!visitedPositions.Contains(currentCell))
//             {
//                 visitedPositions.Add(currentCell);
//                 AddReward(0.05f);
//                 Debug.Log("Visited new position!");
//             }

//             if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex))
//             {
//                 SetReward(1f);
//                 m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//                 EndEpisode();
//             }
//         }
//         else
//         {
//             AddReward(-0.1f); // Penalty for hitting a wall
//             Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//         }
//     }
//     // Rotate
//     if (rotateAmount != 0f)
//     {
//         float rotationAngle = rotateAmount * rotateSpeed * Time.fixedDeltaTime;
//         transform.Rotate(0f, rotationAngle, 0f);
//         AddReward(0.01f); // Small reward for rotation to encourage exploration
//     }

//     // Small penalty for each step to encourage efficiency
//     AddReward(-0.001f);

//     Debug.Log($"Action: {action}, Move: {moveDir}, Rotate: {rotateAmount}, Reward: {GetCumulativeReward()}");
// }


//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         var discreteActionsOut = actionsOut.DiscreteActions;
//         if (Input.GetKey(KeyCode.W)) discreteActionsOut[0] = 0;
//         else if (Input.GetKey(KeyCode.S)) discreteActionsOut[0] = 1;
//         else if (Input.GetKey(KeyCode.A)) discreteActionsOut[0] = 2;
//         else if (Input.GetKey(KeyCode.D)) discreteActionsOut[0] = 3;
//         else if (Input.GetKey(KeyCode.Q)) discreteActionsOut[0] = 4;
//         else if (Input.GetKey(KeyCode.E)) discreteActionsOut[0] = 5;
//         else discreteActionsOut[0] = 6; // No-op action
//     }

//     public override void OnEpisodeBegin()
//     {
//         if (m_MazeGenerator == null)
//         {
//             Debug.LogError("MazeGenerator is not set. Make sure SetupAgent is called before starting the episode.");
//             return;
//         }
//         visitedPositions.Clear();
//         Debug.Log(m_MazeGenerator.GetStartCell(mazeIndex) == null);
//         currentCell = m_MazeGenerator.GetStartCell(mazeIndex);
//         transform.position = CellToWorldPosition(currentCell);
//         transform.rotation = Quaternion.identity;
//         if (m_AgentRb != null)
//         {
//             m_AgentRb.velocity = Vector3.zero;
//             m_AgentRb.angularVelocity = Vector3.zero;
//         }
//         else
//         {
//             Debug.LogWarning("Rigidbody component is missing on the agent.");
//         }

//         m_statsRecorder.Add("Goal/Reached", 0, StatAggregationMethod.Sum);
//     }

// private Vector3 CellToWorldPosition(Vector2Int cell)
// {
//     return new Vector3(
//         cell.x + (mazeIndex * (m_MazeGenerator.gridSize + 1)),
//         transform.position.y, // Keep the current y position
//         cell.y
//     );
// }
//     private void FixedUpdate()
//     {
//         // If the agent hasn't moved significantly in a while, nudge it
//         if (m_AgentRb.velocity.magnitude < 0.01f)
//         {
//             stuckTime += Time.fixedDeltaTime;
//             if (stuckTime > 3f) // If stuck for more than 3 seconds
//             {
//                 Vector3 randomDirection = Random.insideUnitSphere;
//                 randomDirection.y = 0;
//                 m_AgentRb.AddForce(randomDirection.normalized * moveSpeed, ForceMode.VelocityChange);
//                 stuckTime = 0f;
//             }
//         }
//         else
//         {
//             stuckTime = 0f;
//         }
//     }

//     private float stuckTime = 0f;
// }