using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIWedge : WedgeClock
{

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public override void Refresh(float percentage)
    {
        int index = Mathf.RoundToInt(percentage * (sprites.Count + 1));

        if (index <= 0)
            image.enabled = false;
        else if (index < sprites.Count)
        {
            image.sprite = sprites[index];
            image.enabled = true;
        }
    }

}
