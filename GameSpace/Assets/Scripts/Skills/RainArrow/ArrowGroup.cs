using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGroup : MonoBehaviour
{
    public Arrow spawnArrows;
    private Transform spawnTransform;
    private TpsController owner;
    private bool onActive = true;
    public int index;
    private float counter;

    public void SetUp(TpsController owner_,Transform t)
    {
        owner = owner_;
        spawnTransform = t;
        counter = 0;
    }
    void Update()
    {
        if (owner == null) { return; }

        if (onActive == true)
        {
            counter += Time.deltaTime;

            if (counter > 2)
            {
                Arrow arrow = Instantiate(spawnArrows, this.transform.position, this.transform.rotation);
                arrow.SetUpOwner(owner);
                if (index < 3)
                {
                    index++;
                }
                else
                {
                    onActive = false;
                    Destroy(gameObject);
                }
                counter = 0;
            }
        }
    }
}
