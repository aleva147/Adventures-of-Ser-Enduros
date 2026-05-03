[System.Serializable]
public class CharacterStats
{
    public int MaxHealth;
    public int Attack;
    public int Defense;
    public int Magic;

    public CharacterStats(int hp, int at, int df, int mg)
    {
        MaxHealth = hp;
        Attack = at;
        Defense = df;
        Magic = mg;
    }
}