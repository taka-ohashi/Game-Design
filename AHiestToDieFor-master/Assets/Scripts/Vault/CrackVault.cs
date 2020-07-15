using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CrackVault : MonoBehaviour
{
    public GameObject loadingPrefab;
    public float loadSpeed = .1f;
    public float safeHeight = 1f;
    public float openSpeed = .05f;
    private Image unloaded;
    private Image loaded;
    private bool isCracking = false;
    public List<GameObject> nearbyPlayers;
    private string status = "closed";
    private float loading = 0f;
    private float unlockCounter = 0;
    private float openCounter = 0;
    private Quaternion openRotation;

    private float finalRotation;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private GlobalEventManager gem;

    //Sounds
    public AudioClip unlockVault;
    public AudioClip openVault;
    private AudioSource vaultAudio;

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
    }
    // Start is called before the first frame update
    void Start()
    {
        vaultAudio = GetComponent<AudioSource>();
        gem.StartListening("BeginUnlocking", BeginCracking);
        gem.StartListening("StopUnlocking", StopCracking);
    }
    public void OnDestroy()
    {
        gem.StopListening("BeginUnlocking", BeginCracking);
        gem.StopListening("StopUnlocking", StopCracking);
    }

    // Update is called once per frame
    void Update()
    {
        //tracks position of UI element
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

    private void Counter()
    {
        if(isCracking)
            {
                if(loading < 1)
                {
                    loading = loading + (loadSpeed * Time.deltaTime);
                    loaded.fillAmount = loading;
                }
                else
                {
                    loaded.fillAmount = 1;
                    status = "opening";
                    print("opened");
                }
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

    public void BeginCracking(GameObject invoker, List<object> parameters)
    {
        if (!nearbyPlayers.Contains(invoker))
        {
            return;
        }
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find unlocking speed parameter");
        }
        if (parameters[0].GetType() != typeof(float))
        {
            throw new Exception("Illegal argument: parameter wrong type");
        }
        if(unlockCounter == 0)
        {
            vaultAudio.PlayOneShot(unlockVault, 0.5f);
            unlockCounter++;
        }
        isCracking = true;
        loadSpeed = (float)parameters[0];
    }
    public void StopCracking(GameObject invoker, List<object> parameters)
    {
        if (!nearbyPlayers.Contains(invoker))
        {
            return;
        }
        vaultAudio.Stop();
        isCracking = false;
        unlockCounter = 0;
    }

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player" && unloaded == null)
        {
            nearbyPlayers.Add(other.gameObject);
            navMeshAgent = other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            unloaded = Instantiate(loadingPrefab, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
            loaded = new List<Image>(unloaded.GetComponentsInChildren<Image>()).Find(img => img != unloaded);
        }
        else if(other.gameObject.tag == "Player")
        {
            navMeshAgent = other.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            nearbyPlayers.Add(other.gameObject);
        }
    }

    public void OnCollisionExit(Collision other)
    {
        nearbyPlayers.Remove(other.gameObject);
        navMeshAgent = null;
        isCracking = false;
    }
}
