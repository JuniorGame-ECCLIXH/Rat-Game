using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyFOV : MonoBehaviour
{
    [Header("FOV")]
    public float fovRadius;

    [Range(0, 360)]
    public float fovAngle;

    public GameObject player;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstructionMask;

    public bool canSeePlayer;

    [Header("Alerted")]

    [SerializeField] private float alertRadius;

    [SerializeField] private LayerMask enemyMask;

    private void Start()
    {
        //Initializes the detection coroutine
        StartCoroutine(FOVRoutine());
    }

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
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, fovRadius, targetMask);
        
        //Checks if a collision was detected and was added to the collider array
        if(rangeCheck.Length != 0)
        {
            //Gets the transform from the most recent collision from the collider array
            Transform target = rangeCheck[0].transform;
            //Gets the distance between the enemy and the detected collision
            Vector3 targetDirection = (target.position - transform.position).normalized;
            //Checks if the collision was detected within the valid FOV angle
            if(Vector3.Angle(transform.forward, targetDirection) < fovAngle / 2)
            {
                //gets the distance between the enemy and the target
                float targetDistance = Vector3.Distance(transform.position, target.position);

                //Sends a raycast that checks if an obstruction is between the enemy and the target
                if(!Physics.Raycast(transform.position, targetDirection, targetDistance, obstructionMask))
                {
                    //If no obstruction in the way then the player has been spotted
                    canSeePlayer = true;
                    AlertOthers();
                }
                else
                {
                    //If obstuction in the way then the player isn't spotted.
                    canSeePlayer = false;
                }
            }
            //If not in the valid FOV then the player is no longer spotted.
            else
            {
                canSeePlayer = false;
            }
        }
        //Resets the player spotted variable if the player is out of range of detection.
        else if(canSeePlayer == true)
        {
            canSeePlayer = false;
        }
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
                closeEnemies[i].GetComponent<EnemyFOV>().canSeePlayer = true;         
            }
        }
    }
}
