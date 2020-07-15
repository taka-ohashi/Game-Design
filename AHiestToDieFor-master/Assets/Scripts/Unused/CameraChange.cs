using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    public GameObject thisCamera;
    public GameObject otherCamera;

    public bool entered = false;
    AudioListener thisCameraLis;
    AudioListener otherCameraLis;

    // Start is called before the first frame update
    void Start()
    {
        //Get Camera Audio Listeners
        thisCameraLis = thisCamera.GetComponent<AudioListener>();
        otherCameraLis = otherCamera.GetComponent<AudioListener>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //After robber leaves trigger, turn off other camera and switch to this camera
    private void OnTriggerEnter(Collider other)
    {
        thisCamera.SetActive(true);
        thisCameraLis.enabled = true;

        otherCameraLis.enabled = false;
        otherCamera.SetActive(false);
    }

}
