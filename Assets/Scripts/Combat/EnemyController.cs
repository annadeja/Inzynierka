using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    //public float patrollingRadius = 30f;
    [SerializeField] CharacterStats enemyStats;
    [SerializeField] private List<Transform> patrolPoints;

    private CombatController combatController;
    private GameObject player;
    private CharacterStats playerStats;
    private bool isInCombat = false;

    private Transform transform;
    private NavMeshAgent navMeshAgent;
    private Vector3 home;
    private float distance;
    private Vector3 turnDirection;
    private Quaternion turnQuaternion;
    private System.Random random = new System.Random();
    private int destinationIndex = 0;

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
        if (isInCombat)
            engage();
        else
            patrol();
    }

    private void engage()
    {
        enemyStats.decreaseCooldown(Time.deltaTime);
        navMeshAgent.autoBraking = true;
        followPlayer();
        turnToPlayer();
    }

    private void patrol()
    {
        if (!navMeshAgent.pathPending || navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.autoBraking = false;
            navMeshAgent.SetDestination(patrolPoints[destinationIndex].position);
            destinationIndex = random.Next(0, patrolPoints.Count - 1);
        }
    }

    private void followPlayer()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= navMeshAgent.stoppingDistance)
        {
            attack();
        }
        else
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    private void turnToPlayer()
    {
        turnDirection = (player.transform.position - transform.position).normalized;
        turnQuaternion = Quaternion.LookRotation(turnDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, turnQuaternion, Time.deltaTime * 5);
    }

    private void attack()
    {
        bool killedPlayer = combatController.attack(enemyStats, playerStats);
        if (killedPlayer)
            SceneManager.LoadScene("GameOverScreen");
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCombat = true;
        player = other.gameObject;
        playerStats = SaveDataController.instance.loadedSave.PlayerStats;
    }

    private void OnTriggerExit(Collider other)
    {
        isInCombat = false;
    }
}
