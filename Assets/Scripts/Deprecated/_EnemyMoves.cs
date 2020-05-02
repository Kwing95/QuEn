using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoves
{

    public enum State { Pause, Move, Dash, Shoot };
    public enum Priority { Vulnerable, Light, Heavy };

    public struct Action
    {
        public State state;
        public float power;
        public float error;
    }

    public List<Action> attackPattern;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
