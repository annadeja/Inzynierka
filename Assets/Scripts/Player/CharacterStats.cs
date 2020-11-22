using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    [Header("Combat stats")]
    public int maxHP;
    public int currentHP;
    public int attack;
    public int defense;

    [Header("Dialog stats")]
    public int charisma;
    public int deception;
    public int thoughtfulness;

    public CharacterStats()
    {
        maxHP = 20;
        currentHP = maxHP;
        attack = 10;
        defense = 10;
        charisma = 10;
        deception = 10;
        thoughtfulness = 10;
    }
}
