using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] PatrolPoints;
    [SerializeField] private float _waitTimer;
    private float _currentWaitTimer;

    private NavMeshAgent _agent;
    private int _currentPointIndex = 0;
    private Animator _animator;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        Debug.Log($"Agent Type ID: {_agent.agentTypeID}");
        if (_agent.agentTypeID == 0)
        {
            _animator = GetComponent<Animator>();
        }
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

        if (_animator != null)
        {
            _animator.SetFloat("SpeedY", 1);
        }

        _currentPointIndex = (_currentPointIndex + 1) % PatrolPoints.Length;
    }
}
