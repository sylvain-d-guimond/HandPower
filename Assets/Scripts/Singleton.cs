using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : UnityEngine.Object {

    public static T instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                    Debug.LogError("Singleton of type " + typeof(T).Name + " not found in scene");
            }
            return _instance;
        }
    }

    private static T _instance;
}
