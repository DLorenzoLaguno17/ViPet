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
    Love = 0,
    Happy,
    Full,
    Sad,
    Angry,
    Bored,
    Hungry
}

public class StateManager : MonoBehaviour
{
    Renderer ren;
    public List<EmotionMaterial> materials;
    public List<EmotionCloud> clouds;
    public List<Emotions> emotions;

    public EmotionStates current_index1 = 0;

    // Start is called before the first frame update
    void Start()
    {
        ren = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            current_index1++;
            if ((int)current_index1 > emotions.Count - 1)
                current_index1 = 0;
        }
        if (Input.GetKeyDown("s"))
        {
            current_index1--;
            if (current_index1 < 0)
                current_index1 = (EmotionStates)emotions.Count - 1;
        }

        for (int i = 0; i < emotions.Count; ++i)
        {
            if (current_index1 == emotions[i].emotion)
            {
                materials[0].mat.mainTextureOffset = -materials[0].offsets[emotions[i].Eyes - 1];
                materials[1].mat.mainTextureOffset = -materials[1].offsets[emotions[i].Mouth - 1];
                Rect rect = clouds[0].raw.uvRect;
                rect.position = clouds[0].offsets[emotions[i].Cloud - 1];
                clouds[0].raw.uvRect = rect;
                break;
            }
        }
    }
}
