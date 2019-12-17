using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public GameObject tileObject;
    private List<Vector3> tiles;
    private List<Vector3> neighbors;
    private List<Vector3> cardinals;
    private Vector3 walker;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<Vector3>();
        neighbors = new List<Vector3>();
        cardinals = new List<Vector3>(new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right });

        GenerateMap(Vector3.zero, 10000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap(Vector3 start, int size)
    {
        walker = start;
        PlaceTile(start);

        for(int i = 1; i < size; ++i)
        {
            PlaceTile(walker);
            //PlaceTile(neighbors[Random.Range(0, neighbors.Count)]);
        }
    }

    private void PlaceTile(Vector3 location)
    {
        Instantiate(tileObject, location, Quaternion.identity);
        tiles.Add(location);

        walker += cardinals[Random.Range(0, cardinals.Count)];

        foreach (Vector3 cardinal in cardinals) {
            Vector3 newNeighbor = location + cardinal;
            if (!tiles.Contains(newNeighbor)){
                neighbors.Add(newNeighbor);
            }
        }
    }

}
