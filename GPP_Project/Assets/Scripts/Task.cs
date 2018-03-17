using System;
using System.Diagnostics;
// ReSharper disable All

public abstract class Task {

    // An enum representing the current state of the task
    public enum TaskStatus : byte
    {
        Detached, // Task has not been attached to a TaskManager
        Pending, // Task has not been initialized
        Working, // Task has been initialized
        Success, // Task completed successfully
        Fail, // Task completed unsuccessfully
        Aborted // Task was aborted
    }

    // The only member variable that a base task has is its status
    public TaskStatus Status { get; private set; }

    ////////////////////
    // LIFECYCLE
    ////////////////////

    // Override this to handle initialization of the task.
    // This is called when the task enters the Working state
    protected virtual void Init() {}

    // Called whenever the TaskManager updates. Your tasks' work
    // generally goes here
    internal virtual void Update() {}

    // This is called when the tasks completes (i.e. is aborted,
    // fails, or succeeds). It is called after the status change
    // handlers are called
    protected virtual void CleanUp() {}

    // Status change handlers that allow subclasses
    // to do something when the task finishes in some way
    protected virtual void OnAbort() {}

    protected virtual void OnSuccess() {}

    protected virtual void OnFail() {}

    ////////////////////
    // STATUS MANAGEMENT
    ////////////////////

    // Convenience method for external classes to abort the task
    public void Abort()
    {
         SetStatus(TaskStatus.Aborted);
    }

    // A method for changing the status of the task
    // It's marked internal so that the manager can access it
    internal void SetStatus(TaskStatus newStatus)
    {
        if (Status == newStatus) return;

        Status = newStatus;

        switch (newStatus)
        {
            // Initialize the task when the Task first starts
            // It's important to separate initialization from
            // the constructor, since tasks may not start
            // running until long after they've been constructed
            case TaskStatus.Working:
                Init();
                break;

            // Success/Aborted/Failed are the completed states of a task.
            // Subclasses are notified when entering one of these states
            // and are given the opportunity to do any clean up
            case TaskStatus.Success:
                OnSuccess();
                CleanUp();
                break;

            case TaskStatus.Aborted:
                OnAbort();
                CleanUp();
                break;

            case TaskStatus.Fail:
                OnFail();
                CleanUp();
                break;

            // These are "internal" states that are mostly relevant for
            // the task manager
            case TaskStatus.Detached:
            case TaskStatus.Pending:
                break;
            default:
                throw new ArgumentOutOfRangeException(newStatus.ToString(), newStatus, null);
        }
    }

    // Convenience status checking
    public bool IsDetached { get { return Status == TaskStatus.Detached; } }
    public bool IsAttached { get { return Status != TaskStatus.Detached; } }
    public bool IsPending { get { return Status == TaskStatus.Pending; } }
    public bool IsWorking { get { return Status == TaskStatus.Working; } }
    public bool IsSuccessful { get { return Status == TaskStatus.Success; } }
    public bool IsFailed { get { return Status == TaskStatus.Fail; } }
    public bool IsAborted { get { return Status == TaskStatus.Aborted; } }
    public bool IsFinished { get { return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted); } }


    ////////////////////
    // SEQUENCING
    ////////////////////

    // Assign a task to be run if this task runs successfully
    public Task NextTask { get; private set; }

    // Sets a task to be automatically attached when this one completes successfully
    // NOTE: if a task is aborted or fails, its next task will not be queued
    // NOTE: ** DO NOT ** assign attached tasks with this method.
    public Task Then(Task task)
    {
        Debug.Assert(!task.IsAttached);
        NextTask = task;
        return task;
    }

}
