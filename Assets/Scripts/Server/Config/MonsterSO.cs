using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "UnitSO/MonsterSO")]
public class MonsterSO : ScriptableObject
{
    public string Name;
    public CharacterStats Stats;
    public List<MoveSO> Moveset;
    
    public string Description;
    public AIMonsterSO AIProfile;
    public int ExpReward;


    public MonsterDTO ToMonsterDTO()
    {
        List<MoveDTO> movesetDTO = new (Moveset.Count);

        foreach (MoveSO move in Moveset)
            movesetDTO.Add(move.ToMoveDTO());

        return new MonsterDTO
        {
            Name        = Name,
            Stats       = Stats,
            Moveset     = movesetDTO,
            Description = Description,
            ExpReward   = ExpReward,
        };
    }
}