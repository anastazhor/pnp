using UnityEngine;
using UnityEngine.AI;

public class ParolController : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent _agent;
    private int _currentWaypoint;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _currentWaypoint = 0;
        SetNextWaypoint();
    }
    void Update()
    {
        if (_agent.remainingDistance < 0.5f)
            SetNextWaypoint();
    }
    void SetNextWaypoint()
    {
        _agent.SetDestination(waypoints[_currentWaypoint].position);
        _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
    }

}
