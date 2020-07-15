using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    //tells update to slerp doors
    private bool isClosing = false;
    private bool isOpening = false;

    //if num of plates pressed = 2, then dont close
    private float numPlatesPressed = 0;

    //determine new position from old
    public float doorDistanceX = 0;
    public float doorDistanceY = 0;
    public float doorDistanceZ = 0;

    //handles smooth slerping
    private float timer;
    public float translationTime = 3f;

    private Vector3 newPos;
    private Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        oldPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        newPos = new Vector3(transform.position.x + doorDistanceX,
            transform.position.y + doorDistanceY,
            transform.position.z + doorDistanceZ);
    }

    private void Update()
    {
        if (isOpening && timer < translationTime && numPlatesPressed == 1)
        {
            timer += Time.deltaTime;
            this.gameObject.transform.position = Vector3.Lerp(transform.position, newPos, timer / translationTime);
        }
        else if (isOpening)
        {
            isOpening = false;
            timer = 0;
        }

        if (isClosing && timer < translationTime && numPlatesPressed == 0)
        {
            timer += Time.deltaTime;
            this.gameObject.transform.position = Vector3.Lerp(transform.position, oldPos, timer / translationTime);
        }
        else if (isClosing)
        {
            isClosing = false;
            timer = 0;
        }

    }

    //Wont open if door is already opened
    public void openDoor()
    {
        isOpening = true;
        numPlatesPressed++;
    }

    //wont close if door is not opened
    public void closeDoor()
    {
        isClosing = true;
        numPlatesPressed--;
    }
}
