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
    private GameObject instantiatedBall = null;
    private GameObject instantiatedBalla = null;

    public void SpawnBall()
    {
        if (!spawnedBall)
        {
            instantiatedBall = Instantiate(ball, transform.position, transform.rotation);
            instantiatedBall.SetActive(true);
            spawnedBall = true;
        }
        else
        {
            Destroy(instantiatedBall);
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
            instantiatedBalla = Instantiate(ball, transform.position, transform.rotation);
            instantiatedBalla.SetActive(true);
            spawnedBalla = true;
        }
        else
        {
            Destroy(instantiatedBall);
            spawnedBalla = false;
        }
        Instantiate(food, transform.position, transform.rotation);
        manager.alimentation += 20.0f;
        if (manager.alimentation > 100)
            manager.alimentation = 100;
    }
}
