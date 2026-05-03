/*
    Renders the book screen: five enemy buttons on the map, hero stats, titles and equiped moves.
    Pure presentation — receives data from BookController, fires events back.
*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookView : MonoBehaviour
{
    [Header("Enemy Buttons")]
    [SerializeField] private EnemyButtonUI[] enemyButtons;

    [Header("Move Inventory")]
    [SerializeField] private Button openInventoryButton;

    [Header("Hero Stats")]
    [SerializeField] private TMP_Text heroLevelText;
    [SerializeField] private TMP_Text heroExpText;
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text defenseText;
    [SerializeField] private TMP_Text magicText;
    [SerializeField] private TMP_Text healthText;

    [Header("Equipped Moves")]
    [SerializeField] private Image[] equippedMovesImages;
    [SerializeField] private MoveIconsSO moveIcons;

    [Header("Titles")]
    [SerializeField] private TMP_Text titlesText;

    [Header("Map")]
    [SerializeField] private Image mapBg;
    [SerializeField] private Sprite[] mapBgSprites;
    [SerializeField] private TMP_Text[] mapPlacesTexts;
    private string[] mapPlacesDefeatedNames =
    {
        "Town of\nHope",
        "Forest of\nBlossom",
        "Clear Lake",
        "Eagle Mountains",
        "Capital of Resistance"
    };

    public event Action<int> OnEnemyButtonPressed;
    public event Action OnOpenInventoryPressed;


    private void Awake()
    {
        openInventoryButton.onClick.AddListener(() => OnOpenInventoryPressed?.Invoke());
    }


    // Raises an event for BookController when a button on the map is clicked. 
    private void RaiseEnemyButtonPressed(int index)
    {
        OnEnemyButtonPressed?.Invoke(index);
    }


    public void InitializeEnemyButtons(bool[] defeated)
    {
        for (int i = 0; i < enemyButtons.Length; i++)
        {
            // First monster is always unlocked; subsequent ones require the previous monster to be defeated
            bool isLocked = i > 0 && !defeated[i - 1];

            enemyButtons[i].Initialize(i, isLocked, RaiseEnemyButtonPressed);  // TODO: Try with unlocked for all buttons
        }
    }


    // Refreshes the hero stats UI.
    // Called on initial scene load and after returning from battles.
    public void UpdateHeroProfile(HeroState hero)
    {
        var stats = hero.Stats;

        heroLevelText.text = $"Lvl: {hero.Level}";
        heroExpText.text   = $"EXP: {hero.Exp} / {hero.ExpThresholds[hero.Level - 1]}";
        healthText.text    = $"HP  {stats.MaxHealth}";
        attackText.text    = $"AT  {stats.Attack}";
        defenseText.text   = $"DE  {stats.Defense}";
        magicText.text     = $"MG  {stats.Magic}";

        UpdateEquippedMoves(hero.EquippedMoves.ToArray());

        titlesText.text = "";
        foreach (string title in hero.achievedTitles) {
            titlesText.text += title + "\n";
        }
    }

    public void UpdateEquippedMoves(MoveDTO[] equippedMoves)
    {
        for (int i = 0; i < equippedMovesImages.Length; i++)
        {
            var move = i < equippedMoves.Length ? equippedMoves[i] : null;
            equippedMovesImages[i].sprite  = move != null ? moveIcons.GetSprite(move.Name) : null;
            equippedMovesImages[i].enabled = move != null;
        }
    }

    public void UpdateMap(bool[] defeatedFlags)
    {
        int i = 0;
        for (; i < defeatedFlags.Length && defeatedFlags[i]; i++)
        {
            mapPlacesTexts[i].text = mapPlacesDefeatedNames[i];
        }
        mapBg.sprite = mapBgSprites[i];
    }


    private void OnDestroy()
    {
        openInventoryButton.onClick.RemoveAllListeners();
    }
}