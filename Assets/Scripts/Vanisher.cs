using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanisher : MonoBehaviour
{

    public float timeToLive = 10f;
    private float uptime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        uptime += Time.deltaTime;
        if(uptime >= timeToLive)
            Destroy(gameObject);
    }
}
