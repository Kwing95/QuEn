using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{

    private Rigidbody2D rb;
    private TextMeshPro mesh;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mesh = GetComponent<TextMeshPro>();
        rb.velocity = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        mesh.color = new Color(mesh.color.r, mesh.color.g, mesh.color.b, mesh.color.a - Time.deltaTime);

        if (mesh.color.a <= 0)
            Destroy(gameObject);
    }

    public void Initialize(float damage)
    {
        mesh.text = damage.ToString();
        mesh.fontSize = 5 + (25 * damage / 100);
        mesh.color = new Color(1, 1 - (damage / 100), 0);
    }

}
