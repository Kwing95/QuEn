using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaledSize : MonoBehaviour
{

    public float widthPercentage = 0.1f;
    public float heightPercentage = 0.1f;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * widthPercentage, Screen.height * heightPercentage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
