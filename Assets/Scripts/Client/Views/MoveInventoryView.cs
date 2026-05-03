/*
    The move inventory popup panel.
    Displays a fixed-size grid of all moves in the game:
    known moves first (with icons), then empty slots to fill the rest of the grid.
    
    Knows nothing about equip logic — fires OnMoveCardClicked and lets the controller decide what to do.
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveInventoryView : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Grid")]
    [SerializeField] private Transform gridContainer;
    [SerializeField] private MoveCardUI moveCardPrefab;

    [Header("Confirm")]
    [SerializeField] private Button confirmButton;

    [Header("Icons")]
    [SerializeField] private MoveIconsSO moveIcons;

    public event Action OnConfirmPressed;
    public event Action<string> OnMoveCardClicked;

    private readonly List<MoveCardUI> spawnedCards = new();


    private void Awake()
    {
        confirmButton.onClick.AddListener(() => OnConfirmPressed?.Invoke());
        panel.SetActive(false);
    }


    // Opens the panel and populates the grid.
    public void Show(List<MoveDTO> learnedMoves, MoveDTO[] equippedMoves, int totalMoveCount)
    {
        BuildGrid(learnedMoves, equippedMoves, totalMoveCount);
        RefreshConfirmButton(equippedMoves);
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }


    // Refreshes equipped highlights after a toggle without rebuilding the whole grid.
    public void RefreshEquippedStates(MoveDTO[] equippedMoves, MoveDTO[] learnedMoves)
    {
        var equippedIds = BuildEquippedIdSet(equippedMoves);

        // Only learned cards (first N) have a Name — empty cards ignore this
        for (int i = 0; i < learnedMoves.Length && i < spawnedCards.Count; i++)
            spawnedCards[i].SetEquipped(equippedIds.Contains(learnedMoves[i].Name));

        RefreshConfirmButton(equippedMoves);
    }


    private void BuildGrid(List<MoveDTO> learnedMoves, MoveDTO[] equippedMoves, int totalMoveCount)
    {
        // Clear previously spawned cards
        foreach (var card in spawnedCards)
            Destroy(card.gameObject);
        spawnedCards.Clear();

        var equippedIds = BuildEquippedIdSet(equippedMoves);

        for (int i = 0; i < totalMoveCount; i++)
        {
            var card = Instantiate(moveCardPrefab, gridContainer);
            spawnedCards.Add(card);

            if (i < learnedMoves.Count)
            {
                var move     = learnedMoves[i];
                var sprite   = moveIcons.GetSprite(move.Name);
                bool equipped = equippedIds.Contains(move.Name);

                card.BindMove(move.Name, sprite, equipped);
                card.OnCardClicked += RaiseMoveCardClicked;
            }
            else
            {
                card.BindEmpty();
            }
        }
    }

    private void RefreshConfirmButton(MoveDTO[] equippedMoves)
    {
        int filledSlots = 0;
        foreach (var m in equippedMoves)
            if (m != null) filledSlots++;

        confirmButton.interactable = filledSlots == equippedMoves.Length;
    }

    // Raises an event for MoveInventoryController when a move is clicked.
    private void RaiseMoveCardClicked(string Name)
    {
        OnMoveCardClicked?.Invoke(Name);
    }

    private static HashSet<string> BuildEquippedIdSet(MoveDTO[] equippedMoves)
    {
        var set = new HashSet<string>();
        foreach (var m in equippedMoves)
            if (m != null) set.Add(m.Name);
        return set;
    }

    private void OnDestroy()
    {
        confirmButton.onClick.RemoveAllListeners();

        foreach (var card in spawnedCards)
            if (card != null) card.OnCardClicked -= RaiseMoveCardClicked;
    }
}