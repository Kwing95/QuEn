using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Passcode.ClearPasscodes();
        MobMaker.ClearMobTypes();

        for (int i = 0; i < 3; ++i)
            MobMaker.AddMobType();

        RoomArranger.instance.GenerateFloor(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
