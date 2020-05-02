using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScale : MonoBehaviour
{

    public static float scalingFactor = 1;
    public float instanceScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        scalingFactor = Screen.height / 500f;
        transform.localScale = Vector2.one * GameOptions.scalingFactor * instanceScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
