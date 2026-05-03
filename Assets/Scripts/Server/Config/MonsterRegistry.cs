/*
    I wanted to minimize the amount of data transferred between Client and Server.
    So, to avoid sending additional monster information when requesting GetMonsterMove,
    the Client sends only monster's name, and the server 
*/

using UnityEngine;
using System.Collections.Generic;


public static class MonsterRegistry
{
    private static readonly Dictionary<string, MonsterSO> monsters = new();


    public static void Initialize(MonsterSO[] monsters)
    {
        if (IsInitialized()) return;
        
        foreach (MonsterSO monster in monsters)
        {
            MonsterRegistry.monsters.Add(monster.Name, monster);
        }
    }

    public static MonsterSO GetByName(string monsterName)
    {
        return monsters[monsterName];
    }

    public static bool IsInitialized()
    {
        return monsters.Count > 0;
    }
}