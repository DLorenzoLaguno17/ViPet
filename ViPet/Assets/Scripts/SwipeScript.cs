﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeScript : MonoBehaviour {

	Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
	float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to sontrol throw force in Z direction

	[SerializeField]
	float throwForceInXandY = 1f; // to control throw force in X and Y directions

	[SerializeField]
	float throwForceInZ = 50f; // to control throw force in Z direction

	Rigidbody rb;
    public GameObject camera;
    public GameObject world;
    bool thrown = false;
    bool to_update = false;
    Vector3 initialPos;

    public GameObject recover;

	void Awake()
	{
		rb = GetComponent<Rigidbody> ();
        initialPos = gameObject.transform.localPosition;
    }

	// Update is called once per frame
	void Update () {

        if (!thrown && !to_update)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {

                touchTimeStart = Time.time;
                if (Input.touchCount > 0)
                    startPos = Input.GetTouch(0).position;
                else
                    startPos = Input.mousePosition;

            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {

                touchTimeFinish = Time.time;

                timeInterval = touchTimeFinish - touchTimeStart;
                timeInterval *= Time.deltaTime;

                if (Input.touchCount > 0)
                    endPos = Input.GetTouch(0).position;
                else
                    endPos = Input.mousePosition;

                direction = startPos - endPos;

                rb.isKinematic = false;
                gameObject.transform.SetParent(world.transform, true);
                float x = -direction.x * throwForceInXandY;//Mathf.Clamp(-direction.x * throwForceInXandY, -15, 15);
                float y = -direction.y * throwForceInXandY; //Mathf.Clamp(-direction.y * throwForceInXandY, -35, 35);

                rb.AddRelativeForce(new Vector3(x, y, throwForceInZ / timeInterval));

                thrown = true;
                recover.SetActive(true);
            }
        }
        if (to_update && Input.GetMouseButtonUp(0))
        {
            thrown = false;
            to_update = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            gameObject.transform.SetParent(other.transform, true);
            gameObject.transform.localPosition = new Vector3(0, 0.68f, 0);
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            recover.SetActive(false);
            thrown = false;
            rb.isKinematic = true;
        }else if(other.gameObject.tag == "Respawn" && !thrown)
        {
            RecoverObject();
            thrown = false;
            to_update = false;
        }
    }

    public void RecoverObject()
    {
        gameObject.transform.SetParent(camera.transform, true);
        gameObject.transform.localPosition = initialPos;
        gameObject.transform.localEulerAngles = new Vector3(0,0,0);
        recover.SetActive(false);
        rb.isKinematic = true;
        to_update = true;
    }
}