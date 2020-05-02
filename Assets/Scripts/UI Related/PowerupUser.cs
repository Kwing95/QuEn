using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupUser : MonoBehaviour
{

    private Outline outline;
    private UIWedge timerWedge;
    private Text inventoryText;

    public Passcode.Powerup type;

    private static Dictionary<Passcode.Powerup, int> inventory;
    private static List<GameObject> buttons;

    // Start is called before the first frame update
    void Start()
    {
        if (buttons == null)
            buttons = new List<GameObject>();

        if (inventory == null)
            ResetAll();

        buttons.Add(gameObject);
        outline = GetComponent<Outline>();
        inventoryText = GetComponentInChildren<Text>();
        timerWedge = GetComponentInChildren<UIWedge>();
        Refresh();
    }

    private void OnDestroy()
    {
        buttons.Remove(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetAll()
    {
        inventory = new Dictionary<Passcode.Powerup, int>();
        inventory[Passcode.Powerup.Damage] = 0;
        inventory[Passcode.Powerup.Stamina] = 0;
        inventory[Passcode.Powerup.Stun] = 0;
        inventory[Passcode.Powerup.Defense] = 0;
        inventory[Passcode.Powerup.Heal] = 0;

        buttons.Clear();
    }

    public static void AddPowerup(Passcode.Powerup _type)
    {
        ++inventory[_type];
        Refresh();
    }

    public void UsePowerup()
    {
        if(inventory[type] > 0)
        {
            switch (type)
            {
                case Passcode.Powerup.Damage:
                    PasscodeManager.instance.ApplyAttackBuff();
                    timerWedge.Countdown(PasscodeManager.buffDuration);
                    break;
                case Passcode.Powerup.Stamina:
                    PasscodeManager.instance.ApplyStaminaBuff();
                    timerWedge.Countdown(PasscodeManager.buffDuration);
                    break;
                case Passcode.Powerup.Defense:
                    PasscodeManager.instance.ApplyDefenseBuff();
                    timerWedge.Countdown(PasscodeManager.buffDuration);
                    break;
                case Passcode.Powerup.Heal:
                    PlayerMover.instance.GetComponent<PlayerStatus>().Heal(PasscodeManager.healAmount);
                    timerWedge.Countdown(1);
                    break;
                case Passcode.Powerup.Stun:
                    foreach (Transform enemy in PrefabManager.instance.enemies.transform)
                    {
                        Mob mob = enemy.GetComponent<Mob>();
                        if (mob)
                            mob.Stun();
                    }
                    timerWedge.Countdown(1);
                    break;
            }
            --inventory[type];
            Refresh(); // animate cooldown?
        }
    }



    public static void Refresh()
    {
        foreach(GameObject button in buttons)
        {
            PowerupUser powerupUser = button.GetComponent<PowerupUser>();
            // button.SetActive(inventory[powerupUser.type] > 0);
            powerupUser.inventoryText.text = inventory[powerupUser.type].ToString();
            
        }
            
    }

}
