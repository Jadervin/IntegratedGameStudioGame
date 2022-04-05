using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("Scene Names")]
    public string StartSceneName;
    public string CreditsSceneName;
    public string TitleMenuSceneName;
    public string ReferencesSceneName;
    public string PreviousSceneName;
    public string continueSceneName;

    [Header("Sound Sources")]
    public AudioSource soundSource;
    public AudioClip menuClick;
    public float clickTimer = 0.5f;

    public GameData saveData;


    public void StartButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforStartButton(clickTimer));

        
    }
    
    public void CreditsButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforCreditsButton(clickTimer));
       



    }

    public void TitleMenuButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforTitleMenuButton(clickTimer));




    }

    public void ReferencesButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforReferencesButton(clickTimer));




    }

    public void RestartButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforRestartButton(clickTimer));
        



    }

    public void CloseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforCloseButton(clickTimer));



    }

    public void ContinueButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforContinueButton(clickTimer));




    }

    IEnumerator WaitforStartButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait

        //reset save data

        SceneManager.LoadScene(StartSceneName);
    }
    IEnumerator WaitforTitleMenuButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(TitleMenuSceneName);
    }

    IEnumerator WaitforCreditsButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(CreditsSceneName);
    }

    IEnumerator WaitforCloseButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        Application.Quit();

    }

    IEnumerator WaitforReferencesButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(ReferencesSceneName);
    }

    IEnumerator WaitforRestartButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(PreviousSceneName);
    }

    IEnumerator WaitforContinueButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(continueSceneName);
    }

}
