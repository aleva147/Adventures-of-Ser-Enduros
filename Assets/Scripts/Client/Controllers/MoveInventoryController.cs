/*
    Controls the move inventory popup on the Map scene.
    Opened by MapController when the inventory button is pressed.

    It currently reads state from GameManager on every open, never caches hero state locally.
    But since MoveInventory doesn't have to live across scenes, this can be optimized later.
*/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveInventoryController : MonoBehaviour
{
    [SerializeField] private MoveInventoryView view;

    private MoveInventoryService MoveInventory => GameManager.Instance.MoveInventory;
    private HeroState Hero => GameManager.Instance.RunState.Hero;

    public event Action OnInventoryClose; // BookController listens to this event and updates the preview of the four equipped moves when inventory closes. 


    private void Awake() {
        view.OnMoveCardClicked += OnMoveCardClicked;
        view.OnConfirmPressed  += OnConfirmPressed;
    }


    /// Called by MapController when the inventory button is pressed.
    /// Re-reads hero state fresh every time so the grid is always current.
    public void Open()
    {
        var runState = GameManager.Instance.RunState;
        int totalMoves = runState.MoveRegistry.Count;

        view.Show(Hero.LearnedMoves, Hero.EquippedMoves.ToArray(), totalMoves);
    }


    private void OnMoveCardClicked(string moveName)
    {
        var runState = GameManager.Instance.RunState;

        if (!runState.MoveRegistry.TryGetValue(moveName, out var move))
        {
            Debug.LogError($"[MoveInventoryController] MoveId '{moveName}' not found in registry.");
            return;
        }

        MoveInventory.ToggleEquip(Hero, move);

        // Refresh only the equipped highlights — no need to rebuild the whole grid
        view.RefreshEquippedStates(Hero.EquippedMoves.ToArray(), Hero.LearnedMoves.ToArray());
    }

    private void OnConfirmPressed()
    {
        view.Hide();
        OnInventoryClose?.Invoke();
    }


    private void OnDestroy()
    {
        view.OnMoveCardClicked -= OnMoveCardClicked;
        view.OnConfirmPressed  -= OnConfirmPressed;
    }
}