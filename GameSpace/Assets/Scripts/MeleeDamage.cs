using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : DamageBase
{

    private void Awake()
    {
        if (transform.parent.GetComponent<TpsController>())
        {
            SetUpOwner(transform.parent.GetComponent<TpsController>());
        }
    }
    private void OnTriggerEnter(Collider other)
    {
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
