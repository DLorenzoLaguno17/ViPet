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
    public GameObject cam;
    public GameObject world;
    public bool thrown = false;
    bool to_update = false;
    Vector3 initialPos;
    bool sliding = false;

    public GameObject Buizel;
    public GameObject recover;

    public bool Balltype = false;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
        initialPos = gameObject.transform.localPosition;
    }

	// Update is called once per frame
	void Update () {

        if (!thrown && !to_update)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                RaycastHit hit;
                Ray ray;
                if (Input.touchCount > 0) { 
                    ray = Camera.allCameras[0].ScreenPointToRay(Input.GetTouch(0).position);
                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Item"))
                    {
                        sliding = true;
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag.Equals("Item"))
                    {
                        sliding = true;
                    }
                }

                if (sliding)
                {
                    touchTimeStart = Time.time;
                    if (Input.touchCount > 0)
                        startPos = Input.GetTouch(0).position;
                    else
                        startPos = Input.mousePosition;
                }
            }

            if ((Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && sliding)
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
                if (Balltype)
                {
                    recover.SetActive(true);
                    Buizel.GetComponent<StateManager>().playing = true;
                    Buizel.GetComponent<MovementAI>().ball = gameObject;
                }
                else
                {
                    Buizel.GetComponent<StateManager>().eating = true;
                    Buizel.GetComponent<MovementAI>().ball = gameObject;
                }
                thrown = true;
                sliding = false;
            }
        }
        if (to_update)
        {
            thrown = false;
            to_update = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && thrown)
        {
            thrown = false;
            rb.isKinematic = true;
            Buizel.GetComponent<MovementAI>().picked = true;
            gameObject.transform.SetParent(Buizel.transform);
            gameObject.transform.localPosition = new Vector3(0, 0.68f, 0.4f);
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            if (Buizel.GetComponent<StateManager>().playing)
            {
                recover.SetActive(false);
                Buizel.GetComponent<MovementAI>().setDestination(new Vector3(cam.transform.Find("Collider").transform.position.x, 0, cam.transform.Find("Collider").transform.position.z + 0.2f));
                
            }
            else if(Buizel.GetComponent<StateManager>().eating)
            {
                Buizel.GetComponent<MovementAI>().setDestination(new Vector3(cam.transform.Find("Collider").transform.position.x, 0, cam.transform.Find("Collider").transform.position.z + 0.8f));
                Buizel.GetComponent<MovementAI>().Look(new Vector3(cam.transform.Find("Collider").transform.position.x, Buizel.transform.position.y, cam.transform.Find("Collider").transform.position.z));
                Buizel.GetComponent<MovementAI>().to_end = true;
            }
        }
        else if(other.gameObject.tag == "Respawn" && !thrown)
        {
            RecoverObject();
            thrown = false;
            to_update = false;
            if (Buizel.GetComponent<StateManager>().playing)
            {
                //HAPPY EMOTION HERE
                Buizel.GetComponent<StateManager>().newEmotion(EmotionStates.Happy);
                Buizel.GetComponent<MovementAI>().setDestination(new Vector3(cam.transform.Find("Collider").transform.position.x, 0, cam.transform.Find("Collider").transform.position.z + 0.8f));
                Buizel.GetComponent<MovementAI>().Look(new Vector3(cam.transform.Find("Collider").transform.position.x, Buizel.transform.position.y, cam.transform.Find("Collider").transform.position.z));
                Buizel.GetComponent<MovementAI>().to_end = true;
            }
        }
    }

    public void RecoverObject()
    {
        gameObject.transform.SetParent(cam.transform);
        gameObject.transform.localPosition = initialPos;
        gameObject.transform.localEulerAngles = new Vector3(0,0,0);
        recover.SetActive(false);
        rb.isKinematic = true;
        to_update = true;
        Buizel.GetComponent<MovementAI>().moving = false;
        Buizel.GetComponent<MovementAI>().picked = false;
    }
}
