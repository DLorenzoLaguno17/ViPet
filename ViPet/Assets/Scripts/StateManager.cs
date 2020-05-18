using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private BarController AlimentationBar;
    private BarController HappinessBar;
    private BarController LoveBar;

    public Camera camera;
    public GameObject particle;

    public float alimentation = 100;
    public float happiness = 100;
    public float love = 100;

    private float lastTimeDecreased = 0;

    void Start()
    {
        AlimentationBar = GameObject.Find("AlimentationBar").GetComponent<BarController>();
        HappinessBar = GameObject.Find("HappinessBar").GetComponent<BarController>();
        LoveBar = GameObject.Find("LoveBar").GetComponent<BarController>();

        AlimentationBar.SetMaxValue(alimentation);
        HappinessBar.SetMaxValue(happiness);
        LoveBar.SetMaxValue(love);
    }

    void Update()
    {
        if (Time.time > lastTimeDecreased + 0.1) 
        {
            lastTimeDecreased = Time.time;
            
            alimentation -= 0.1f;
            happiness -= 0.1f;
            love -= 0.1f;

            AlimentationBar.SetSize(alimentation);
            HappinessBar.SetSize(happiness);
            LoveBar.SetSize(love);
        }

        foreach(Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Create a ray from the current touch coordinates
                Ray ray = Camera.allCameras[0].ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name == "Buizel") {
                        Instantiate(particle, transform.position, transform.rotation);
                        love += 30.0f;
                        if (love < 100) love = 100;
                    }
                    else if (hit.collider.gameObject.name == "Ball")
                    {

                    }
                }

            }
        }
    }
}
