using UnityEngine;
using System.Collections;

public class DungeonInfo : MonoBehaviour
{
    public string dungeonName = "Scary Dungeon";
    public int dungeonLevel = 1;
    public int minLevel = 1;
    public int maxLevel = 2;
    public int enemyCount = 5;

    public bool unlocked = false;
    public DungeonInfo nextDungeon;

    
    public GameObject[] Enemies; // these are the enemy design prefabs 
    // TODO ENEMY TYPE
    // TODO BOSS
    // TODO BACKGROUND
}
