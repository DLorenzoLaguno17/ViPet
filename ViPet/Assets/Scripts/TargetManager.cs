using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject go;

    public void CreateObject()
    {
        Instantiate(go, transform.position, transform.rotation);
    }

    public void DestroyObject()
    {
        Destroy(go);
    }
}
