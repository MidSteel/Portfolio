using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public delegate void OnUpdate();
    public OnUpdate onUpdate;

    private void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        onUpdate?.Invoke();
    }

    public bool DoesContainMethodInOnUpdate(string MethodName)
    {
        if (onUpdate == null) { return false; }

        foreach (Delegate del in onUpdate.GetInvocationList())
        {
            if (del.Method.Name == MethodName) { return true; }
        }

        return false;
    }
}
