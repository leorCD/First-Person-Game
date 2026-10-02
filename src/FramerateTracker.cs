using Godot;
using System;

public partial class FramerateTracker : Label
{
    public override void _Process(double delta)
    {
        double fps = Engine.GetFramesPerSecond();
        Text = (fps + " FPS");
    }
}
