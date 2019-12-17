using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imploder : MonoBehaviour
{

    public GameObject explosion;

    public SpriteRenderer sr;
    private DamageDealer.Status status;

    private float damage;
    private DamageDealer.Status priority;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.1f);

        if(transform.localScale.x <= 0.01f)
        {
            GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
            newExplosion.GetComponent<Exploder>().Initialize(sr.color, 0.33f, 0.9f, "Default", damage, priority);

            Destroy(gameObject);
        }
    }

    public void Initialize(Color _color, float _damage, DamageDealer.Status _priority)
    {
        sr.color = _color;
        damage = _damage;
        priority = _priority;
    }

}
