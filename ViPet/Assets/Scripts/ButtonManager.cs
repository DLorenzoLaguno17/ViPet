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
    public GameObject Buizel;

    public void SpawnBall()
    {
        if (spawnedBalla)
        {
            Buizel.GetComponent<StateManager>().eating = false;
            food.SetActive(false);
            spawnedBalla = false;
        }
        if (!spawnedBall)
        {
            ball.SetActive(true);
            spawnedBall = true;
            Buizel.GetComponent<StateManager>().playing = true;
            ball.GetComponent<SwipeScript>().RecoverObject();
        }
        else
        {
            Buizel.GetComponent<StateManager>().playing = false;
            ball.SetActive(false);
            spawnedBall = false;
        }
    }

    public void SpawnFood()
    {
        if (spawnedBall)
        {
            Buizel.GetComponent<StateManager>().playing = false;
            ball.SetActive(false);
            spawnedBall = false;
        }
        if (!spawnedBalla)
        {
            food.SetActive(true);
            spawnedBalla = true;
            Buizel.GetComponent<StateManager>().eating = true;
            food.GetComponent<SwipeScript>().RecoverObject();
        }
        else
        {
            Buizel.GetComponent<StateManager>().eating = false;
            food.SetActive(false);
            spawnedBalla = false;
        }
    }
}
