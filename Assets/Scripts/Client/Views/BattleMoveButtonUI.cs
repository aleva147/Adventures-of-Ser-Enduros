/*
    This class is attached to every move button in the BattleScene.
    Initialized by BattleBound to a MoveData by BattleView.SetMoveButtons().
    Fires the shared OnMoveButtonClicked callback BattleView provides.
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BattleMoveButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;

    private MoveDTO boundMove;


    public void Initialize(MoveDTO move, Sprite moveSprite, Action<MoveDTO> onClick)
    {
        boundMove = move;
        image.sprite = moveSprite;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick?.Invoke(boundMove));

        // button.interactable = false;
    }

    public void SetInteractable(bool interactable)
    {
        button.interactable = interactable;
    }
}