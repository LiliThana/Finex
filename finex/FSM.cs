using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class FSM : Node
{
	private List<State> stateStack = new List<State>();

	private Dictionary<string, State> stateMap = new Dictionary<string, State>();

	[Signal]
	public delegate void StateEnteredEventHandler(State state);

	[Signal]
	public delegate void StateChangedEventHandler(State newState);

	[Signal]
	public delegate void StatePushedEventHandler(State newState);

	[Signal]
	public delegate void StatePoppedEventHandler();

	[Signal]
	public delegate void StateExitedEventHandler(State state);

	/// <summary>
	/// ## Summary
	/// Gets the currently active state.
	/// </summary>
	/// <value>
	/// ## Value
	/// The IState at the top of the state stack.
	/// </value>
	/// <remarks>
	/// ## Remarks
	/// <c>CurrentState</c> is the state sitting on the top of the state stack. All operations done by the FSM will be done to this state.
	/// </remarks>
	public State CurrentState
	{
		get { return stateStack[0]; }
		private set { stateStack.Insert(0, value); }
	}

	/// <summary>
	/// ## Summary
	/// Gets the state map for this FSM.
	/// </summary>
	/// <value>
	/// ## Value
	/// Dictionary&lt;string, IState>
	/// <para>
	/// string is the name of the state.
	/// </para>
	/// </value>
	/// <remarks>
	/// ## Remarks
	/// The <c>StateMap</c> is a dictionary that maps the name of the string to the actual State object. The FSM
	/// can then instantiate all the states and easily swap between them using ChangeState(), PushState(),
	/// and PopState().
	/// </remarks>
	public Dictionary<string, State> StateMap
	{
		get { return stateMap; }
		protected set { stateMap = value; }
	}
	
    public override void _Ready()
    {
        Configure();
    }
	
    public override void _Input(InputEvent @event)
    {
        CurrentState.HandleInput(@event);
    }

	public override void _Process(double delta)
	{
		CurrentState.Update(delta);
	}

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.UpdatePhysics(delta);
    }

	/// <summary>
	/// Configures the FSM.
	/// </summary>
	/// <remarks>
	/// Configure takes its child states in the node hierarchy and maps them to the State Map. At least one
	/// state node must be added as a child or <c>Configure</c> will throw an exception.
	/// </remarks>
	public void Configure()
	{
		Godot.Collections.Array<Node> children = GetChildren();
		if (children.Count <= 0)
		{
			throw new InvalidOperationException("FSM has no States as children");
		}

		// Populate StateMap and connect signals
		foreach(State state in children)
		{
			string stateName = state.Name.ToString();
			StateMap[stateName] = state;
			state.ChangeState = new Callable(this, MethodName.ChangeState);
			state.PopState = new Callable(this, MethodName.PopState);
			state.PushState = new Callable(this, MethodName.PushState);
		}

		CurrentState = GetChild<State>(0);
		CurrentState.Enter();
	}

	/// <summary>
	/// ## Summary
	/// Changes the current state to the one provided.
	/// </summary>
	/// <param name="newState">The new state that will become the current state.</param>
	/// <remarks>
	/// ## Remarks
	/// This changes <see cref="CurrentState"/> to <c>newState</c>. This removes the current state from <see cref="StateMap"/> and replaces it with <c>newState</c>.
	/// Use this for whenever you don't intend to need to return to the current state immediately.
	/// </remarks>
	public void ChangeState(string newState)
	{
		State previousState = CurrentState;

		CurrentState.Exit();
		EmitSignal(SignalName.StateExited, CurrentState);

		CurrentState = StateMap[newState];
		CurrentState.Enter();
		EmitSignal(SignalName.StateEntered, CurrentState);
		EmitSignal(SignalName.StateChanged, CurrentState);
	}

	/// <summary>
	/// ## Summary
	/// Pushes a new state onto the state stack.
	/// </summary>
	/// <param name="newState">The new state that is being pushed onto the state stack.</param>
	/// <remarks>
	/// ## Remarks
	/// This pushes <c>newState</c> onto the top of the state stack. <c>newState</c> then becomes <see cref="CurrentState"/>. Use this in conjunction with PopState() for using states that swap from one to
	/// the other.
	/// </remarks>
	/// <example>
	/// ## Example
	/// An examples is pushing Jump onto Idle, Run, or Walk states. Once the jump is over you can pop it off and then return to the previous state.
	/// </example>
	public void PushState(string newState)
	{
		CurrentState.Exit();
		EmitSignal(SignalName.StateExited, CurrentState);

		stateStack.Insert(0, StateMap[newState]);
		CurrentState.Enter();
		EmitSignal(SignalName.StateEntered, CurrentState);
		EmitSignal(SignalName.StatePushed, CurrentState);
	}

	/// <summary>
	/// ## Summary
	/// Pops the current state off the state stack.
	/// </summary>
	/// <remarks>
	/// ## Remarks
	/// This pops <see cref="CurrentState"/> off the top of the stack, moving the state underneath it onto the top of the stack. Use this to return to a previous state.
	/// </remarks>
	/// <example>
	/// ## Example
	/// An example is popping Jump upon the player landing and returning to Idle, Walk, or Run.
	/// </example>
	public void PopState()
	{
		CurrentState.Exit();
		EmitSignal(SignalName.StateExited, CurrentState);

		stateStack.RemoveAt(0);
		CurrentState.Enter();
		EmitSignal(SignalName.StateEntered, CurrentState);
		EmitSignal(SignalName.StatePopped);
	}
}
