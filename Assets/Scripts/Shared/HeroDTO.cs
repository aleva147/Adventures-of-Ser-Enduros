using System.Collections.Generic;


[System.Serializable]
public class HeroDTO
{
    public string Name;
    public CharacterStats BaseStats;
    public List<MoveDTO> BaseMoveset;

    // Leveling:
    public List<int> ExpThresholds;
    public List<CharacterStats> StatGrowthPerLevel;

    // Other:
    public List<string> AllTitles;
}