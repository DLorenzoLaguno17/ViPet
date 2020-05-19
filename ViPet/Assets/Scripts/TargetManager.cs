using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject prefab;
    GameObject GO;
    public void CreateObject()
    {
        GO = Instantiate(prefab, transform.position, transform.rotation);
        GO.SetActive(true);
    }

    public void DestroyObject()
    {
        Destroy(GO);
    }
}
