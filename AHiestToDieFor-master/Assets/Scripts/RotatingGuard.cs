using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a sub class of Guard Controller

[System.Serializable] 
public class RotatingGuard : GuardController
{
    public int numRotations = 4;
    private int currentView = 0;
    public Vector3[] views;
    private Vector3 rotation = Vector3.zero;
    private Vector3 origin;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //instantiate Guard Controller and set original start point
        ParentStart();
        origin = transform.position;
        SetAction("guard");
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
                //chase the player
                StopCoroutine("Rotating");
                Chase();
                break;

            case "return":
                //return to patrol area
                findPlayer();
                Return();
                break;

            default:
                //error, action wasn't right
                StopCoroutine("Rotating");
                SetAction("idle");
                break;
        }
    }

    private void Guard()
    {
        //if there isn't a rotation, set it to the next one in the views array
        if(rotation == Vector3.zero)
        {
            //How it works: add 1 to a direction in 90 degree inciments. must set views in
            //Unity editor. ONLY use 1s or 0s. Ensures 90 degree turns.
            rotation = new Vector3(transform.position.x + views[currentView].x, 
                                   transform.position.y,
                                   transform.position.z + views[currentView].z);
        }
        //if facing the right direction, turn another 90 degrees after an idle.
        if(!RotateTowards(transform.position, rotation))
        {
            rotation = Vector3.zero;
            currentView ++;
            if(currentView % views.Length == 0) {currentView = 0;}
            SetAction("idle");
        }
    }

    private void Return()
    {
        //if not at the original patrol area, go to it
        if(Vector3.Distance(transform.position, origin) > .1f)
        {
            agent.SetDestination(origin);
        }
        //if at the patrol area, face north and reset angle so always turns 90 degrees
        else
        {
            //if not facing north, rotate north.
            if(!RotateTowards(transform.position, new Vector3(origin.x, origin.y, origin.z + 1)))
            {
                SetAction("idle");
                currentView = 0;
            }
        }
    }

    private bool RotateTowards(Vector3 start, Vector3 end)
    {
        //this method rotates the guard from one angle (start = transform.position) to
        //the desired facing location (end). Used both to face 90 degrees and to reset
        //rotation after getting back to patrol area.

        if(Quaternion.Angle(Quaternion.LookRotation(end - start), transform.rotation) != 0)
        {
            //look towards point
            Quaternion targetRotation = Quaternion.LookRotation(end - start);
            float strength = Mathf.Min(3 * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, strength);
            return true;
        }
        return false;
    }

    IEnumerator Waiting()
    {
        //custom waiting to make sure that returns to patrol area.

        SetWaitCoOn(true);
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(waitingTime);

        if(transform.position != origin) {SetAction("return");}
        else{SetAction("guard");}

        SetWaitCoOn(false);
    }
}
