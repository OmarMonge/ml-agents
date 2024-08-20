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
// using System.Collections.Generic;
// using System.Collections;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MazeAgent : Agent
// {
//     public GameObject ground;
//     public GameObject area;
//     public GameObject goal;
//     public bool useVectorObs;
//     Rigidbody m_AgentRb;
//     Material m_GroundMaterial;
//     Renderer m_GroundRenderer;
//     MazeSettings m_MazeSettings;
//     StatsRecorder m_statsRecorder;
//     public float moveSpeed = 5f; // Speed of the agent's movement
//     public float rotateSpeed = 180f; // Speed of the agent's rotation

//     public override void Initialize()
//     {
//         m_MazeSettings = FindObjectOfType<MazeSettings>();
//         m_AgentRb = GetComponent<Rigidbody>();
//         m_GroundRenderer = ground.GetComponent<Renderer>();
//         m_GroundMaterial = m_GroundRenderer.material;
//         m_statsRecorder = Academy.Instance.StatsRecorder;
//     }

//     public override void CollectObservations(VectorSensor sensor)
//     {
//         if (useVectorObs)
//         {
//             // Agent's position and rotation
//             Vector3 normalizedPosition = (transform.position - ground.transform.position) / m_MazeSettings.mazeSize;
//             sensor.AddObservation(normalizedPosition);
//             sensor.AddObservation(transform.rotation.eulerAngles.y / 360f);

//             // Direction and distance to goal
//             Vector3 directionToGoal = (goal.transform.position - transform.position).normalized;
//             float distanceToGoal = Vector3.Distance(transform.position, goal.transform.position) / m_MazeSettings.mazeSize;
//             sensor.AddObservation(directionToGoal);
//             sensor.AddObservation(distanceToGoal);

//             // Agent's velocity
//             sensor.AddObservation(m_AgentRb.velocity / moveSpeed);

//             // Wall detection
//             AddWallObservations(sensor);
//         }
//     }

//     IEnumerator GoalScoredSwapGroundMaterial(Material mat, float time)
//     {
//         m_GroundRenderer.material = mat;
//         yield return new WaitForSeconds(time);
//         m_GroundRenderer.material = m_GroundMaterial;
//     }

//     public void MoveAgent(ActionSegment<int> act)
//     {
//         var dirToGo = Vector3.zero;
//         var rotateDir = Vector3.zero;

//         var action = act[0];
//         switch (action)
//         {
//             case 1:
//                 dirToGo = transform.forward * 1f;
//                 break;
//             case 2:
//                 dirToGo = transform.forward * -1f;
//                 break;
//             case 3:
//                 rotateDir = transform.up * 1f;
//                 break;
//             case 4:
//                 rotateDir = transform.up * -1f;
//                 break;
//         }
//         transform.Rotate(rotateDir, Time.deltaTime * 150f);
//         m_AgentRb.AddForce(dirToGo * m_MazeSettings.agentRunSpeed, ForceMode.VelocityChange);
//     }

//     private void AddWallObservations(VectorSensor sensor)
// {
//     float rayDistance = 2f;
//     int rayCount = 8;

//     for (int i = 0; i < rayCount; i++)
//     {
//         float angle = i * (360f / rayCount);
//         Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
//         Ray ray = new Ray(transform.position, direction);
//         RaycastHit hit;

//         if (Physics.Raycast(ray, out hit, rayDistance))
//         {
//             sensor.AddObservation(hit.distance / rayDistance);
//         }
//         else
//         {
//             sensor.AddObservation(1.0f);
//         }
//     }
// }


//     public override void OnActionReceived(ActionBuffers actionBuffers)
//     {
//         var action = actionBuffers.DiscreteActions[0];
//         Vector3 moveDir = Vector3.zero;
//         float rotateAmount = 0f;

//         switch (action)
//         {
//             case 1: // Move forward
//                 moveDir = transform.forward;
//                 break;
//             case 2: // Move backward
//                 moveDir = -transform.forward;
//                 break;
//             case 3: // Rotate right
//                 rotateAmount = 1f;
//                 break;
//             case 4: // Rotate left
//                 rotateAmount = -1f;
//                 break;
//         }

//         // Check for wall collision before moving
//         bool hitWall = false;
//         if (moveDir != Vector3.zero)
//         {
//             if (Physics.Raycast(transform.position, moveDir, moveSpeed * Time.fixedDeltaTime, LayerMask.GetMask("Wall")))
//             {
//                 hitWall = true;
//                 AddReward(-0.5f); // Significant penalty for hitting a wall
//                 Debug.Log("Hit Wall!");
//             }
//             else
//             {
//                 // Move only if not hitting a wall
//                 Vector3 newPosition = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;
//                 m_AgentRb.MovePosition(newPosition);
//             }
//         }

//         // Apply rotation
//         if (rotateAmount != 0f)
//         {
//             float rotationAngle = rotateAmount * rotateSpeed * Time.fixedDeltaTime;
//             transform.Rotate(0f, rotationAngle, 0f);
//             AddReward(0.01f); // Small reward for rotation to encourage exploration
//         }

//         // Calculate reward based on distance to goal
//         float distanceToGoal = Vector3.Distance(transform.position, goal.transform.position);
//         float previousDistanceToGoal = Vector3.Distance(transform.position - moveDir * moveSpeed * Time.fixedDeltaTime, goal.transform.position);
//         float distanceReward = previousDistanceToGoal - distanceToGoal;

//         // Add distance-based reward only if not hitting a wall
//         if (!hitWall)
//         {
//             AddReward(distanceReward * 0.1f);
//         }

//         // Small penalty for each step to encourage efficiency
//         AddReward(-0.001f);

//         // Encourage exploration by giving a small reward for new positions
//         if (!IsPositionVisited(transform.position))
//         {
//             visitedPositions.Add(transform.position);
//             AddReward(0.05f);
//             Debug.Log("Visited new position!");
//         }

//         // Debug logging
//         Debug.Log($"Action: {action}, Move: {moveDir}, Rotate: {rotateAmount}, Hit Wall: {hitWall}, Reward: {GetCumulativeReward()}");
//     }

//     private HashSet<Vector3> visitedPositions = new HashSet<Vector3>();

//     private bool IsPositionVisited(Vector3 position)
//     {
//         float threshold = 0.1f; // Adjust this value based on your maze scale
//         foreach (Vector3 visitedPos in visitedPositions)
//         {
//             if (Vector3.Distance(position, visitedPos) < threshold)
//             {
//                 return true;
//             }
//         }
//         return false;
//     }


//     void OnCollisionEnter(Collision col)
//     {
//         if (col.gameObject.CompareTag("Goal"))
//         {
//             SetReward(1f);
//             StartCoroutine(GoalScoredSwapGroundMaterial(m_MazeSettings.goalScoredMaterial, 0.5f));
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//         else if (col.gameObject.CompareTag("Wall"))
//         {
//             AddReward(-0.1f); // Penalize for hitting walls
//         }
//     }

//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         var discreteActionsOut = actionsOut.DiscreteActions;
//         if (Input.GetKey(KeyCode.D))
//         {
//             discreteActionsOut[0] = 3;
//         }
//         else if (Input.GetKey(KeyCode.W))
//         {
//             discreteActionsOut[0] = 1;
//         }
//         else if (Input.GetKey(KeyCode.A))
//         {
//             discreteActionsOut[0] = 4;
//         }
//         else if (Input.GetKey(KeyCode.S))
//         {
//             discreteActionsOut[0] = 2;
//         }
//     }

//     public override void OnEpisodeBegin()
//     {
//         visitedPositions.Clear();
//         // Randomize agent start position within the maze
//         // transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f))
//         // + ground.transform.position;
//         //transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
//         // m_AgentRb.velocity *= 0f;

//         // Randomize goal position within the maze
//         //goal.transform.position = new Vector3(Random.Range(-3f, 3f), 0.5f, Random.Range(-3f, 3f))
//         //  + area.transform.position;

//         //m_statsRecorder.Add("Goal/Reached", 0, StatAggregationMethod.Sum);
//     }
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

// public override void OnActionReceived(ActionBuffers actionBuffers)
// {
//     int action = actionBuffers.DiscreteActions[0];
//     Vector3 moveDir = Vector3.zero;
//     float rotateAmount = 0f;

//     switch (action)
//     {
//         case 0: moveDir = transform.forward; break;
//         case 1: moveDir = -transform.forward; break;
//         case 2: moveDir = -transform.right; break;
//         case 3: moveDir = transform.right; break;
//         case 4: rotateAmount = -1f; break;
//         case 5: rotateAmount = 1f; break;
//         case 6: break; // No action
//         default: Debug.LogWarning($"Unexpected action value: {action}"); break;
//     }

//     // Move
//     // if (moveDir != Vector2Int.zero)
//     // {
//     //     Vector2Int newCell = currentCell + moveDir;
//     //     bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);
        
//     //     // Debug visualization
//     //     Debug.DrawLine(CellToWorldPosition(currentCell), CellToWorldPosition(newCell), isValidMove ? Color.green : Color.red, 1f);

//     //     if (isValidMove)
//     //     {
//     //         currentCell = newCell;
//     //         Vector3 newPosition = CellToWorldPosition(currentCell);
//     //         m_AgentRb.MovePosition(newPosition);
            
//     //         if (!visitedPositions.Contains(currentCell))
//     //         {
//     //             visitedPositions.Add(currentCell);
//     //             AddReward(0.05f);
//     //             Debug.Log("Visited new position!");
//     //         }

//     //         if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex))
//     //         {
//     //             SetReward(1f);
//     //             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//     //             EndEpisode();
//     //         }
//     //     }
//     //     else
//     //     {
//     //         AddReward(-0.1f); // Penalty for hitting a wall
//     //         Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//     //     }
//     // }
//         // Goal and wall interaction
//     Vector2Int newCell = currentCell + new Vector2Int(Mathf.RoundToInt(moveDir.x), Mathf.RoundToInt(moveDir.z));
//     bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

//     if (isValidMove)
//     {
//         currentCell = newCell;
//         transform.position = CellToWorldPosition(currentCell);

//         // Small reward for moving closer to the goal
//         Vector2Int goalCell = m_MazeGenerator.GetGoalCell(mazeIndex);
//         float distanceToGoal = Vector2Int.Distance(currentCell, goalCell);
//         AddReward(0.1f / (distanceToGoal + 1)); // Reward is higher when closer to the goal

//         if (currentCell == goalCell)
//         {
//             SetReward(1f); // Large reward for reaching the goal
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//     }
//     else
//     {
//         AddReward(-0.05f); // Penalty for hitting a wall (reduced)
//         Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//     }
    
//     // Apply movement
//     if (moveDir != Vector3.zero)
//     {
//         Vector3 movement = moveDir * moveSpeed * Time.fixedDeltaTime;
//         m_AgentRb.MovePosition(transform.position + movement);
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

// public override void OnActionReceived(ActionBuffers actionBuffers)
// {
//     int action = actionBuffers.DiscreteActions[0];
//     Vector3 moveDir = Vector3.zero;
//     float rotateAmount = 0f;

//     switch (action)
//     {
//         case 0: moveDir = transform.forward; break;
//         case 1: moveDir = -transform.forward; break;
//         case 2: moveDir = -transform.right; break;
//         case 3: moveDir = transform.right; break;
//         case 4: rotateAmount = -1f; break;
//         case 5: rotateAmount = 1f; break;
//         case 6: break; // No action
//         default: Debug.LogWarning($"Unexpected action value: {action}"); break;
//     }

//     // Convert moveDir (Vector3) to a Vector2Int representing the cell move direction
//     Vector2Int moveDirInt = new Vector2Int(Mathf.RoundToInt(moveDir.x), Mathf.RoundToInt(moveDir.z));

//     if (moveDirInt != Vector2Int.zero)
//     {
//         Vector2Int newCell = currentCell + moveDirInt;
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

//     // Apply movement (physical move in the world)
//     if (moveDir != Vector3.zero)
//     {
//         Vector3 movement = moveDir * moveSpeed * Time.fixedDeltaTime;
//         m_AgentRb.MovePosition(transform.position + movement);
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

// public override void OnActionReceived(ActionBuffers actionBuffers)
// {
//     int action = actionBuffers.DiscreteActions[0];
//     Vector3 moveDir = Vector3.zero;
//     float rotateAmount = 0f;

//     switch (action)
//     {
//         case 0: moveDir = transform.forward; break;
//         case 1: moveDir = -transform.forward; break;
//         case 2: moveDir = -transform.right; break;
//         case 3: moveDir = transform.right; break;
//         case 4: rotateAmount = -1f; break;
//         case 5: rotateAmount = 1f; break;
//         case 6: break; // No action
//         default: Debug.LogWarning($"Unexpected action value: {action}"); break;
//     }

//     Vector2Int moveDirInt = new Vector2Int(Mathf.RoundToInt(moveDir.x), Mathf.RoundToInt(moveDir.z));
//     Vector2Int previousCell = currentCell;
//     float previousDistanceToGoal = Vector2Int.Distance(previousCell, m_MazeGenerator.GetGoalCell(mazeIndex));

//     if (moveDirInt != Vector2Int.zero)
//     {
//         Vector2Int newCell = currentCell + moveDirInt;
//         bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

//         Vector3 startPos = CellToWorldPosition(currentCell);
//         Vector3 endPos = CellToWorldPosition(newCell);
//         Debug.DrawRay(startPos, endPos - startPos, isValidMove ? Color.green : Color.red, 0.5f);
//         Debug.DrawLine(startPos, startPos + Vector3.up, Color.blue, 0.5f);

//         if (isValidMove)
//         {
//             currentCell = newCell;
//             Vector3 newPosition = CellToWorldPosition(currentCell);
//             m_AgentRb.MovePosition(newPosition);

//             // Update currentCell based on actual position
//             currentCell = WorldPositionToCell(transform.position);

//             // Reward based on distance to goal
//             float currentDistanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
//             float distanceReward = previousDistanceToGoal - currentDistanceToGoal;
//             AddReward(distanceReward * 0.1f);

//             if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex))
//             {
//                 SetReward(1f);
//                 m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//                 EndEpisode();
//             }
//         }
//         else
//         {
//             AddReward(-0.2f); // Increased penalty for hitting a wall
//             Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//         }
//     }

//     // Apply rotation
//     if (rotateAmount != 0f)
//     {
//         float rotationAngle = rotateAmount * rotateSpeed * Time.fixedDeltaTime;
//         transform.Rotate(0f, rotationAngle, 0f);
//     }

//     // Increased penalty for each step to encourage efficiency
//     AddReward(-0.01f);

//     Vector3 agentWorldPos = transform.position;
//     Vector3 cellWorldPos = CellToWorldPosition(currentCell);
//     Debug.DrawLine(agentWorldPos, agentWorldPos + Vector3.up, Color.yellow, 0.5f);
//     Debug.DrawLine(cellWorldPos, cellWorldPos + Vector3.up, Color.magenta, 0.5f);

//     Debug.Log($"Action: {action}, Move: {moveDir}, Rotate: {rotateAmount}, CurrentCell: {currentCell}, AgentPos: {agentWorldPos}, CellPos: {cellWorldPos}, Reward: {GetCumulativeReward()}");
// }

// public override void Heuristic(in ActionBuffers actionsOut)
// {
//     var discreteActionsOut = actionsOut.DiscreteActions;
//     int action = 6; // Default to no action

//     if (Input.GetKey(KeyCode.W)) action = 0;
//     else if (Input.GetKey(KeyCode.S)) action = 1;
//     else if (Input.GetKey(KeyCode.A)) action = 2;
//     else if (Input.GetKey(KeyCode.D)) action = 3;
//     else if (Input.GetKey(KeyCode.Q)) action = 4;
//     else if (Input.GetKey(KeyCode.E)) action = 5;

//     discreteActionsOut[0] = action;
//     Debug.Log($"Heuristic called. Action: {action}");
// }

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
// private Vector2Int WorldPositionToCell(Vector3 worldPosition)
// {
//     return new Vector2Int(
//         Mathf.RoundToInt(worldPosition.x - (mazeIndex * (m_MazeGenerator.gridSize + 1))),
//         Mathf.RoundToInt(worldPosition.z)
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

// using System.Collections.Generic;
// using System.Collections;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MazeAgent : Agent
// {
//     public float moveSpeed = 5f;
//     public float rotateSpeed = 180f;
//     public float moveCooldown = 0.01f; // Adjust this value to control movement speed
// private float lastMoveTime = 0f;

//     private Rigidbody m_AgentRb;
//     private MazeGenerator m_MazeGenerator;
//     private StatsRecorder m_statsRecorder;
//     private int mazeIndex;
//     private Vector2Int currentCell;
//     private HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();

//     public override void Initialize()
//     {
//         m_AgentRb = GetComponent<Rigidbody>();
//         m_statsRecorder = Academy.Instance.StatsRecorder;
//     }

//     public void SetupAgent(int index, MazeGenerator generator)
//     {
//         mazeIndex = index;
//         m_MazeGenerator = generator;
//     }

//     public override void OnEpisodeBegin()
//     {
//         if (m_MazeGenerator == null)
//         {
//             Debug.LogError("MazeGenerator is not set. Make sure SetupAgent is called before starting the episode.");
//             return;
//         }

//         visitedPositions.Clear();
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

//         Debug.Log($"Episode started. Start cell: {currentCell}, Position: {transform.position}");
//     }

//     public override void CollectObservations(VectorSensor sensor)
//     {
//         // Add observations here if needed
//         sensor.AddObservation(transform.position);
//         sensor.AddObservation(transform.forward);
//         Vector2Int goalCell = m_MazeGenerator.GetGoalCell(mazeIndex);
//         sensor.AddObservation((Vector2)goalCell - (Vector2)currentCell);
//     }
//     private List<Vector2Int> GetValidAdjacentCells(Vector2Int cell)
// {
//     List<Vector2Int> validMoves = new List<Vector2Int>();
//     Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

//     foreach (Vector2Int dir in directions)
//     {
//         Vector2Int adjacentCell = cell + dir;
//         if (m_MazeGenerator.IsValidMove(cell, adjacentCell, mazeIndex))
//         {
//             validMoves.Add(adjacentCell);
//         }
//     }

//     return validMoves;
// }

// //     public override void OnActionReceived(ActionBuffers actionBuffers)
// // {
// //     int action = actionBuffers.DiscreteActions[0];
// //     Vector2Int moveDir = Vector2Int.zero;
// //     float rotateAmount = 0f;

// //     switch (action)
// //     {
// //         case 0: moveDir = Vector2Int.up; break;    // Forward
// //         case 1: moveDir = Vector2Int.down; break;  // Backward
// //         case 2: moveDir = Vector2Int.left; break;  // Left
// //         case 3: moveDir = Vector2Int.right; break; // Right
// //         case 4: rotateAmount = -1f; break;         // Rotate left
// //         case 5: rotateAmount = 1f; break;          // Rotate right
// //         case 6: break; // No action
// //         default: Debug.LogWarning($"Unexpected action value: {action}"); break;
// //     }

// //     Vector2Int previousCell = currentCell;
// //     float previousDistanceToGoal = Vector2Int.Distance(previousCell, m_MazeGenerator.GetGoalCell(mazeIndex));

// //     // Move
// //     if (moveDir != Vector2Int.zero)
// //     {
// //         Vector2Int newCell = currentCell + moveDir;
// //         bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

// //         Vector3 startPos = CellToWorldPosition(currentCell);
// //         Vector3 endPos = CellToWorldPosition(newCell);
// //         Debug.DrawLine(startPos, endPos, isValidMove ? Color.green : Color.red, 0.5f);

// //         if (isValidMove)
// //         {
// //             currentCell = newCell;
// //             Vector3 newPosition = CellToWorldPosition(currentCell);
// //             transform.position = newPosition;
// //             m_AgentRb.MovePosition(newPosition);

// //             if (!visitedPositions.Contains(currentCell))
// //             {
// //                 visitedPositions.Add(currentCell);
// //                 AddReward(0.05f);
// //                 Debug.Log("Visited new position!");
// //             }

// //             float currentDistanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
// //             float distanceReward = previousDistanceToGoal - currentDistanceToGoal;
// //             AddReward(distanceReward * 0.1f);

// //             if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex))
// //             {
// //                 SetReward(1f);
// //                 m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
// //                 EndEpisode();
// //             }
// //         }
// //         else
// //         {
// //             AddReward(-0.2f); // Penalty for hitting a wall
// //             Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
// //         }
// //     }

// //     // Rotate
// //     if (rotateAmount != 0f)
// //     {
// //         float rotationAngle = rotateAmount * rotateSpeed * Time.fixedDeltaTime;
// //         transform.Rotate(0f, rotationAngle, 0f);
// //     }

// //     // Small penalty for each step to encourage efficiency
// //     AddReward(-0.01f);

// //     Debug.Log($"Action: {action}, Move: {moveDir}, Rotate: {rotateAmount}, CurrentCell: {currentCell}, Position: {transform.position}, Reward: {GetCumulativeReward()}");
// // }
// // 

// public override void OnActionReceived(ActionBuffers actionBuffers)
// {
//     // Check if we're still in cooldown
//     if (Time.time - lastMoveTime < moveCooldown)
//     {
//         return;
//     }

//     int action = actionBuffers.DiscreteActions[0];
//     Vector2Int moveDir = Vector2Int.zero;
//     bool isRotateAction = false;

//     switch (action)
//     {
//         case 0: moveDir = Vector2Int.up; break;    // Forward
//         case 1: moveDir = Vector2Int.down; break;  // Backward
//         case 2: moveDir = Vector2Int.left; break;  // Left
//         case 3: moveDir = Vector2Int.right; break; // Right
//         case 4: // Rotate left
//         case 5: // Rotate right
//             isRotateAction = true;
//             break;
//         case 6: break; // No action
//         default:
//             Debug.LogWarning($"Unexpected action value: {action}");
//             return;
//     }

//     bool actionTaken = false;

//     if (moveDir != Vector2Int.zero)
//     {
//         Vector2Int newCell = currentCell + moveDir;
//         bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

//         Vector3 startPos = CellToWorldPosition(currentCell);
//         Vector3 endPos = CellToWorldPosition(newCell);
//         Debug.DrawLine(startPos, endPos, isValidMove ? Color.green : Color.red, moveCooldown);

//         if (isValidMove)
//         {
//             currentCell = newCell;
//             Vector3 newPosition = CellToWorldPosition(currentCell);
//             transform.position = newPosition;
//             m_AgentRb.MovePosition(newPosition);
//             actionTaken = true;

//             if (!visitedPositions.Contains(currentCell))
//             {
//                 visitedPositions.Add(currentCell);
//                 AddReward(0.05f);
//                 Debug.Log("Visited new position!");
//             }

//             float distanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
//             float distanceReward = -distanceToGoal * 0.01f; // Small reward based on distance to goal
//             AddReward(distanceReward);

//             if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex))
//             {
//                 SetReward(1f);
//                 m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//                 EndEpisode();
//             }
//         }
//         else
//         {
//             AddReward(-0.2f); // Penalty for hitting a wall
//             Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//         }
//     }
//     else if (isRotateAction)
//     {
//         float rotationAngle = (action == 4) ? -90f : 90f;
//         transform.Rotate(0f, rotationAngle, 0f);
//         actionTaken = true;
//         AddReward(-0.01f); // Small penalty for rotation to discourage unnecessary spinning
//     }

//     if (actionTaken)
//     {
//         lastMoveTime = Time.time;
//         AddReward(-0.01f); // Small penalty for each action to encourage efficiency
//     }

//     // Debugging information
//     Debug.Log($"Action: {action}, CurrentCell: {currentCell}, Position: {transform.position}, " +
//               $"Rotation: {transform.eulerAngles.y}, Reward: {GetCumulativeReward()}, " +
//               $"Distance to Goal: {Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex))}");
// }

// public override void Heuristic(in ActionBuffers actionsOut)
// {
//     var discreteActionsOut = actionsOut.DiscreteActions;
//     discreteActionsOut[0] = 6; // Default to no action

//     if (Time.time - lastMoveTime >= moveCooldown)
//     {
//         if (Input.GetKeyDown(KeyCode.W)) discreteActionsOut[0] = 0;
//         else if (Input.GetKeyDown(KeyCode.S)) discreteActionsOut[0] = 1;
//         else if (Input.GetKeyDown(KeyCode.A)) discreteActionsOut[0] = 2;
//         else if (Input.GetKeyDown(KeyCode.D)) discreteActionsOut[0] = 3;
//         else if (Input.GetKeyDown(KeyCode.Q)) discreteActionsOut[0] = 4;
//         else if (Input.GetKeyDown(KeyCode.E)) discreteActionsOut[0] = 5;
//     }

//     if (discreteActionsOut[0] != 6)
//     {
//         Debug.Log($"Heuristic called. Action: {discreteActionsOut[0]}");
//     }
// }
// private Vector3 CellToWorldPosition(Vector2Int cell)
// {
//     return new Vector3(
//         cell.x + (mazeIndex * (m_MazeGenerator.gridSize + 1)),
//         0.5f, // Fixed height
//         cell.y
//     );
// }

//     private Vector2Int WorldPositionToCell(Vector3 worldPosition)
//     {
//         return new Vector2Int(
//             Mathf.RoundToInt(worldPosition.x - (mazeIndex * (m_MazeGenerator.gridSize + 1))),
//             Mathf.RoundToInt(worldPosition.z)
//         );
//     }

// private void FixedUpdate()
// {
//     // Ensure the agent stays aligned to the grid
//     Vector3 snappedPosition = CellToWorldPosition(currentCell);
//     transform.position = snappedPosition;
//     m_AgentRb.MovePosition(snappedPosition);

//     // Unstuck mechanism (if needed)
//     if (m_AgentRb.velocity.magnitude < 0.01f)
//     {
//         stuckTime += Time.fixedDeltaTime;
//         if (stuckTime > 3f)
//         {
//             Vector2Int randomDirection = new Vector2Int(Random.Range(-1, 2), Random.Range(-1, 2));
//             Vector2Int newCell = currentCell + randomDirection;
//             if (m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex))
//             {
//                 currentCell = newCell;
//                 transform.position = CellToWorldPosition(currentCell);
//                 m_AgentRb.MovePosition(transform.position);
//                 stuckTime = 0f;
//                 Debug.Log("Agent unstuck!");
//             }
//         }
//     }
//     else
//     {
//         stuckTime = 0f;
//     }
// }

//     private float stuckTime = 0f;
// }

// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MazeAgent : Agent
// {
//     public float moveSpeed = 1f;
//     public float rotateSpeed = 180f;
//     public float moveCooldown = 0.01f;
//     private float lastMoveTime = 0f;

//       public GameObject goalObject; // Reference to the goal object in the scene
//     private bool reachedGoal = false;
//     private Rigidbody m_AgentRb;
//     private MazeGenerator m_MazeGenerator;
//     private StatsRecorder m_statsRecorder;
//     private int mazeIndex;
//     private Vector2Int currentCell;
//     private HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();

//     public override void Initialize()
//     {
//         m_AgentRb = GetComponent<Rigidbody>();
//         m_statsRecorder = Academy.Instance.StatsRecorder;
//     }

//     public void SetupAgent(int index, MazeGenerator generator)
//     {
//         mazeIndex = index;
//         m_MazeGenerator = generator;
//     }

//     public override void OnEpisodeBegin()
//     {
//         if (m_MazeGenerator == null)
//         {
//             Debug.LogError("MazeGenerator is not set. Make sure SetupAgent is called before starting the episode.");
//             return;
//         }

//         ResetAgent();
//        // episodeEnded = false;
//         Debug.Log("New episode started.");
//     }


//     public void ResetAgent()
//     {
//         visitedPositions.Clear();
//         currentCell = m_MazeGenerator.GetStartCell(mazeIndex);
//         transform.position = CellToWorldPosition(currentCell);
//         transform.rotation = Quaternion.identity;

//         if (m_AgentRb != null)
//         {
//             m_AgentRb.velocity = Vector3.zero;
//             m_AgentRb.angularVelocity = Vector3.zero;
//         }

//         m_statsRecorder.Add("Goal/Reached", 0, StatAggregationMethod.Sum);
//         lastMoveTime = Time.time - moveCooldown;

//         Debug.Log($"Agent reset. Start cell: {currentCell}, Goal cell: {m_MazeGenerator.GetGoalCell(mazeIndex)}");
//     }

//     public override void CollectObservations(VectorSensor sensor)
//     {
//         sensor.AddObservation(transform.position);
//         sensor.AddObservation(transform.forward);
//         Vector2Int goalCell = m_MazeGenerator.GetGoalCell(mazeIndex);
//         sensor.AddObservation((Vector2)goalCell - (Vector2)currentCell);
//     }

//     public override void OnActionReceived(ActionBuffers actionBuffers)
//     {
//         if (Time.time - lastMoveTime < moveCooldown)
//         {
//             return;
//         }

//         int action = actionBuffers.DiscreteActions[0];
//         Vector2Int moveDir = Vector2Int.zero;
//         bool isRotateAction = false;

//         switch (action)
//         {
//             case 0: moveDir = Vector2Int.up; break;
//             case 1: moveDir = Vector2Int.down; break;
//             case 2: moveDir = Vector2Int.left; break;
//             case 3: moveDir = Vector2Int.right; break;
//             case 4:
//             case 5:
//                 isRotateAction = true;
//                 break;
//             case 6: break;
//             default:
//                 Debug.LogWarning($"Unexpected action value: {action}");
//                 return;
//         }

//         bool actionTaken = false;

//         if (moveDir != Vector2Int.zero)
//         {
//             Vector2Int newCell = currentCell + moveDir;
//             bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

//             Vector3 startPos = CellToWorldPosition(currentCell);
//             Vector3 endPos = CellToWorldPosition(newCell);
//             Debug.DrawLine(startPos, endPos, isValidMove ? Color.green : Color.red, moveCooldown);

//             if (isValidMove)
//             {
//                 currentCell = newCell;
//                 Vector3 newPosition = CellToWorldPosition(currentCell);
//                 transform.position = newPosition;
//                 m_AgentRb.MovePosition(newPosition);
//                 actionTaken = true;

//                 if (!visitedPositions.Contains(currentCell))
//                 {
//                     visitedPositions.Add(currentCell);
//                     AddReward(0.05f);
//                     Debug.Log("Visited new position!");
//                 }

//                 float distanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
//                 float distanceReward = -distanceToGoal * 0.01f;
//                 AddReward(distanceReward);

//                 if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex))
//                 {
//                     SetReward(1f);
//                      Debug.Log("<color=green>GOAL REACHED! Ending episode.</color>");
//                     m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//                     EndEpisode();
//                 }
//             }
//             else
//             {
//                 AddReward(-0.2f);
//                 Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//             }
//         }
//         else if (isRotateAction)
//         {
//             float rotationAngle = (action == 4) ? -90f : 90f;
//             transform.Rotate(0f, rotationAngle, 0f);
//             actionTaken = true;
//             AddReward(-0.01f);
//         }

//         if (actionTaken)
//         {
//             lastMoveTime = Time.time;
//             AddReward(-0.01f);
//                         // Check for goal after movement
//             CheckForGoal();
//         }

//         Debug.Log($"Action: {action}, CurrentCell: {currentCell}, Position: {transform.position}, " +
//                   $"Rotation: {transform.eulerAngles.y}, Reward: {GetCumulativeReward()}, " +
//                   $"Distance to Goal: {Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex))}");
//     }

//     private void CheckForGoal()
//     {
//         if (reachedGoal) return;

//         if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex) || IsAtGoal())
//         {
//             reachedGoal = true;
//             Debug.Log("<color=green>GOAL REACHED! Ending episode.</color>");
//             SetReward(1f);
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//     }

//     private bool IsAtGoal()
//     {
//         if (goalObject == null) return false;

//         float distanceToGoal = Vector3.Distance(transform.position, goalObject.transform.position);
//         return distanceToGoal < 0.5f; // Adjust this threshold as needed
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject == goalObject)
//         {
//             reachedGoal = true;
//             Debug.Log("<color=blue>GOAL REACHED via Trigger!</color>");
//             SetReward(1f);
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//     }

//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         var discreteActionsOut = actionsOut.DiscreteActions;
//         discreteActionsOut[0] = 6;

//         if (Time.time - lastMoveTime >= moveCooldown)
//         {
//             if (Input.GetKeyDown(KeyCode.W)) discreteActionsOut[0] = 0;
//             else if (Input.GetKeyDown(KeyCode.S)) discreteActionsOut[0] = 1;
//             else if (Input.GetKeyDown(KeyCode.A)) discreteActionsOut[0] = 2;
//             else if (Input.GetKeyDown(KeyCode.D)) discreteActionsOut[0] = 3;
//             else if (Input.GetKeyDown(KeyCode.Q)) discreteActionsOut[0] = 4;
//             else if (Input.GetKeyDown(KeyCode.E)) discreteActionsOut[0] = 5;
//         }

//         if (discreteActionsOut[0] != 6)
//         {
//             Debug.Log($"Heuristic called. Action: {discreteActionsOut[0]}");
//         }
//     }

//     private Vector3 CellToWorldPosition(Vector2Int cell)
//     {
//         return new Vector3(
//             cell.x + (mazeIndex * (m_MazeGenerator.gridSize + 1)),
//             0.5f,
//             cell.y
//         );
//     }

//     private Vector2Int WorldPositionToCell(Vector3 worldPosition)
//     {
//         return new Vector2Int(
//             Mathf.RoundToInt(worldPosition.x - (mazeIndex * (m_MazeGenerator.gridSize + 1))),
//             Mathf.RoundToInt(worldPosition.z)
//         );
//     }

//     private void FixedUpdate()
//     {
//         Vector3 snappedPosition = CellToWorldPosition(currentCell);
//         transform.position = snappedPosition;
//         m_AgentRb.MovePosition(snappedPosition);
//     }
// }

// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MazeAgent : Agent
// {
//     public float moveSpeed = 1f;
//     public float rotateSpeed = 180f;
//     public float moveCooldown = 0.01f;
//     private float lastMoveTime = 0f;

//     public GameObject goalObject; // Reference to the goal object in the scene
//     private bool reachedGoal = false;
//     private bool episodeEnded = false;
//     private Rigidbody m_AgentRb;
//     private MazeGenerator m_MazeGenerator;
//     private StatsRecorder m_statsRecorder;
//     private int mazeIndex;
//     private Vector2Int currentCell;
//     private HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();

//     public override void Initialize()
//     {
//         m_AgentRb = GetComponent<Rigidbody>();
//         m_statsRecorder = Academy.Instance.StatsRecorder;
//     }

//     public void SetupAgent(int index, MazeGenerator generator)
//     {
//         mazeIndex = index;
//         m_MazeGenerator = generator;
//     }

//     public override void OnEpisodeBegin()
//     {
//         if (m_MazeGenerator == null)
//         {
//             Debug.LogError("MazeGenerator is not set. Make sure SetupAgent is called before starting the episode.");
//             return;
//         }

//         ResetAgent();
//         episodeEnded = false;
//         reachedGoal = false;
//         Debug.Log("New episode started.");
//     }

//     public void ResetAgent()
//     {
//         visitedPositions.Clear();
//         currentCell = m_MazeGenerator.GetStartCell(mazeIndex);
//         transform.position = CellToWorldPosition(currentCell);
//         transform.rotation = Quaternion.identity;

//         if (m_AgentRb != null)
//         {
//             m_AgentRb.velocity = Vector3.zero;
//             m_AgentRb.angularVelocity = Vector3.zero;
//         }

//         m_statsRecorder.Add("Goal/Reached", 0, StatAggregationMethod.Sum);
//         lastMoveTime = Time.time - moveCooldown;

//         Debug.Log($"Agent reset. Start cell: {currentCell}, Goal cell: {m_MazeGenerator.GetGoalCell(mazeIndex)}");
//     }

//     public override void CollectObservations(VectorSensor sensor)
//     {
//         sensor.AddObservation(transform.position);
//         sensor.AddObservation(transform.forward);
//         Vector2Int goalCell = m_MazeGenerator.GetGoalCell(mazeIndex);
//         sensor.AddObservation((Vector2)goalCell - (Vector2)currentCell);
//     }

//     public override void OnActionReceived(ActionBuffers actionBuffers)
//     {
//         if (episodeEnded || Time.time - lastMoveTime < moveCooldown)
//         {
//             return;
//         }

//         int action = actionBuffers.DiscreteActions[0];
//         Vector2Int moveDir = Vector2Int.zero;
//         bool isRotateAction = false;

//         switch (action)
//         {
//             case 0: moveDir = Vector2Int.up; break;
//             case 1: moveDir = Vector2Int.down; break;
//             case 2: moveDir = Vector2Int.left; break;
//             case 3: moveDir = Vector2Int.right; break;
//             case 4:
//             case 5:
//                 isRotateAction = true;
//                 break;
//             case 6: break;
//             default:
//                 Debug.LogWarning($"Unexpected action value: {action}");
//                 return;
//         }

//         bool actionTaken = false;

//         if (moveDir != Vector2Int.zero)
//         {
//             Vector2Int newCell = currentCell + moveDir;
//             bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

//             Vector3 startPos = CellToWorldPosition(currentCell);
//             Vector3 endPos = CellToWorldPosition(newCell);
//             Debug.DrawLine(startPos, endPos, isValidMove ? Color.green : Color.red, moveCooldown);

//             if (isValidMove)
//             {
//                 currentCell = newCell;
//                 Vector3 newPosition = CellToWorldPosition(currentCell);
//                 transform.position = newPosition;
//                 m_AgentRb.MovePosition(newPosition);
//                 actionTaken = true;

//                 if (!visitedPositions.Contains(currentCell))
//                 {
//                     visitedPositions.Add(currentCell);
//                     AddReward(0.05f);
//                     Debug.Log("Visited new position!");
//                 }

//                 float distanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
//                 float distanceReward = -distanceToGoal * 0.01f;
//                 AddReward(distanceReward);

//                 CheckForGoal();
//             }
//             else
//             {
//                 AddReward(-0.2f);
//                 Debug.Log($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//             }
//         }
//         else if (isRotateAction)
//         {
//             float rotationAngle = (action == 4) ? -90f : 90f;
//             transform.Rotate(0f, rotationAngle, 0f);
//             actionTaken = true;
//             AddReward(-0.01f);
//         }

//         if (actionTaken)
//         {
//             lastMoveTime = Time.time;
//             AddReward(-0.01f);
//         }

//         Debug.Log($"Action: {action}, CurrentCell: {currentCell}, Position: {transform.position}, " +
//                   $"Rotation: {transform.eulerAngles.y}, Reward: {GetCumulativeReward()}, " +
//                   $"Distance to Goal: {Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex))}");
//     }

//     private void CheckForGoal()
//     {
//         if (reachedGoal) return;

//         if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex) || IsAtGoal())
//         {
//             reachedGoal = true;
//             episodeEnded = true;
//             Debug.Log("<color=green>GOAL REACHED! Ending episode.</color>");
//             SetReward(1f);
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//     }

//     private bool IsAtGoal()
//     {
//         if (goalObject == null) return false;

//         float distanceToGoal = Vector3.Distance(transform.position, goalObject.transform.position);
//         return distanceToGoal < 0.5f; // Adjust this threshold as needed
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.gameObject == goalObject && !reachedGoal)
//         {
//             reachedGoal = true;
//             episodeEnded = true;
//             Debug.Log("<color=blue>GOAL REACHED via Trigger!</color>");
//             SetReward(1f);
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//     }

//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         var discreteActionsOut = actionsOut.DiscreteActions;
//         discreteActionsOut[0] = 6;

//         if (Time.time - lastMoveTime >= moveCooldown)
//         {
//             if (Input.GetKeyDown(KeyCode.W)) discreteActionsOut[0] = 0;
//             else if (Input.GetKeyDown(KeyCode.S)) discreteActionsOut[0] = 1;
//             else if (Input.GetKeyDown(KeyCode.A)) discreteActionsOut[0] = 2;
//             else if (Input.GetKeyDown(KeyCode.D)) discreteActionsOut[0] = 3;
//             else if (Input.GetKeyDown(KeyCode.Q)) discreteActionsOut[0] = 4;
//             else if (Input.GetKeyDown(KeyCode.E)) discreteActionsOut[0] = 5;
//         }

//         if (discreteActionsOut[0] != 6)
//         {
//             Debug.Log($"Heuristic called. Action: {discreteActionsOut[0]}");
//         }
//     }

//     private Vector3 CellToWorldPosition(Vector2Int cell)
//     {
//         return new Vector3(
//             cell.x + (mazeIndex * (m_MazeGenerator.gridSize + 1)),
//             0.5f,
//             cell.y
//         );
//     }

//     private Vector2Int WorldPositionToCell(Vector3 worldPosition)
//     {
//         return new Vector2Int(
//             Mathf.RoundToInt(worldPosition.x - (mazeIndex * (m_MazeGenerator.gridSize + 1))),
//             Mathf.RoundToInt(worldPosition.z)
//         );
//     }

//     private void FixedUpdate()
//     {
//         Vector3 snappedPosition = CellToWorldPosition(currentCell);
//         transform.position = snappedPosition;
//         m_AgentRb.MovePosition(snappedPosition);
//     }
// }

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;
// using Unity.MLAgents.Actuators;
// using Unity.MLAgents.Sensors;

// public class MazeAgent : Agent
// {
//     public float moveDuration = 0.5f; // Time to move one cell
//     public GameObject goalObject;
//     public bool verboseDebug = true;

//     public RayPerceptionSensorComponent3D raySensor;
//     private Rigidbody m_AgentRb;
//     private MazeGenerator m_MazeGenerator;
//     private StatsRecorder m_statsRecorder;
//     private int mazeIndex;
//     private Vector2Int currentCell;
//     private HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();
//     private bool isMoving = false;
//     private bool reachedGoal = false;
//     private bool episodeEnded = false;
//     private float previousDistanceToGoal;

// public override void Initialize()
//     {
//         m_AgentRb = GetComponent<Rigidbody>();
//         m_statsRecorder = Academy.Instance.StatsRecorder;
        
//         // Lock rotation of the Rigidbody
//         if (m_AgentRb != null)
//         {
//             m_AgentRb.freezeRotation = true;
//         }
//     }

//     public void SetupAgent(int index, MazeGenerator generator)
//     {
//         mazeIndex = index;
//         m_MazeGenerator = generator;
//     }

//     public override void OnEpisodeBegin()
//     {
//         if (m_MazeGenerator == null)
//         {
//             Debug.LogError("MazeGenerator is not set. Make sure SetupAgent is called before starting the episode.");
//             return;
//         }

//         ResetAgent();
//         episodeEnded = false;
//         reachedGoal = false;
//         ForceLog("New episode started.");
//         previousDistanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
  
//     }

//     public void ResetAgent()
//     {
//         StopAllCoroutines();
//         isMoving = false;
//         visitedPositions.Clear();
//         currentCell = m_MazeGenerator.GetStartCell(mazeIndex);
//         transform.position = CellToWorldPosition(currentCell);
//         transform.rotation = Quaternion.identity;
//             // Reset rotation to face forward (adjust the Y value if needed)
//         transform.rotation = Quaternion.Euler(0f, 0f, 0f);  

//         if (m_AgentRb != null)
//         {
//             m_AgentRb.velocity = Vector3.zero;
//             m_AgentRb.angularVelocity = Vector3.zero;
//         }

//         m_statsRecorder.Add("Goal/Reached", 0, StatAggregationMethod.Sum);

//         ForceLog($"Agent reset. Start cell: {currentCell}, Goal cell: {m_MazeGenerator.GetGoalCell(mazeIndex)}");
//     }

//     // public override void CollectObservations(VectorSensor sensor)
//     // {
//     //     sensor.AddObservation(transform.position);
//     //     sensor.AddObservation(transform.forward);
//     //     Vector2Int goalCell = m_MazeGenerator.GetGoalCell(mazeIndex);
//     //     sensor.AddObservation((Vector2)goalCell - (Vector2)currentCell);
//     // }
//     public override void CollectObservations(VectorSensor sensor)
//     {

//         if (goalObject != null)
//         {
//             Vector3 relativePosition = goalObject.transform.localPosition - transform.localPosition;
//             sensor.AddObservation(relativePosition);
//         }

//         // Add the agent's position
//         sensor.AddObservation(transform.localPosition);

//         // Add the relative position of the goal
//         Vector3 relativeGoalPosition = goalObject.transform.localPosition - transform.localPosition;
//         sensor.AddObservation(relativeGoalPosition);

//         // The ray perception results are automatically collected by ML-Agents
//     }
//     private void InterpretRayPerceptionOutput()
//     {
//         if (raySensor == null)
//         {
//             Debug.LogError("Ray Perception Sensor is not assigned!");
//             return;
//         }

//         var rayOutputs = raySensor.RaySensor.RayPerceptionOutput;

//         for (int i = 0; i < rayOutputs.RayOutputs.Length; i++)
//         {
//             var rayOutput = rayOutputs.RayOutputs[i];

//             if (rayOutput.HasHit)
//             {
//                 string hitObjectTag = rayOutput.HitGameObject.tag;
//                 float hitDistance = rayOutput.HitFraction;

//                 switch (hitObjectTag)
//                 {
//                     case "Wall":
//                         // Logic for detecting walls
//                         ForceLog($"Ray {i} hit a wall at distance {hitDistance}");
//                         break;
//                     case "Goal":
//                         // Logic for detecting the goal
//                         ForceLog($"Ray {i} hit the goal at distance {hitDistance}");
//                         break;
//                     // Add more cases as needed for other objects in your maze
//                 }
//             }
//         }
//     }

//     // public override void OnActionReceived(ActionBuffers actionBuffers)
//     // {
//     //         InterpretRayPerceptionOutput();
//     // AdjustRewardBasedOnRays();
//     //     if (episodeEnded || isMoving)
//     //     {
//     //         return;
//     //     }

//     //     int action = actionBuffers.DiscreteActions[0];
//     //     Vector2Int moveDir = Vector2Int.zero;
//     //     bool isRotateAction = false;

//     //     switch (action)
//     //     {
//     //         case 0: moveDir = Vector2Int.up; break;
//     //         case 1: moveDir = Vector2Int.down; break;
//     //         case 2: moveDir = Vector2Int.left; break;
//     //         case 3: moveDir = Vector2Int.right; break;
//     //         case 4:
//     //         case 5:
//     //             isRotateAction = true;
//     //             break;
//     //         case 6: break;
//     //         default:
//     //             ForceLog($"Unexpected action value: {action}");
//     //             return;
//     //     }

//     //     if (moveDir != Vector2Int.zero)
//     //     {
//     //         Vector2Int newCell = currentCell + moveDir;
//     //         bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

//     //         if (isValidMove)
//     //         {
//     //             StartCoroutine(MoveToCell(newCell));
//     //         }
//     //         else
//     //         {
//     //             AddReward(-0.2f);
//     //             ForceLog($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//     //         }
//     //     }
//     //     else if (isRotateAction)
//     //     {
//     //         float rotationAngle = (action == 4) ? -90f : 90f;
//     //         transform.Rotate(0f, rotationAngle, 0f);
//     //         AddReward(-0.01f);
//     //     }

//     //     if (verboseDebug)
//     //     {
//     //         ForceLog($"Action: {action}, CurrentCell: {currentCell}, Position: {transform.position}, " +
//     //                   $"Rotation: {transform.eulerAngles.y}, Reward: {GetCumulativeReward()}, " +
//     //                   $"Distance to Goal: {Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex))}");
//     //     }
//     // }

//     // public override void OnActionReceived(ActionBuffers actionBuffers)
//     // {
//     //     InterpretRayPerceptionOutput();
//     //     AdjustRewardBasedOnRays();

//     //     if (episodeEnded || isMoving)
//     //     {
//     //         return;
//     //     }

//     //     int action = actionBuffers.DiscreteActions[0];
//     //     Vector2Int moveDir = Vector2Int.zero;

//     //     switch (action)
//     //     {
//     //         case 0: moveDir = Vector2Int.up; break;
//     //         case 1: moveDir = Vector2Int.down; break;
//     //         case 2: moveDir = Vector2Int.left; break;
//     //         case 3: moveDir = Vector2Int.right; break;
//     //         case 4: break; // No movement
//     //         default:
//     //             ForceLog($"Unexpected action value: {action}");
//     //             return;
//     //     }

//     //     if (moveDir != Vector2Int.zero)
//     //     {
//     //         Vector2Int newCell = currentCell + moveDir;
//     //         bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

//     //         if (isValidMove)
//     //         {
//     //             StartCoroutine(MoveToCell(newCell));
//     //         }
//     //         else
//     //         {
//     //             AddReward(-0.2f);
//     //             ForceLog($"Hit Wall! Attempted move from {currentCell} to {newCell}");
//     //         }
//     //     }

//     //     if (verboseDebug)
//     //     {
//     //         ForceLog($"Action: {action}, CurrentCell: {currentCell}, Position: {transform.position}, " +
//     //                   $"Reward: {GetCumulativeReward()}, " +
//     //                   $"Distance to Goal: {Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex))}");
//     //     }
//     // }
//        public override void OnActionReceived(ActionBuffers actionBuffers)
//     {
//         int action = actionBuffers.DiscreteActions[0];
        
//         Vector3 moveDirection = Vector3.zero;

//         switch(action)
//         {
//             case 0: moveDirection = Vector3.forward; break;
//             case 1: moveDirection = Vector3.back; break;
//             case 2: moveDirection = Vector3.left; break;
//             case 3: moveDirection = Vector3.right; break;
//             case 4: break; // No movement
//         }

//         // Move the agent
//         transform.position += moveDirection * Time.deltaTime;

//         // Check if goal is reached
//         if (Vector3.Distance(transform.position, goalObject.transform.position) < 0.5f)
//         {
//             SetReward(10.0f);
//             EndEpisode();
//         }

//         // Add small negative reward for each step to encourage efficiency
//         AddReward(-0.01f);
//     }


//     // private IEnumerator MoveToCell(Vector2Int newCell)
//     // {
//     //     isMoving = true;
//     //     Vector3 startPosition = transform.position;
//     //     Vector3 endPosition = CellToWorldPosition(newCell);
//     //     float elapsedTime = 0f;

//     //     while (elapsedTime < moveDuration)
//     //     {
//     //         transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
//     //         elapsedTime += Time.deltaTime;
//     //         yield return null;
//     //     }

//     //     transform.position = endPosition;
//     //     currentCell = newCell;
//     //     isMoving = false;

//     //     if (!visitedPositions.Contains(currentCell))
//     //     {
//     //         visitedPositions.Add(currentCell);
//     //         AddReward(0.05f);
//     //         ForceLog("Visited new position!");
//     //     }

//     //     float distanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
//     //     float distanceReward = -distanceToGoal * 0.01f;

//     //     AddReward(distanceReward);

//     //     AddReward(-0.01f); // Small penalty for each move
//     //     CheckForGoal();
//     // }
//         private IEnumerator MoveToCell(Vector2Int newCell)
//     {
//         isMoving = true;
//         Vector3 startPosition = transform.position;
//         Vector3 endPosition = CellToWorldPosition(newCell);
//         float elapsedTime = 0f;

//         while (elapsedTime < moveDuration)
//         {
//             transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
//             elapsedTime += Time.deltaTime;
//             yield return null;
//         }

//         transform.position = endPosition;
//         currentCell = newCell;
//         isMoving = false;

//         // Calculate reward based on distance to goal
//         float currentDistanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
//         float distanceReward = previousDistanceToGoal - currentDistanceToGoal;
        
//         if (distanceReward > 0)
//         {
//             // Reward for moving closer to the goal
//             AddReward(distanceReward * 0.1f);
//         }
//         else
//         {
//             // Small penalty for moving away from the goal
//             AddReward(distanceReward * 0.05f);
//         }

//         previousDistanceToGoal = currentDistanceToGoal;

//         // Small penalty for each move to encourage efficiency
//         AddReward(-0.01f);

//         if (!visitedPositions.Contains(currentCell))
//         {
//             visitedPositions.Add(currentCell);
//             // Reduced exploration reward
//             AddReward(0.01f);
//             ForceLog("Visited new position!");
//         }

//         CheckForGoal();
//     }
// private void AdjustRewardBasedOnRays()
// {
//     var rayOutputs = raySensor.RaySensor.RayPerceptionOutput;
    
//     for (int i = 0; i < rayOutputs.RayOutputs.Length; i++)
//     {
//         var rayOutput = rayOutputs.RayOutputs[i];
        
//         if (rayOutput.HasHit)
//         {
//             if (rayOutput.HitGameObject.CompareTag("Goal"))
//             {
//                 // Reward for detecting the goal
//                 float goalProximityReward = 1f - rayOutput.HitFraction; // Closer hits give higher rewards
//                 AddReward(goalProximityReward * 0.5f);
//             }
//             else if (rayOutput.HitGameObject.CompareTag("Wall"))
//             {
//                 // Small penalty for being close to walls
//                 float wallProximityPenalty = 1f - rayOutput.HitFraction; // Closer hits give higher penalties
//                 AddReward(-wallProximityPenalty * 0.01f);
//             }
//         }
//     }
// }


// private void CheckForGoal()
//     {
//         if (reachedGoal) return;

//         if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex) || IsAtGoal())
//         {
//             reachedGoal = true;
//             episodeEnded = true;
//             ForceLog("<color=green>GOAL REACHED! Ending episode.</color>");
//             // Increased reward for reaching the goal
//             SetReward(100f);
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//     }

//     private bool IsAtGoal()
//     {
//         if (goalObject == null) return false;

//         float distanceToGoal = Vector3.Distance(transform.position, goalObject.transform.position);
//         return distanceToGoal < 0.5f; // Adjust this threshold as needed
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         ForceLog($"Trigger entered: {other.gameObject.name}");
//         if (other.gameObject == goalObject && !reachedGoal)
//         {
//             reachedGoal = true;
//             episodeEnded = true;
//             ForceLog("<color=blue>GOAL REACHED via Trigger!</color>");
//             SetReward(1f);
//             m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
//             EndEpisode();
//         }
//     }

//     public override void Heuristic(in ActionBuffers actionsOut)
//     {
//         var discreteActionsOut = actionsOut.DiscreteActions;
//         discreteActionsOut[0] = 6;

//         if (!isMoving)
//         {
//             if (Input.GetKeyDown(KeyCode.W)) discreteActionsOut[0] = 0;
//             else if (Input.GetKeyDown(KeyCode.S)) discreteActionsOut[0] = 1;
//             else if (Input.GetKeyDown(KeyCode.A)) discreteActionsOut[0] = 2;
//             else if (Input.GetKeyDown(KeyCode.D)) discreteActionsOut[0] = 3;
//             else if (Input.GetKeyDown(KeyCode.Q)) discreteActionsOut[0] = 4;
//             else if (Input.GetKeyDown(KeyCode.E)) discreteActionsOut[0] = 5;
//         }

//         if (discreteActionsOut[0] != 6)
//         {
//             ForceLog($"Heuristic called. Action: {discreteActionsOut[0]}");
//         }
//     }

//     private Vector3 CellToWorldPosition(Vector2Int cell)
//     {
//         return new Vector3(
//             cell.x + (mazeIndex * (m_MazeGenerator.gridSize + 1)),
//             0.5f,
//             cell.y
//         );
//     }

//     private Vector2Int WorldPositionToCell(Vector3 worldPosition)
//     {
//         return new Vector2Int(
//             Mathf.RoundToInt(worldPosition.x - (mazeIndex * (m_MazeGenerator.gridSize + 1))),
//             Mathf.RoundToInt(worldPosition.z)
//         );
//     }

//     private void ForceLog(string message)
//     {
//         Debug.Log($"[MazeAgent {mazeIndex}] {Time.time}: {message}");
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MazeAgent : Agent
{
    public float moveDuration = 0.5f;
    public GameObject goalObject;
    public bool verboseDebug = true;
    public RayPerceptionSensorComponent3D raySensor;

    private Rigidbody m_AgentRb;
    private MazeGenerator m_MazeGenerator;
    private StatsRecorder m_statsRecorder;
    private int mazeIndex;
    private Vector2Int currentCell;
    private HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();
    private bool isMoving = false;
    private bool reachedGoal = false;
    private float previousDistanceToGoal;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_statsRecorder = Academy.Instance.StatsRecorder;
        
        if (m_AgentRb != null)
        {
            m_AgentRb.freezeRotation = true;
        }
    }

    public void SetupAgent(int index, MazeGenerator generator)
    {
        mazeIndex = index;
        m_MazeGenerator = generator;
    }

    public override void OnEpisodeBegin()
    {
        if (m_MazeGenerator == null)
        {
            Debug.LogError("MazeGenerator is not set. Make sure SetupAgent is called before starting the episode.");
            return;
        }

        ResetAgent();
        reachedGoal = false;
        ForceLog("New episode started.");
        previousDistanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
    }

    public void ResetAgent()
    {
        StopAllCoroutines();
        isMoving = false;
        visitedPositions.Clear();
        currentCell = m_MazeGenerator.GetStartCell(mazeIndex);
        transform.position = CellToWorldPosition(currentCell);
        transform.rotation = Quaternion.identity;

        if (m_AgentRb != null)
        {
            m_AgentRb.velocity = Vector3.zero;
            m_AgentRb.angularVelocity = Vector3.zero;
        }

        m_statsRecorder.Add("Goal/Reached", 0, StatAggregationMethod.Sum);

        ForceLog($"Agent reset. Start cell: {currentCell}, Goal cell: {m_MazeGenerator.GetGoalCell(mazeIndex)}");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (goalObject != null)
        {
            Vector3 relativePosition = goalObject.transform.localPosition - transform.localPosition;
            sensor.AddObservation(relativePosition);
        }

        sensor.AddObservation(transform.localPosition);

        // The ray perception results are automatically collected by ML-Agents
    }

    private void InterpretRayPerceptionOutput()
    {
        if (raySensor == null || raySensor.RaySensor == null || raySensor.RaySensor.RayPerceptionOutput == null)
        {
            return;
        }

        var rayOutputs = raySensor.RaySensor.RayPerceptionOutput.RayOutputs;
        for (int i = 0; i < rayOutputs.Length; i++)
        {
            var rayOutput = rayOutputs[i];
            if (rayOutput.HasHit && rayOutput.HitGameObject != null)
            {
                string hitObjectTag = rayOutput.HitGameObject.tag;
                float hitDistance = rayOutput.HitFraction;

                if (verboseDebug)
                {
                    ForceLog($"Ray {i} hit {hitObjectTag} at distance {hitDistance}");
                }
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        if (isMoving || reachedGoal)
        {
            return;
        }

        InterpretRayPerceptionOutput();
        AdjustRewardBasedOnRays();

        int action = actionBuffers.DiscreteActions[0];
        Vector2Int moveDir = Vector2Int.zero;

        switch (action)
        {
            case 0: moveDir = Vector2Int.up; break;
            case 1: moveDir = Vector2Int.down; break;
            case 2: moveDir = Vector2Int.left; break;
            case 3: moveDir = Vector2Int.right; break;
            case 4: break; // No movement
            default:
                ForceLog($"Unexpected action value: {action}");
                return;
        }

        if (moveDir != Vector2Int.zero)
        {
            Vector2Int newCell = currentCell + moveDir;
            bool isValidMove = m_MazeGenerator.IsValidMove(currentCell, newCell, mazeIndex);

            if (isValidMove)
            {
                StartCoroutine(MoveToCell(newCell));
            }
            else
            {
                AddReward(-0.2f);
                ForceLog($"Hit Wall! Attempted move from {currentCell} to {newCell}");
            }
        }

        if (verboseDebug)
        {
            ForceLog($"Action: {action}, CurrentCell: {currentCell}, Position: {transform.position}, " +
                      $"Reward: {GetCumulativeReward()}, " +
                      $"Distance to Goal: {Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex))}");
        }
    }

    private IEnumerator MoveToCell(Vector2Int newCell)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = CellToWorldPosition(newCell);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        currentCell = newCell;
        isMoving = false;

        float currentDistanceToGoal = Vector2Int.Distance(currentCell, m_MazeGenerator.GetGoalCell(mazeIndex));
        float distanceReward = previousDistanceToGoal - currentDistanceToGoal;
        
        if (distanceReward > 0)
        {
            AddReward(distanceReward * 10f);
        }
        else
        {
            AddReward(distanceReward * 0.5f);
        }

        previousDistanceToGoal = currentDistanceToGoal;

        AddReward(-0.01f);

        if (!visitedPositions.Contains(currentCell))
        {
            visitedPositions.Add(currentCell);
            AddReward(0.01f);
            ForceLog("Visited new position!");
        }

        CheckForGoal();
    }

    private void AdjustRewardBasedOnRays()
    {
        if (raySensor == null || raySensor.RaySensor == null || raySensor.RaySensor.RayPerceptionOutput == null)
        {
            return;
        }

        var rayOutputs = raySensor.RaySensor.RayPerceptionOutput.RayOutputs;
        
        for (int i = 0; i < rayOutputs.Length; i++)
        {
            var rayOutput = rayOutputs[i];
            
            if (rayOutput.HasHit && rayOutput.HitGameObject != null)
            {
                if (rayOutput.HitGameObject.CompareTag("Goal"))
                {
                    ForceLog("<color=green>GOAL hit.</color>");
                    float goalProximityReward = 1f - rayOutput.HitFraction;
                    AddReward(goalProximityReward * 5f);
                }
                else if (rayOutput.HitGameObject.CompareTag("Wall"))
                {
                    float wallProximityPenalty = 1f - rayOutput.HitFraction;
                    AddReward(-wallProximityPenalty * 0.01f);
                }
            }
        }
    }

    private void CheckForGoal()
    {
        if (reachedGoal) return;

        if (currentCell == m_MazeGenerator.GetGoalCell(mazeIndex) || IsAtGoal())
        {
            reachedGoal = true;
            ForceLog("<color=green>GOAL REACHED! Ending episode.</color>");
            SetReward(1000f);
            m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
    }

    private bool IsAtGoal()
    {
        if (goalObject == null) return false;
        float distanceToGoal = Vector3.Distance(transform.position, goalObject.transform.position);
        return distanceToGoal < 0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        ForceLog($"Trigger entered: {other.gameObject.name}");
        if (other.gameObject == goalObject && !reachedGoal)
        {
            reachedGoal = true;
            ForceLog("<color=blue>GOAL REACHED via Trigger!</color>");
            SetReward(100f);
            m_statsRecorder.Add("Goal/Reached", 1, StatAggregationMethod.Sum);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 4; // Default to no movement

        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.W)) discreteActionsOut[0] = 0;
            else if (Input.GetKeyDown(KeyCode.S)) discreteActionsOut[0] = 1;
            else if (Input.GetKeyDown(KeyCode.A)) discreteActionsOut[0] = 2;
            else if (Input.GetKeyDown(KeyCode.D)) discreteActionsOut[0] = 3;
        }

        if (discreteActionsOut[0] != 4)
        {
            ForceLog($"Heuristic called. Action: {discreteActionsOut[0]}");
        }
    }

    private Vector3 CellToWorldPosition(Vector2Int cell)
    {
        return new Vector3(
            cell.x + (mazeIndex * (m_MazeGenerator.gridSize + 1)),
            0.5f,
            cell.y
        );
    }

    private Vector2Int WorldPositionToCell(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x - (mazeIndex * (m_MazeGenerator.gridSize + 1))),
            Mathf.RoundToInt(worldPosition.z)
        );
    }

    private void ForceLog(string message)
    {
        Debug.Log($"[MazeAgent {mazeIndex}] {Time.time}: {message}");
    }
}