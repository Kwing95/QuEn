using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{

    public GameObject mob;
    public int mobType;

    public float spawnRate = 5;
    private float spawnTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer >= spawnRate * Mob.GetMobCount())
        {
            GameObject newMob = Instantiate(mob, transform.position, Quaternion.identity);
            //newMob.GetComponent<Mob>().Initialize(MobMaker.mobTypes[mobType], 0, 0);
            spawnTimer = 0;
        }
    }
}
