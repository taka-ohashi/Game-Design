using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    private bool isAlive = true;
    private bool selected = true;

    public int health = 1;
    public float speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive && selected)
        {
            MovePlayer();
        }
        else if(!isAlive)
        {
            //play death animation
            //create collider that blocks path
            //affect money (?)
        }
    }

    private void MovePlayer()
    {
        //This function moves a robber if they're alive and if the mouse has been clicked
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //getting mouse location in space
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if found a spot, move player there
            if (Physics.Raycast(ray, out hit))
            {
            Vector3 location = hit.point;
            agent.SetDestination(location);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        //This tests to see if shot by a bullet
        health --;
        if(health < 1)
        {
            agent.SetDestination(transform.position);
            isAlive = false;
        }

    }

    
}
