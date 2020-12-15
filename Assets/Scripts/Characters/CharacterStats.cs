using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStats
{
    [Header("Progression stats")]
    public int Level;
    public int CurrentXP;
    public int ToNextLevel;
    public int RewardXP;

    [Header("Combat stats")]
    public int MaxHP;
    public int CurrentHP;
    public int Attack;
    public int Defense;
    public float AttackCooldown;
    public float MaxCooldown;

    [Header("Dialog stats")]
    public int Charisma;
    public int Deception;
    public int Thoughtfulness;

    public CharacterStats()
    {
        Level = 1;
        CurrentXP = 0;
        ToNextLevel = 10;
        RewardXP = 10;

        MaxHP = 20;
        CurrentHP = MaxHP;
        Attack = 10;
        Defense = 10;
        AttackCooldown = 2.0f;
        MaxCooldown = AttackCooldown;

        Charisma = 10;
        Deception = 10;
        Thoughtfulness = 10;
    }

    public void takeDamage(int damage)
    {
        CurrentHP -= damage;
    }

    public void decreaseCooldown(float time)
    {
        AttackCooldown -= time;
    }

    public void levelUp(int experience)
    {
        CurrentXP += experience;
        if (CurrentXP >= ToNextLevel)
        {
            Level++;
            CurrentXP = 0;
            ToNextLevel = 2 * ToNextLevel;
            Debug.Log("Leveled up.");
        }
    }
}
