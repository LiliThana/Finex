using Godot;
using System;

public interface IState
{
    /// <summary>
    /// ## Summary
    /// This signals the FSM to change to the provided state.
    /// </summary>
    /// <param name="newState">The state that is to be changed to.</param>
    [Signal]
    delegate void ChangeState(string newState);

    /// <summary>
    /// ## Summary
    /// This signals the FSM to pop off the current state.
    /// </summary>
    [Signal]
    delegate void PopState();

    /// <summary>
    /// ## Summary
    /// This signals the FSM to push the provided state onto the stack.
    /// </summary>
    /// <param name="newState">The state to be pushed</param>
    [Signal]
    delegate void PushState(string newState);

    /// <summary>
    /// ## Summary
    /// This is called once upon entering the state, before any processing occurs.
    /// </summary>
    /// <remarks>
    /// ## Remarks
    /// This method is meant to be overridden to handle any configuration that needs to be done once a state is entered. It is called only once, when entering the state.
    /// </remarks>
    public void Enter();

    /// <summary>
    /// ## Summary
    /// This method handles all the input for the state.
    /// </summary>
    /// <remarks>
    /// ## Remarks
    /// This method is called whenever <see cref="FSM._Input"/> is called.
    /// </remarks>
	public void HandleInput(InputEvent @event);

    /// <summary>
    /// ## Summary
    /// This method handles all logic updates for the state.
    /// </summary>
    /// <param name="delta">The delta time since the last call.</param>
    /// <remarks>
    /// ## Remarks
    /// This method is called whenever <see cref="FSM._Process(double)"/> is called.
    /// </remarks>
	public void Update(double delta);

    /// <summary>
    /// ## Summary
    /// This method handles all physics updates for the state.
    /// </summary>
    /// <param name="delta">The delta time since the last call.</param>
    /// <remarks>
    /// ## Remarks
    /// This method is called whenever <see cref="FSM._PhysicsProcess(double)"/> is called.
    /// </remarks>
    public void UpdatePhysics(double delta);

    /// <summary>
    /// ## Summary
    /// This method is called once upon exiting the state, after all processing occurs.
    /// </summary>
    /// <remarks>
    /// ## Remarks
    /// This method is meant to be overridden to handle any deconstruction or configuring that needs to be done once a state is exiting. It is only called once, upon exiting the state.
    /// </remarks>
    public void Exit();
}
