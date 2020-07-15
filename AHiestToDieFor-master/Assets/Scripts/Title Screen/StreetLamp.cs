using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    public float speed;
    public float boundary;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x > boundary)
        {
            transform.position = new Vector3(-boundary,
                                             transform.position.y,
                                             transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime),
                                             transform.position.y,
                                             transform.position.z);
        }
    }
}
