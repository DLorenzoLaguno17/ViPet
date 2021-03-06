﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementAI : MonoBehaviour
{
    public GameObject ball;
    public GameObject cam;
    NavMeshAgent agent;
    NavMeshPath path;
    public bool calculated = false;
    public bool picked = false;
    public bool moving = false;
    public bool to_end = false;
    public bool look = false;
    float lastTime = 0.0f;
    Vector3 destinyPos;
    Vector3 destinyLook;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ball.GetComponent<SwipeScript>().thrown)
        {
            lastTime = Time.time + 0.01f;
        }
        else if (Time.time - lastTime > 0.5f && ball.GetComponent<SwipeScript>().thrown)
        {
            lastTime = Time.time;
            if (!picked)
            {
                setDestination(new Vector3(ball.transform.position.x, 0, ball.transform.position.z));
                to_end = false;
            }
        }

        if (moving && !look)
        {
            Vector3 dir = destinyPos - transform.position;
            dir.y = 0;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 5.0f * Time.deltaTime);
        }
        else
        {
            transform.LookAt(destinyLook);
        }

        if(!agent.pathPending && agent.pathStatus == NavMeshPathStatus.PathComplete && to_end && agent.remainingDistance < 0.01 && moving)
        {
            
            if (gameObject.GetComponent<StateManager>().eating)
            {
                GameObject.Find("ButtonManager").GetComponent<ButtonManager>().DisableFood();
                //EAT EMOTION HERE
                GetComponent<StateManager>().newEmotion(EmotionStates.Full);
                GetComponent<StateManager>().UpdateHungry(GetComponent<StateManager>().gain + 5);
            }
            else if (gameObject.GetComponent<StateManager>().playing)
            {
                //HAPPY EMOTION HERE
                GetComponent<StateManager>().newEmotion(EmotionStates.Happy);
                GetComponent<StateManager>().UpdateBored(GetComponent<StateManager>().gain + 15);
            }
            to_end = false;
        }
    }
    public void setDestination(Vector3 dest)
    {
        GetComponent<StateManager>().anim.SetBool("Happy", false);
        GetComponent<StateManager>().anim.SetBool("Eat", false);
        GetComponent<StateManager>().anim.SetBool("Walk", true);
        agent.SetDestination(dest);
        destinyPos = dest;
        moving = true;
        look = false;
    }
    public void Look(Vector3 pos)
    {
        destinyLook = pos;
        look = true;
    }
}

