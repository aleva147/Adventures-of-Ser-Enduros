using System.Collections.Generic;

public class HeroState
{
    public string Name;
    public CharacterStats Stats;
    public int Level;
    public int Exp;
    public List<int> ExpThresholds;
    public List<CharacterStats> StatGrowthPerLevel;

    public int MaxEquipedMoves;
    public HashSet<string> KnownMovesSet;
    public List<MoveDTO> EquippedMoves;
    public List<MoveDTO> LearnedMoves;

    public List<string> allTitles;
    public List<string> achievedTitles;


    public HeroState(HeroDTO heroDTO)
    {
        Name = heroDTO.Name;
        Stats = heroDTO.BaseStats;
        Level = 1;
        Exp = 0;
        ExpThresholds = heroDTO.ExpThresholds;
        StatGrowthPerLevel = heroDTO.StatGrowthPerLevel;

        MaxEquipedMoves = 4;
        EquippedMoves = new List<MoveDTO>(MaxEquipedMoves);
        LearnedMoves = new List<MoveDTO>(heroDTO.BaseMoveset);
        KnownMovesSet = new HashSet<string>();
        for (int i = 0; i < heroDTO.BaseMoveset.Count; i++)
        {
            KnownMovesSet.Add(heroDTO.BaseMoveset[i].Name);
            if (i < MaxEquipedMoves) 
                EquippedMoves.Add(heroDTO.BaseMoveset[i]);
        }

        allTitles = heroDTO.AllTitles;
        achievedTitles = new List<string> { allTitles[0] };
    }


    public bool HasLearned(string moveName)
    {
        return KnownMovesSet.Contains(moveName);
    }

    public bool IsMoveEquipped(string moveName)
    {
        foreach (MoveDTO move in EquippedMoves)
        {
            if (move == null) continue;
            if (move.Name == moveName) return true;
        }
        return false;
    }
}