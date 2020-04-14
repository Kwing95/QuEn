using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{

    public GameObject tile;
    public const int TILE_LENGTH = 15;
    public static Image[,] map = new Image[RoomArranger.MAX_DIMENSION, RoomArranger.MAX_DIMENSION];

    // Start is called before the first frame update
    void Awake()
    {
        CreateEmptyMap();
    }

    // Creates a full grid of map panels and hides all of them
    private void CreateEmptyMap()
    {
        int dx = 0;
        int dy = 0;

        for (int i = 0; i < RoomArranger.MAX_DIMENSION; ++i)
        {
            dx = 0;
            for (int j = 0; j < RoomArranger.MAX_DIMENSION; ++j)
            {
                GameObject newTile = Instantiate(tile, transform.position, Quaternion.identity, transform);
                newTile.transform.position = (Vector2)transform.position + new Vector2(0 - dx, dy);
                dx += TILE_LENGTH;
                map[i, j] = newTile.GetComponent<Image>();
                SetTile(j, i, Color.clear);
            }
            dy += TILE_LENGTH;
        }
    }

    public static void SetTile(int x, int y, Color newColor)
    {
        // visited -/-> seen, current -/-> seen
        if (map[y, x].color == Color.white && newColor == Color.gray)
            return;

        map[y, x].color = newColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
