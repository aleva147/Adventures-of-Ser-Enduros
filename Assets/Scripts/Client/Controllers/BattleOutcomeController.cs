/*
    Activated by BattleController when a battle ends.
    BattleController's job ends when the phase changes to Victory or Defeat. Everything that happens after that lives here.

    Owns all post-battle logic: XP award, move learning, and marking the monster as defeated in RunState.  
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleOutcomeController : MonoBehaviour
{
    [SerializeField] private BattleOutcomeView view;
    [SerializeField] private MoveIconsSO moveIcons;


    private void Awake()
    {
        view.OnReturnToBookPressed += OnReturnToBookPressed;
    }


    // Called by BattleController when BattlePhase.Victory is reached.
    // Handles all post-battle progression before showing the view.
    public void ShowVictory(MonsterDTO defeatedMonster)
    {
        var gameManager    = GameManager.Instance;
        var hero           = gameManager.RunState.Hero;
        var moveInventory  = gameManager.MoveInventory;
        var levelUpService = gameManager.LevelUp;
        var runState       = gameManager.RunState;

        // Award XP (LevelUpService will level hero up if needed and update all its stats)
        levelUpService.AwardExp(hero, defeatedMonster.ExpReward);

        // Earn a title if this is the first time this monster was beaten
        if (!runState.IsMonsterDefeated(runState.SelectedMonsterIndex))
        {
            hero.achievedTitles.Add(hero.allTitles[runState.SelectedMonsterIndex + 1]);
        }

        // Mark the monster as defeated in RunState
        runState.MarkMonsterDefeated(runState.SelectedMonsterIndex);

        // Learn a random move from the defeated monster
        MoveDTO learnedMove = moveInventory.LearnRandomMoveFromMonster(hero, defeatedMonster);
        Sprite learnedMoveSprite = learnedMove != null
            ? moveIcons.GetSprite(learnedMove.Name)
            : null;


        view.ShowVictory(learnedMove, learnedMoveSprite, defeatedMonster.Name, defeatedMonster.ExpReward);
    }

    /// Called by BattleController when BattlePhase.Defeat is reached.
    public void ShowDefeat(MonsterDTO monster)
    {
        view.ShowDefeat(monster.Name);
    }


    // Triggered by BattleOutcomeView. 
    private void OnReturnToBookPressed()
    {
        SceneManager.LoadScene("Book");
    }


    private void OnDestroy()
    {
        view.OnReturnToBookPressed -= OnReturnToBookPressed;
    }
}