using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] private float _waitTimer;
    [SerializeField] private float _currentWaitTimer;

    private NavMeshAgent _agent;
    private int _currentPointIndex = 0;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (PatrolPoints.Length > 0)
        {
            MoveToNextPoint();
        }
    }

    void Update()
    {
        SetNextDestination();
    }

    private void SetNextDestination()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentWaitTimer += Time.deltaTime;

            if (_currentWaitTimer >= _waitTimer)
            {
                MoveToNextPoint();
                _currentWaitTimer = 0f;
            }
        }
    }

    private void MoveToNextPoint()
    {
        _agent.destination = PatrolPoints[_currentPointIndex].position;

        _currentPointIndex = (_currentPointIndex + 1) % PatrolPoints.Length;
    }
}
