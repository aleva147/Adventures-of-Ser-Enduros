using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Monster AIs SO/Aggressive")]
public class AIAgressiveSO : AIMonsterSO
{
    [Tooltip("Monster attempts to heal when HP falls below this fraction.")]
    [Range(0f, 1f)]
    public float criticalHpThreshold = 0.25f;

    public override MoveSO SelectMove(MonsterSO monster, BattleStateSnapshot battleState)
    {
        float hpRatio = (float)battleState.MonsterCurrentHp / monster.Stats.MaxHealth;

        if (hpRatio <= criticalHpThreshold)
        {
            var heal = FindFirstOfEffect(monster.Moveset, MoveEffectType.HEAL)
                    ?? FindFirstOfEffect(monster.Moveset, MoveEffectType.DRAIN);
            if (heal != null) return heal;
        }

        return FindFirstOfEffect(monster.Moveset, MoveEffectType.DAMAGE)
            ?? FindFirstOfEffect(monster.Moveset, MoveEffectType.DRAIN)
            ?? monster.Moveset[Random.Range(0, monster.Moveset.Count)];
    }

    private MoveSO FindFirstOfEffect(List<MoveSO> moveset, MoveEffectType effect)
    {
        var candidates = new List<MoveSO>();
        foreach (var m in moveset)
            if (m.Effect == effect) candidates.Add(m);
        return candidates.Count > 0 ? candidates[Random.Range(0, candidates.Count)] : null;
    }
}