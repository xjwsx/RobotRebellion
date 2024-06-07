public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine playerstateMachine) : base(playerstateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.WalkParamaterHash);
        StopAnimation(stateMachine.Player.AnimationData.IdleParamaterHash);
        StopAnimation(stateMachine.Player.AnimationData.AttackParamaterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParamaterHash);
    }
}
