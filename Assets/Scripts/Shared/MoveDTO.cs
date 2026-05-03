[System.Serializable]
public class MoveDTO {
    public string Name;
    public string Description;
    public MoveType Type;
    public MoveEffectType Effect;
    public int BaseValue;

    // Buff:
    public int BuffDuration;  // How many turns the buff lasts; 0 if the move performs no a buff.
    public StatType BuffAffectedStat;
    public int BuffAmount;
}