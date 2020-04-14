using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RoomArranger.instance.GenerateFloor(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
