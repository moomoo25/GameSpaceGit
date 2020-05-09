using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILight
{
    void ILighting();
}

public class Test : MonoBehaviour,ILight
{
   public void ILighting()
    {
        print("Eboloa");
    }
}
