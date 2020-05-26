using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : DamageBase
{
    public float speed = 1;
    private void Update()
    {
        Vector3 direction = transform.forward ;
        transform.position += ( direction.normalized * speed * Time.deltaTime);
    }
    public void SetProjectileSpeed(float speedValue)
    {
        speed = speedValue;
    }
    private void OnTriggerEnter(Collider other)
    {
        TpsController target = other.GetComponent<TpsController>();
        if (target != null)
        {
            if (target != GetOwner())
            {
                DoDamage(target);
                Destroy(gameObject);
            }
        }
       
    }
}
