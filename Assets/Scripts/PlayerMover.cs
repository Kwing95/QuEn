using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerMover : MonoBehaviour
{
    public Expander charge;
    public RingRenderer chargeMax;
    public BarDisplay staminaBar;
    public GameObject lightningRod;
    public GameObject explosion;
    public SpriteRenderer sr;

    public static PlayerMover instance;

    public float maxStamina = 100;
    public float staminaRegeneration = 8;
    public float moveForce = 3000;
    public float maxTapLength = 0.25f;
    public float bulletSpeed = 20f;
    public float jumpChargeSpeed = 10;
    public float jumpTime = 0.33f;

    private GraphicRaycaster raycaster;
    private Rigidbody2D rb;
    private LineRenderer jumpLine;
    private TrailRenderer tr;
    private new CircleCollider2D collider;
    private DamageDealer damager;

    private float stamina = 0;
    private float timeDown = 0f;
    private bool mouseDown = false;
    private enum State { None, Dashing, Jumping, Blocking };
    private State state = State.None;

    // Last logged mouse position
    private Vector3 mousePosition;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        raycaster = gameObject.AddComponent<GraphicRaycaster>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        damager = GetComponent<DamageDealer>();

        jumpLine = GetComponent<LineRenderer>();
        jumpLine.material = new Material(Shader.Find("Sprites/Default"));
        jumpLine.startColor = jumpLine.endColor = Color.red;

        tr = GetComponent<TrailRenderer>();
        tr.material = new Material(Shader.Find("Sprites/Default"));
        tr.startColor = tr.endColor = Color.yellow;

        chargeMax.SetEnabled(false, false);
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if(stamina < maxStamina)
            stamina += Time.deltaTime * staminaRegeneration;
        else
            stamina = maxStamina;

        staminaBar.SetScale(stamina / maxStamina);

        if (stamina < 10)
            staminaBar.GetComponentInChildren<Image>().color = Color.red;
        else if (stamina < 20)
            staminaBar.GetComponentInChildren<Image>().color = new Color(1, 0.5f, 0);
        else
            staminaBar.GetComponentInChildren<Image>().color = Color.yellow;

        if(state != State.Dashing)
        {
            mousePosition = MousePosition();
            distance = Vector2.Distance(mousePosition, transform.position);
        }

        if (state == State.Jumping)
        {
            sr.transform.localScale = Vector3.Lerp(sr.transform.localScale, Vector3.one * 0.5f, 0.04f);
            sr.gameObject.GetComponent<RingRenderer>().Redraw();
        }

        // Checking if mouse is CONTINUALLY up/down
        if (mouseDown)
        {
            timeDown += Time.deltaTime;
            // When performing a charged action
            if(timeDown > maxTapLength) {
                if (state == State.Dashing)
                {
                    if(stamina >= 20)
                        ChargeJump(mousePosition);
                }
                else if(state == State.Blocking)
                    PlaceRod();
            }
        }
        else
            timeDown = 0;

        // Checking events for mouse PRESS/RELEASE
        // https://stackoverflow.com/questions/38198745/how-to-detect-left-mouse-click-but-not-when-the-click-occur-on-a-ui-button-compo
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            mouseDown = true;

            if (distance > 1)
                state = State.Dashing;
            else
            {
                charge.GetComponent<RingRenderer>().SetEnabled(false, true);
                chargeMax.SetEnabled(true, false);
                charge.Count(maxTapLength);
                state = State.Blocking;
            }            
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            jumpLine.enabled = false;
            chargeMax.SetEnabled(false, false);
            if(state != State.Jumping)
                charge.GetComponent<RingRenderer>().SetEnabled(false, false);
            mouseDown = false;

            switch (state)
            {
                case State.None:
                    charge.Hide();
                    break;
                case State.Dashing:
                    state = State.None;
                    break;
                case State.Jumping:
                    charge.Hide();
                    state = State.None;
                    return;
            }


            if (timeDown < maxTapLength)
                if (distance > 1)
                    Dash(mousePosition);
                else
                    Block();
        }

    }
    
    private void Dash(Vector3 mousePosition)
    {
        if (!StaminaCheck(10))
            return;

        tr.enabled = true;
        rb.AddForce(moveForce * Vector3.Normalize(mousePosition - transform.position));
        StartCoroutine(damager.TempStatus(DamageDealer.Status.Light, 10, 0.3f));
    }

    private void Block()
    {
        if (!StaminaCheck(10))
            return;

        tr.enabled = false;
        GameObject hitbox = Instantiate(explosion, transform.position, Quaternion.identity);
        hitbox.GetComponent<Exploder>().Initialize(new Color(1, 0.5f, 0), 0.25f, 0.5f, "HitEnemy", 5, DamageDealer.Status.Heavy);
        hitbox.GetComponentInChildren<DamageDealer>().isStun = true;
    }

    private void ChargeJump(Vector3 mousePosition)
    {
        tr.enabled = false;
        jumpLine.enabled = true;
        jumpLine.SetPosition(0, transform.position);
        Vector3 direction = Vector3.Normalize(mousePosition - transform.position);
        float chargeProgress = jumpChargeSpeed * (timeDown - maxTapLength);
        jumpLine.SetPosition(1, transform.position + (chargeProgress * direction));

        if (chargeProgress > Vector3.Distance(transform.position, mousePosition))
        {
            JumpAttack(mousePosition);
            jumpLine.enabled = false;
            state = State.Jumping;
        }
    }

    private void JumpAttack(Vector3 direction)
    {
        stamina -= 20;
        collider.enabled = false;
        rb.isKinematic = true;
        float speed = Vector3.Distance(direction, transform.position) / jumpTime;
        rb.velocity = speed * Vector3.Normalize(direction - transform.position);
        StartCoroutine(Stomp());
        // Need to cancel after time
    }

    IEnumerator Stomp()
    {
        yield return new WaitForSeconds(jumpTime);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.5f, Vector3.zero, 0, LayerMask.GetMask("Enemy"));

        foreach(RaycastHit2D hit in hits)
            if (hit.collider)
            {
                Mob target = hit.collider.gameObject.GetComponent<Mob>();
                if (target)
                    target.GetComponent<UnitStatus>().TakeDamage(40, transform.position);
            }

        GameObject hitbox = Instantiate(explosion, transform.position, Quaternion.identity);
        hitbox.GetComponent<Exploder>().Initialize(Color.red, 0.33f, 0.9f, "HitEnemy", 20, DamageDealer.Status.Heavy);
        hitbox.GetComponentInChildren<DamageDealer>().SetSpecial(DamageDealer.Special.Radial);

        state = State.None;
        ResizeSprite(0.2f);
        collider.enabled = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;


    }

    private void PlaceRod()
    {
        if (!StaminaCheck(10))
            return;

        tr.enabled = false;
        Instantiate(lightningRod, transform.position, Quaternion.identity);
        state = State.None;
    }

    private void ResizeSprite(float size)
    {
        sr.transform.localScale = size * Vector3.one;
        sr.gameObject.GetComponent<RingRenderer>().Redraw();
    }

    private bool StaminaCheck(float minimum)
    {
        if(stamina >= minimum)
        {
            stamina -= minimum;
            return true;
        }

        return false;
    }

    private Vector3 MousePosition()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    //dummied
    private Vector2 GetMousePosition()
    {
        //Set up the new Pointer Event
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        pointerData.position = Input.mousePosition;
        raycaster.Raycast(pointerData, results);

        return pointerData.position;
    }

    public void ResetTimeDown()
    {
        timeDown = 0;
    }

}
