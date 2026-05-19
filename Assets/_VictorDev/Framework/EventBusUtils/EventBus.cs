using System;
using System.Collections.Generic;

public static class EventBus
{
    // 每個事件名稱對應一個 Delegate（可能是 Action、Action<T> 等）
    private static readonly Dictionary<string, Delegate> eventTable =
        new(StringComparer.OrdinalIgnoreCase);

    // --- 訂閱 ---
    public static void Subscribe(string eventName, Action listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
            eventTable[eventName] = (Action)del + listener;
        else
            eventTable[eventName] = listener;
    }

    public static void Subscribe<T>(string eventName, Action<T> listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
            eventTable[eventName] = (Action<T>)del + listener;
        else
            eventTable[eventName] = listener;
    }

    // --- 取消訂閱 ---
    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            del = (Action)del - listener;

            if (del == null) eventTable.Remove(eventName);
            else eventTable[eventName] = del;
        }
    }

    public static void Unsubscribe<T>(string eventName, Action<T> listener)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            del = (Action<T>)del - listener;

            if (del == null) eventTable.Remove(eventName);
            else eventTable[eventName] = del;
        }
    }

    // --- 發送事件 ---
    public static void Publish(string eventName)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            (del as Action)?.Invoke();
        }
    }

    public static void Publish<T>(string eventName, T data)
    {
        if (eventTable.TryGetValue(eventName, out var del))
        {
            (del as Action<T>)?.Invoke(data);
        }
    }
}