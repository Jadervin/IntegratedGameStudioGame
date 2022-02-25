using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExcursionBreakMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string CherryBlossomSceneName;
    public string SpiderLilySceneName;
    public string HollySceneName;
    public string BelladonnaSceneName;
    public string IvySceneName;

    [Header("Sound Sources")]
    public AudioSource soundSource;
    public AudioClip menuClick;
    public float clickTimer = 0.5f;


    public void CherryBlossomButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforCherryBlossomButton(clickTimer));


    }


    IEnumerator WaitforCherryBlossomButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(CherryBlossomSceneName);
    }


    public void SpiderLilyButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforSpiderLilyButton(clickTimer));


    }


    IEnumerator WaitforSpiderLilyButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(SpiderLilySceneName);
    }


    public void HollyButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforHollyButton(clickTimer));


    }


    IEnumerator WaitforHollyButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(HollySceneName);
    }


    public void BelladonnaButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforBelladonnaButton(clickTimer));


    }


    IEnumerator WaitforBelladonnaButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(BelladonnaSceneName);
    }

    public void IvyButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforIvyButton(clickTimer));


    }


    IEnumerator WaitforIvyButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(IvySceneName);
    }


}
