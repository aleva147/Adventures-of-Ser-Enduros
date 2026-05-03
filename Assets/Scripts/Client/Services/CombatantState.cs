/*
    Runtime representation of a combatant during a single battle.
    Created fresh at the start of each fight, discarded when it ends.
    
    Tracks current HP and active buffs on top of immutable base stats.
    GetEffectiveStats() computes the post-buff stat block that
    BattleResolver uses — never pass BaseStats to the resolver directly.
*/

using UnityEngine;
using System.Collections.Generic;

public class CombatantState
{
    public string Name { get; }
    public int CurrentHp { get; set; }
    public CharacterStats BaseStats { get; }
    public List<ActiveBuff> ActiveBuffs { get; } = new List<ActiveBuff>();


    public CombatantState(string name, int startingHp, CharacterStats baseStats)
    {
        Name = name;
        CurrentHp = startingHp;
        BaseStats = baseStats;
    }


    // Returns combatants AT,DF,MG stats with all active buffs applied (hp is not currentHp but maxHp).
    // BattleCalculator always calls this, it never reads BaseStats directly.
    public CharacterStats GetEffectiveStats()
    {
        int hp  = BaseStats.MaxHealth;
        int atk = BaseStats.Attack;
        int def = BaseStats.Defense;
        int mag = BaseStats.Magic;

        foreach (var buff in ActiveBuffs)
        {
            switch (buff.AffectedStat)
            {
                case StatType.AT: atk += buff.Modifier; break;
                case StatType.DF: def += buff.Modifier; break;
                case StatType.MG: mag += buff.Modifier; break;
            }
        }

        Debug.Log("----GET EFFECTIVE STATS----");
        Debug.Log("current attack: " + atk);
        Debug.Log("current defense: " + def);
        Debug.Log("current magic: " + mag);
        Debug.Log("-----------------------");

        // Prevents debuffs from reducing a stat below 1.
        return new CharacterStats(
            hp,
            System.Math.Max(1, atk),
            System.Math.Max(0, def),
            System.Math.Max(1, mag)
        );

    }


    public bool IsAlive() => CurrentHp > 0;


    public bool HasBuffFromMove(string moveName)
    {
        foreach (var buff in ActiveBuffs)
            if (buff.SourceMoveId == moveName) return true;
        return false;
    }
}