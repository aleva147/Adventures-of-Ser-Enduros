/*
    One slot in the move inventory grid.
    Can be either bound to a move or empty.
    Passes its moveId back on click so the controller
    can identify which move was tapped.
*/

using System;
using UnityEngine;
using UnityEngine.UI;



public class MoveCardUI : MonoBehaviour
{
    private string boundMoveName;
    [SerializeField] private Button button;
    [SerializeField] private Image moveIcon;
    [SerializeField] private Image background;
    [SerializeField] private Color32 equippedColor;
    [SerializeField] private Color32 normalColor;
    [SerializeField] private Color32 emptyColor;


    public event Action<string> OnCardClicked;


    private void Awake()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        if (boundMoveName != null)
            OnCardClicked?.Invoke(boundMoveName);
    }


    // Binds this card to a known move.
    public void BindMove(string moveName, Sprite sprite, bool isEquipped)
    {
        boundMoveName = moveName;

        moveIcon.sprite  = sprite;
        moveIcon.enabled = true;
        background.color = isEquipped ? equippedColor : normalColor;

        button.interactable = true;
    }

    // Renders this card as an empty, non-interactable slot.
    public void BindEmpty()
    {
        boundMoveName = null;

        moveIcon.enabled = false;
        background.color = emptyColor;

        button.interactable = false;
    }

    // Refreshes only the equipped highlight without rebinding the full card.
    // Called by MoveInventoryView.RefreshEquippedStates() after a toggle.
    public void SetEquipped(bool isEquipped)
    {
        background.color = isEquipped ? equippedColor : normalColor;
    }


    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}