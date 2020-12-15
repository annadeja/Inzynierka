using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatController
{
    private static CombatController instance;

    private float attackModifier = 1.5f;

    private CombatController()
    {}

    public static CombatController getInstance()
    {
        if (instance == null)
            instance = new CombatController();
        return instance;
    }

    public bool attack(CharacterStats attacker, CharacterStats target)
    {
        if (attacker.AttackCooldown <= 0.0f)
        {
            int damage = Math.Max(0, (int)(attackModifier * attacker.Attack) - target.Defense);
            target.takeDamage(damage);
            Debug.Log(attacker.Attack);
            Debug.Log("Damage: " + damage);
            attacker.AttackCooldown = attacker.MaxCooldown;
            Debug.Log("HP left: " + target.CurrentHP);
        }
        if (target.CurrentHP <= 0)
            return true;
        else
            return false;
    }

    public void killPlayer(bool killedPlayer)
    {
        if (killedPlayer)
            SceneManager.LoadScene("GameOverScreen");
    }

    public void killEnemy(bool killedEnemy, GameObject enemy, CharacterStats playerStats, CharacterStats enemyStats)
    {
        if (killedEnemy)
        {
            Debug.Log("Killed enemy");
            enemy.SetActive(false);
            playerStats.levelUp(enemyStats.RewardXP);
        }
    }
}
