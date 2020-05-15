using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBase : MonoBehaviour
{
    TpsController owner;
    public float damage;
    public void SetUpOwner(TpsController owner_)
    {
        owner = owner_;
    }
    public TpsController GetOwner()
    {
        return owner;
    }
    public void SetDirection(Transform baseRotation)
    {
        transform.rotation = baseRotation.rotation;
    }
    public void DoDamage(TpsController target)
    {
        print(target.name);
        target.PlayBloodEffect();
        target.health-= damage;
    }
}

