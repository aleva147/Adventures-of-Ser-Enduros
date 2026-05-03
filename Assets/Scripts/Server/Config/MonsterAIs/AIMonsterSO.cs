using UnityEngine;

public abstract class AIMonsterSO : ScriptableObject
{
    public abstract MoveSO SelectMove(MonsterSO monster, BattleStateSnapshot battleState);
}