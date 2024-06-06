using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(IStateMachine<AiState>))]
public class AiPatrolState : StateBehaviorBase<AiState>
{
    [Header("config")] 
    [SerializeField] private float patrolRadius;
    private Vector3 patrolOrigin;

    private IDisposable _patrolStream;
    private Vector3 _destination = Vector3.zero;
    
    private IStateMachine<AiState> _stateMachine;
    private NavMeshAgent _navMeshAgent;

    #region Unity
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _stateMachine = GetComponent<IStateMachine<AiState>>();
        patrolOrigin = transform.position;
    }
    #endregion
    
    #region FSM
    public override AiState GetStateKey() => AiState.Patrol;
    
    public override void StateEnter()
    {
        base.StateEnter();
        
        _patrolStream = Observable.Interval(TimeSpan.FromSeconds(3f)).Subscribe(_ => Patrol());
        Patrol();
    }

    public override void StateExit()
    {
        base.StateExit();
        _patrolStream.Dispose();
    }
    #endregion

    #region Patrol

    private void Patrol()
    {
        if (!_navMeshAgent) return;
        
        _destination = GetRandomPosition();
        _navMeshAgent.SetDestination(_destination);
    }
    
    private Vector3 GetRandomPosition()
    {
        Vector3 randomPos = patrolOrigin + Random.insideUnitSphere * patrolRadius;
        
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hitInfo, patrolRadius, NavMesh.AllAreas))
        {
            return hitInfo.position;
        }
        
        throw new Exception($"[{this.name}] AiAgent Stuck");
    }
    #endregion
}