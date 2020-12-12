using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            int damage = (int)(attackModifier * attacker.attack) - target.defense;
            target.takeDamage(damage);
            Debug.Log("Damage: " + damage);
            attacker.attackCooldown = attacker.maxCooldown;
            Debug.Log("HP left: " + target.currentHP);
        }
        if (target.currentHP <= 0)
            return true;
        else
            return false;
    }
}
