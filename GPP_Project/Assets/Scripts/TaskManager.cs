using System.Collections.Generic;
using UnityEngine;

public class TaskManager {

    private readonly List<Task> _tasks = new List<Task>();

    // Tasks can only be added and you have to abort them
    // to remove them before they complete on their own
    public Task Do(Task task)
    {
        Debug.Assert(task != null);
        // NOTE: Only add tasks that aren't already attached.
        // Don't want multiple task managers updating the same task
        Debug.Assert(!task.IsAttached);
        _tasks.Add(task);
        task.SetStatus(Task.TaskStatus.Pending);
        return task;
    }

    public void Update()
    {
        // iterate through all the tasks
        for (var i = _tasks.Count - 1; i >= 0; --i)
        {
            var task = _tasks[i];

            // Initialize any tasks that have just been added
            if (task.IsPending)
            {
                task.SetStatus(Task.TaskStatus.Working);
            }

            if (task.IsFinished)
            {
                HandleCompletion(task, i);
            }
            else
            {
                task.Update();
            }

        }
    }

    private void HandleCompletion(Task task, int taskIndex)
    {
        // If the finished task has a "next" task
        // queue it up - but only if the original task was
        // successful
        if (task.NextTask != null && task.IsSuccessful)
        {
            Do(task.NextTask);
        }
        // clear the task from the manager and let it know
        // it's nodsa longer being managed
        _tasks.RemoveAt(taskIndex);
        task.SetStatus(Task.TaskStatus.Detached);
    }
}
