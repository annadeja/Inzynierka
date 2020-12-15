using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private CombatController combatController;
    private CharacterStats playerStats;
    private bool isInRange = false;
    private GameObject enemy;
    private CharacterStats enemyStats;

    void Start()
    {
        combatController = CombatController.getInstance();
        playerStats = SaveDataController.getInstance().loadedSave.PlayerStats;
    }

    void Update()
    {
        playerStats.decreaseCooldown(Time.deltaTime);
        if (Input.GetButtonDown("Attack"))
            attack();
    }

    private void attack()
    {
        if (isInRange)
        {
            bool killedEnemy = combatController.attack(playerStats, enemyStats);
            combatController.killEnemy(killedEnemy, enemy, playerStats, enemyStats);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger) //Sprawdza czy wchodzi się w faktyczny collider przeciwnika.
        {
            EnemyController enemyController = other.gameObject.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                isInRange = true;
                enemy = other.gameObject;
                enemyStats = enemyController.enemyStats;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
    }
}
