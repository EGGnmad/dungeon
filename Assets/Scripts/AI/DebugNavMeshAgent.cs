using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DebugNavMeshAgent : MonoBehaviour
{
    [Header("Toggle")]
    public bool velocity;
    public bool desiredVelocity;
    public bool path;

    [Header("Color")] 
    public Color velocityColor = Color.blue;
    public Color desiredVelocityColor = Color.green;
    public Color pathColor = Color.black;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        if (velocity)
        {
            Gizmos.color = velocityColor;
            Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
        }

        if (desiredVelocity)
        {
            Gizmos.color = desiredVelocityColor;
            Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
        }

        if (path)
        {
            Gizmos.color = pathColor;

            var agentPath = agent.path;
            Vector3 prevCorner = transform.position;
            foreach (var corner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.1f);
                prevCorner = corner;
            }
        }
    }
}
