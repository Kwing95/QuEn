using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteWedge : WedgeClock
{
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void Refresh(float percentage)
    {
        int index = Mathf.RoundToInt(percentage * (sprites.Count + 1));

        if (index <= 0)
            sr.enabled = false;
        else if (index < sprites.Count)
        {
            sr.sprite = sprites[index];
            sr.enabled = true;
        }
    }
}
