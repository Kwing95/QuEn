using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPhysics : MonoBehaviour
{

    public float frequency = 1;

    private Rigidbody rb;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= frequency)
        {
            Torque();
            frequency *= 0.9f;
            timer = 0;
        }
        
    }

    private void Torque()
    {
        float intensity = 1 / frequency;

        rb.AddForce(300 * Vector3.up * frequency);
        rb.AddTorque(intensity * 10 * new Vector3(Random.Range(-1, 1), intensity, Random.Range(-6, 6)));
    }

}
