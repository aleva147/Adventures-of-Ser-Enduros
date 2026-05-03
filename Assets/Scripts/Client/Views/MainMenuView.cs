/*
    Owns all UI references for the MainMenu scene.
    Fires events for user interactions and is fully decoupled from the . 
    MainMenuController is subscribed to its events and it's responsible for all the logic.
*/

using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitButton;

    public event Action OnNewGamePressed;
    public event Action OnQuitPressed;


    private void Awake()
    {
        newGameButton.onClick.AddListener(() => OnNewGamePressed?.Invoke());
        quitButton.onClick.AddListener(() => OnQuitPressed?.Invoke());
    }


    // MainMenuController uses this method to prevent additional clicks while RunConfig data is being loaded from the server.
    public void SetInteractable(bool interactable)
    {
        newGameButton.interactable = interactable;
        quitButton.interactable    = interactable;
    }
}