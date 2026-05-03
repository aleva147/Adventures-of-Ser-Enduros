/*
    BattleController is the entry point of the Battle scene.

    It wires BattleView events with BattleService calls, and vice-versa.
    Blocks player input during the monster's turn.
    Gives control over to the BattleOutcomeController when the fight ends.
*/

using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    private BattleService battleService;
    [SerializeField] private BattleView battleView;
    [SerializeField] private BattleOutcomeController outcomeController;
    private MonsterDTO currentMonster;


    private void Start()
    {
        RunStateService runState = GameManager.Instance.RunState;

        currentMonster = runState.GetSelectedMonster();

        if (currentMonster == null)
        {
            Debug.LogError("[BattleController] No monster selected — cannot start battle.");
            return;
        }

        battleService = new BattleService(GameManager.Instance.Gateway, runState);

        // Subscribe to service events
        battleService.OnActionResolved += OnActionResolved;
        battleService.OnPhaseChanged   += OnPhaseChanged;

        // Subscribe to view events
        battleView.OnMoveButtonClicked += OnMoveButtonClicked;

        battleView.SetupBattle(currentMonster, runState.Hero);

        battleService.StartBattle(runState.Hero, currentMonster);
    }


    // Triggered by BattleView. 
    private async void OnMoveButtonClicked(MoveDTO move)
    {
        Debug.Log("[CONTROLLER] MOVE CLICKED");

        battleView.SetInputEnabled(false);
        await battleService.PlayerAction(move);
    }

    // Triggered by BattleService.
    private void OnActionResolved(BattleActionResult result)
    {
        battleView.UpdateHpBars(
            battleService.HeroCombatant.CurrentHp,
            battleService.HeroCombatant.GetEffectiveStats(),
            battleService.MonsterCombatant.CurrentHp,
            battleService.MonsterCombatant.GetEffectiveStats()
        );

        battleView.AnimateActionResult(result);
    }

    // Triggered by BattleService.
    private void OnPhaseChanged(BattlePhase phase)
    {
        Debug.Log("PHASE CHANGED TO: " + phase);
        switch (phase)
        {
            case BattlePhase.PlayerTurn:
                battleView.SetInputEnabled(true);
                break;

            case BattlePhase.MonsterTurn:
                battleView.SetInputEnabled(false);
                break;

            case BattlePhase.Victory:
                battleView.SetInputEnabled(false);
                outcomeController.ShowVictory(currentMonster);
                break;

            case BattlePhase.Defeat:
                battleView.SetInputEnabled(false);
                outcomeController.ShowDefeat(currentMonster);
                break;
        }
    }


    private void OnDestroy()
    {
        if (battleService != null)
        {
            battleService.OnActionResolved -= OnActionResolved;
            battleService.OnPhaseChanged   -= OnPhaseChanged;
        }

        battleView.OnMoveButtonClicked -= OnMoveButtonClicked;
    }
}