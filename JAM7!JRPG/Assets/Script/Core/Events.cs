using UnityEngine.Events;

public class EventOnDataUpdate<T> : UnityEvent<T> { }           // EventHandler(T current)
public class EventOnDataChange<T> : UnityEvent<T, T> { }        // EventHandler(T current, T previous)
