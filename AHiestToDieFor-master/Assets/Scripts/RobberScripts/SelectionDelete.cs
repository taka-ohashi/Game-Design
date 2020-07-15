using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionDelete : MonoBehaviour
{
    public float time = .5f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroySelf", time);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DestroySelf() {Destroy(gameObject);}
}
