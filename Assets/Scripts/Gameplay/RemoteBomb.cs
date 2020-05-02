using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitStatus))]
public class RemoteBomb : MonoBehaviour
{

    public static RemoteBomb activeBomb;

    // Start is called before the first frame update
    void Start()
    {
        if (activeBomb != null)
        {
            activeBomb.GetComponent<UnitStatus>().Death();
            DestroyImmediate(gameObject);
        }
        else
            activeBomb = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
