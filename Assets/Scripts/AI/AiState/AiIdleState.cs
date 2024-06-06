using System;
using UniRx;

public class AiIdleState : StateBehaviorBase<AiState>
{
    private IStateMachine<AiState> _stateMachine;
    private IDisposable timerStream;

    #region Unity

    private void Start()
    {
        _stateMachine = GetComponent<IStateMachine<AiState>>();
    }

    #endregion

    public override AiState GetStateKey() => AiState.Idle;

    public override void StateEnter()
    {
        base.StateEnter();
        timerStream = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ => _stateMachine.ChangeState(AiState.Patrol));
    }

    public override void StateExit()
    {
        base.StateExit();
        timerStream.Dispose();
    }
}