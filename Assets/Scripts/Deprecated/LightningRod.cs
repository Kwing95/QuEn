using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRod : MonoBehaviour
{

    public Color color;
    private float gracePeriod = 0.5f;
    private LineRenderer lr;

    private static float lineWidth = 0.1f;
    private int health;
    private float currentGrace;

    public static int maxHealth = 100;
    public static float maxDistance = 10f;
    public static int maxRods = 2;
    public static List<LightningRod> rods;

    private void OnDestroy()
    {
        rods.Remove(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGrace = 0;
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));

        lr.startWidth = lr.endWidth = lineWidth;
        lr.startColor = lr.endColor = color;

        health = maxHealth;

        if(rods == null)
            rods = new List<LightningRod>();

        rods.Add(this);
        if(rods.Count > maxRods)
            DestroyImmediate(rods[0].gameObject);
            
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGrace > 0)
            currentGrace -= Time.deltaTime;

        if(health <= 0)
            DestroyImmediate(gameObject);

        foreach(LightningRod rod in rods)
        {
            if (rod != this && Vector3.Distance(transform.position, rod.transform.position) <= maxDistance)
            {
                if(currentGrace <= 0)
                {
                    RaycastHit2D[] rays = Physics2D.RaycastAll(transform.position, rod.transform.position - transform.position, Vector3.Distance(transform.position, rod.transform.position));

                    foreach (RaycastHit2D ray in rays)
                    {
                        Mob target = ray.collider.GetComponent<Mob>();
                        if (target)
                        {
                            target.GetComponent<UnitStatus>().TakeDamage(5);
                            currentGrace = gracePeriod;
                        }
                    }
                }

                lr.enabled = true;
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, rod.transform.position);
            }
            else
                lr.enabled = false;

        }
    }
}
