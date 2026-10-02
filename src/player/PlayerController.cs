using System;
using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float Speed = 6f;
    [Export] public float JumpVelocity = 5f;
    [Export] public float CoyoteTime = 0.135f;
    [Export] public float InputBuffer = 0.085f;
    private float _projectGravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
    private Vector3 _velocity = Vector3.Zero;
    private Timer _coyoteTimer;
    private Timer _inputBuffer;
    private Camera3D CurrentCamera;
    private void CameraAdded(Camera3D newcamera)
    {
        CurrentCamera = newcamera;
    }







    public override void _Ready()
    {
        CameraManager.CameraRegistered += CameraAdded;

        _coyoteTimer = GetNode<Timer>("CoyoteTimer");
        _inputBuffer = GetNode<Timer>("InputBuffer");
    }
    
    private readonly StringName _forward = "W", _backward = "S", _left = "A", _right = "D", _jump = "Jump";
    public override void _PhysicsProcess(double delta)
    {
        _velocity = Velocity;

        // gravity
        if (!IsOnFloor())
        {
            _velocity.Y -= _projectGravity * (float)delta;
        }
        
        // jump
        _JumpLogic();

        // movement
        Vector2 _input = Input.GetVector(_left, _right, _forward, _backward);
        Vector3 Direction = new Vector3(_input.X, 0, _input.Y);
        Vector3 TransformedDirection = (Transform.Basis * Direction).Normalized();

        _velocity.X = TransformedDirection.X * Speed;
        _velocity.Z = TransformedDirection.Z * Speed;



        Velocity = _velocity;
        MoveAndSlide();
    }

    private void _JumpLogic()
    {
        if (IsOnFloor() && Velocity.Y <= 0)
        {
            _coyoteTimer.Start(CoyoteTime);
        }

        if (Input.IsActionJustPressed(_jump))
        {
            _inputBuffer.Start(InputBuffer);
        }

        if (!_coyoteTimer.IsStopped() && !_inputBuffer.IsStopped())
        {
            Jump();
            _coyoteTimer.Stop();
            _inputBuffer.Stop();
        }
    }

    private void Jump()
    {
        _velocity.Y = JumpVelocity;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            if (CurrentCamera == null) return;

            Rotation = new Vector3(0, CurrentCamera.Rotation.Y, 0);
        }
    }

}
