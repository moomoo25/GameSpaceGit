using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : DamageBase
{
    private bool isStop;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStop == false)
        {
            transform.position += transform.forward * speed;
        }
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Ground")
        {
            isStop = true;
        }

        TpsController target = other.GetComponent<TpsController>();
        if (target != null)
        {
            if (target != GetOwner())
            {
                DoDamage(target);
            }
        }
    }
}
