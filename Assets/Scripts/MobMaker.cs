using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMaker : MonoBehaviour
{

    public const int BASE_DEATH_DAMAGE = 10;
    public const float MIN_STUN_LENGTH = 1.5f;
    public const float MAX_STUN_LENGTH = 7.5f;
    public const int MAX_NUM_ACTIONS = 4;

    public static List<MobType> mobTypes;

    public class MobType
    {
        public List<Mob.Action> attackPattern;
        public float deathDamage;
        public float stunLength;
        public int sides;
    }

    private void Start()
    {

    }

    public static void ClearMobTypes()
    {
        mobTypes = new List<MobType>();
    }

    public static void AddMobType()
    {
        if (mobTypes == null)
            mobTypes = new List<MobType>();
        
        MobType newMob = RandomMob();
        newMob.sides = 3 + mobTypes.Count;
        mobTypes.Add(newMob);
    }

    public static void CreateMob(Vector2 position, MobType mobType)
    {
        GameObject newMob = Instantiate(PrefabManager.instance.mob, position, Quaternion.identity, PrefabManager.instance.enemies.transform);
        newMob.GetComponent<Mob>().Initialize(mobType.attackPattern, mobType.deathDamage, mobType.stunLength);
    }

    public static void CreateMob(Vector2 position)
    {
        CreateMob(position, mobTypes[PRNG.Range(0, mobTypes.Count)]);
    }

    public static void KillAllEnemies()
    {
        List<UnitStatus> enemies = new List<UnitStatus>();

        foreach (Transform child in PrefabManager.instance.enemies.transform)
            child.GetComponent<UnitStatus>().TakeDamage(100);
    }

    public static MobType RandomMob()
    {
        MobType newMob = new MobType();

        newMob.deathDamage = BASE_DEATH_DAMAGE * PRNG.Range(0, 3);
        newMob.stunLength = PRNG.Range(MIN_STUN_LENGTH, MAX_STUN_LENGTH);

        List<Mob.Action> pattern = new List<Mob.Action>();

        int numActions = PRNG.Range(1, MAX_NUM_ACTIONS);

        // An action is a combination of an attack and move action
        for (int i = 0; i < numActions; ++i)
        {
            // Add an action
            pattern.Add(RandomAttack());
            pattern.Add(RandomPause());

            // Add 0-2 moves
            int numMoves = PRNG.Range(0, 3);
            for (int j = 0; j < numMoves; ++j)
            {
                pattern.Add(RandomMove());
                pattern.Add(RandomPause());
            }
        }

        newMob.attackPattern = pattern;
        return newMob;
    }

    // Returns a pause
    private static Mob.Action RandomPause(float min=0.25f, float max=1)
    {
        float pauseTime = PRNG.Range(min, max);
        return new Mob.Action(Mob.State.Pause, DamageDealer.Status.Vulnerable, 0, 0, pauseTime, 0, 0);
    }

    // Returns a random attack action
    private static Mob.Action RandomAttack()
    {
        int whichAction = PRNG.Range(0, 2);
        if (whichAction == 0)
        {
            int power = PRNG.Range(5, 20);
            int speed = PRNG.Range(1500, 4000);
            DamageDealer.Status priority = PRNG.Range(0, 2) == 0 ? DamageDealer.Status.Light : DamageDealer.Status.Heavy;
            float maxError = PRNG.Range(0f, 30f);
            return new Mob.Action(Mob.State.Dash, priority, power, speed, 0, 0, maxError);
        }
        else
        {
            int power = PRNG.Range(5, 20);
            int speed = PRNG.Range(5, 20);
            float error = PRNG.Range(0, 30);
            DamageDealer.Status priority = PRNG.Range(0, 2) == 0 ? DamageDealer.Status.Light : DamageDealer.Status.Heavy;
            return new Mob.Action(Mob.State.Shoot, priority, power, speed, 0, 0, error);
        }
    }

    // Returns a random movement action
    private static Mob.Action RandomMove()
    {
        bool isDash = PRNG.Range(0, 2) == 0;
        if (isDash)
        {
            int speed = PRNG.Range(250, 1500);
            float minError = 90 * PRNG.Range(0, 3);
            float maxError = 90 * PRNG.Range(0, 3);
            return new Mob.Action(Mob.State.Dash, DamageDealer.Status.Vulnerable, 0, speed, 0, minError, maxError);
        }
        else
        {
            int speed = PRNG.Range(300, 4000);
            float angle = 45 * PRNG.Range(0, 5);
            float duration = PRNG.Range(0.5f, 2f) * (angle >= 3 ? 0.5f : 1);
            return new Mob.Action(Mob.State.Move, DamageDealer.Status.Vulnerable, 0, speed, duration, angle, 0);
        }
    }
}
