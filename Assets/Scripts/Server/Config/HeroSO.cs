using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "UnitSO/HeroSO")]
public class HeroSO : ScriptableObject
{
    public string Name;
    public CharacterStats BaseStats;
    public List<MoveSO> BaseMoveset;

    // Leveling:
    public List<int> ExpThresholds;
    public List<CharacterStats> StatGrowthPerLevel;

    // Other:
    public List<string> AllTitles;


    public HeroDTO ToHeroDTO()
    {
        List<MoveDTO> movesetDTO = new (BaseMoveset.Count);

        foreach (MoveSO move in BaseMoveset)
            movesetDTO.Add(move.ToMoveDTO());

        return new HeroDTO
        {
            Name                = Name,
            BaseStats           = BaseStats,
            BaseMoveset         = movesetDTO,
            ExpThresholds       = ExpThresholds,
            StatGrowthPerLevel  = StatGrowthPerLevel,
            AllTitles           = AllTitles,
        };
    }
}