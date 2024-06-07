public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParamaterHash);
        StopAnimation(stateMachine.Player.AnimationData.WalkParamaterHash);
        StopAnimation(stateMachine.Player.AnimationData.AttackParamaterHash);
    }
    public override void Exit() 
    { 
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParamaterHash);
    }
    public override void Execute()
    {
        base.Execute();
    }
}
