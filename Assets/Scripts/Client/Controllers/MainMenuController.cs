/*
    Mediates between MainMenuView and GameManager.
    Subscribes to the MainMenuView's button events, fetches RunConfig from the Server 
    and hands it to GameManager, then transitions to the BookScene.
*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuView view;


    private void OnEnable()
    {
        view.OnNewGamePressed += HandleNewGame;
        view.OnQuitPressed    += HandleQuit;   
    }

    private void OnDisable()
    {
        view.OnNewGamePressed -= HandleNewGame;
        view.OnQuitPressed    -= HandleQuit;
    }

    private async void HandleNewGame()
    {
        view.SetInteractable(false);

        RunConfigDTO runConfig = await GameManager.Instance.Gateway.FetchRunConfig();

        if (runConfig == null)
        {
            Debug.LogError("[MainMenuController] FetchRunConfig returned null.");
            view.SetInteractable(true);
            return;
        }

        GameManager.Instance.Initialize(runConfig);

        SceneManager.LoadScene("Book");
    }

    private void HandleQuit()
    {
        Application.Quit();
    }
}