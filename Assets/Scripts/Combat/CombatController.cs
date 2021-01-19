using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//!Uniwersalna klasa singleton, która odpowiada za walkę.
public class CombatController 
{
    private static CombatController instance; //!<Instancja klasy.

    private float attackModifier = 1.5f; //!<Mnożnik statystyk ataku na obrażenia.

    private CombatController()
    {}
    //!Zwraca instancję.
    public static CombatController getInstance() 
    {
        if (instance == null)
            instance = new CombatController();
        return instance;
    }
    //!Uniwersalna funkcja ataku.
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
        if (target.CurrentHP <= 0) //Sprawdza czy atakowany umarł.
            return true;
        else
            return false;
    }
    //!Przenosi do ekranu końca gry jeżeli gracz został zabity.
    public void killPlayer(bool killedPlayer) 
    {
        if (killedPlayer)
            SceneManager.LoadScene("GameOverScreen");
    }
    //!Usuwa przeciwnika ze świata gry jeżeli został zabity.
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
