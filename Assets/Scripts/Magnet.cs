using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float magnetForce = 5000;

    List<Rigidbody2D> caughtRigidbodies = new List<Rigidbody2D>();

    void FixedUpdate()
    {
        for (int i = 0; i < caughtRigidbodies.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, caughtRigidbodies[i].transform.position);

            if(distance < 1)
            {
                if (caughtRigidbodies[i].GetComponent<Mob>())
                    caughtRigidbodies[i].GetComponent<UnitStatus>().TakeDamage(100);
                else if (caughtRigidbodies[i].GetComponent<PlayerMover>())
                {
                    caughtRigidbodies[i].AddForce(9000 * (caughtRigidbodies[i].transform.position - transform.position));
                    caughtRigidbodies[i].GetComponent<UnitStatus>().TakeDamage(20);
                }
            }
            else
                caughtRigidbodies[i].AddForce((1 / distance * distance) * (transform.position - caughtRigidbodies[i].transform.position) * magnetForce * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D r = other.GetComponent<Rigidbody2D>();

            if (!caughtRigidbodies.Contains(r))
                caughtRigidbodies.Add(r);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D r = other.GetComponent<Rigidbody2D>();
            if (caughtRigidbodies.Contains(r))
                caughtRigidbodies.Remove(r);
        }
    }
}
