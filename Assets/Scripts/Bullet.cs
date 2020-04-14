using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    private Vector2 initialVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeVelocity(Vector2 direction)
    {
        initialVelocity = direction;
        GetComponent<Rigidbody2D>().velocity = direction;
    }

    public Vector2 GetInitialVelocity()
    {
        return initialVelocity;
    }

    public void Reflect(float multiplier=-1)
    {
        GetComponent<Rigidbody2D>().velocity = initialVelocity * multiplier;
    }

}
