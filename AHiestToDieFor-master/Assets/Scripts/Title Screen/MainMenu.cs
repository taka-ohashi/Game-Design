using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public int creditsTime;
    public GameObject mainMenuItems;
    public GameObject howToPlayItems;
    public GameObject creditItems;
    public GameObject rules;
    public GameObject ControlsAndGoals;
    public Camera mainCamera;
    private Animator camAnim;

    private Vector3 mainMenuCamPos = new Vector3();
    private Vector3 creditsCamPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        camAnim = mainCamera.GetComponent<Animator>();
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(1);

        SceneManager.LoadSceneAsync(2);
    }

    public void HowToPlay()
    {
        //hides menu and displays "how to play"
        mainMenuItems.SetActive(false);
        howToPlayItems.SetActive(true);
    }

    public void BackToMenu()
    {
        //displays the main menu and hides both the credits and the "how to play"
        camAnim.SetBool("MainMenu", true);

        howToPlayItems.SetActive(false);
        mainMenuItems.SetActive(true);

        creditItems.SetActive(false);
        StopCoroutine("CreditsRoll");

        

        rules.SetActive(true);
        ControlsAndGoals.SetActive(false);
    }

    public void Credits()
    {
        //hides the menu and displays the credits
        camAnim.SetBool("MainMenu", false);

        mainMenuItems.SetActive(false);
        creditItems.SetActive(true);

        StartCoroutine("CreditsRoll");
    }

    public void Continue()
    {
        //hides the "how to play" and shows the "movement" and "goals"
        rules.SetActive(false);
        ControlsAndGoals.SetActive(true);
    }

    IEnumerator CreditsRoll()
    {
        yield return new WaitForSeconds(creditsTime);
        BackToMenu();
    }
}
