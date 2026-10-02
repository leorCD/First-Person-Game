using Godot;
using System;

public partial class CameraManager : Node
{
    public static event Action<Camera3D> CameraRegistered;
    public static Camera3D ActiveCamera {get; private set;}



    public static void RegisterCamera(Camera3D newcamera)
    {
        ActiveCamera = newcamera;

        CameraRegistered?.Invoke(newcamera);
    }

    public static Camera3D GetActiveCamera()
    {
        return ActiveCamera;
    }
}
