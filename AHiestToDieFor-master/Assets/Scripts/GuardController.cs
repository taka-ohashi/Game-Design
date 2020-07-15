using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[System.Serializable]
public class GuardController : MonoBehaviour
{
    private GlobalEventManager gem;
    //nav mesh stuff
    public NavMeshAgent agent;
    public Animator animator;

    //references
    public GameObject player;
    public GameObject viewPoint;

    //guard view info
    public float viewDistance = 5;
    public float fov = 60;
    private Vector3 lastPointSeen;

    //guard mode info
    private string action = "idle";

    //Gun stuff
    public GameObject bullet;
    public GameObject gunPoint;
    public float shootAngle = 3f;

    //coroutine stuff
    private bool waitCoOn = false;
    private Coroutine Co;
    public int waitingTime = 5;

    //sunrise
    private float duration = 300f;
    public float timePassed = 0f;
    public GameObject flashlight;

    private bool guessLocation = false;

    //Sounds
    public AudioClip sawPlayer;
    public AudioClip gunshots;
    private AudioSource guardAudio;

    private void Awake()
    {
        gem = FindObjectOfType(typeof(GlobalEventManager)) as GlobalEventManager;
    }
        // Start is called before the first frame update
    protected void Start()
    {
        guardAudio = GetComponent<AudioSource>();
        gem.StartListening("Death", CheckIfTargetIsDead);
        animator = GetComponent<Animator>();
    }
    protected void OnDestroy()
    {
        gem.StopListening("Death", CheckIfTargetIsDead);
    }

    public void ParentStart()
    {
        //get components
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(transform.position);
    } 

    // Update is called once per frame
    void Update()
    {
        GuardWakeUp();
    }

    public void CheckIfTargetIsDead(GameObject target, List<object> parameters)
    {
        if (player != target)
        {
            return;
        }
        player = null;
        SetAction("idle");
    }

    public void findPlayer()
    {
        player = null;
        //player = GameObject.FindWithTag("Player");

        //Create Raycast
        RaycastHit[] hits = Physics.SphereCastAll(viewPoint.transform.position, viewDistance, Vector3.forward);
        
        foreach(RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.CompareTag("Player"))
            {
                player = hit.transform.gameObject;
                break;
            }
        }
        //see if the raycast hits something
        //if the object is tagged the player
        if (player != null && Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fov / 2f)
        {

            //if the player is within viewing distance of the guard
            //if the player is in front of the guard
            //Create Raycast
            RaycastHit hit2;
            //Ray ray = new Ray(viewPoint.transform.position, player.transform.position);

            //see if the raycast hits something
            if (Physics.Raycast(viewPoint.transform.position, (player.transform.position - viewPoint.transform.position), out hit2) && hit2.transform.CompareTag("Player"))
            {
                guardAudio.PlayOneShot(sawPlayer, 0.5f);
                //if the object is tagged the player
                //replace with chase the player
                action = "attack";
            }
        }
    }

    public void Chase()
    {
        //stop whatever else guard was doing
        StopCoroutine("Reloading");
        StopCoroutine("idle");
        waitCoOn = false;

        //create raycast hit
        RaycastHit hit;

        //if can directly see player, attack case
        if(player &&
           Physics.Raycast(viewPoint.transform.position, (player.transform.position - viewPoint.transform.position), out hit) &&
           hit.transform.CompareTag("Player") &&
           Vector3.Distance(hit.transform.position, transform.position) < viewDistance)
        {
            guardAudio.PlayOneShot(sawPlayer, 0.5f);
            //switch to attack mode in update
            action = "attack";
        }
        //else, hunt down the last known location of the player
        else if(Vector3.Distance(lastPointSeen, transform.position) > .1)
        {
            if(!waitCoOn && guessLocation) {StartCoroutine("GuessPlayer");}

            agent.SetDestination(lastPointSeen);
        }
        //if can't find player at last location, go back to idling
        else
        {
            action = "idle";
            player = null;
            guessLocation = true;
        }
    }

    public void Attack()
    {
        //stop moving
        agent.SetDestination(transform.position);

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        // //see if facing the player
        // if(Quaternion.Angle(Quaternion.LookRotation(player.transform.position - transform.position), transform.rotation) > shootAngle &&
        //     distanceToPlayer > 5 &&
        //     distanceToPlayer < viewDistance + 5)
        // {
        //     //look towards player
        //     Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        //     float strength = Mathf.Min(10 * Time.deltaTime, 1);
        //     transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, strength);
        // }

        //attack player
        if(LookAtPlayer(distanceToPlayer))
        {
            //shoot at player
            if (!waitCoOn)
            {
                //call IEnumerator Reloading()
                Co = StartCoroutine("Reloading");
            }
        }
        //can't see player or facing player
        else
        {
            //chase last known location of player in update
            action = "chase";
        }

        //set the last known location of the player
        lastPointSeen = player.transform.position;
    }

    public bool LookAtPlayer(float distanceToPlayer)
    {
        Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        float strength = Mathf.Min(30 * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, strength);
        
        //print(Quaternion.LookRotation(player.transform.position - transform.position) + " " + transform.rotation + "  " + (Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up).y - transform.rotation.y));

        //print((distanceToPlayer < 5) + " " +
        //     (Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up).y - transform.rotation.y < shootAngle) + " " + 
        //     (distanceToPlayer < viewDistance + 5));

        return distanceToPlayer < 5 ||
               (Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up).y - transform.rotation.y < shootAngle &&
               distanceToPlayer < viewDistance + 5);
    }

    public void Idle()
    {
        //if not already waiting or reloading
        if (!waitCoOn)
        {
            Co = StartCoroutine("Waiting");
        }
    }

    IEnumerator Waiting()
    {
        //wait in place
        waitCoOn = true;
        agent.SetDestination(transform.position);
        yield return new WaitForSeconds(waitingTime);
        action = "guard";
        waitCoOn = false;
    }

    IEnumerator Reloading()
    {
        //wait to shoot bullets every half second
        waitCoOn = true;
        yield return new WaitForSeconds(.5f);
        Instantiate(bullet, gunPoint.transform.position, gunPoint.transform.rotation);
        guardAudio.PlayOneShot(gunshots, 0.5f);
        waitCoOn = false;
    }

    IEnumerator GuessPlayer()
    {
        waitCoOn = true;
        yield return new WaitForSeconds(2);
        if(player)
        {
            lastPointSeen = player.transform.position;
            action = "chase";
            print("guessed");
        }

        guessLocation = false;
        waitCoOn = false;
    }

    public void SetAction(string newAction)
    {
        //used in subclasses to set action for cases
        action = newAction;
    }

    //used in sub classes to get action string for cases
    public string GetAction() {return action;}

    //used in sub classes to turn on/off waiting/reloading
    public void SetWaitCoOn(bool value) {waitCoOn = value;}

    //used for increasing guard viewDistance in regards to sunrise
    public void GuardWakeUp()
    {
        if (timePassed < 1)
        {
            viewDistance = Mathf.Min(7.5f, viewDistance += ((2.5f * timePassed) / (40 * duration)));

            Vector3 lTemp = flashlight.transform.localScale;
            lTemp.z += ((750f * timePassed) / duration);
            flashlight.transform.localScale = lTemp;

            timePassed += Time.deltaTime / duration;


        }
    }
}
