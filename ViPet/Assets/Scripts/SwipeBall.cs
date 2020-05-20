using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwipeBall : MonoBehaviour
{
    // Touch start position, touch end position, swipe direction
    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;

    // To control throw force in X, Y and Z directions
    float throwForceInXandY = 0.04f;
    float throwForceInZ = 2.0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // If the user touches the screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // We get tiycg position and marking time when you touch the screen
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        // If the user releases their finger
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // We get the time of the release and calculate interval time
            touchTimeFinish = Time.time;
            timeInterval = touchTimeFinish - touchTimeStart;

            // We get release finger position and calculate swipe direction in 2D space
            endPos = Input.GetTouch(0).position;
            direction = startPos - endPos;

            // Add force to ball's rigidbody
            rb.isKinematic = false;
            Debug.Log(direction.x * throwForceInXandY);
            Debug.Log(direction.y * throwForceInXandY);
            rb.AddForce(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);
        }
    }
}
