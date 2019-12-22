using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : UnitStatus
{

    private float waitTime = 5;
    private int life = 1;
    private int maxLives = 3;
    private PlayerMover player;
    private bool isRespawning = false;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        player = GetComponent<PlayerMover>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (isRespawning)
        {
            health += (maxHealth * Time.deltaTime) / (waitTime * life);
            if (health >= maxHealth)
            {
                health = maxHealth;
                life += 1;
                isRespawning = false;

                collider.enabled = true;
                player.enabled = true;
                rb.isKinematic = false;
            }
            healthBar.SetHealth(health / maxHealth);
        }
    }

    public void Heal(float amount)
    {
        // Add some animation

        health = Mathf.Min(maxHealth, health + amount);
        healthBar.SetHealth(health / maxHealth);
    }

    protected override void Death()
    {
        GameObject newImplosion = Instantiate(implosion, transform.position, Quaternion.identity);

        if(deathDamage > 0)
            newImplosion.GetComponent<Imploder>().Initialize(Color.red, deathDamage, DamageDealer.Status.Heavy);
        else
            newImplosion.GetComponent<Imploder>().Initialize(Color.green, 0, DamageDealer.Status.Vulnerable);

        rb.velocity = Vector3.zero;
        collider.enabled = false;
        player.enabled = false;
        rb.isKinematic = true;

        if (life >= 3)
            StartCoroutine(GameOver());
        else
            isRespawning = true;
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5);
        // Do something
    }

}
