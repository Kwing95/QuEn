using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public static Fader instance;
    private Image image;
    public enum Status { None, ToBlack, Black, FromBlack };
    private Status status = Status.None;
    private Action action1;
    private Action action2;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        image = GetComponent<Image>();
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        FromBlack();
    }

    // Update is called once per frame
    void Update()
    {
        float newAlpha = 0;
        if (status == Status.ToBlack)
        {
            newAlpha = image.color.a + (Time.deltaTime * 2);
            if (newAlpha >= 1)
            {
                status = Status.FromBlack;
                action1?.Invoke();
            }
        }
        else if (status == Status.FromBlack)
        {
            newAlpha = image.color.a - (Time.deltaTime * 2);
            if (newAlpha <= 0)
            {
                status = Status.None;
                action2?.Invoke();
            }
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
    }

    private void FromBlack()
    {
        image.color = Color.black;
        status = Status.FromBlack;
    }

    public void FadeTransition(Action onBlack=null, Action onComplete=null)
    {
        action1 = onBlack;
        action2 = onComplete;
        if(status == Status.None)
        {
            status = Status.ToBlack;
        }
    }
}
