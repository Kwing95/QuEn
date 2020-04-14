using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public const int ROOM_HEIGHT = 30;
    public const int ROOM_WIDTH = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool InRoom(Vector2 position)
    {
        return Mathf.Abs(position.x) < (ROOM_WIDTH / 2) - 0.5f &&
            Mathf.Abs(position.y) < (ROOM_HEIGHT / 2) - 0.5f;
    }
}
