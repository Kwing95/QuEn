using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeManager : MonoBehaviour
{

    public static CodeManager instance;

    private static float buffAttackTimer = 0;
    private static float buffDefenseTimer = 0;
    private static float buffStaminaTimer = 0;

    private int unixTime;
    private int nonce = 1;
    public List<Powerup> codes;

    public abstract class Powerup
    {
        public string code;
        public int spriteIndex = -1;
        public abstract void Activate();

        public Powerup()
        {
            code = instance.GenerateCode();
        }

    }

    public class Buff : Powerup
    {
        public enum Type { Damage, Defense, Stamina, Heal };
        public Type type;
        public float potency = 10;

        public Buff(Type _type, float _potency) : base()
        {
            type = _type;
            potency = _potency;
            switch (type)
            {
                case Type.Damage:
                    spriteIndex = 0;
                    break;
                case Type.Defense:
                    spriteIndex = 4;
                    break;
                case Type.Stamina:
                    spriteIndex = 1;
                    break;
                case Type.Heal:
                    spriteIndex = 3;
                    break;

            }
        }

        public override void Activate()
        {
            switch (type)
            {
                case Type.Damage:
                    buffAttackTimer = potency;
                    break;
                case Type.Defense:
                    buffDefenseTimer = potency;
                    break;
                case Type.Stamina:
                    buffStaminaTimer = potency;
                    break;
                case Type.Heal:
                    PlayerMover.instance.GetComponent<PlayerStatus>().Heal(potency);
                    break;

            }
        }
    }

    public class Kill : Powerup
    {
        public UnitStatus target;

        public Kill(UnitStatus _target) : base()
        {
            spriteIndex = 2;
            target = _target;
        }

        public override void Activate()
        {
            target.TakeDamage(100);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        codes = new List<Powerup>();

        UpdateTime();

        codes.Add(new Buff(Buff.Type.Damage, 10));
        codes.Add(new Buff(Buff.Type.Heal, 10));
        codes.Add(new Buff(Buff.Type.Stamina, 10));
        codes.Add(new Kill(gameObject.GetComponent<UnitStatus>()));

        PrintCodes();
    }

    // Update is called once per frame
    void Update()
    {
        buffAttackTimer -= Time.deltaTime;
        buffDefenseTimer -= Time.deltaTime;
        buffStaminaTimer -= Time.deltaTime;
    }

    // Generates an unused code to be used for a powerup
    public string GenerateCode()
    {
        bool isDuplicate = true;

        while (isDuplicate)
        {
            isDuplicate = false;
            UpdateNonce();
            string newCode = Mathf.RoundToInt(999999 * Random.value).ToString().PadLeft(6, '0');

            // Loop through all codes
            for (int i = 0; i < codes.Count; ++i)
                if (codes[i].code == newCode)
                    isDuplicate = true;

            // If loop finishes without duplicate being found
            if (!isDuplicate)
                return newCode;
        }

        return "";
    }

    public void PrintCodes()
    {
        foreach(Powerup elt in codes)
            Debug.Log(elt.code);
    }

    public void UpdateNonce()
    {
        Random.InitState(unixTime * nonce);
        nonce += 1;
    }

    // Returns current time, rounded to the nearest minute
    public void UpdateTime()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        unixTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds / 60;
    }

    public float GetBuffAttackTimer()
    {
        return buffAttackTimer;
    }

    public float GetBuffDefenseTimer()
    {
        return buffDefenseTimer;
    }

    public float GetBuffStaminaTimer()
    {
        return buffStaminaTimer;
    }

}
