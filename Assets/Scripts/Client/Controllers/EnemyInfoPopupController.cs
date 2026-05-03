/*
    Controls the enemy info popup in the Book scene.
    Opened by BookController when an enemy button is pressed.  
*/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyInfoPopupController : MonoBehaviour
{
    [SerializeField] private EnemyInfoPopupView view;
    private int pendingMonsterIndex = -1;


    private void Awake()
    {
        view.OnFightPressed += OnFightPressed;
        view.OnClosePressed += OnClosePressed;
    }

    private void OnDestroy()
    {
        view.OnFightPressed -= OnFightPressed;
        view.OnClosePressed -= OnClosePressed;
    }


    /// Called by MapController when an enemy button is pressed.
    /// Stores the enemy index locally until the player confirms choice or dismisses.
    public void Open(MonsterDTO monster, int monsterIndex)
    {
        pendingMonsterIndex = monsterIndex;
        view.Show(monster);
    }

    private void OnFightPressed()
    {
        GameManager.Instance.RunState.SelectedMonsterIndex = pendingMonsterIndex;

        view.Hide();
        SceneManager.LoadScene("Battle");
    }

    private void OnClosePressed()
    {
        pendingMonsterIndex = -1;
        view.Hide();
    }
}