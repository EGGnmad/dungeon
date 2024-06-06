public class PlayerIdleState : StateBehaviorBase<PlayerState>
{
    public override PlayerState GetStateKey() => PlayerState.Idle;
}