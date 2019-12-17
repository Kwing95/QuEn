using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expander : MonoBehaviour
{

    public Color partColor;
    public Color fullColor;

    //public PlayerMover mover;
    public float minScale = 0.0f;
    public float maxScale = 1f;

    //private float minJumpScale = 0.0f;
    //private float maxJumpScale = 3f;

    private float growthRate = 0.01f;
    private bool ready = true;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        maxScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x < maxScale)
        {
            transform.localScale = Vector3.one * (transform.localScale.x + (growthRate * Time.deltaTime));
            if (GetComponent<RingRenderer>())
                GetComponent<RingRenderer>().Redraw();
                
        }
        else if(!ready)
        {
            transform.localScale = Vector3.one * maxScale;
            ready = true;
            sr.color = RingRenderer.SetAlpha(fullColor, sr.color.a);
            //mover.ResetTimeDown();
        }
    }

    public void Count(float cooldown)
    {
        Count(cooldown, minScale, maxScale);
    }

    public void Count(float cooldown, float min, float max)
    {
        //sr.enabled = true;

        minScale = min;
        maxScale = max;

        //sr.enabled = true;
        growthRate = (maxScale - minScale) / cooldown;
        transform.localScale = Vector3.one * minScale;
        //rowthRate = speed;
        ready = false;
        sr.color = RingRenderer.SetAlpha(partColor, sr.color.a);
    }

    public void Hide()
    {
        //sr.enabled = false;
        transform.localScale = Vector3.zero;
        growthRate = 0;
    }

    public bool GetReady()
    {
        return ready;
    }

    public void HaltGrowth()
    {
        growthRate = 0;
    }
}
