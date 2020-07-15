using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : MonoBehaviour
{
    private GlobalEventManager gem;

    private Dictionary<GameObject, Camera> roomToCameraMap;
    private Dictionary<GameObject, GameObject> robberToRoomMap;

    public List<GameObject> rooms;
    public List<Camera> cameras;

    public float speed;
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
        if (rooms.Count != cameras.Count)
        {
            throw new Exception("Invalid state: The number of rooms is not equal to the number of cameras");
        }
        if (rooms.Count == 0)
        {
            throw new Exception("Invalid state: There must at least exist 1 room");
        }
        roomToCameraMap = new Dictionary<GameObject, Camera>();
        robberToRoomMap = new Dictionary<GameObject, GameObject>();
        for (int i = 0; i < rooms.Count; i++)
        {
            roomToCameraMap[rooms[i]] = cameras[i];
        }
    }
    private void Start()
    {
        gem.StartListening("UpdateCamera", UpdateCamera);
        gem.StartListening("RobberEnteredRoom", UpdateRobberLocation);
    }
    private void OnDestroy()
    {
        gem.StopListening("UpdateCamera", UpdateCamera);
        gem.StopListening("RobberEnteredRoom", UpdateRobberLocation);
    }

    private Camera GetActiveCamera()
    {
        return Camera.main;
    }

    private Vector3 previousPos;
    private Camera current;
    private Camera target;
    private Coroutine transitioning;
    public void UpdateCamera(GameObject robber, List<object> parameters)
    {
        if (robber == null)
        {
            throw new Exception("Invalid parameter: Cannot update camera with null");
        }
        GameObject room = robberToRoomMap[robber];
        if (room == null)
        {
            throw new Exception("Invalid parameter: Could not find the room attached to the robber");
        }
        current = GetActiveCamera();
        previousPos = current.transform.position;
        target = roomToCameraMap[room];

        if (transitioning != null)
        {
            StopCoroutine(transitioning);
        }
        transitioning = StartCoroutine(MoveToTarget());

        //foreach (Camera camera in cameras)
        //{
        //    camera.enabled = false;
        //}
        //roomToCameraMap[room].enabled = true;
    }
    private IEnumerator MoveToTarget()
    {
        float interp = 0;
        while (Vector3.Distance(current.transform.position, target.transform.position) > 0.1)
        {
            yield return new WaitForEndOfFrame();
            interp += speed * Time.deltaTime;
            current.transform.position = Vector3.Slerp(current.transform.position, target.transform.position, interp);
        }
    }
    public void UpdateRobberLocation(GameObject target, List<object> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find robber game object");
        }
        if (parameters[0].GetType() != typeof(GameObject))
        {
            throw new Exception("Illegal argument: parameter wrong type");
        }
        GameObject robber = (GameObject)parameters[0];
        robberToRoomMap[robber] = target;
        gem.TriggerEvent("NotifyLocationChanged", robber);
        // notifies SelectedManager
    }
}
