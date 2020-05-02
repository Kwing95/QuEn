using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LateTracker : MonoBehaviour
{
    public float refreshRate = 0.1f;
    public int logSize = 10;

    private Rigidbody2D rb;
    private Vector2[] positionLog;
    private int currentIndex = 0;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        positionLog = new Vector2[logSize];
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= refreshRate)
        {
            positionLog[currentIndex] = rb.position;
            currentIndex = (currentIndex + 1) % logSize;

            timer = 0;
        }
    }

    // Return position of object as it was [latency] seconds ago
    public Vector2 GetPosition(float latency)
    {
        int discreteOffset = Mathf.RoundToInt(latency / refreshRate);
        return positionLog[(currentIndex - discreteOffset) % logSize];
    }

}
