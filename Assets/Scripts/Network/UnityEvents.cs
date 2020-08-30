using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringEvent : UnityEvent<string> { }

[System.Serializable]
public class StringArrEvent : UnityEvent<string[]> { }

[System.Serializable]
public class VectorEvent : UnityEvent<Vector3> { }

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

[System.Serializable]
public class HitInfoEvent : UnityEvent<RaycastHit> { }

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

[System.Serializable]
public class IntEvent : UnityEvent<int> { }