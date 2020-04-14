using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeManager : MonoBehaviour
{

    public static CodeManager instance;
    public enum Type { Damage, Defense, Stamina, Heal, Kill };

    private static float buffAttackTimer = 0;
    private static float buffDefenseTimer = 0;
    private static float buffStaminaTimer = 0;

    public List<Powerup> codes;

    [System.Serializable]
    public abstract class Powerup
    {
        private static int powerupCount;
        public int index;
        public string code;
        public int spriteIndex = -1;
        public abstract void Activate();

        public Powerup()
        {
            index = powerupCount;
            powerupCount += 1;
            code = instance.GenerateCode();
        }

    }

    [System.Serializable]
    public class Buff : Powerup
    {
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

    [System.Serializable]
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

        codes.Add(new Buff(Type.Damage, 10));
        codes.Add(new Buff(Type.Heal, 10));
        codes.Add(new Buff(Type.Stamina, 10));
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
            // PRNG.UpdateNonce();
            
            // Use raw; these should always be unique
            string newCode = PRNG.Range(0, 999999).ToString().PadLeft(6, '0');

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
