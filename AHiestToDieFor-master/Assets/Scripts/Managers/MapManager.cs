using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapManager : MonoBehaviour
{
    private Camera mainCamera;
    private Camera mapCamera;

    private GlobalEventManager gem;
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
        GameObject maincam = GameObject.Find("MainCamera");
        GameObject mapcam = GameObject.Find("MapCamera");
        if (maincam == null || mapcam == null)
        {
            throw new Exception("Missing GameObject(s) in scene: MainCamera or MapCamera missing");
        }
        this.mainCamera = maincam.GetComponent<Camera>();
        this.mapCamera = mapcam.GetComponent<Camera>();
    }
    private void Start()
    {
        gem.StartListening("StartGame", StartGame);
    }
    private void OnDestroy()
    {
        gem.StopListening("StartGame", StartGame);
    }
    private void StartGame(GameObject target, List<object> parameters)
    {
        gameObject.SetActive(false);
    }
    public void ShowMapCamera()
    {
        mainCamera.enabled = false;
        mapCamera.enabled = true;
    }
    public void HideMapCamera()
    {
        mainCamera.enabled = true;
        mapCamera.enabled = false;
    }
}
