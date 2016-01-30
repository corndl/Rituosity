using UnityEngine.Events;

namespace Engine
{
    public class BoolEvent : UnityEvent<bool> { }
    public class StringEvent : UnityEvent<string> { }
    public class FloatEvent : UnityEvent<float> { }
    public class IntEvent : UnityEvent<int> { }
}