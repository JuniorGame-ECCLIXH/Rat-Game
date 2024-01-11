using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyFOV : MonoBehaviour
{
    [Header("FOV")]
    [SerializeField] private float FOVRadius;
    [Range(0, 360)] [SerializeField] private float FOVAngle;
    private GameObject player;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;
    private bool combatStart = false;
    private bool canSeePlayer = false;

    [Header("Alerted")]
    [SerializeField] private float alertRadius;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float spottedCountdown = 0f;
    [SerializeField] private float maxSpot = 60f;

    [Header("Editor Visuals")]
    [SerializeField] private bool drawGizmos = true;

    private void Start()
    {
        //Initializes the detection coroutine
        StartCoroutine(FOVRoutine());
    }

    void Update()
    {
        if(spottedCountdown > 0f && canSeePlayer)
        {
            spottedCountdown -= Time.deltaTime;
        }
        else if(spottedCountdown <= 0 && canSeePlayer)
        {
            combatStart = true;
            AlertOthers();
        }
        else
        {
            spottedCountdown = 0f;
        }
    }

    public bool CanSeePlayer() => canSeePlayer;
    public GameObject GetPlayer() => player;

    private IEnumerator FOVRoutine()
    {
        //Initilizes delay amount for the coroutine
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        //Activates the CheckPOV function as many times as the delay amount allows for.
        while(true) 
        {
            yield return wait;
            CheckFOV();
        }
    }

    private void CheckFOV()
    {
        //Populates an array with colliders that are detected by a overlap sphere
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, FOVRadius, targetMask);
        //Checks if a collision was detected and was added to the collider array
        if(rangeCheck.Length != 0)
        {
            //Gets the transform from the most recent collision from the collider array
            Transform target = rangeCheck[0].transform;
            //Gets the distance between the enemy and the detected collision
            Vector3 targetDirection = (target.position - transform.position).normalized;
            //Checks if the collision was detected within the valid FOV angle
            if(Vector3.Angle(transform.forward, targetDirection) < FOVAngle / 2)
            {
                //gets the distance between the enemy and the target
                float targetDistance = Vector3.Distance(transform.position, target.position);

                //Sends a raycast that checks if an obstruction is between the enemy and the target
                if(!Physics.Raycast(transform.position, targetDirection, targetDistance, obstructionMask))
                {
                    //If no obstruction in the way then the player has been spotted
                    player = target.gameObject;
                    canSeePlayer = true;
                    return;
                }
            }
        }

        canSeePlayer = false;
    }

    private void AlertOthers()
    {
        //Creates an array of other enemies within a radius around the alerted enemy
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius, enemyMask);
        //Create an empty array of gameobjects
        GameObject[] closeEnemies = new GameObject[colliders.Length];
        //Populate the array with game objects based on the array of colliders
        for (int i = 0; i < colliders.Length; i++)
        {
            closeEnemies[i] = colliders[i].gameObject;
        }
        //Alerts all of the enemies in the array
        for(int i = 0; i < closeEnemies.Length; i++) 
        {
            if (closeEnemies[i].GetComponent("EnemyFOV") != null)
            {
                closeEnemies[i].GetComponent<EnemyFOV>().combatStart = true;         
            }
        }
    }

    //editor visualizations
    private void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, FOVRadius);

        Vector3 angleDirection1 = DirectionFromAngle(transform.eulerAngles.y, FOVAngle / 2);
        Vector3 angleDirection2 = DirectionFromAngle(transform.eulerAngles.y, -FOVAngle / 2);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, angleDirection1 * FOVRadius);
        Gizmos.DrawRay(transform.position, angleDirection2 * FOVRadius);

        if(canSeePlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float degreeAngle)
    {
        degreeAngle += eulerY;

        return new Vector3(Mathf.Sin(degreeAngle * Mathf.Deg2Rad), 0, Mathf.Cos(degreeAngle * Mathf.Deg2Rad));
    }
}
