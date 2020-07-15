using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class InputManager : MonoBehaviour
{
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
    }
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            gem.TriggerEvent("Space", gameObject);
            // triggers event in SelectedManager
        }
        if (Input.GetMouseButtonDown(0))
        {
            //getting mouse location in space
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            List<GameObject> robbers = Physics.RaycastAll(ray)
                .Where(hit => hit.transform.CompareTag("Player"))
                .Select(hit => hit.transform.gameObject)
                .ToList();
            gem.TriggerEvent("LeftClick", gameObject, new List<object> { robbers });
            // triggers event in SelectedManager
        }
        if (Input.GetMouseButtonDown(1))
        {
            //getting mouse location in space
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if found a spot, move player there
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 location = hit.point;
                gem.TriggerEvent("RightClick", gameObject, new List<object> { location });
                // triggers event in SelectedManager
            }
        }
    }
}
