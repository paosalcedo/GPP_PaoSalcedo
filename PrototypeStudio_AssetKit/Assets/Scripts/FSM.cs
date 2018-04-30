using System;
using System.Collections.Generic;
using System.Diagnostics;


// To make it a little easier to reference the state machine's context we make the FSM
// a generic class that is parameterized on the context type. This way states in the
// machine won't have to cast the context when accessing it.
public class FSM<TContext>
{
    // States are going to need access to the objects whose state they represent (e.g. PlayerStates
    // need to have access to a player object) The state machine keeps a reference to that context
    // so states can access it.
    // We make the context readonly so we can be sure that states in the machine can't get their
    // context swapped on them.
    private readonly TContext _context;

    // We cache the machine's states in a dictionary in case we need to use them again.
    // This bit is entirely optional though...
    private readonly Dictionary<Type, State> _stateCache = new Dictionary<Type, State>();

    // We keep track of the state machine's current state and expose it through a public
    // property in case someone needs to query it.
    public State CurrentState { get; private set; }

    // We don't want to change the current state in the middle of an update, so when a transition is called
    private State _pendingState;

    // A trivial constructor. We have to initialize the context here since it's readonly.
    public FSM(TContext context)
    {
        _context = context;
    }

    // We use a simple update method to keep the current state moving along...
    public void Update()
    {
        // Handle any pending transition if someone called TransitionTo externally (although they probably shouldn't)
        PerformPendingTransition();
        // Make sure there's always a current state to update...
        Debug.Assert(CurrentState != null, "Updating FSM with null current state. Did you forget to transition to a starting state?");
        CurrentState.Update();
        // Handle any pending transition that might have happened during the update
        PerformPendingTransition();
    }

    // Queues transition to a new state
    public void TransitionTo<TState>() where TState : State
    {
        // We do the actual transtion
        _pendingState = GetOrCreateState<TState>();
    }

    // Actually transition to any pending state
    private void PerformPendingTransition()
    {
        if (_pendingState != null)
        {
            if (CurrentState != null) CurrentState.OnExit();
            CurrentState = _pendingState;
            CurrentState.OnEnter();
            _pendingState = null;
        }
    }

    // A helper method to help with managing the caching of the state instances
    private TState GetOrCreateState<TState>() where TState : State
    {
        State state;
        if (_stateCache.TryGetValue(typeof (TState), out state))
        {
            return (TState) state;
        }
        else
        {
            // This activator business is required to create instances of states
            // using only the type
            var newState = Activator.CreateInstance<TState>();
            newState.Parent = this;
            newState.Init();
            _stateCache[typeof(TState)] = newState;
            return newState;
        }
    }

    // We define the base class for states inside the FSM class so it's tied to the
    // context type of the FSM class. This way you can't try to transition to a state
    // that is for a different type of context.
    public abstract class State
    {
        // We keep track of the FSM the state belongs to so we can tell it to
        // transition from within states.
        internal FSM<TContext> Parent { get; set; }

        // This property is just a little sugar so that State subclasses don't have
        // write out Parent._context every time.
        protected TContext Context { get { return Parent._context; } }

        // A convenience method for transitioning from inside a state
        protected void TransitionTo<TState>() where TState : State
        {
            Parent.TransitionTo<TState>();
        }

        // NOTE: These methods are all public because if they were protected or private the FSM couldn't
        // call them, and if they were internal subclasses in different assemblies couldn't access them
        // It's weird but it's the best solution I can think of without adding a bunch of boilerplate

        // This is called once when the state is first created (think of it like Unity's Awake)
        public virtual void Init() { }

        // This is called whenever the state becomes active (think of it like Unity's Start)
        public virtual void OnEnter() { }

        // this is called whenever the state becomes inactive
        public virtual void OnExit() { }

        // This is your standard update method where most of your work should go
        public virtual void Update() { }

        // called when the state machine is cleared, and where you should clear resources
        public virtual void CleanUp() { }
    }

}