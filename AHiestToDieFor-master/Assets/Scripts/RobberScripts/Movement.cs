using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System;

public class Movement : MonoBehaviour
{
    private GlobalEventManager gem;
    private NavMeshAgent agent;
    private Animator animator;
    public float speed;
    public GameObject ring;

    private void Awake()
    {
        List<MonoBehaviour> deps = new List<MonoBehaviour>
        {
            (gem = FindObjectOfType(typeof(GlobalEventManager)) as GlobalEventManager),
        };
        if (deps.Contains(null))
        {
            throw new Exception("Could not find dependency");
        }

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //normalize vector
        animator.SetFloat("velocity", agent.velocity.magnitude * agent.speed);
    }

    private void Start()
    {
        gem.StartListening("Move", Move);
        gem.StartListening("Death", PreventFurtherMovement);
        animator = GetComponent<Animator>();
    }
    private void OnDestroy()
    {
        gem.StopListening("Move", Move);
        gem.StopListening("Death", PreventFurtherMovement);

    }
    private void PreventFurtherMovement(GameObject target, List<object> parameters)
    {
        agent.SetDestination(transform.position);

        if (target != gameObject)
        {
            return;
        }
        Destroy(agent);
        Destroy(this);
    }
    private void Move(GameObject target, List<object> parameters)
    {
        if (target != gameObject)
        {
            return;
        }
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find target location of movement");
        }
        if (parameters[0].GetType() != typeof(Vector3))
        {
            throw new Exception("Illegal argument: parameter wrong type");
        }

        Vector3 location = (Vector3) parameters[0];// as Vector3;

        Instantiate(ring, location, ring.transform.rotation);
        
        agent.SetDestination(location);
        agent.speed = speed;
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "Vault" )
        {
            if(animator.GetFloat("velocity") < .1)
            {
                animator.SetBool("isCracking", true);
            }
            else
            {
                animator.SetBool("isCracking", false);
            }
        }
    }

    void OnCollisionExit(Collision other)
    {
        animator.SetBool("isCracking", false);
    }
}
