/*
    Pure static class — no state, no dependencies.
    Contains all damage and healing formulas. The results are applied to CombatantState.
*/

using UnityEngine;

public static class BattleResolver
{
    // Processes the chosen Move (from the hero or from the monster)
    public static BattleActionResult Resolve(MoveDTO move, CombatantState actor, CombatantState target)
    {
        BattleActionResult result = new BattleActionResult { MoveUsed = move, ActorId = actor.Name };

        switch (move.Effect)
        {
            case MoveEffectType.DAMAGE:
                result.DamageDealt = CalculateDamage(move, actor, target);
                ApplyDamage(target, result.DamageDealt);
                break;

            case MoveEffectType.HEAL:
                result.HealingDone = CalculateHeal(move, actor);
                ApplyHeal(actor, result.HealingDone);
                break;

            case MoveEffectType.BUFF:
                result.BuffApplied = ApplyBuff(move, actor);
                break;

            case MoveEffectType.DRAIN:
                result.DamageDealt = CalculateMagicDamage(move, actor);
                result.HealingDone = result.DamageDealt;
                ApplyDamage(target, result.DamageDealt);
                ApplyHeal(actor, result.HealingDone);
                break;
        }

        result.TargetDefeated = !target.IsAlive();
        return result;
    }


    private static int CalculateDamage(MoveDTO move, CombatantState actor, CombatantState target)
    {
        return move.Type == MoveType.PHYSICAL
            ? CalculatePhysicalDamage(move, actor, target)
            : CalculateMagicDamage(move, actor);
    }

    // Scales off Attack, reduced by target's Defense, minimum 1.
    private static int CalculatePhysicalDamage(MoveDTO move, CombatantState actor, CombatantState target)
    {
        CharacterStats actorStats  = actor.GetEffectiveStats();  // EffectiveStats take into account active buffs.
        CharacterStats targetStats = target.GetEffectiveStats();

        // Physical: 
        int damage = actorStats.Attack + move.BaseValue - targetStats.Defense;
        return Mathf.Max(1, damage);
    }

    // Scales off Magic, bypasses Defense entirely.
    private static int CalculateMagicDamage(MoveDTO move, CombatantState actor)
    {
        CharacterStats actorStats = actor.GetEffectiveStats();  // EffectiveStats take into account active buffs.

        int damage = actorStats.Magic + move.BaseValue;
        return Mathf.Max(1, damage);  // Just in case damage becomes less than 1 because of buffs. 
    }

    private static int CalculateHeal(MoveDTO move, CombatantState actor)
    {
        CharacterStats actorStats = actor.GetEffectiveStats();  // EffectiveStats take into account active buffs.

        return actorStats.Magic + move.BaseValue;
    }



    private static void ApplyDamage(CombatantState target, int amount)
    {
        target.CurrentHp = Mathf.Max(0, target.CurrentHp - amount);
    }

    private static void ApplyHeal(CombatantState actor, int amount)
    {
        int maxHp = actor.GetEffectiveStats().MaxHealth;
        actor.CurrentHp = Mathf.Min(maxHp, actor.CurrentHp + amount);
    }

    private static ActiveBuff ApplyBuff(MoveDTO move, CombatantState actor)
    {
        ActiveBuff buff = new ActiveBuff(
            move.BuffAffectedStat,
            move.BuffAmount,
            move.BuffDuration,
            move.Name
        );

        actor.ActiveBuffs.Add(buff);

        return buff;
    }
}