/*
    Having a dictionary on RunState that maps MoveNames to Sprites is wrong
    because sprites are a presentation concern and RunState holds game logic data. 
    
    MoveData is also a wrong place because it's a shared DTO that the server layer uses too, 
    and the server doesn't have to know about sprites, only logic.

    By using a ScriptableObject that lives purely on the client side and maps MoveNames to Sprites, 
    designers can do this mapping entirely in the Inspector, as long as they use the same MoveNames used in the backend.
    Now, any View that needs Move icons can reference this Scriptable Object.
*/

using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Sprites SO/Moves")]
public class MoveIconsSO : ScriptableObject
{
    [Serializable]
    public struct Entry
    {
        public string moveName;
        public Sprite sprite;
    }

    [SerializeField] private Entry[] entries;

    private Dictionary<string, Sprite> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<string, Sprite>(entries.Length);
        foreach (var entry in entries)
            if (!string.IsNullOrEmpty(entry.moveName))
                lookup[entry.moveName] = entry.sprite;
    }

    public Sprite GetSprite(string moveId)
    {
        if (lookup.TryGetValue(moveId, out var sprite))
            return sprite;

        Debug.LogWarning($"[MoveIconsSO] No sprite for move '{moveId}'.");
        return null;
    }
}