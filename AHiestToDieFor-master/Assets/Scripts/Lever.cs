using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject door;

    public float doorDistanceX = 0;
    public float doorDistanceY = 0;
    public float doorDistanceZ = 0;

    private Vector3 moveDoor;
    private bool doorIsOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //opens door and changes doorIsOpen boolean to true
    private void openDoor()
    {
        moveDoor = new Vector3(-doorDistanceX, -doorDistanceY, -doorDistanceZ);
        door.transform.Translate(moveDoor);
        doorIsOpen = true;
    }

    //closes door and changes doorIsOpen boolean to false
    private void closeDoor()
    {
        moveDoor = new Vector3(doorDistanceX, doorDistanceY, doorDistanceZ);
        door.transform.Translate(moveDoor);
        doorIsOpen = false;
    }

    //door changes when player stands next to lever and presses z to interact
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown("z"))
        {
            if (doorIsOpen)
            {
                closeDoor();
            }
            else
            {
                openDoor();
            }
        }
    }

}
