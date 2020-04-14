using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpriteRenderer))]

public class RingRenderer : MonoBehaviour
{
    public Color color;
    public float width = 0.1f;
    public int segments = 15;
    public bool isBox = false;

    private float xRadius;
    private float yRadius;
    private float baseAlpha;
    
    private LineRenderer lr;
    private SpriteRenderer sr;

    private Vector3 boundingBox;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        sr = GetComponent<SpriteRenderer>();
        baseAlpha = sr.color.a;

        lr.positionCount = segments + 1;
        lr.useWorldSpace = false;

        SetEnabled(true, false);
    }

    public void SetSegments(int num)
    {
        segments = num;
        Redraw();
    }

    public void Redraw()
    {
        // Update line color and width
        lr.startColor = lr.endColor = color;
        lr.startWidth = lr.endWidth = width;

        if (transform.localScale.x == 0 || transform.localScale.y == 0)
            return;

        // Account for any changes in object size
        boundingBox = sr.bounds.size;
        xRadius = boundingBox.x / (2 * transform.localScale.x);
        yRadius = boundingBox.y / (2 * transform.localScale.y);

        // Redraw circle 
        if (isBox)
            DrawBox();
        else
            DrawCircle();

    }

    public void SetEnabled(bool ringEnabled, bool spriteEnabled)
    {
        lr.enabled = ringEnabled;

        if (ringEnabled)
            Redraw();

        if(spriteEnabled)
            sr.color = SetAlpha(sr.color, baseAlpha);
        else
            sr.color = SetAlpha(sr.color, 0);
    }

    public static Color SetAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    private void DrawCircle()
    {
        float x, y;
        float z = 0f;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;

            lr.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments);
        }
    }

    private void DrawBox(float size = 1)
    {
        size /= 2;
        lr.startWidth = lr.endWidth = 0.2f;
        lr.positionCount = 10;
        lr.SetPositions(new Vector3[] {
            new Vector3(-size, -size), new Vector3(-size, -size),
            new Vector3(-size, size), new Vector3(-size, size),
            new Vector3(size, size), new Vector3(size, size),
            new Vector3(size, -size), new Vector3(size, -size),
            new Vector3(-size, -size), new Vector3(-size, -size)});

    }
}
