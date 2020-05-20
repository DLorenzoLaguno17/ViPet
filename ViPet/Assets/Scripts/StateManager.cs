﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public struct EmotionCloud
{
    public RawImage raw;
    public List<Vector2> offsets;
}

[System.Serializable]
public struct EmotionMaterial
{
    public Material mat;
    Material list;
    public List<Vector2> offsets;
}

[System.Serializable]
public struct Emotions
{
    public EmotionStates emotion;
    public int Eyes;
    public int Mouth;
    public int Cloud;
}

public enum EmotionStates
{
    Love,
    Happy,
    Full,
    Sad,
    Angry,
    Bored,
    Hungry,
    Normal
}

public class StateManager : MonoBehaviour
{
    Renderer ren;
    public List<EmotionMaterial> materials;
    public List<EmotionCloud> clouds;
    public List<Emotions> emotions;
    IEnumerator routine = null;
    public EmotionStates current_emotion = 0;
    public GameObject cloud = null;
    public GameObject hand = null;

    private BarController AlimentationBar;
    private BarController HappinessBar;
    private BarController LoveBar;

    public bool playing = false;
    public bool eating = false;
    public float alimentation = 100;
    public float happiness = 100;
    public float love = 100;
    private float lastTimeDecreased = 0;
    float lastEmotionTime = 0;
    float lastEmotion = 0;
    int[] queue = { 0, 0, 0 };
    public float emoteduration = 1.5f;

    Vector2 startPos, endPos, direction;
    bool startedTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
        AlimentationBar = GameObject.Find("AlimentationBar").GetComponent<BarController>();
        HappinessBar = GameObject.Find("HappinessBar").GetComponent<BarController>();
        LoveBar = GameObject.Find("LoveBar").GetComponent<BarController>();

        AlimentationBar.SetMaxValue(alimentation);
        HappinessBar.SetMaxValue(happiness);
        LoveBar.SetMaxValue(love);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastTimeDecreased + 0.1)
        {
            lastTimeDecreased = Time.time;

            if (alimentation > 0)
                alimentation -= 0.1f;
            if (happiness > 0)
                happiness -= 0.1f;
            if (love > 0)
                love -= 0.1f;

            AlimentationBar.SetSize(alimentation);
            HappinessBar.SetSize(happiness);
            LoveBar.SetSize(love);
        }

        if (!playing && !eating)
        {
            if (Time.time - lastEmotionTime > 3)
            {
                if((int)alimentation % 15 == 0)
                {
                    queue[0] = 1;
                    lastEmotionTime = Time.time;
                }
                if((int)happiness % 15 == 0)
                {
                    queue[1] = 1;
                    lastEmotionTime = Time.time;
                }
                if ((int)love % 15 == 0)
                {
                    queue[2] = 1;
                    lastEmotionTime = Time.time;
                }
            }

            if(Time.time - lastEmotion > emoteduration + 0.5f)
            {
                if (queue[0] == 1)
                {
                    if(alimentation < 50) { 
                        newEmotion(EmotionStates.Hungry);
                        lastEmotion = Time.time;
                    }
                    queue[0] = 0;
                }
                else if (queue[1] == 1)
                {
                    if (happiness < 50)
                    {
                        newEmotion(EmotionStates.Bored);
                        lastEmotion = Time.time;
                    }
                    else
                    {
                        newEmotion(EmotionStates.Happy);
                        lastEmotion = Time.time;
                    }
                    queue[1] = 0;
                }
                else if (queue[2] == 1)
                {
                    if (love < 50)
                    {
                        newEmotion(EmotionStates.Sad);
                        lastEmotion = Time.time;
                    }
                    else
                    {
                        newEmotion(EmotionStates.Love);
                        lastEmotion = Time.time;
                    }
                    queue[2] = 0;
                }
            }

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    // Create a ray from the current touch coordinates
                    Ray ray = Camera.allCameras[0].ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.name == "Buizel")
                        {
                            startPos = touch.position;
                            startedTouching = true;
                            hand.SetActive(true);
                        }
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    // Create a ray again to check where has released
                    Ray ray = Camera.allCameras[0].ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.name == "Buizel")
                        {
                            // Get release finger position and calculate swipe direction in 2D space
                            endPos = touch.position;
                            direction = startPos - endPos;

                            if (startedTouching)
                            {
                                love += (Mathf.Abs(direction.magnitude * 0.01f));
                                if (love > 100.0f) love = 100.0f;
                                startedTouching = false;
                                newEmotion(EmotionStates.Love);
                            }
                        }
                    }

                    hand.SetActive(false);
                }

                if (startedTouching)
                {
                    hand.transform.position = touch.position;
                }
            }

        
            //DO RANDOM STUFF
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ball")
        {
            newEmotion(EmotionStates.Angry);
        }

    }

    public void newEmotion(EmotionStates emotion)
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = playEmotion(emotion);
        StartCoroutine(routine);
    }

    private IEnumerator playEmotion(EmotionStates emotion)
    {
        int i = (int)emotion;
        if(emotions[i].Cloud != -1)
            cloud.SetActive(true);
        else
            cloud.SetActive(false);

        AssigEmotion(i);

        yield return new WaitForSeconds(emoteduration);

        i = (int)EmotionStates.Normal;
        AssigEmotion(i);

        cloud.SetActive(false);
    }

    void AssigEmotion(int i)
    {
        current_emotion = (EmotionStates)i;
        Rect rect = clouds[0].raw.uvRect;
        materials[0].mat.mainTextureOffset = -materials[0].offsets[emotions[i].Eyes - 1];
        materials[1].mat.mainTextureOffset = -materials[1].offsets[emotions[i].Mouth - 1];
        if(emotions[i].Cloud != -1)
            rect.position = clouds[0].offsets[emotions[i].Cloud - 1];
        clouds[0].raw.uvRect = rect;
    }


}
