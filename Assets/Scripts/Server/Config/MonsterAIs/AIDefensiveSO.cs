using UnityEngine;
using System.Collections.Generic;
using System.Linq; // Required for Where() and First()


[CreateAssetMenu(menuName = "Monster AIs SO/Defensive")]
public class AIDefensiveSO : AIMonsterSO
{
    [Tooltip("Monster attempts to heal when HP falls below this fraction.")]
    [Range(0f, 1f)]
    public float criticalHpThreshold = 0.25f;

    public override MoveSO SelectMove(MonsterSO monster, BattleStateSnapshot battleState)
    {
        float hpPercent = (float)battleState.MonsterCurrentHp / monster.Stats.MaxHealth;
        bool hasShieldUp = monster.Moveset.Any(m => m.Effect == MoveEffectType.BUFF);

        if (hpPercent < criticalHpThreshold && hasShieldUp)
            return monster.Moveset.First(m => m.Effect == MoveEffectType.BUFF);

        return monster.Moveset
            .Where(m => m.Effect == MoveEffectType.DAMAGE)
            .OrderByDescending(m => m.BaseValue)
            .First();
    }
}