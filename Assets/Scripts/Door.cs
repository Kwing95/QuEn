using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Door : MonoBehaviour
{

    public List<SpriteRenderer> blocks;

    public enum Direction { Left, Up, Down, Right };
    public Direction direction;
    public enum Status { Open, Locked, Blocked };
    private Status status = Status.Blocked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && status == Status.Open)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Fader.instance.FadeTransition(() => Room.ChangeRooms(direction));
        }
    }

    public void SetStatus(Status value)
    {
        status = value;
        Color newColor = new Color();

        switch (value)
        {
            case Status.Open:
                newColor = Color.green;
                break;
            case Status.Locked:
                newColor = Color.yellow;
                break;
            case Status.Blocked:
                newColor = Color.white;
                break;
        }
        
        newColor.a = 0.66f;

        foreach (SpriteRenderer elt in blocks)
            elt.color = newColor;
    }
}
