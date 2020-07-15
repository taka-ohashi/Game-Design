using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedPlate : MonoBehaviour
{
    public float distanceDown = .5f;

    //keeps track if multiple robbers inside button collider
    private float numRobbersInside = 0;

    //We will call door scripts
    public GameObject door;
    public Door doorScript;

    //Sounds
    public AudioClip OnPlate;
    private AudioSource plateAudio;

    private Vector3 movePlate;
    // Start is called before the first frame update
    void Start()
    {
        plateAudio = GetComponent<AudioSource>();
        doorScript = door.GetComponent<Door>();
    }

    //Move plate back up upon leaving and close the door
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Bullet"))
        {
            if (numRobbersInside == 1)
            {
                movePlate = new Vector3(0, distanceDown, 0);
                doorScript.closeDoor();
                this.gameObject.transform.Translate(movePlate);
            }
            numRobbersInside--;
        }
    }

    //Move plate down upon entering and open the door
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Bullet"))
        {
            if (numRobbersInside == 0)
            {
                plateAudio.PlayOneShot(OnPlate, 0.5f);
                movePlate = new Vector3(0, -distanceDown, 0);
                doorScript.openDoor();
                this.gameObject.transform.Translate(movePlate);
            }
            numRobbersInside++;
        }
    }
}
