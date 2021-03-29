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
    private Vector3 droneVelocity;
    private float gravityValue;
    public float burst = 10f;
    public float thrust = 5.0f;
    [Header("Hovering Timer")]
    [SerializeField] private float hover;
    [SerializeField] private float hoverTime;
    [SerializeField] private float hoverStop = 2f;
    private float distance;

    void Awake()
    {
        drone = GetComponent<CharacterController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerInfo = player.GetComponent<PlayerController>();
        if (navMeshAgent.isOnNavMesh == false)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        ChasePlayer();
        //droneVelocity.y += gravityValue * Time.deltaTime;
        //drone.Move(droneVelocity * Time.deltaTime);
        //InAttackRadius();
    }

    void Update()
    {

    }

    public void FloatUp()
    {
        hover += Time.deltaTime;
        hoverTime = hover - hover % 1;
        if (hoverTime % 2 == 0)
        {
            //drone.AddForce(0, thrust, 0);
        }
        //Debug.Log(hoverTime);

        Debug.Log(navMeshAgent.isOnNavMesh);
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
        //drone.MoveAt(drone.getPlayerPosition());
        if (navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }
    public bool InAttackRadius()
    {
        if (GetDistance() <= playerInfo.radius)
        {
            Debug.LogWarning("Player in kill range");
            return true;
        }
        else
        {
            Debug.LogWarning("Player NOT in attack range");
            return false;
        }
    }

    public float GetDistance()
    {
        return distance = Vector3.Distance(getPlayerPosition(), drone.transform.position);
    }
}
