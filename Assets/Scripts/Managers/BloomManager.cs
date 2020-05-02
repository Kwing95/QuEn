using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloomManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!GameOptions.useBloom)
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
