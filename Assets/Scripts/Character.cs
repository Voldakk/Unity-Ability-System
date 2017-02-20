using UnityEngine;
using System.Collections.Generic;

public enum TargetTeam { None, Friendly, Enemy, Both }
public enum Team { None, Team1, Team2 }
public class Character : MonoBehaviour
{
    public CharacterStats stats;
    public Team team;
    public static List<Character> characters = new List<Character>();

    void Awake()
    {
        characters.Add(this);
    }

    void OnDestroy()
    {
        if (characters.Contains(this))
            characters.Remove(this);
    }
}