using System.Collections;
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

    private BarController AlimentationBar;
    private BarController HappinessBar;
    private BarController LoveBar;
    public GameObject particle;

    public float alimentation = 100;
    public float happiness = 100;
    public float love = 100;
    private float lastTimeDecreased = 0;
    float lastEmotionTime = 0;
    float lastEmotion = 0;
    int[] queue = { 0, 0, 0 };
    public float emoteduration = 1.5f;

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
        if(Time.time - lastEmotionTime > 3)
        {
            Debug.Log((int)alimentation % 15);
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
                    queue[0] = 0;
                    lastEmotion = Time.time;
                }
                else
                {
                    queue[0] = 0;
                }
            }
            else if (queue[1] == 1)
            {
                if (happiness < 50)
                {
                    newEmotion(EmotionStates.Bored);
                    queue[1] = 0;
                    lastEmotion = Time.time;
                }
                else
                {
                    newEmotion(EmotionStates.Happy);
                    queue[1] = 0;
                    lastEmotion = Time.time;
                }
            }
            else if (queue[2] == 1)
            {
                if (love < 50)
                {
                    newEmotion(EmotionStates.Sad);
                    queue[2] = 0;
                    lastEmotion = Time.time;
                }
                else
                {
                    newEmotion(EmotionStates.Love);
                    queue[2] = 0;
                    lastEmotion = Time.time;
                }
            }
        }


        if (Time.time > lastTimeDecreased + 0.1)
        {
            lastTimeDecreased = Time.time;

            if(alimentation > 0)
                alimentation -= 0.1f;
            if (happiness > 0)
                happiness -= 0.1f;
            if (love > 0)
                love -= 0.1f;

            AlimentationBar.SetSize(alimentation);
            HappinessBar.SetSize(happiness);
            LoveBar.SetSize(love);
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
                        Instantiate(particle, transform.position, transform.rotation);
                        love += 20.0f;
                        if (love > 100)
                            love = 100;

                        newEmotion(EmotionStates.Love);
                    }
                }

            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ball")
        {
            newEmotion(EmotionStates.Angry);
        }

    }

    private void newEmotion(EmotionStates emotion)
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
