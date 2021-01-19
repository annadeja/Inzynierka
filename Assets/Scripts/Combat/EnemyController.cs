using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//!Skrypt definiujący zachowanie przeciwników.
public class EnemyController : MonoBehaviour 
{
    [SerializeField] public CharacterStats enemyStats; //!<Statystyki przeciwnika.
    [SerializeField] private List<Transform> patrolPoints; //!<Punkty jakie patroluje przeciwnik.

    private CombatController combatController; //!<Przechowuje uniwersalny kontroler walki.
    private GameObject player; //!<Przechowuje obiekt gracza.
    private CharacterStats playerStats; //!<Przechowuje statystyki gracza.
    private bool isInCombat = false; //!<Okresla czy przeciwnik jest w aktywnej walce.

    private Transform transform; //!<Pozycja i transformacje pozycji przeciwnika.
    private NavMeshAgent navMeshAgent; //!<Realizuje wbudowany mechanizm Unity co do podążania i wyznaczania ścieżek dla postaci niezależnych.
    private Vector3 home; //!<Początkowa pozycja przeciwnika.
    private float distance; //!<Dystans od gracza.
    private Vector3 turnDirection; //!<Kierunek obrotu.
    private Quaternion turnQuaternion; //!<Kwaternion obrotu.
    private System.Random random = new System.Random(); //!<Wartość pomocnicza do losowania kolejnego punktu patrolu.
    private int destinationIndex = 0; //!<Indeks punktu patrolu.

    void Start()
    {
        combatController = CombatController.getInstance();
        transform = gameObject.GetComponentInChildren<Transform>();
        navMeshAgent = gameObject.GetComponentInChildren<NavMeshAgent>();
        home = transform.position;
        navMeshAgent.autoBraking = false;
        navMeshAgent.SetDestination(patrolPoints[destinationIndex].position);
    }

    void Update()
    {
        enemyStats.decreaseCooldown(Time.deltaTime); //Zmniejszanie czasu odświeżania ataku.
        if (isInCombat)
            engage(); //Kontynuuje walkę.
        else
            patrol(); //Patroluje.
    }
    //!Funkcja przygotowania do walki.
    private void engage() 
    {
        if (player != null)
        {
            navMeshAgent.autoBraking = true;
            followPlayer();
            turnToPlayer();
        }
    }
    //!Funkcja patrolowania.
    private void patrol() 
    {
        if (!navMeshAgent.pathPending || navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete) //Wyznacza nowy punkt jeżeli przeciwnik osiągnął swój cel lub nie był w stanie do niego dojść.
        {
            navMeshAgent.autoBraking = false;
            navMeshAgent.SetDestination(patrolPoints[destinationIndex].position); //Wybiera wcześniej wylosowany punkt.
            destinationIndex = random.Next(0, patrolPoints.Count - 1); //Losuje nowy punkt.
        }
    }
    //!Funkcja podążania za graczem.
    private void followPlayer() 
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= navMeshAgent.stoppingDistance) //Przestaje podążąć jeżeli jest wystarczająco blisko gracza.
            attack();
        else
            navMeshAgent.SetDestination(player.transform.position);
    }
    //!Funkcja obracania się do gracza.
    private void turnToPlayer() //Funkcja obracania się do gracza.
    {
        turnDirection = (player.transform.position - transform.position).normalized;
        turnQuaternion = Quaternion.LookRotation(turnDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, turnQuaternion, Time.deltaTime * 5);
    }
    //!Atak wobec gracza.
    private void attack() 
    {
        if (playerStats != null)
        {
            bool killedPlayer = combatController.attack(enemyStats, playerStats);
            combatController.killPlayer(killedPlayer);
        }
    }
    //!Sprawdza czy gracz wszedł w obszar percepcji przeciwnika.
    private void OnTriggerEnter(Collider other) 
    {
        PlayerCombatController playerCombatController = other.gameObject.GetComponent<PlayerCombatController>();
        if (playerCombatController != null)
        {
            isInCombat = true;
            player = other.gameObject;
            playerStats = SaveDataController.getInstance().LoadedSave.PlayerStats;
        }
    }
    //!Wyłącza flagę walki gdy gracz się wystarczająco oddali.
    private void OnTriggerExit(Collider other) 
    {
        isInCombat = false;
    }
}