using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

    public float contactDamage = 0;

    public LineRenderer lr;
    public bool isStun = false;

    private List<GameObject> unitsHit;
    private bool isUnit = false;
    private bool isProjectile = false;
    private bool isFriendly = false;

    public enum Special { None, Block, Radial };
    private Special special;

    public enum Status { Vulnerable, Light, Heavy };
    private Status status;

    // Start is called before the first frame update
    void Awake()
    {
        isUnit = GetComponent<UnitStatus>();
        status = Status.Vulnerable;
        special = Special.None;
        unitsHit = new List<GameObject>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collide(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Collide(collision);
    }

    private void Collide(Collision2D collision)
    {
        DamageDealer otherDamager = collision.gameObject.GetComponentInChildren<DamageDealer>();
        UnitStatus otherUnit = collision.gameObject.GetComponent<UnitStatus>();

        bool isUnit = otherDamager && otherUnit;
        bool isWall = otherUnit && !otherDamager;
        bool isAttack = otherDamager && !otherUnit;

        if (isWall)
        {
            unitsHit.Add(otherUnit.gameObject);
            otherUnit.TakeDamage(contactDamage);
        }

        // otherUnit is (intentionally) null for projectiles or disjointed hitboxes
        if (isUnit && DamagesOther(otherDamager.status) &&
            !unitsHit.Contains(otherUnit.gameObject))
        {
            unitsHit.Add(otherUnit.gameObject);

            Mob mobHit = otherUnit.GetComponent<Mob>();
            if (mobHit && isStun)
                mobHit.Stun();

            // Used to be a switch(case) for specials here

            otherUnit.TakeDamage(contactDamage, transform.position);
        }

        // If a projectile hits a stun attack
        if (isProjectile && isAttack && otherDamager.isStun)
        {
            if (status == Status.Light)
            {
                GetComponent<Bullet>().Reflect(-1.5f);
                isFriendly = !otherDamager.isFriendly;
                gameObject.layer = LayerMask.NameToLayer("PlayerProj");
            }
            return;
        }

        if(isProjectile)
            Destroy(gameObject);
            
    }

    private bool DamagesOther(Status otherStatus)
    {
        if (status == Status.Vulnerable)
            return false;
        // Enemies take priority over enemies
        if (status == otherStatus)
            return !isFriendly;
        return status > otherStatus;
    }

    public Status GetStatus()
    {
        return status;
    }

    public void SetStatus(Status newStatus, float damage, bool _isProjectile = false)
    {
        unitsHit = new List<GameObject>();
        contactDamage = damage;
        status = newStatus;
        isProjectile = _isProjectile;

        if (!lr)
            return;

        switch (status)
        {
            case Status.Vulnerable:
                lr.startColor = lr.endColor = Color.green;
                break;
            case Status.Light:
                lr.startColor = lr.endColor = Color.yellow;
                break;
            case Status.Heavy:
                lr.startColor = lr.endColor = Color.red;
                break;
        }
    }

    public void SetSpecial(Special newSpecial)
    {
        special = newSpecial;
    }
    
    public IEnumerator TempStatus(Status newStatus, float damage, float duration)
    {
        SetStatus(newStatus, damage);
        yield return new WaitForSeconds(duration);
        SetStatus(Status.Vulnerable, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
