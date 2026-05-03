/*
    Manages the hero's move collection and equipped moves.
    Enforces the 4-slot maximum and prevents duplicate learning.
    
    All mutation goes through this service. 
    HeroState.KnownMoves and HeroState.EquippedMoves are never written to directly from outside this class.
    
    Fires OnInventoryChanged after any state mutation so that MoveInventoryController can refresh the view.
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveInventoryService
{
    public const int MaxEquippedMoves = 4;
    public event Action OnInventoryChanged;  // MoveInventoryController is subscribed to this.


    /// Picks one random but new move from a defeated monster's moveset and assigns it to the hero's KnownMoves. 
    /// Called by BattleOutcomeController after victory.
    public MoveDTO LearnRandomMoveFromMonster(HeroState hero, MonsterDTO monster)
    {
        if (monster.Moveset == null || monster.Moveset.Count == 0)
        {
            Debug.LogWarning($"[MoveInventoryService] Monster '{monster.Name}' has no moves to learn.");
            return null;
        }

        // Filter only moves the hero doesn't already know from the monster's moveset.
        var newMoves = new List<MoveDTO>();
        foreach (var move in monster.Moveset)
            if (!hero.HasLearned(move.Name))
                newMoves.Add(move);

        if (newMoves.Count == 0) return null;

        MoveDTO moveToLearn = newMoves[UnityEngine.Random.Range(0, newMoves.Count)];
        LearnMove(hero, moveToLearn);
        
        return moveToLearn;
    }

    public void LearnMove(HeroState hero, MoveDTO move)
    {
        hero.LearnedMoves.Add(move);
        hero.KnownMovesSet.Add(move.Name);
        
        OnInventoryChanged?.Invoke();
    }



    // Called by MoveInventoryController when a card is clicked. 
    // The view doesn't need to know the current state to decide what to do.
    public void ToggleEquip(HeroState hero, MoveDTO move)
    {
        if (hero.IsMoveEquipped(move.Name))
            UnequipMove(hero, move);
        else
            EquipMove(hero, move);
    }

    public bool EquipMove(HeroState hero, MoveDTO move)
    {
        for (int i = 0; i < hero.EquippedMoves.Count; i++)
        {
            if (hero.EquippedMoves[i] == null)
            {
                hero.EquippedMoves[i] = move;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.Log("[MoveInventoryService] All four slots are full.");
        return false;
    }

    public void UnequipMove(HeroState hero, MoveDTO move)
    {
        for (int i = 0; i < hero.EquippedMoves.Count; i++)
        {
            if (hero.EquippedMoves[i] != null && hero.EquippedMoves[i].Name == move.Name)
            {
                hero.EquippedMoves[i] = null;
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }



    // /// Returns true if the hero's loadout is valid to enter battle with.
    // /// The confirm button in MoveInventoryView should be locked until this is true.
    // public bool IsLoadoutComplete(HeroState hero)
    //     => hero.EquippedMoveCount == MaxEquippedMoves;

    // /// Returns how many slots are still open.
    // /// Useful for the "2 slots remaining" label in the inventory UI.
    // public int RemainingSlots(HeroState hero)
    //     => MaxEquippedMoves - hero.EquippedMoveCount;
}