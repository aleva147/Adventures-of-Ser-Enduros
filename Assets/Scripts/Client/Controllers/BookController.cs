/*
    On Start(): reads all state it needs from GameManager.RunState and pushes it into BookView.
    
    On enemy button press: writes SelectedMonsterIndex to RunState then loads the Battle scene. 
    The index is how this scene communicates the player's choice to the next scene.
    
    On inventory button press: delegates to MoveInventoryController.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class BookController : MonoBehaviour
{
    [SerializeField] private BookView bookView;
    [SerializeField] private MoveInventoryController moveInventoryController;
    [SerializeField] private EnemyInfoPopupController enemyInfoPopupController;


    private void Start()
    {
        // Guard: if somehow the map is loaded before GameManager is initialized
        // (e.g. during development starting from the Map scene directly)
        if (GameManager.Instance == null)
        {
            Debug.LogError("[MapController] GameManager is not initialized. " +
                            "Start the game from the Main Menu scene.");
            return;
        }

        bookView.OnEnemyButtonPressed  += OnEnemyButtonPressed;
        bookView.OnOpenInventoryPressed += OnOpenInventoryPressed;
        moveInventoryController.OnInventoryClose += OnInventoryClosed;

        PopulateView();
    }


    private void PopulateView()
    {
        var runState = GameManager.Instance.RunState;

        bookView.InitializeEnemyButtons(runState.defeatedFlags);
        bookView.UpdateHeroProfile(runState.Hero);
        bookView.UpdateMap(runState.defeatedFlags);
    }


    private void OnEnemyButtonPressed(int monsterIndex)
    {
        var monster = GameManager.Instance.RunState.GetMonsterAt(monsterIndex);
        enemyInfoPopupController.Open(monster, monsterIndex);
    }

    private void OnOpenInventoryPressed()
    {
        moveInventoryController.Open();
    }

    private void OnInventoryClosed()
    {
        bookView.UpdateEquippedMoves(GameManager.Instance.RunState.Hero.EquippedMoves.ToArray());
    }


    private void OnDestroy()
    {
        bookView.OnEnemyButtonPressed  -= OnEnemyButtonPressed;
        bookView.OnOpenInventoryPressed -= OnOpenInventoryPressed;
        moveInventoryController.OnInventoryClose -= OnInventoryClosed;
    }
}