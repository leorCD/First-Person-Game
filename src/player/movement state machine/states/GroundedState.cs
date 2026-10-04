using Godot;
using System;

public partial class GroundedState : MovementState
{
    public override bool CanEnter()
    {
        if (!this.Player.IsOnFloor()) return false;

        return true;
    }



    private float _projectGravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    private readonly StringName _forward = "W", _backward = "S", _left = "A", _right = "D", _jump = "Jump";

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate(double delta)
    {
        Player.CurrentVelocity = Player.Velocity;

        // gravity
        if (!Player.IsOnFloor())
        {
            Player.CurrentVelocity.Y -= _projectGravity * (float)delta;
        }
        
        // jump
        _JumpLogic();

        // movement
        Vector2 _input = Input.GetVector(_left, _right, _forward, _backward);
        Vector3 Direction = new Vector3(_input.X, 0, _input.Y);
        Vector3 TransformedDirection = (Player.Transform.Basis * Direction).Normalized();

        Player.CurrentVelocity.X = TransformedDirection.X * Player.Speed;
        Player.CurrentVelocity.Z = TransformedDirection.Z * Player.Speed;



        Player.Velocity = Player.CurrentVelocity;
        Player.MoveAndSlide();
    }


    private void _JumpLogic()
    {
        if (Player.IsOnFloor() && Player.Velocity.Y <= 0)
        {
            Player.CoyoteTimer.Start(Player.CoyoteTimeLength);
        }

        if (Input.IsActionJustPressed(_jump))
        {
            Player.InputBuffer.Start(Player.InputBufferLength);
        }

        if (!Player.CoyoteTimer.IsStopped() && !Player.InputBuffer.IsStopped())
        {
            Jump();
            Player.CoyoteTimer.Stop();
            Player.InputBuffer.Stop();
        }
    }

    private void Jump()
    {
        Player.CurrentVelocity.Y = Player.JumpVelocity;
    }
}
