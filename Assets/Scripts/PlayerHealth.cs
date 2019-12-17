using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : HealthBar
{

    private float baseScale;
    public Image img;

    // Start is called before the first frame update
    void Awake()
    {
        baseScale = transform.localScale.x;
        //img = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void SetHealth(float amount)
    {
        transform.localScale = new Vector3(baseScale * amount, transform.localScale.y, 1);
        img.color = new Color(1 - amount, amount, 0);
    }

}
