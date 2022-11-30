using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSingleton<T> : MonoBehaviour where T : TSingleton<T>
{
    private static T _uniqueInstance = null;
   // static volatile GameObject _uniqueObject = null;

    public static T _instance
    {
        get
        {
            if (_uniqueInstance == null)
            {
                GameObject obj = GameObject.Find(typeof(T).Name);
                if(obj == null)
                {
                    obj = new GameObject(typeof(T).Name);
                    _uniqueInstance = obj.AddComponent<T>();
                }
                else
                {
                    _uniqueInstance =obj.GetComponent<T>();
                }
            }
            return _uniqueInstance;
        }
    }

    protected virtual void Init()
    {
        DontDestroyOnLoad(gameObject);
    }
}

