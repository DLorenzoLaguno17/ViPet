using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject mountain;
    public GameObject forest;
    public GameObject buizel;

    bool moun = false;

    private void Update()
    {
        if (moun)
        {
            mountain.SetActive(true);
            forest.SetActive(false);
        }
        else
        {
            mountain.SetActive(false);
            if(buizel.active)
                forest.SetActive(true);
            else
                forest.SetActive(false);
        }
    }

    public void seeMountain(bool en)
    {
        moun = en;
    }
    
}
