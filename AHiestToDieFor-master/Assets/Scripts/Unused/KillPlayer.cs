using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject deadBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("touched");
        if(other.gameObject.tag == "Player")
        {
            Instantiate(deadBody, other.gameObject.transform.position, other.gameObject.transform.rotation);
            other.gameObject.transform.position = new Vector3(0, 1, 0);
        }
    }
}
