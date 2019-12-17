using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{

    public SpriteRenderer sr;
    public new CircleCollider2D collider;
    private DamageDealer damager;

    private float alpha = 1f;
    private float maxLifetime = 1;
    private float lifetime = 1;
    private float maximumSize = 0.4f;

    // Start is called before the first frame update
    void Awake()
    {
        damager = collider.GetComponent<DamageDealer>();
    }

    // Update is called once per frame
    void Update()
    {
        lifetime -= Time.deltaTime;
        float progress = lifetime / maxLifetime;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, progress);
        sr.transform.localScale = Vector3.Lerp(sr.transform.localScale, Vector3.one * maximumSize, 0.2f);

        if (lifetime <= 0)
            Destroy(gameObject);
    }

    public void Initialize(Color color, float time, float maxSize, string layer, float damage, DamageDealer.Status priority)
    {
        damager.SetStatus(priority, damage);
        
        collider.transform.localScale = Vector3.one * maxSize;
        collider.gameObject.layer = LayerMask.NameToLayer(layer);

        sr.transform.localScale = Vector3.one * 0.01f;
        sr.color = color;
        maxLifetime = lifetime = time;
        maximumSize = maxSize;
    }

}
