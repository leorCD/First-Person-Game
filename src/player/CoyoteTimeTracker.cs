using Godot;
using System;

public partial class CoyoteTimeTracker : Label
{
    [Export] public Timer Timer;

    public override void _Process(double delta)
    {
        Text = Timer.TimeLeft.ToString();
    }

}
