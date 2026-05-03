/*
    Displays monster stats and name when an enemy button is clicked on the map.
    Fires events for its two buttons — knows nothing about what either does.
*/

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyInfoPopupView : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;

    [Header("Monster Info")]
    [SerializeField] private TMP_Text monsterName;
    [SerializeField] private TMP_Text monsterDesc;

    [Header("Buttons")]
    [SerializeField] private Button fightButton;
    [SerializeField] private Button closeButton;

    public event Action OnFightPressed;
    public event Action OnClosePressed;


    private void Awake()
    {
        popupPanel.SetActive(false);
    }

    private void OnEnable() 
    {    
        fightButton.onClick.AddListener(() => OnFightPressed?.Invoke());
        closeButton.onClick.AddListener(() => OnClosePressed?.Invoke());
    }

    private void OnDisable() 
    {
        fightButton.onClick.RemoveAllListeners();
        closeButton.onClick.RemoveAllListeners();
    }


    public void Show(MonsterDTO monster)
    {
        var stats = monster.Stats;

        monsterName.text = monster.Name;
        monsterDesc.text = monster.Description;

        popupPanel.SetActive(true);
    }

    public void Hide()
    {
        popupPanel.SetActive(false);
    }

}