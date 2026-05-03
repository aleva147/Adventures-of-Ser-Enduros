using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "RunConfigSO")]
public class RunConfigSO : ScriptableObject {
    public HeroSO Hero;
    public List<MonsterSO> Monsters;
    public List<MoveSO> allMoves;


    public RunConfigDTO ToRunConfigDTO()
    {
        HeroDTO heroDTO = Hero.ToHeroDTO();

        List<MonsterDTO> monstersDTO = new (Monsters.Count);
        foreach (MonsterSO monster in Monsters)
            monstersDTO.Add(monster.ToMonsterDTO());

        List<MoveDTO> allMovesDTO = new (allMoves.Count);
        foreach (MoveSO move in allMoves)
            allMovesDTO.Add(move.ToMoveDTO());

        return new RunConfigDTO
        {
            Hero        = heroDTO,
            Monsters    = monstersDTO,
            AllMoves    = allMovesDTO,
        };
    }
}