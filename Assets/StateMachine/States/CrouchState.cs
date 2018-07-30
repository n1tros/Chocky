//internal class CrouchState : PlayerMovementState
//{
//    public CrouchState(PlayerInput input) : base(input)
//    {
//    }

//    public override void Enter()
//    {
//        _input.Agent.IsCrouched = true;
//    }

//    public override void Tick()
//    {
//        if (_input.Crouch() == false)
//            _input.ChangeState(new GroundedState(_input));

//        base.Tick();
//    }

//    public override void Exit()
//    {
//        _input.Agent.IsCrouched = false;
//    }
//}