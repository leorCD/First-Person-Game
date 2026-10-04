using Godot;
using System;

public abstract partial class MovementState : Node
{
    protected PlayerController Player {get; private set;}
    public virtual void Init(PlayerController player) => this.Player = player;

    public virtual bool CanEnter() => true; // return true by default
    public virtual void Enter() // execute on enter
    {
        GD.Print($"Entered {this.GetType()}");
    }
    public virtual void Exit(){} // execute on exit

    public virtual void PhysicsUpdate(double delta){}
}
