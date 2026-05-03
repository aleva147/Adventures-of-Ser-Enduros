/*
    Renders the BattleScene screen and animates action results.
    Receives all data from BattleController.
    
    Responsibilities:
      - Updates on-screen hero and monster HP
      - Renders the equipped moves buttons
      - Animates damage numbers, heals, and buff notifications
      - Blocks/unblocks user input on controller triggers
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using NUnit.Framework;

public class BattleView : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private CharacterSpritesSO characterSpritesSO;
    [SerializeField] private MoveIconsSO moveIconsSO;

    [Header("Monster")]
    [SerializeField] private Image monsterImage;
    [SerializeField] private TMP_Text  monsterNameText;
    [SerializeField] private TMP_Text monsterHpText;
    [SerializeField] private TMP_Text[] monsterStatsTexts; // AT, DF, MG

    [Header("Hero")]
    [SerializeField] private Image heroImage;
    [SerializeField] private TMP_Text heroNameText;
    [SerializeField] private TMP_Text heroHpText;
    [SerializeField] private TMP_Text[] heroStatsTexts; // AT, DF, MG

    [Header("Move Buttons")]
    [SerializeField] private BattleMoveButtonUI[] moveButtons;

    [Header("Battle Log")]
    [SerializeField] private TMP_Text battleLogText;

    // [Header("Floating Text")]
    // [SerializeField] private TextMeshProUGUI floatingTextPrefab;
    // [SerializeField] private Transform heroFloatingTextAnchor;
    // [SerializeField] private Transform monsterFloatingTextAnchor;

    [Header("Colors")]
    [SerializeField] private Color damageColor = new Color(0.9f, 0.2f, 0.2f);
    [SerializeField] private Color healColor = new Color(0.2f, 0.9f, 0.4f);
    [SerializeField] private Color buffColor = new Color(0.9f, 0.8f, 0.2f);
    [SerializeField] private Color attackMoveColor;
    [SerializeField] private Color magicMoveColor;
    [SerializeField] private Color heroTextColor;
    [SerializeField] private Color monsterTextColor;
    
    // Cached values for future renders
    private int heroMaxHp;
    private int monsterMaxHp;

    public event Action<MoveDTO> OnMoveButtonClicked;


    
    /// Called once by BattleController before the first turn.
    /// Initializes everything on the screen.
    public void SetupBattle(MonsterDTO monster, HeroState hero)
    {
        // Cache max hp values for future renders:
        monsterMaxHp = monster.Stats.MaxHealth;
        heroMaxHp = hero.Stats.MaxHealth;

        // Monster Panel:
        monsterNameText.text = monster.Name;
        monsterHpText.text = $"HEALTH: {monsterMaxHp} / {monsterMaxHp}";
        monsterStatsTexts[0].text = $"AT: {monster.Stats.Attack}";
        monsterStatsTexts[1].text = $"DF: {monster.Stats.Defense}";
        monsterStatsTexts[2].text = $"MG: {monster.Stats.Magic}";

        monsterImage.sprite = characterSpritesSO.GetSprite(monster.Name);

        // Hero Panel:
        heroNameText.text = hero.Name;
        heroHpText.text = $"HEALTH: {heroMaxHp} / {heroMaxHp}";
        heroStatsTexts[0].text = $"AT: {hero.Stats.Attack}";
        heroStatsTexts[1].text = $"DF: {hero.Stats.Defense}";
        heroStatsTexts[2].text = $"MG: {hero.Stats.Magic}";

        heroImage.sprite = characterSpritesSO.GetSprite(hero.Name);

        // Move buttons
        SetMoveButtons(hero.EquippedMoves.ToArray());

        battleLogText.text = "The opponents are staring each other in the eyes\nfrom a far...";

        Debug.Log("View setup finished!");
    }
    

    public void SetMoveButtons(MoveDTO[] equippedMoves)
    {
        for (int i = 0; i < moveButtons.Length; i++)
        {
            MoveDTO move = i < equippedMoves.Length ? equippedMoves[i] : null;
            Sprite moveSprite = moveIconsSO.GetSprite(move.Name);

            moveButtons[i].Initialize(move, moveSprite, OnMoveButtonClicked);
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        Debug.Log("INPUT ENABLED is " + enabled);
        foreach (var btn in moveButtons)
            btn.SetInteractable(enabled);
    }


    // Called by BattleController after every performed move.
    public void UpdateHpBars(int heroCurrentHp, CharacterStats heroCurrentStats, int monsterCurrentHp, CharacterStats monsterCurrentStats)
    {
        heroHpText.text = $"HEALTH: {heroCurrentHp} / {heroMaxHp}";
        monsterHpText.text = $"HEALTH: {monsterCurrentHp} / {monsterMaxHp}";

        // Update stats as well because of buffs:
        monsterStatsTexts[0].text = $"AT: {monsterCurrentStats.Attack}";
        monsterStatsTexts[1].text = $"DF: {monsterCurrentStats.Defense}";
        monsterStatsTexts[2].text = $"MG: {monsterCurrentStats.Magic}";

        heroStatsTexts[0].text = $"AT: {heroCurrentStats.Attack}";
        heroStatsTexts[1].text = $"DF: {heroCurrentStats.Defense}";
        heroStatsTexts[2].text = $"MG: {heroCurrentStats.Magic}";
    }


    /// Entry point for all action feedback. Reads BattleActionResult and
    /// triggers the appropriate visual responses.
    public void AnimateActionResult(BattleActionResult result)
    {
        bool isHero = result.ActorIsHero;

        string actorName = isHero ? heroNameText.text : monsterNameText.text;
        string actorTextColor = isHero ? heroTextColor.ToHexString() : monsterTextColor.ToHexString();
        string moveTextColor = (result.MoveUsed.Type == MoveType.MAGIC) ? magicMoveColor.ToHexString() : attackMoveColor.ToHexString();

        // // Determine which anchor the floating text appears on:
        // // damage/debuffs appear on the target, heals/buffs on the actor
        // var targetAnchor = isHero ? monsterFloatingTextAnchor : heroFloatingTextAnchor;
        // var actorAnchor  = isHero ? heroFloatingTextAnchor    : monsterFloatingTextAnchor;


        battleLogText.text = "";

        if (result.DamageDealt > 0)
        {
            // SpawnFloatingText($"-{result.DamageDealt}", targetAnchor, damageColor);
            AppendToLog($"<color=#{actorTextColor}>{actorName}</color> uses <color=#{moveTextColor}>{result.MoveUsed.Name}</color>\n" +
                        $"and deals {result.DamageDealt} DAMAGE!");
        }

        if (result.HealingDone > 0)
        {
            // SpawnFloatingText($"+{result.HealingDone}", actorAnchor, healColor);
            AppendToLog($"<color=#{actorTextColor}>{actorName}</color> restored {result.HealingDone} HP.");
        }

        if (result.BuffApplied != null)
        {
            // SpawnFloatingText(result.MoveUsed.Name, actorAnchor, buffColor);
            AppendToLog($"<color=#{actorTextColor}>{actorName}</color> uses {result.MoveUsed.Name}!");
        }

        if (result.TargetDefeated)
        {
            string targetName = isHero ? monsterNameText.text : heroNameText.text;
            string targetColor = isHero ? monsterTextColor.ToHexString() : heroTextColor.ToHexString();
            AppendToLog($"<color=#{targetColor}>{targetName}</color> was defeated!");
        }
    }

    private void AppendToLog(string message)
    {
        battleLogText.text += "\n\n" + message;
    }


    // private void SpawnFloatingText(string message, Transform anchor, Color color)
    // {
    //     if (floatingTextPrefab == null || anchor == null) return;

    //     var instance = Instantiate(floatingTextPrefab, anchor.position, Quaternion.identity, anchor);
    //     instance.text  = message;
    //     instance.color = color;

    //     StartCoroutine(AnimateFloatingText(instance));
    // }

    // private IEnumerator AnimateFloatingText(TextMeshProUGUI label)
    // {
    //     float duration  = 1.2f;
    //     float elapsed   = 0f;
    //     var   startPos  = label.rectTransform.anchoredPosition;
    //     var   startColor = label.color;

    //     while (elapsed < duration)
    //     {
    //         elapsed += Time.deltaTime;
    //         float t  = elapsed / duration;

    //         // Float upward
    //         label.rectTransform.anchoredPosition = startPos + Vector2.up * (50f * t);

    //         // Fade out in the second half
    //         float alpha = t < 0.5f ? 1f : 1f - ((t - 0.5f) / 0.5f);
    //         label.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

    //         yield return null;
    //     }

    //     Destroy(label.gameObject);
    // }
}