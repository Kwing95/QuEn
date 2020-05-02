using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    public GameObject subject;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.position = rb.position - (rb.position - (Vector2)subject.transform.position) * 10f * Time.deltaTime;
        //rb.position = Vector3.Lerp(rb.position, subject.transform.position, 0.4f);
    }
}
