using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroyObject : MonoBehaviour
{
    public float time;
    private void Start()
    {
        Invoke("DestroyObj", time);
    }
    void DestroyObj()
    {
        if (this != null)
        {
            Destroy(gameObject);
        }
    }
}
