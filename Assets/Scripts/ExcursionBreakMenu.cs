using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExcursionBreakMenu : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject CherryBlossomButton;
    public GameObject SpiderLilyButton;
    public GameObject HollyButton;
    public GameObject BelladonnaButton;
    public GameObject IvyButton;
    public GameObject XzciarButton;
    public GameObject GaledricButton;
    public GameObject AetherButton;


    [Header("Scene Names")]
    public string CherryBlossom1SceneName;
    public string SpiderLily1SceneName;
    public string Holly1SceneName;
    public string Belladonna1SceneName;
    public string Ivy1SceneName;
    public string Xzciar1SceneName;
    public string Galedric1SceneName;
    public string Aether1SceneName;


    public string CherryBlossom2SceneName;
    public string SpiderLily2SceneName;
    public string Holly2SceneName;
    public string Belladonna2SceneName;
    public string Xzciar2SceneName;
    public string Galedric2SceneName;
    public string Aether2SceneName;

    public string BattleSceneName;


    //[Header("Integers")]
    public static int CherryBlossomExcursionChoice = 0;
    public static int SpiderLilyExcursionChoice = 0;
    public static int HollyExcursionChoice = 0;
    public static int BelladonnaExcursionChoice = 0;
    public static int IvyExcursionChoice = 0;
    public static int XzciarExcursionChoice = 0;
    public static int GaledricExcursionChoice = 0;
    public static int AetherExcursionChoice = 0;

    [Header("Sound Sources")]
    public AudioSource soundSource;
    public AudioClip menuClick;
    public float clickTimer = 0.5f;

    [Header("Saving")]
    public GameData saveData;



    void Start()
    {
        SaveStateManager.instance.SaveGame(saveData);
    }

        private void Awake()
    {
        if (ExcursionBreaksTaken.EXBNum < 1)
        {
            XzciarButton.SetActive(false);
            GaledricButton.SetActive(false);
            AetherButton.SetActive(false);
        }
        else
        {
            XzciarButton.SetActive(true);
            GaledricButton.SetActive(true);
            AetherButton.SetActive(true);

            if (CherryBlossomExcursionChoice > 1)
            {
                CherryBlossomButton.SetActive(false);
            }

            if (SpiderLilyExcursionChoice > 1)
            {
                CherryBlossomButton.SetActive(false);
            }

            if (IvyExcursionChoice > 0)
            {
                CherryBlossomButton.SetActive(false);
            }


            if (HollyExcursionChoice > 1)
            {
                CherryBlossomButton.SetActive(false);
            }


            if (BelladonnaExcursionChoice > 1)
            {
                CherryBlossomButton.SetActive(false);
            }

            if (XzciarExcursionChoice > 1)
            {
                CherryBlossomButton.SetActive(false);
            }

            if (GaledricExcursionChoice > 1)
            {
                CherryBlossomButton.SetActive(false);
            }

            if (AetherExcursionChoice > 1)
            {
                CherryBlossomButton.SetActive(false);
            }

        }
        
    }


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

        if (CherryBlossomExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(CherryBlossom1SceneName);
        }
        else
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(CherryBlossom2SceneName);
        }
        
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

        if (SpiderLilyExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(SpiderLily1SceneName);
        }
        else
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(SpiderLily2SceneName);
        }
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

        if (HollyExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Holly1SceneName);
        }
        else
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Holly2SceneName);
        }
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


        if (BelladonnaExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Belladonna1SceneName);
        }
        else
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Belladonna2SceneName);
        }
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


        if (CherryBlossomExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Ivy1SceneName);
        }
        
    }



    public void XzciarButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforXzciarButton(clickTimer));


    }


    IEnumerator WaitforXzciarButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait

        if (XzciarExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Xzciar1SceneName);
        }
        else
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Xzciar2SceneName);
        }
    }


    public void GaledricButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforGaledricButton(clickTimer));


    }


    IEnumerator WaitforGaledricButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait

        if (GaledricExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Galedric1SceneName);
        }
        else
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Galedric2SceneName);
        }
    }


    public void AetherButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforAetherButton(clickTimer));


    }


    IEnumerator WaitforAetherButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait

        if (CherryBlossomExcursionChoice == 0)
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Aether1SceneName);
        }
        else
        {
            CherryBlossomExcursionChoice++;
            SceneManager.LoadScene(Aether2SceneName);
        }
    }

    public void BattleButtonPressed()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        soundSource.PlayOneShot(menuClick);
        StartCoroutine(WaitforBattleButton(clickTimer));


    }

    IEnumerator WaitforBattleButton(float duration)
    {

        yield return new WaitForSeconds(duration);   //Wait
        SceneManager.LoadScene(BattleSceneName);
       
    }

}
