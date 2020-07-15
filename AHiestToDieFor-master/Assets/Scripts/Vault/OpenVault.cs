using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenVault : MonoBehaviour
{
    public GameObject loadingPrefab;
    public float loadSpeed = .1f;
    public float safeHeight = 1f;
    public float openSpeed = .05f;
    private Image unloaded;
    private Image loaded;
    private bool isCracking = false;
    private string status = "closed";
    private float loading = 0f;
    private float openCounter = 0;

    private float finalRotation;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Unlocking playerUnlocking;

    private GlobalEventManager gem;

    //Sounds
    public AudioClip unlockVault;
    public AudioClip openVault;
    private AudioSource vaultAudio;

    // Start is called before the first frame update
    void Start()
    {
        vaultAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(unloaded) {unloaded.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, safeHeight, 0));}

        switch(status)
        {
            case "closed":
            {
                Counter();
                break;
            }
            case "opening":
            {
                Open();
                break;
            }
            case "opened":
            {
                Destroy(gameObject);
                break;
            }
            default:
            {
                status = "closed";
                break;
            }
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        // if other is player             if havent started    if another isnt already
        if(other.gameObject.tag == "Player" && unloaded == null && !isCracking)
        {
            playerUnlocking = other.gameObject.GetComponent<Unlocking>();
            navMeshAgent = other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            unloaded = Instantiate(loadingPrefab, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
            loaded = new List<Image>(unloaded.GetComponentsInChildren<Image>()).Find(img => img != unloaded);
        }
        else if(other.gameObject.tag == "Player" && !isCracking)
        {
            playerUnlocking = other.gameObject.GetComponent<Unlocking>();
            navMeshAgent = other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
    }

    public void OnCollisionStay(Collision other)
    {
        if(navMeshAgent)
        {
            if(navMeshAgent.velocity.magnitude < .1f)
            {
                isCracking = true;
            }
            else
            {
                isCracking = false;
            }
        }
    }

    public void OnCollisionExit(Collision other)
    {
        navMeshAgent = null;
        isCracking = false;
        playerUnlocking = null;
    }

    private void Counter()
    {
        if(isCracking && playerUnlocking)
        {
            //if it isn't loaded, open the vault at the robber's unlocking speed
            if(loading < 1)
            {
                loading += (playerUnlocking.GetUnlockingSpeed() * Time.deltaTime);
                loaded.fillAmount = loading;
            }
            else
            {
                loaded.fillAmount = 1;
                status = "opening";
                print("opened");
            }
        }
        else
        {
            print(isCracking + " " + (playerUnlocking != null));
        }
    }

    private void Open()
    {
        if(openCounter == 0)
        {
            vaultAudio.PlayOneShot(openVault, 0.5f);
            openCounter++;
        }
        if(transform.position.y  > -2.1)
        {   
            transform.Translate(0, -openSpeed * Time.deltaTime, 0);
        }
        else if(unloaded)
        {
            Destroy(unloaded.gameObject);
            status = "opened";
        }
    }
}
