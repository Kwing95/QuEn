using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkData {
    public int rotation;
    public int shape;
    public int material;
    public bool isVortex;
    public int enemy;

    public ChunkData(int _rotation, int _shape, int _material, bool _isVortex, int _enemy)
    {
        rotation = _rotation;
        shape = _shape;
        material = _material;
        isVortex = _isVortex;
        enemy = _enemy;
    }
}

public class Chunk : MonoBehaviour
{

    public enum Rotation { North, East, South, West }
    public Rotation rotation;
    public enum Shape { Empty, One, Parallel, Corner }
    public Shape shape;
    public enum Material { Wall, Impasse, Bomb, Barrier }
    //public Material material;
    public bool isVortex;

    public GameObject material;

    public void Initialize(ChunkData data)
    {
        transform.Rotate(0, 0, 90 * data.rotation, Space.Self);
        GameObject material = PrefabManager.instance.chunkMaterials[data.material];

        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
            children.Add(child);

        foreach (Transform child in children)
        {
            if (child.CompareTag("BombSlot") || data.material != 0)
                Instantiate(material, child.position, Quaternion.identity, transform);

            Destroy(child.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize(new ChunkData(0, 0, 0, false, false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
