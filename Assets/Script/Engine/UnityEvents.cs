using UnityEngine;
using UnityEngine.Events;

namespace Engine
{
    [SerializeField]
    public class BoolEvent : UnityEvent<bool> { }

    [SerializeField]
    public class StringEvent : UnityEvent<string> { }

    [SerializeField]
    public class FloatEvent : UnityEvent<float> { }

    [SerializeField]
    public class IntEvent : UnityEvent<int> { }

    [SerializeField]
    public class Vector3Event : UnityEvent<Vector3> { }

    [SerializeField]
    public class QuaternionEvent : UnityEvent<Quaternion> { }
}