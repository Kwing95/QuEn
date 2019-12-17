using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetScale(0.5f);
        //transform.position = transform.position + new Vector3(-1, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScale(float x)
    {
        transform.localScale = new Vector3(x, 1, 1);
    }
}
