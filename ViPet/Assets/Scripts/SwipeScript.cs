using System.Collections;
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
    public bool thrown = false;
    bool to_update = false;
    Vector3 initialPos;

    public GameObject Buizel;
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
                float x = -direction.x * throwForceInXandY;
                float y = -direction.y * throwForceInXandY; 

                rb.AddRelativeForce(new Vector3(x, y, throwForceInZ / timeInterval));
                Buizel.GetComponent<StateManager>().playing = true;
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
        if(other.gameObject.tag == "Player" && thrown)
        {
            gameObject.transform.SetParent(Buizel.transform);
            gameObject.transform.localPosition = new Vector3(0, 0.68f, 0.4f);
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            recover.SetActive(false);
            thrown = false;
            rb.isKinematic = true;
            Buizel.GetComponent<MovementAI>().picked = true;
            if(Buizel.GetComponent<StateManager>().playing)
                Buizel.GetComponent<MovementAI>().setDestination(new Vector3(camera.transform.Find("Collider").transform.position.x, 0, camera.transform.Find("Collider").transform.position.z + 0.2f));
        }
        else if(other.gameObject.tag == "Respawn" && !thrown)
        {
            RecoverObject();
            thrown = false;
            to_update = false;
            if (Buizel.GetComponent<StateManager>().playing)
            {
                Buizel.GetComponent<MovementAI>().setDestination(new Vector3(camera.transform.Find("Collider").transform.position.x, 0, camera.transform.Find("Collider").transform.position.z + 0.8f));
                Buizel.GetComponent<MovementAI>().Look(new Vector3(camera.transform.Find("Collider").transform.position.x, Buizel.transform.position.y, camera.transform.Find("Collider").transform.position.z));
            }
        }
    }

    public void RecoverObject()
    {
        gameObject.transform.SetParent(camera.transform);
        gameObject.transform.localPosition = initialPos;
        gameObject.transform.localEulerAngles = new Vector3(0,0,0);
        recover.SetActive(false);
        rb.isKinematic = true;
        to_update = true;
        Buizel.GetComponent<MovementAI>().moving = false;
        Buizel.GetComponent<MovementAI>().picked = false;
    }
}
