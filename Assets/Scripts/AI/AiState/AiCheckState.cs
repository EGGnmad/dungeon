using UnityEngine;
using UnityEngine.AI;

public class AiCheckState : StateBehaviorBase<AiState>
{
    [HideInInspector] public Vector3 checkPos;
    [Header("config")] 
    [SerializeField] private float maxDistance = 1f;

    private AiHearingSensor _hearingSensor;

    private NavMeshAgent _navMeshAgent;
    private IStateMachine<AiState> _stateMachine;

    public override AiState GetStateKey() => AiState.Check;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _stateMachine = GetComponent<IStateMachine<AiState>>();
        _hearingSensor = GetComponent<AiHearingSensor>();
    }

    #region FSM
    public override void StateEnter()
    {
        if (NavMesh.SamplePosition(checkPos, out NavMeshHit hitInfo, 3f, NavMesh.AllAreas))
        {
            checkPos = hitInfo.position;
        }

        _navMeshAgent.SetDestination(checkPos);
    }

    public override void StateUpdate()
    {
        float distance = Vector3.Distance(transform.position, checkPos);
        if (distance <= maxDistance)
        {
            _stateMachine.ChangeState(AiState.Idle);
        }
    }
    #endregion
    
    
}