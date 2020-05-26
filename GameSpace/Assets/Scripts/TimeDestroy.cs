using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("D", 8);
    }

    private void D()
    {
        Destroy(gameObject);
    }
}
