/*
    Owns all in-battle state for a single fight.
    Created by BattleController at the start of each battle, discarded when the battle ends.
    
    Drives the turn loop:
      PlayerAction() → MonsterAction() → repeat until someone reaches 0 HP.
    
    Fires events that BattleController subscribes to.
    Never touches Unity UI — that is the view's responsibility.
*/

using System;
using System.Threading.Tasks;
using UnityEngine;
    
public class BattleService
{
    private readonly ServerGateway gateway;
    private readonly RunStateService runState;

    public CombatantState HeroCombatant { get; private set; }
    public CombatantState MonsterCombatant { get; private set; }
    public BattlePhase CurrentPhase { get; private set; }
    public int TurnNumber { get; private set; }

    // private string playerLastMoveName = string.Empty;  // The move the player used this turn. Can be sent to the server in the snapshot for more intelligent enemies.

    public event Action<BattleActionResult> OnActionResolved;
    public event Action<BattlePhase> OnPhaseChanged;


    public BattleService(ServerGateway gateway, RunStateService runState)
    {
        this.gateway = gateway;
        this.runState = runState;
    }


    // Sets up combatant states and starts the first player turn.
    // Called by BattleController in Start().
    public void StartBattle(HeroState hero, MonsterDTO monster)
    {
        HeroCombatant = new CombatantState(
            "hero",  // Important to remain "hero" because of BattleActionResult implementation.
            hero.Stats.MaxHealth,
            hero.Stats
        );

        MonsterCombatant = new CombatantState(
            monster.Name,
            monster.Stats.MaxHealth,
            monster.Stats
        );

        TurnNumber = 1;
        SetPhase(BattlePhase.PlayerTurn);

        Debug.Log("Start battle service finished!");
    }


    // Resolves the player's chosen move, then triggers the monster's response.
    // The method is async because the monster's turn requires a server call.
    // BattleController calls this when the player taps a move button.
    public async Task PlayerAction(MoveDTO move)
    {
        if (CurrentPhase != BattlePhase.PlayerTurn)
        {
            Debug.LogWarning("[BattleService] PlayerAction called outside of PlayerTurn phase.");
            return;
        }

        // playerLastMoveName = move.Name;

        // Perform player move
        var result = BattleResolver.Resolve(move, HeroCombatant, MonsterCombatant);
        OnActionResolved?.Invoke(result);

        // Pause a bit:
        await Task.Delay(1200);

        if (result.TargetDefeated)
        {
            SetPhase(BattlePhase.Victory);
            return;
        }
        
        // Let the monster perform its turn:
        SetPhase(BattlePhase.MonsterTurn);
        await MonsterAction();
    }


    private async Task MonsterAction()
    {
        BattleStateSnapshot snapshot = BuildBattleSnapshot();
        MonsterMoveResponse response = await gateway.FetchMonsterMove(snapshot);

        if (response == null)
        {
            Debug.LogError("[BattleService] Received null response from FetchMonsterMove.");
            return;
        }

        MoveDTO move = runState.GetMove(response.ChosenMoveName);
        if (move == null)
        {
            Debug.LogError($"[BattleService] Move '{response.ChosenMoveName}' not found in registry.");
            return;
        }

        var result = BattleResolver.Resolve(move, MonsterCombatant, HeroCombatant);
        OnActionResolved?.Invoke(result);

        // Pause a bit:
        await Task.Delay(800);

        if (result.TargetDefeated)
        {
            SetPhase(BattlePhase.Defeat);
            return;
        }

        TickBuffs(HeroCombatant);
        TickBuffs(MonsterCombatant);

        TurnNumber++;
        SetPhase(BattlePhase.PlayerTurn);
    }



    private void SetPhase(BattlePhase phase)
    {
        Debug.Log("Battle phase " + phase);

        CurrentPhase = phase;
        OnPhaseChanged?.Invoke(phase);
    }

    private BattleStateSnapshot BuildBattleSnapshot()
    {
        return new BattleStateSnapshot
        {
            HeroCurrentHp      = HeroCombatant.CurrentHp,
            HeroActiveBuffIds  = HeroCombatant.ActiveBuffs,
            MonsterName        = MonsterCombatant.Name,
            MonsterCurrentHp   = MonsterCombatant.CurrentHp,
            TurnNumber         = TurnNumber,
            MonsterActiveBuffIds = MonsterCombatant.ActiveBuffs,
            // PlayerLastMoveId   = playerLastMoveName,  // When you implement more intelligent enemies. 
        };
    }

    private void TickBuffs(CombatantState combatant)
    {
        // Iterate backwards so removal by index is safe
        for (int i = combatant.ActiveBuffs.Count - 1; i >= 0; i--)
        {
            combatant.ActiveBuffs[i].TurnsRemaining--;
            if (combatant.ActiveBuffs[i].TurnsRemaining <= 0)
                combatant.ActiveBuffs.RemoveAt(i);
        }
    }
}