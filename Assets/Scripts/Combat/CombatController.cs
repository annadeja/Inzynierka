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
        if (attacker.attackCooldown <= 0.0f)
        {
            int damage = Math.Max(0, (int)(attackModifier * attacker.attack) - target.defense);
            target.takeDamage(damage);
            Debug.Log(attacker.attack);
            Debug.Log("Damage: " + damage);
            attacker.attackCooldown = attacker.maxCooldown;
            Debug.Log("HP left: " + target.currentHP);
        }
        if (target.currentHP <= 0)
            return true;
        else
            return false;
    }

    public void killPlayer(bool killedPlayer)
    {
        if (killedPlayer)
            SceneManager.LoadScene("GameOverScreen");
    }

    public void killEnemy(bool killedEnemy, GameObject enemy)
    {
        if (killedEnemy)
        {
            Debug.Log("Killed enemy");
            enemy.SetActive(false);
        }
    }
}
