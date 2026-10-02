using System;
using Godot;

public partial class PlayerController : CharacterBody3D
{
    [Export] public float SPEED = 6f;
    [Export] public float JUMP_VELOCITY = 5f;
    private float _projectGravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

    private Camera3D CurrentCamera;
    public override void _Ready()
    {
        CameraManager.CameraRegistered += CameraAdded;
    }
    private void CameraAdded(Camera3D newcamera)
    {
        CurrentCamera = newcamera;
    }







    
    private readonly StringName _forward = "W", _backward = "S", _left = "A", _right = "D", _jump = "Jump";
    public override void _PhysicsProcess(double delta)
    {
        Vector3 _velocity = Velocity;

        // gravity
        if (!IsOnFloor())
        {
            _velocity.Y -= _projectGravity * (float)delta;
        }
        
        // jump
        if (Input.IsActionJustPressed(_jump) && IsOnFloor())
        {
            _velocity.Y = JUMP_VELOCITY;
        }

        // movement
        Vector2 _input = Input.GetVector(_left, _right, _forward, _backward);
        Vector3 Direction = new Vector3(_input.X, 0, _input.Y);
        Vector3 TransformedDirection = (Transform.Basis * Direction).Normalized();

        _velocity.X = TransformedDirection.X * SPEED;
        _velocity.Z = TransformedDirection.Z * SPEED;



        Velocity = _velocity;
        MoveAndSlide();
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
