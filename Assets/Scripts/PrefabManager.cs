using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{

    public static PrefabManager instance;

    public List<GameObject> chunkShapes;
    public List<GameObject> chunkMaterials;
    public GameObject roomObjects;
    public GameObject mob;
    public GameObject enemies;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
