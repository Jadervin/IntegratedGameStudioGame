using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EXBBattleVictoryScript : MonoBehaviour
{
    [Header("Scene Names")]
    public string continueSceneName;

    [Header("Sound Sources")]
    public AudioSource soundSource;
    public AudioClip menuClick;
    public float clickTimer = 0.5f;

    [Header("Scenes")]
    //public string nextSceneName;
    public string Scene4Name;
    public string Scene6Name;
    public string Scene10Name;
    public string Scene12Name;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
