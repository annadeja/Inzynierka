using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//!Skrypt obsługujący walkę gracza.
public class PlayerCombatController : MonoBehaviour 
{
    private CombatController combatController; //!<Przechowuje uniwersalny kontroler walki.
    private CharacterStats playerStats; //!<Statystyki gracza.
    private bool isInRange = false; //!<Czy przeciwnik jest w zasięgu ataku?
    private GameObject enemy; //!<Przechowuje obiekt przeciwnika.
    private CharacterStats enemyStats; //!<Przechowuje statystyki przeciwnika.

    void Start()
    {
        combatController = CombatController.getInstance();
        playerStats = SaveDataController.getInstance().LoadedSave.PlayerStats;
    }

    void Update()
    {
        playerStats.decreaseCooldown(Time.deltaTime); //Zmniejszanie czasu odświeżania ataku.
        if (Input.GetButtonDown("Attack")) //Wyprowadzenie ataku przy naciśnięciu przycisku.
            attack();
    }
    //!Funkcja ataku.
    private void attack()
    {
        if (isInRange)
        {
            bool killedEnemy = combatController.attack(playerStats, enemyStats);
            combatController.killEnemy(killedEnemy, enemy, playerStats, enemyStats);
            if (killedEnemy)
            {
                isInRange = false;
                enemy = null;
                enemyStats = null;
            }
        }
    }
    //!Sprawdza czy przeciwnik wszedł w zasięg gracza.
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
    //!Wyłącza flagę zasiegu gdy przeciwnik się wystarczająco oddali.
    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
    }
}
