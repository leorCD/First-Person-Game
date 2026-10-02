using Godot;

public partial class CameraController : Camera3D
{
    // [Signal] public delegate void YawInputEventHandler(float newYaw);

    [Export] public float MouseSensitivity = 0.001f;
    [Export] public Node3D adornee;
    [Export] public Vector3 offset = new Vector3(0, 0, 0);
    
    private float _yaw = 0f;
    private float _pitch = 0f;

    public override void _Ready()
    {
        ProcessPhysicsPriority = 100;

        CameraManager.RegisterCamera(this);

        Input.MouseMode = Input.MouseModeEnum.Captured;
        _yaw = Rotation.Y;
        _pitch = Rotation.X;
    }




    public override void _UnhandledInput(InputEvent @event)
    {
        // toggle mouse lock via Tab
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Tab)
        {
            Input.MouseMode = (Input.MouseMode == Input.MouseModeEnum.Captured) ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
        }

        if (@event is InputEventMouseMotion mouseMotion)
        {
            _yaw -= mouseMotion.Relative.X * MouseSensitivity;
            _pitch = Mathf.Clamp(_pitch - mouseMotion.Relative.Y * MouseSensitivity, Mathf.DegToRad(-90), Mathf.DegToRad(90));

            Vector3 newRotation = new Vector3(_pitch, _yaw, 0.0f);
            Rotation = newRotation;         

            // EmitSignal(SignalName.YawInput, _yaw);   
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        // update position
        if (adornee != null)
        {
            GlobalPosition = adornee.GlobalPosition + offset;
        }
    }
}
