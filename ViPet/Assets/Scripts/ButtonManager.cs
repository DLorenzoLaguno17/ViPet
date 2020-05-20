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
            DisableFood();
            GetComponent<StateManager>().newEmotion(EmotionStates.Sad);
        }
        if (!spawnedBall)
        {
            EnableBall();
        }
        else
        {
            DisableBall();
        }
    }

    public void SpawnFood()
    {
        if (spawnedBall)
        {
            DisableBall();
        }
        if (!spawnedBalla)
        {
            EnableFood();
        }
        else
        {
            DisableFood();
        }
    }

    public void EnableFood()
    {
        food.SetActive(true);
        spawnedBalla = true;
        Buizel.GetComponent<StateManager>().eating = true;
        food.GetComponent<SwipeScript>().RecoverObject();
    }
    public void EnableBall()
    {
        ball.SetActive(true);
        spawnedBall = true;
        Buizel.GetComponent<StateManager>().playing = true;
        ball.GetComponent<SwipeScript>().RecoverObject();
    }
    public void DisableFood()
    {
        Buizel.GetComponent<StateManager>().eating = false; 
        food.SetActive(false);
        spawnedBalla = false;
    }
    public void DisableBall()
    {
        Buizel.GetComponent<StateManager>().playing = false;
        ball.SetActive(false);
        spawnedBall = false;
    }
}
