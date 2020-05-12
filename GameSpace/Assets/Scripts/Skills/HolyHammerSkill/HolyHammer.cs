using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyHammer : DamageBase
{
    public float speed = 1;
    private void Update()
    {
        Vector3 direction = transform.forward ;
        transform.position += direction.normalized * speed;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        TpsController player = other.GetComponent<TpsController>();
        if (player != null)
        {
            if (player != GetOwner())
            {
                Destroy(gameObject);
            }
        }
       
    }
}
