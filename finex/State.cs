using Godot;
using System;

public partial class State : Node, IState
{
	public virtual void Enter(){}

	public virtual void HandleInput(InputEvent @event){}

	public virtual void Update(double delta){}

	public virtual void UpdatePhysics(double delta){}

	public virtual void Exit(){}
}
