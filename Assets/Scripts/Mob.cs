﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

    public GameObject bullet;

    public List<Action> attackPattern;
    public enum State { Pause, Move, Dash, Shoot };
    public enum Priority { Vulnerable, Light, Heavy };
    public float stunLength = 3;

    private float stunTimer = 0;

    private Rigidbody2D rb;
    private UnitStatus status;
    private DamageDealer damager;

    [System.Serializable]
    public class Action
    {
        public Action()
        {
            state = State.Pause;
            duration = 1;
        }

        public Action(State _state, DamageDealer.Status _priority, float _power,
            float _speed, float _duration, float _angle, float _error)
        {
            state = _state;
            priority = _priority;
            power = _power;
            speed = _speed;
            duration = _duration;
            angle = _angle;
            error = _error;
        }

        public State state;
        public DamageDealer.Status priority;
        public float power;
        public float speed;
        public float duration;
        public float angle;
        public float error;
    }

    private State state;
    private Priority priority;

    private bool canAct = true;
    private bool canMove = false;
    private int patternProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        status = GetComponent<UnitStatus>();
        damager = GetComponent<DamageDealer>();

        if(attackPattern.Count == 0)
            attackPattern.Add(new Action());
        else
            state = attackPattern[0].state;
    }

    // Update is called once per frame
    void Update()
    {
        if(stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
            return;
        }

        if (canAct)
        {
            patternProgress = (patternProgress + 1) % attackPattern.Count;
            canAct = false;

            Action action = attackPattern[patternProgress];

            switch (action.state)
            {
                case State.Pause:
                    StartCoroutine(Pause(action.duration));
                    break;
                case State.Dash:
                    StartCoroutine(Dash(action.power, action.speed, action.priority, action.angle, action.error));
                    break;
                case State.Shoot:
                    StartCoroutine(Shoot(action.power, action.priority, action.error));
                    break;
                case State.Move:
                    MoveTimer(action.duration);
                    break;
            }
        }
        else if (canMove)
        {
            Action action = attackPattern[patternProgress];
            Move(action.speed, action.angle);
        }
    }

    public IEnumerator Shoot(float power, DamageDealer.Status priority, float error)
    {
        Vector3 direction = Vector3.Normalize(PlayerMover.instance.transform.position - transform.position);
        direction = Quaternion.Euler(0, 0, Random.Range(-error, error)) * direction;

        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        newBullet.layer = LayerMask.NameToLayer("EnemyProj");
        newBullet.GetComponent<Rigidbody2D>().velocity = 10 * direction; // adjust projectile speed
        newBullet.GetComponent<DamageDealer>().SetStatus(priority, power, true);

        yield return new WaitForSeconds(0);
        canAct = true;
    }

    public void Move(float speed, float angle)
    {
        Vector3 direction = speed * Vector3.Normalize(PlayerMover.instance.transform.position - transform.position);
        direction = Quaternion.Euler(0, 0, angle) * direction;
        rb.AddForce(direction);
    }

    public IEnumerator MoveTimer(float duration)
    {
        canMove = true;

        yield return new WaitForSeconds(duration);

        canAct = true;
        canMove = false;
    }

    public IEnumerator Dash(float power, float speed, DamageDealer.Status priority, float minError, float maxError)
    {
        Vector3 direction = speed * Vector3.Normalize(PlayerMover.instance.transform.position - transform.position);
        float error = (minError + Random.Range(0, maxError)) * (Random.Range(0, 2) == 0 ? 1 : -1);
        direction = Quaternion.Euler(0, 0, error) * direction;
        rb.AddForce(direction);
        StartCoroutine(damager.TempStatus(priority, 10, 0.3f));

        yield return new WaitForSeconds(0);
        canAct = true;
    }

    public IEnumerator Pause(float duration)
    {
        yield return new WaitForSeconds(duration);
        canAct = true;
    }

    public void Stun()
    {
        stunTimer = stunLength;
    }

    public Priority GetPriority()
    {
        return priority;
    }

}
