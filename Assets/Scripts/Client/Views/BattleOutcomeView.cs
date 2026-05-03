/*
    The outcome popup that appears over the battle scene when a fight ends.
    Activated by BattleOutcomeController.
    
    Two states: Victory and Defeat.
      - Victory shows what move was learned from the enemy (or no move if they were all already learned).
      - Defeat just shows a failure message.
    Both states show a button to return back to the BookScene.
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleOutcomeView : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform contentRectTransform; // For forcing the panel to resize when there is less content in the panel.

    [Header("Outcome Header")]
    [SerializeField] private TextMeshProUGUI outcomeHeaderText;
    [SerializeField] private TextMeshProUGUI outcomeDescText;
    [SerializeField] private Color victoryColor;
    [SerializeField] private Color defeatColor;


    [Header("Move Learned Section")]
    [SerializeField] private GameObject moveLearnedSection;
    [SerializeField] private TextMeshProUGUI moveLearnedText;
    [SerializeField] private Image moveLearnedIcon;

    [Header("Button")]
    [SerializeField] private Button continueButton;

    public event Action OnReturnToBookPressed;


    private void Awake()
    {
        continueButton.onClick.AddListener(() => OnReturnToBookPressed?.Invoke());
        panel.SetActive(false);
    }
    

    public void ShowVictory(MoveDTO movelearned, Sprite moveSprite, string monsterName, int expEarned)
    {
        outcomeHeaderText.text  = "Victory";
        outcomeHeaderText.color = victoryColor;

        outcomeDescText.text  = $"You defeated <color=#3CAF3D><b>{monsterName}</color></b> and gained\n<color=#9C3C3C><b>{expEarned} EXP!</b></color>";
        
        PopulateMoveLearnedSection(movelearned, moveSprite);

        panel.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }

    public void ShowDefeat(string monsterName)
    {
        outcomeHeaderText.text  = "Defeated";
        outcomeHeaderText.color = defeatColor;

        outcomeDescText.text = $"You were outmatched by <color=#3CAF3D><b>{monsterName}</color></b>\nand had to retreat to fight another day...";

        moveLearnedSection.SetActive(false);

        panel.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRectTransform);
    }

    
    private void PopulateMoveLearnedSection(MoveDTO move, Sprite moveSprite)
    {
        if (move == null)
        {
            moveLearnedSection.SetActive(false);
            return;
        }

        moveLearnedSection.SetActive(true);
        moveLearnedText.text = $"{move.Name}";

        moveLearnedIcon.sprite  = moveSprite;
        moveLearnedIcon.enabled = moveSprite != null;
    }


    private void OnDestroy()
    {
        continueButton.onClick.RemoveAllListeners();
    }
}