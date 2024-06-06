using UnityEngine;
using UnityEngine.AI;

public class AiChaseState : StateBehaviorBase<AiState>
{
    [HideInInspector] public GameObject target;

    [Header("config")]
    [SerializeField] private float maxChaseDistane;
    [SerializeField] private float maxAttackDistance = 1.5f;

    [SerializeField] private AudioSource sound;

    private NavMeshAgent _navMeshAgent;
    private IStateMachine<AiState> _stateMachine;

    public override AiState GetStateKey() => AiState.Chase;

    #region Unity

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _stateMachine = GetComponent<IStateMachine<AiState>>();
    }

    #endregion

    #region FSM

    public override void StateEnter()
    {
        sound?.Play();
    }

    public override void StateUpdate()
    {
        if (!target)
        {
            _stateMachine.ChangeState(AiState.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.transform.position);
        
        // Can Chase
        if (distance > maxChaseDistane)
        {
            _stateMachine.ChangeState(AiState.Idle);
            return;
        }

        // Attack
        if (distance <= maxAttackDistance)
        {
            target.GetComponent<PlayerAgent>()?.Die("라면먹고 갈래?");
            _stateMachine.ChangeState(AiState.Idle);
        }

        _navMeshAgent.SetDestination(target.transform.position);
    }
    #endregion
}