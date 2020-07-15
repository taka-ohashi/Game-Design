using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a sub class of Guard Controller

[System.Serializable] 
public class PatrollingGuard : GuardController
{
    public Vector3[] patrolPoints;
    private int currentPoint = 0;
    public float rotation = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        ParentStart();
    }

    // Update is called once per frame
    void Update()
    {
        GuardWakeUp();

        //Visualize raycast in Scene play. Doesn't affect gameplay
        //Debug.DrawRay(viewPoint.transform.position,(GameObject.FindWithTag("Player").transform.position - viewPoint.transform.position), Color.white, 0.0f, true);

        animator.SetFloat("velocity", agent.velocity.magnitude * agent.speed);

        switch (GetAction())
        {
            case "idle":
                //stand and wait
                findPlayer();
                Idle();
                break;

            case "attack":
                //attack the player
                Attack();
                break;

            case "guard":
                //walk to next destination point
                findPlayer();
                Guard();
                break;
            
            case "chase":
                //Chase the player
                Chase();
                break;

            default:
                //error, action wasn't right
                SetAction("idle");
                break;
        }
    }

    private void Guard()
    {
        //This function sets a new destination fot the guard
        if(Vector3.Distance(transform.position, patrolPoints[currentPoint % patrolPoints.Length]) < .2)
        {
            currentPoint ++;
            if(currentPoint % patrolPoints.Length == 0) {currentPoint = 0;}
            agent.SetDestination(patrolPoints[currentPoint]);
            SetAction("idle");
        }
        else
        {
            agent.SetDestination(patrolPoints[currentPoint]);
        }
    }
}
