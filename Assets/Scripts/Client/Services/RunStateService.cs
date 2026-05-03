/*
    Single source of truth for the state of the current run.
    Constructed by GameManager.Initialize() after RunConfigDTO is loaded from the server 
    and lives for the rest of the session.

    Holds all data about the run, including the hero's mutable HeroState, passed world levels, and all moves and monsters.
    It doesn't change any of this data, except for passed world levels (defeated monsters). 
    Other services are responsible for data modification.
*/

using System.Collections.Generic;
using UnityEngine;

public class RunStateService
{
    // Run configuration (immutable after construction).
    // public RunConfigDTO RunConfig { get; }

    // Any system that needs MoveDTO by MoveName uses this instead of searching a list every time.
    public IReadOnlyDictionary<string, MoveDTO> MoveRegistry { get; }

    // Hero's stats, exp and level (mutable across the run).
    public HeroState Hero { get; }

    // All monsters on the map, in chronological order.
    public List<MonsterDTO> Monsters { get; }

    // Tracks which world levels have been cleared.
    public bool[] defeatedFlags;

    // Used for cross-scene communication in the GetSelectedMonster method.
    // Written by MapController before loading the Battle scene.
    // Read by BattleController in Start() to know which monster to fight.
    public int SelectedMonsterIndex { get; set; } = -1;



    public RunStateService(RunConfigDTO config)
    {
        // RunConfig = config; 
        MoveRegistry  = BuildMoveRegistry(config.AllMoves);
        Hero          = new HeroState(config.Hero);
        Monsters      = config.Monsters;
        defeatedFlags = new bool[config.Monsters.Count];
    }


    
    public MonsterDTO GetMonsterAt(int index)
    {
        if (index < 0 || index >= Monsters.Count)
        {
            Debug.LogError($"[RunStateService] Monster index {index} is out of range.");
            return null;
        }

        return Monsters[index];
    }

    public MonsterDTO GetSelectedMonster()
    {
        if (SelectedMonsterIndex < 0)
        {
            Debug.LogError("[RunStateService] GetSelectedMonster called but SelectedMonsterIndex is -1.");
            return null;
        }

        return GetMonsterAt(SelectedMonsterIndex);
    }

    public bool IsMonsterDefeated(int index) => defeatedFlags[index];

    public void MarkMonsterDefeated(int index)
    {
        if (index < 0 || index >= defeatedFlags.Length) return;
        defeatedFlags[index] = true;
    }

    public bool IsRunComplete()
    {
        foreach (bool flag in defeatedFlags)
            if (!flag) return false;
        return true;
    }

    /// Used by BattleService when resolving the monster's chosen move id into a full MoveDTO object.
    public MoveDTO GetMove(string moveId)
    {
        if (MoveRegistry.TryGetValue(moveId, out var move))
            return move;

        Debug.LogError($"[RunStateService] MoveId '{moveId}' not found in registry.");
        return null;
    }



    // Helper function.
    private static Dictionary<string, MoveDTO> BuildMoveRegistry(List<MoveDTO> allMoves)
    {
        var registry = new Dictionary<string, MoveDTO>(allMoves.Count);
        foreach (var move in allMoves)
        {
            if (!registry.ContainsKey(move.Name))
                registry[move.Name] = move;
            else
                Debug.LogWarning($"[RunStateService] Duplicate Name '{move.Name}' in AllMoves — skipping.");
        }
        return registry;
    }
}