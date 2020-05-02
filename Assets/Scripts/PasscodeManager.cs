using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passcode
{
    public enum Powerup { Damage, Heal, Defense, Stamina, Kill, Stun }
    public enum Status { Made, Revealed, Entered, Used }

    // Separate lists for made, entered, used? Or one list and check statuses?
    private static List<Passcode> passcodes;

    private Point location;
    private Powerup powerup;
    private Status status;
    private int index;

    private string code;
    private GameObject killTarget;

    public Passcode(Powerup _powerup, Point _location, GameObject killTarget=null)
    {
        // Lists and indexing
        NullCheck();

        index = passcodes.Count;
        passcodes.Add(this);

        // Member variables
        code = GenerateCode();
        location = _location;
        powerup = _powerup;
        status = Status.Made;
    }

    public static void ClearPasscodes()
    {
        passcodes = new List<Passcode>();
    }

    private static void NullCheck()
    {
        if (passcodes == null)
            passcodes = new List<Passcode>();
    }

    public static List<Passcode> GetPasscodeList()
    {
        return passcodes;
    }

    public static List<Passcode> GetPasscodeList(Status filter)
    {
        return GetPasscodeList(new List<Status> { filter });
    }

    public static List<Passcode> GetPasscodeList(List<Status> filters)
    {
        NullCheck();
        List<Passcode> newList = new List<Passcode>();

        foreach (Passcode passcode in passcodes)
            foreach(Status filter in filters)
                if (passcode.status == filter)
                    newList.Add(passcode);

        return newList;
    }

    public static void ResetPasscodes()
    {
        passcodes.Clear();
    }

    // Return index of icon given type of powerup
    public static int IconIndex(Powerup _powerup)
    {
        switch (_powerup)
        {
            case Powerup.Damage:
                return 0;
            case Powerup.Stamina:
                return 1;
            case Powerup.Kill:
                return 2;
            case Powerup.Heal:
                return 3;
            case Powerup.Defense:
                return 4;
            case Powerup.Stun:
                return 5;
            default:
                return -1;
        }
    }

    // Generates a code distinct from all existing codes and returns it
    public static string GenerateCode()
    {
        bool isDuplicate = true;

        while (isDuplicate)
        {
            isDuplicate = false;
            string newCode = PRNG.Range(0, 999999).ToString().PadLeft(6, '0');

            foreach (Passcode passcode in passcodes)
                isDuplicate = isDuplicate || passcode.code == newCode;

            if (!isDuplicate)
                return newCode;
        }

        return "";
    }

    public void Activate()
    {
        status = Status.Used; // this may not be necessary

        switch (powerup)
        {
            case Powerup.Damage:
                PasscodeManager.instance.ApplyAttackBuff();
                break;
            case Powerup.Stamina:
                PasscodeManager.instance.ApplyStaminaBuff();
                break;
            case Powerup.Defense:
                PasscodeManager.instance.ApplyDefenseBuff();
                break;
            case Powerup.Heal:
                PlayerMover.instance.GetComponent<PlayerStatus>().Heal(PasscodeManager.healAmount);
                break;
            case Powerup.Kill:
                // killTarget.GetComponent<BossPart>().Death();
                break;
        }
        /* Keep track of timers inside PasscodeManager, because it needs Update to be running
         * (and also we don't want the static "passcodes" incorrect from Passcode being MonoBehaviour)
         
         
         */
    }

    // Generates and returns a random passcode (never KILL type)
    public static Passcode AddRandomCode(Point location)
    {
        int typeIndex = PRNG.Range(0, 5);
        Powerup type = Powerup.Damage;
        switch (typeIndex)
        {
            case 0:
                type = Powerup.Damage;
                break;
            case 1:
                type = Powerup.Defense;
                break;
            case 2:
                type = Powerup.Heal;
                break;
            case 3:
                type = Powerup.Stamina;
                break;
            case 4:
                type = Powerup.Stun;
                break;
        }

        return new Passcode(type, location, null);
    }

    public void RevealCode()
    {
        PrefabManager.instance.notification.SetActive(true);
        status = Status.Revealed;
    }

    // Return passcode matching input; return null if none found
    public static Passcode GetPowerupByCode(string input)
    {
        foreach(Passcode passcode in passcodes)
            if ((passcode.status == Status.Made || passcode.status == Status.Revealed) && passcode.code == input)
            {
                RoomArranger.ClearRoom(passcode.location);
                PowerupUser.AddPowerup(passcode.powerup);
                passcode.status = Status.Entered;

                if (GetPasscodeList(Status.Revealed).Count == 0)
                    PrefabManager.instance.notification.SetActive(false);

                return passcode;
            }

        return null;
    }

    // Return passcode at given location
    public static Passcode GetPasscodeByLocation(Point location)
    {
        foreach(Passcode passcode in passcodes)
            if (passcode.location.x == location.x && passcode.location.y == location.y)
                return passcode;

        return null;
    }

    // Return color corresponding to passcode type
    public static Color GetColorByType(Passcode passcode)
    {
        switch (passcode.powerup)
        {
            case Powerup.Damage:
                return Color.red;
            case Powerup.Stamina:
                return Color.yellow;
            case Powerup.Defense:
                return new Color(0, 153, 255);
            case Powerup.Heal:
                return Color.green;
            case Powerup.Stun:
                return new Color(255, 128, 0);
            case Powerup.Kill:
                return new Color(255, 0, 255);
        }

        return Color.black;
    }

    public Powerup GetPowerup()
    {
        return powerup;
    }

    public string GetCode()
    {
        return code;
    }

    public int GetIndex()
    {
        return index;
    }

}

// Manages the timers attached to powerups
public class PasscodeManager : MonoBehaviour
{

    public static readonly float buffDuration = 10;
    public static readonly float healAmount = 50;

    public static PasscodeManager instance;

    private float buffAttackTimer = 0;
    private float buffDefenseTimer = 0;
    private float buffStaminaTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        buffAttackTimer -= Time.deltaTime;
        buffDefenseTimer -= Time.deltaTime;
        buffStaminaTimer -= Time.deltaTime;
    }

    // Apply buffs
    public void ApplyAttackBuff()
    {
        buffAttackTimer = buffDuration;
    }

    public void ApplyDefenseBuff()
    {
        buffDefenseTimer = buffDuration;
    }

    public void ApplyStaminaBuff()
    {
        buffStaminaTimer = buffDuration;
    }

    // Get active buffs
    public bool AttackBuffActive()
    {
        return buffAttackTimer > 0;
    }

    public bool DefenseBuffActive()
    {
        return buffDefenseTimer > 0;
    }

    public bool StaminaBuffActive()
    {
        return buffStaminaTimer > 0;
    }

}
