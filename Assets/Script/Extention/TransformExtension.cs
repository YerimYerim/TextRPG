using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public static T FindComponent<T>(this Transform tr, string name) where T : MonoBehaviour
    {
        var transform = tr.Find(name);
        return transform == null ? null : transform.GetComponent<T>();
    }
}
