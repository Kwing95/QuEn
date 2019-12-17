using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealth : HealthBar
{

    private float baseScale;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Awake()
    {
        baseScale = transform.localScale.x;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void SetHealth(float amount)
    {
        transform.localScale = new Vector3(baseScale * amount, transform.localScale.y, 1);
        sr.color = new Color(1 - amount, amount, 0);
    }

}
