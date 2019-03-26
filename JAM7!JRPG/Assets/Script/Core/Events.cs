using UnityEngine.Events;

public class EventOnDataChange1<T> : UnityEvent<T> { }           // EventHandler(T current)
public class EventOnDataChange2<T> : UnityEvent<T, T> { }        // EventHandler(T current, T previous)
