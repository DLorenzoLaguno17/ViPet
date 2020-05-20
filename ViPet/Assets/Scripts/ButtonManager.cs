using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject food;
    public StateManager manager = null;
    bool spawnedBall = false;
    bool spawnedBalla = false;

    public void SpawnBall()
    {
        if (!spawnedBall)
        {
            ball.SetActive(true);
            spawnedBall = true;
            ball.GetComponent<SwipeScript>().RecoverObject();
        }
        else
        {
            ball.SetActive(false);
            spawnedBall = false;
        }
        manager.love += 20.0f;
        if (manager.love > 100)
            manager.love = 100;
    }

    public void SpawnFood()
    {
        if (!spawnedBalla)
        {
            food.SetActive(true);
            spawnedBall = true;
            food.GetComponent<SwipeScript>().RecoverObject();
        }
        else
        {
            food.SetActive(false);
            spawnedBall = false;
        }
        manager.alimentation += 20.0f;
        if (manager.alimentation > 100)
            manager.alimentation = 100;
    }
}
