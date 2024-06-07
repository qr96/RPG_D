using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
	public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
	{
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

	public static bool IsValid(this GameObject go)
	{
		return go != null && go.activeSelf;
	}

    public static GameObject Find(this GameObject go, string path)
    {
		var names = path.Split("/");
		var parent = go.transform;

		foreach (var name in names)
            parent = parent.Find(name);

        return parent.gameObject;
    }

    public static T Find<T>(this GameObject go, string path) where T : Component
	{
		var component = go.Find(path).GetComponent<T>();
		if (component == null) Debug.LogError("Try to find wrong component");
		return component;
	}

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
}
