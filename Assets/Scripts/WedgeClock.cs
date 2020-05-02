using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WedgeClock : MonoBehaviour
{

    private float maxCountdown = 1;
    private float currentCountdown = 1;
    public List<Sprite> sprites;

    void Update()
    {
        currentCountdown -= Time.deltaTime;
        Refresh(currentCountdown / maxCountdown);
    }

    public abstract void Refresh(float percentage);

    public void Countdown(float duration)
    {
        maxCountdown = duration;
        currentCountdown = duration;
    }

}
