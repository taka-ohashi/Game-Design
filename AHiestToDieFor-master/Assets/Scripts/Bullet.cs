using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    private GlobalEventManager gem;

    public float speed = 20f;
    private bool collided = false;

    //components
    private Rigidbody rb;
    private MeshRenderer mr;

    private float startTime;
    public float lifeTime = 1f;
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
        startTime = Time.time;
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > lifeTime)
        {
            collided = true;
        }

        if(collided)
        {
            mr.enabled = false;
            transform.Translate(Vector3.zero);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            collided = true;
            gem.TriggerEvent("Shot", other.gameObject);
        }
        else
        {
            collided = true;
        }
    }

    void FixedUpdate()
    {
        if(!collided)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    }
}
