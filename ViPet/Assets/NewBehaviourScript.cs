using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Renderer ren;
    public Material mat;
    List<Material> list;
    public List<Vector2> offsets;

    public int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
        //mat = ren.materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
            index++;

        if (index > 7)
            index = 0;
        mat.mainTextureOffset = offsets[index];
    }
}
