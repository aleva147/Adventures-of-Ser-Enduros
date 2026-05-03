/*
    Script for each of the five buttons on the map (world levels/different enemies).
    Knows nothing about what happens when clicked, bubbles the event up 
    to MapView which bubbles it up to MapController.
*/

using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class EnemyButtonUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Color32 buttonColorUnlocked;
    [SerializeField] private Color32 buttonColorLocked;


    // Called by MapView.InitializeEnemyButtons().
    public void Initialize(int buttonId, bool isLocked, Action<int> onPressed)
    {
        button.interactable = !isLocked;
        button.GetComponent<Image>().color = isLocked ? buttonColorLocked : buttonColorUnlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onPressed(buttonId));
    }
}