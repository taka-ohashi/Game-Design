using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image progressBar;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadAsyncOperation()
    {
        //Begins loading the level
        //assigns loading to a variable 
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);

        //While the game is unloaded:
        while (gameLevel.progress < 1)
        {
            //make the fill amount equal to the progress bar
            progressBar.fillAmount = gameLevel.progress;

            //wait till the next frame before continueing while loop
            yield return new WaitForEndOfFrame();
        }
    }
}
