using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public AudioClip uiConfirm;
    public AudioClip uiCancel;
    public Animator overlay;
    public Text victoryTime;

    private void Awake()
    {
        if (victoryTime != null) victoryTime.text = "Tempo: " + GameManager.currentTime;
    }
    public void StartGame()
    {
        overlay.SetInteger("state", 1);
        GetComponent<AudioSource>().PlayOneShot(uiConfirm);
        Invoke(nameof(LoadGameScene), 1.5f);
    }

    public void LoadGameScene()
    {
        overlay.SetInteger("state", 0);
        SceneManager.LoadScene("Game");

    }

    public void GoCredits()
    {
        GetComponent<Animator>().SetInteger("state", 2);
        GetComponent<AudioSource>().PlayOneShot(uiConfirm);
    }

    public void GoMain()
    {
        GetComponent<Animator>().SetInteger("state", 0);
        GetComponent<AudioSource>().PlayOneShot(uiCancel);
    }
    public void GoInstructions()
    {
        GetComponent<Animator>().SetInteger("state", 1);
        GetComponent<AudioSource>().PlayOneShot(uiConfirm);
    }

    public void Exit()
    {
        overlay.SetInteger("state", 1);
        GetComponent<AudioSource>().PlayOneShot(uiCancel);
        Invoke(nameof(ExitApplication), 1.5f);
    }

    public void ExitApplication()
    {
        overlay.SetInteger("state", 0);
        Application.Quit();

    }

    public void BackToMenu()
    {
        overlay.SetInteger("state", 1);
        GetComponent<AudioSource>().PlayOneShot(uiConfirm);
        Invoke(nameof(LoadMenuScene), 1.5f);
    }

    public void LoadMenuScene()
    {
        overlay.SetInteger("state", 0);
        SceneManager.LoadScene("Menu");

    }

}
