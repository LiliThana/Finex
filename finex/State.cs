using Godot;
using System;

public partial class State : Node
{
	/// <summary>
    /// ## Summary
    /// This signals the FSM to change to the provided state.
    /// </summary>
    /// <param name="newState">The state that is to be changed to.</param>
    [Signal]
    public delegate void ChangeStateEventHandler(string newState);

    /// <summary>
    /// ## Summary
    /// This signals the FSM to pop off the current state.
    /// </summary>
    [Signal]
    public delegate void PopStateEventHandler();

    /// <summary>
    /// ## Summary
    /// This signals the FSM to push the provided state onto the stack.
    /// </summary>
    /// <param name="newState">The state to be pushed</param>
    [Signal]
    public delegate void PushStateEventHandler(string newState);

	public virtual void Enter(){}

	public virtual void HandleInput(InputEvent @event){}

	public virtual void Update(double delta){}

	public virtual void UpdatePhysics(double delta){}

	public virtual void Exit(){}
}
