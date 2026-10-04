using System;
using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] private MovementStateMachine _movementMachine;

    [ExportCategory("Movement")]
    [Export] public float Speed = 6f;
    [Export] public float JumpVelocity = 5f;
    [Export] public float CoyoteTimeLength = 0.135f;
    [Export] public float InputBufferLength = 0.085f;
    public Vector3 CurrentVelocity = Vector3.Zero;
    public Timer CoyoteTimer;
    public Timer InputBuffer;

    public Camera3D CurrentCamera;
    private float _projectGravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    private void CameraAdded(Camera3D newcamera)
    {
        CurrentCamera = newcamera;
    }







    public override void _Ready()
    {
        _movementMachine = GetNodeOrNull<MovementStateMachine>("MovementStateMachine");

        CameraManager.CameraRegistered += CameraAdded;

        CoyoteTimer = GetNode<Timer>("CoyoteTimer");
        InputBuffer = GetNode<Timer>("InputBuffer");

        _movementMachine?.Init(this);
    }
    
    // private readonly StringName _forward = "W", _backward = "S", _left = "A", _right = "D", _jump = "Jump";
    // public override void _PhysicsProcess(double delta)
    // {
    //     _velocity = Velocity;

    //     // gravity
    //     if (!IsOnFloor())
    //     {
    //         _velocity.Y -= _projectGravity * (float)delta;
    //     }
        
    //     // jump
    //     _JumpLogic();

    //     // movement
    //     Vector2 _input = Input.GetVector(_left, _right, _forward, _backward);
    //     Vector3 Direction = new Vector3(_input.X, 0, _input.Y);
    //     Vector3 TransformedDirection = (Transform.Basis * Direction).Normalized();

    //     _velocity.X = TransformedDirection.X * Speed;
    //     _velocity.Z = TransformedDirection.Z * Speed;



    //     Velocity = _velocity;
    //     MoveAndSlide();
    // }

    // private void _JumpLogic()
    // {
    //     if (IsOnFloor() && Velocity.Y <= 0)
    //     {
    //         CoyoteTimer.Start(CoyoteTimeTime);
    //     }

    //     if (Input.IsActionJustPressed(_jump))
    //     {
    //         InputBuffer.Start(InputBufferTime);
    //     }

    //     if (!CoyoteTimer.IsStopped() && !InputBuffer.IsStopped())
    //     {
    //         Jump();
    //         CoyoteTimer.Stop();
    //         InputBuffer.Stop();
    //     }
    // }

    // private void Jump()
    // {
    //     _velocity.Y = JumpVelocity;
    // }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (CurrentCamera == null) return;

            Rotation = new Vector3(0, CurrentCamera.Rotation.Y, 0);
        }
    }

}
