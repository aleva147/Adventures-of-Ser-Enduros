using System.Collections.Generic;


[System.Serializable]
public class MonsterDTO
{
    public string Name;
    public CharacterStats Stats;
    public List<MoveDTO> Moveset;
    
    public string Description;
    public int ExpReward;
}