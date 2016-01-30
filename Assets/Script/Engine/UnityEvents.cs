using UnityEngine;
using UnityEngine.Events;
using System;

namespace Engine
{
    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [Serializable]
    public class IntEvent : UnityEvent<int> { }

    [Serializable]
    public class Vector3Event : UnityEvent<Vector3> { }

    [Serializable]
    public class QuaternionEvent : UnityEvent<Quaternion> { }
}