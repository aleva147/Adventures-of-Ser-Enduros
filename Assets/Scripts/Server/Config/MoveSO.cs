using UnityEngine;


[CreateAssetMenu(menuName = "MoveSO")]
public class MoveSO : ScriptableObject {
    public string Name;
    public string Description;
    public MoveType Type;
    public MoveEffectType Effect;
    public int BaseValue;

    [Header("Buff")]
    public int BuffDuration;  // How many turns the buff lasts; 0 if the move performs no a buff.
    public StatType BuffAffectedStat;
    public int BuffAmount;


    public MoveDTO ToMoveDTO()
    {
        return new MoveDTO
        {
            Name                = Name,
            Description         = Description,
            Type                = Type,
            Effect              = Effect,
            BaseValue           = BaseValue,
            BuffDuration        = BuffDuration,
            BuffAffectedStat    = BuffAffectedStat,
            BuffAmount          = BuffAmount,
        };
    }
}