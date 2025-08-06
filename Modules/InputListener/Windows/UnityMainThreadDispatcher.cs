using System.Collections.Concurrent;
using System;
using UnityEngine;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static readonly ConcurrentQueue<Action> queue = new();
    private const int MaxPerFrame = 10;

    public static void Enqueue(Action action)
    {
        if (action != null)
            queue.Enqueue(action);
    }

    void Update()
    {
        int count = 0;
        while (count++ < MaxPerFrame && queue.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }
}
