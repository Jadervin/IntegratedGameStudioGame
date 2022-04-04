using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EXBBattleVictoryScript : MonoBehaviour
{
    [Header("Scene Names")]
    public string Scene4Name;
    public string Scene6Name;
    public string Scene10Name;
    public string Scene12Name;

    [Header("Sound Sources")]
    public AudioSource soundSource;
    public AudioClip menuClick;
    public float clickTimer = 0.5f;

    


    public void ContinueButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforContinueButton(clickTimer));


    }

    IEnumerator WaitforContinueButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait

        if (ExcursionBreaksTaken.EXBNum == 0)
        {
            ExcursionBreaksTaken.EXBNum++;
            SceneManager.LoadScene(Scene4Name);
        }

        else if (ExcursionBreaksTaken.EXBNum == 1)
        {
            ExcursionBreaksTaken.EXBNum++;
            SceneManager.LoadScene(Scene6Name);
        }

        else if (ExcursionBreaksTaken.EXBNum == 2)
        {
            ExcursionBreaksTaken.EXBNum++;
            SceneManager.LoadScene(Scene10Name);
        }

        else
        {
            ExcursionBreaksTaken.EXBNum++;
            SceneManager.LoadScene(Scene10Name);
        }


    }



}
