using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneScript : MonoBehaviour
{

    [SerializeField] private CharacterController drone;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerController playerInfo;

    [Header("States")]
    [SerializeField] bool isChasing;
    [SerializeField] bool isAiming;
    [SerializeField] bool isAttacking;
    [SerializeField] bool isRecovering;
    [SerializeField] bool isCharging;
    [SerializeField] bool attacked;
    #pragma warning disable 0649
    [Header("Attacking Stats")]
    [SerializeField] float aggroRange;
    [SerializeField] float chargeSpeed;
    [SerializeField] float aimDelay;
    [SerializeField] float recoveryTime;
    [SerializeField] float chargeTime;
    #pragma warning restore 0649

    [Header("Death Timer")]
    [SerializeField] private bool willDie = true;
    [SerializeField] float lifeSpan = 10f;
    float killTimer = 0f;
    float aimTimer = 0f;
    float recTimer = 0f;
    float chargingTimer = 0f;
    float originalSpeed;
    float originalAngle;

    void Awake()
    {
        drone = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerInfo = player.GetComponent<PlayerController>();

        if (navMeshAgent.isOnNavMesh == false)
        {
            float range = 2000f;
            Vector3 point;
            RandomPoint(transform.position, range, out point);
            if (RandomPoint(transform.position, range, out point))
            {
                navMeshAgent.Warp(point);
            }
            else
                Destroy(transform.gameObject);
            Destroy(transform.parent.gameObject);
        }
        originalSpeed = navMeshAgent.speed;
        originalAngle = navMeshAgent.angularSpeed;
        isChasing = true;
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    void FixedUpdate()
    {
        if(attacked == true)
        {
            if (navMeshAgent.hasPath == false)
            {
                isRecovering = true;
            }
        }

        if(isCharging)
        {
            ChargePlayer();
        }

        if(isRecovering == true)
        {
            Recovery();
        }

        if(isAttacking == true)
        {
            AttackState();
        }

        if (isAiming)
        {
            AimAttack();
        }

        if (playerInfo.crossed == false && isChasing == true)
        {
            ChasePlayer();
        }


        if (willDie)
        {
            killTimer += Time.unscaledDeltaTime;

            if (killTimer >= lifeSpan)
            {
                Destroy(transform.parent.gameObject);
            }
        }


    }

    void Update()
    {
    }

    public void MoveAt(Vector3 Direction)
    {
        navMeshAgent.SetDestination(Direction);
    }



    public Vector3 getPlayerPosition()
    {
        return player.transform.position;
    }

    void ChasePlayer()
    {
        if (InAttackRadius())
        {
            navMeshAgent.isStopped = true;
            isAttacking = true;
            isChasing = false;
        }
        else if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    void ChargePlayer()
    {
        navMeshAgent.speed = navMeshAgent.speed * chargeSpeed;
        navMeshAgent.angularSpeed = 360f;

        isAiming = false;

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.transform.position);
        if (navMeshAgent.hasPath)
        {
            attacked = true;
            chargingTimer += Time.deltaTime;
            if (chargingTimer >= chargeTime)
            {
                isCharging = false;
                isAttacking = false;
            }
        }
    }

    void AttackState()
    {
        navMeshAgent.isStopped = true;
        if(isAiming == false)
        {
            isAiming = true;
        }
    }

    void AimAttack()
    {
        transform.LookAt(player.transform);
        aimTimer += Time.deltaTime;
        if (aimTimer >= aimDelay)
        {
            isCharging = true;
        }
    }

    void Recovery()
    {
        aimTimer = 0.0f;
        chargingTimer = 0.0f;
        navMeshAgent.isStopped = true;
        recTimer += Time.deltaTime;
        navMeshAgent.speed = originalSpeed;
        navMeshAgent.angularSpeed = originalAngle;
        transform.LookAt(player.transform);
        if (recTimer >= recoveryTime)
        {
            isRecovering = false;
            isChasing = true;
            attacked = false;
            navMeshAgent.isStopped = false;
            recTimer = 0.0f;
        }
    }

    void RemoveSelf()
    {
        gameObject.SetActive(false);
    }

    public bool InAttackRadius()
    {
        if (GetDistance() <= aggroRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetDistance()
    {
        return Vector3.Distance(getPlayerPosition(), drone.transform.position);
    }
}
