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

    private BarController AlimentationBar;
    private BarController HappinessBar;
    private BarController LoveBar;
    public GameObject particle;

    public float alimentation = 100;
    public float happiness = 100;
    public float love = 100;
    private float lastTimeDecreased = 0;

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
        if (Input.GetKeyDown("w"))
        {
            current_emotion++;
            if ((int)current_emotion > emotions.Count - 1)
                current_emotion = 0;
        }
        if (Input.GetKeyDown("s"))
        {
            current_emotion--;
            if (current_emotion < 0)
                current_emotion = (EmotionStates)emotions.Count - 1;
        }
        if (Input.GetKeyDown("space"))
        {
            for (int i = 0; i < emotions.Count; ++i)
            {
                if (current_emotion == emotions[i].emotion)
                {
                    newEmotion(current_emotion);
                    break;
                }
            }
        }

        if (Time.time > lastTimeDecreased + 0.1)
        {
            lastTimeDecreased = Time.time;

            alimentation -= 0.1f;
            happiness -= 0.1f;
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
                    else if (hit.collider.gameObject.name == "Ball")
                    {
                        newEmotion(EmotionStates.Angry);
                    }
                }

            }
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
        GameObject cloud = gameObject.transform.Find("Cloud").gameObject;
        if(emotions[i].Cloud != -1)
            cloud.SetActive(true);
        else
            cloud.SetActive(false);

        AssigEmotion(i);

        yield return new WaitForSeconds(1.5f);

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
