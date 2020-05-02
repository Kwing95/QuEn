using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public LineRenderer lr;
    public HealthBar healthBar;
    public GameObject damageDisplay;
    public GameObject implosion;

    public float deathDamage = 0;
    public float gracePeriod = 0.25f;

    protected Rigidbody2D rb;
    protected new CircleCollider2D collider;

    public float maxHealth = 100;
    protected float health;
    protected float graceTimer = 0;

    // Start is called before the first frame update
    public void Start()
    {
        collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        health = maxHealth;
        healthBar.SetHealth(health / maxHealth);
    }

    // Update is called once per frame
    public void Update()
    {
        graceTimer -= Time.deltaTime;
        EnforceBoundaries();
    }

    private void EnforceBoundaries()
    {
        if (!GetComponent<Rigidbody2D>())
            return;

        if (rb.position.x > Room.ROOM_BOUNDARY)
            rb.position = new Vector2(Room.ROOM_BOUNDARY, rb.position.y);
        else if (rb.position.x < -Room.ROOM_BOUNDARY)
            rb.position = new Vector2(-Room.ROOM_BOUNDARY, rb.position.y);

        if (rb.position.y > Room.ROOM_BOUNDARY)
            rb.position = new Vector2(rb.position.x, Room.ROOM_BOUNDARY);
        else if (rb.position.y < -Room.ROOM_BOUNDARY)
            rb.position = new Vector2(rb.position.x, -Room.ROOM_BOUNDARY);
    }

    public float GetHealth()
    {
        return health;
    }

    private static void RoomCLearCheck()
    {
        if (PrefabManager.instance.enemies.transform.childCount == 0)
        {
            // OnRoomClear
            // Passcode goes 
            Passcode.GetPasscodeByLocation(new Point(Room.location.x, Room.location.y)).RevealCode();

            RoomArranger.GetCurrent().Clear();
            RoomArranger.instance.ConfigureDoors();
        }
    }

    public virtual void Death()
    {
        GameObject newImplosion = Instantiate(implosion, transform.position, Quaternion.identity, PrefabManager.instance.roomObjects.transform);

        if (deathDamage > 0)
            newImplosion.GetComponent<Imploder>().Initialize(Color.red, deathDamage, DamageDealer.Status.Heavy);
        else
            newImplosion.GetComponent<Imploder>().Initialize(Color.green, 0, DamageDealer.Status.Vulnerable);

        transform.SetParent(null);

        if(GetComponent<Mob>() != null)
            RoomCLearCheck();

        Destroy(gameObject);

        // if player, stop enemies from referencing null
        //Instantiate(explosion)
    }

    public virtual void TakeDamage(float damage)
    {
        if (graceTimer > 0)
            return;

        damage = PowerupCheck(damage);

        graceTimer = gracePeriod;

        health -= damage;
        healthBar.SetHealth(health / maxHealth);

        GameObject damageText = Instantiate(damageDisplay, transform.position, Quaternion.identity, PrefabManager.instance.roomObjects.transform);
        damageText.GetComponent<DamageNumber>().Initialize(damage);

        if (health <= 0)
            Death();
    }

    public void TakeDamage(float damage, Vector3 otherPosition)
    {
        TakeDamage(damage);

        if (rb)
        {
            Vector3 direction = Vector3.Normalize(otherPosition - transform.position);
            rb.AddForce(-1000 * direction); // This should be adjustable
        }
    }

    // attack or defense
    public float PowerupCheck(float amount)
    {
        if(GetComponent<PlayerMover>())
            return amount * (PasscodeManager.instance.DefenseBuffActive() ? 0.25f : 1);
        else
            return amount * (PasscodeManager.instance.AttackBuffActive() ? 2 : 1);
    }

}
