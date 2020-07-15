using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewScene : MonoBehaviour
{
    private Animator fading;
    // Start is called before the first frame update
    void Start()
    {
        fading = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel()
    {
        fading.SetBool("Fade", true);
        StartCoroutine("LoadNextLevel");
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(3);
        if(StaticMoney.GetLastScene() <= 4)
        {
            SceneManager.LoadSceneAsync(StaticMoney.GetLastScene());
        }
        else
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

}
