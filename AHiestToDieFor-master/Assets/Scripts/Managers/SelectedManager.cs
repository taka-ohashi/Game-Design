using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SelectedManager : MonoBehaviour
{
    private GlobalEventManager gem;

    private List<Selected> selectedRobbers;

    private List<GameObject> robbers;

    //Sounds
    public AudioClip dying;
    private AudioSource deathAudio;

    public GameObject selectedRingPrefab;
    private GameObject selectedRing;

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
        robbers = new List<GameObject>();
        selectedRobbers = new List<Selected>();
    }
    private void Update()
    {

    }
    private void Start()
    {
        deathAudio = GetComponent<AudioSource>();
        gem.StartListening("NotifyLocationChanged", CheckIfCameraNeedsToUpdate);
        gem.StartListening("RightClick", MoveSelectedRobbers);
        gem.StartListening("Space", SwitchRobber);
        gem.StartListening("RobberEnteredSpawnArea", TrackRobber);
        gem.StartListening("Death", RemoveRobber);

        selectedRing = Instantiate(selectedRingPrefab,
                                   new Vector3(100, 0, 100),
                                   selectedRingPrefab.transform.rotation);
    }
    private void OnDestroy()
    {
        gem.StopListening("RightClick", MoveSelectedRobbers);
        gem.StopListening("Space", SwitchRobber);
        gem.StopListening("RobberEnteredSpawnArea", TrackRobber);
        gem.StopListening("Death", RemoveRobber);
    }

    private void TrackRobber(GameObject target, List<object> parameters)
    {
        if (robbers.Contains(target))
        {
            return;
        }
        robbers.Add(target);
    }
    private void RemoveRobber(GameObject target, List<object> parameters)
    {
        if (!robbers.Contains(target))
        {
            throw new Exception("Missing robber: Tried to remove robber that didn't exist");
        }
        deathAudio.PlayOneShot(dying, 0.5f);
        robbers.Remove(target);
        selectedRobbers = selectedRobbers.Where(sel => sel.go != target).ToList();

        ResetRing();
    }
    private void CheckIfCameraNeedsToUpdate(GameObject target, List<object> parameters)
    {
        if (selectedRobbers.Any(sel => sel.go == target))
        {
            gem.TriggerEvent("UpdateCamera", target);
            // notifies CameraManager
        }
    }
    private void MoveSelectedRobbers(GameObject target, List<object> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find target location of movement");
        }
        if (parameters[0].GetType() != typeof(Vector3))
        {
            throw new Exception("Illegal argument: parameter wrong type: " + parameters[0].GetType().ToString());
        }
        foreach(Selected robber in selectedRobbers)
        {
            gem.TriggerEvent("Move", robber.go, parameters);
            // notifies Movement component script
        }
    }
    private void SwitchRobber(GameObject target, List<object> parameters)
    {
        if (robbers.Count == 0)
        {
            throw new Exception("Missing robbers in list: Cannot switch between robbers when none exist");
        }
        if (selectedRobbers.Count == 0)
        {
            Select(new List<GameObject> { robbers[0] });
            return;
        }
        else if (selectedRobbers.Count > 1)
        {
            Select(new List<GameObject> { robbers[0] });
            return;
        }
        for (int i = 0; i < robbers.Count; i++)
        {
            if (robbers[i] == selectedRobbers[0].go)
            {
                Select(new List<GameObject> { robbers[(i + 1) % robbers.Count] });
                break;
            }
        }
    }
    private void Select(List<GameObject> robbers)
    {
        selectedRobbers = robbers
            .Select(robber => new Selected(robber, selectedRing))
            .ToList();

        if(selectedRobbers.Count == 0) {ResetRing();}
        foreach(Selected robber in selectedRobbers)
        {
            robber.ApplyHighlight();
        }
        if (selectedRobbers.Count != 0)
        {
            gem.TriggerEvent("UpdateCamera", robbers[0]);
            // notifies CameraManager
        }
    }

    private void ResetRing() {selectedRing.transform.parent = null; selectedRing.transform.position = new Vector3(100, 0, 100);}

    private class Selected
    {
        internal GameObject go;
        Color original;

        public GameObject selectedRing;
        public Selected(GameObject go, GameObject selectedRing)
        {
            this.go = go;
            this.selectedRing = selectedRing;
        }

        public void ApplyHighlight()
        {
            selectedRing.transform.SetParent(go.transform);
            selectedRing.transform.position = go.transform.position;
        }
    }

}
