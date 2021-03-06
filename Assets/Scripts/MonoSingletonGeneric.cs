﻿using UnityEngine;

public class MonoSingletonGeneric<T> : MonoBehaviour where T : MonoSingletonGeneric<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance == null) { instance = (T)this; }
        else
        {
            Debug.LogError("Duplicate singleton creation detected, deleting duplicate - " + typeof(T));
            Destroy(this);
        }
    }
}
