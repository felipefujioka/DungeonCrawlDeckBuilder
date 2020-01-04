using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{

    private static CoroutineManager instance;
    public static CoroutineManager Instance => instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new Exception("Another coroutine manager already instantiated");
        }
        
        DontDestroyOnLoad(gameObject);
    }
}
