using UnityEngine;

public class MonsterMoveEndpoint
{
    public MonsterMoveResponse HandleRequest(BattleStateSnapshot battleState)
    {
        MonsterSO monster = MonsterRegistry.GetByName(battleState.MonsterName);

        MoveSO chosenMove = monster.AIProfile.SelectMove(monster, battleState);
        return new MonsterMoveResponse { ChosenMoveName = chosenMove.Name };
    }
}