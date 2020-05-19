using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject ball;
    public GameObject food;

    public void SpawnBall()
    {
        Instantiate(ball, transform.position, transform.rotation);
        GameObject.Find("GameManager").GetComponent<GameManager>().love += 20.0f;
    }

    public void SpawnFood()
    {
        Instantiate(food, transform.position, transform.rotation);
        GameObject.Find("GameManager").GetComponent<GameManager>().love -= 20.0f;
    }
}
