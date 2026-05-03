/*
    Represents a buff currently affecting a fighter. 
    It's included in BattleStateSnapshot and tracked on the client's side. 
*/

[System.Serializable]
public class ActiveBuff
{
    public StatType AffectedStat;
    public int Modifier;
    public int TurnsRemaining;
    public string SourceMoveId;
    

    public ActiveBuff(StatType statType, int value, int turns, string sourceMoveName)
    {
        AffectedStat = statType;
        Modifier = value;
        TurnsRemaining = turns;
        SourceMoveId = sourceMoveName;
    }
}