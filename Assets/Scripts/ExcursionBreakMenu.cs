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
    public GameObject CombatButton;


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


    [Header("Tags")]
    public static string currentTag;



    void Start()
    {


        SaveStateManager.instance.SaveGame(saveData);

        if (ExcursionBreaksTaken.EXBNum % 2 == 1) 
        {
            if (currentTag == "Inquiries")
            {
                
                CherryBlossomButton.SetActive(false);
                SpiderLilyButton.SetActive(false);
                HollyButton.SetActive(false);
                BelladonnaButton.SetActive(false);
                IvyButton.SetActive(false);
            }
            else if(currentTag == "Hangouts")
            {
                XzciarButton.SetActive(false);
                GaledricButton.SetActive(false);
                AetherButton.SetActive(false);
                
            }
            else
            {
                CombatButton.SetActive(false);
            }
        }
        else
        {
            currentTag = "";
        }
    }

    private void Awake()
    {



        if (ExcursionBreaksTaken.EXBNum < 2)
        {
            XzciarButton.SetActive(false);
            GaledricButton.SetActive(false);
            AetherButton.SetActive(false);
        }
        else if(ExcursionBreaksTaken.EXBNum >= 2 && ExcursionBreaksTaken.EXBNum < 4)
        {
            GaledricButton.SetActive(true);
            XzciarButton.SetActive(false);
            AetherButton.SetActive(false);
        }
        else
        {
            XzciarButton.SetActive(true);
            GaledricButton.SetActive(true);
            AetherButton.SetActive(true);

        }


        if (CherryBlossomExcursionChoice > 1)
        {
            CherryBlossomButton.SetActive(false);
        }

        if (SpiderLilyExcursionChoice > 1)
        {
            SpiderLilyButton.SetActive(false);
        }

        if (IvyExcursionChoice > 0)
        {
            IvyButton.SetActive(false);
        }


        if (HollyExcursionChoice > 1)
        {
            HollyButton.SetActive(false);
        }


        if (BelladonnaExcursionChoice > 1)
        {
            BelladonnaButton.SetActive(false);
        }

        if (XzciarExcursionChoice > 1)
        {
            XzciarButton.SetActive(false);
        }

        if (GaledricExcursionChoice > 1)
        {
            GaledricButton.SetActive(false);
        }

        if (AetherExcursionChoice > 1)
        {
            AetherButton.SetActive(false);
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

        currentTag = CherryBlossomButton.tag;


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

        currentTag = SpiderLilyButton.tag;

        if (SpiderLilyExcursionChoice == 0)
        {
            SpiderLilyExcursionChoice++;
            SceneManager.LoadScene(SpiderLily1SceneName);
        }
        else
        {
            SpiderLilyExcursionChoice++;
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


        currentTag = HollyButton.tag;


        if (HollyExcursionChoice == 0)
        {
            HollyExcursionChoice++;
            SceneManager.LoadScene(Holly1SceneName);
        }
        else
        {
            HollyExcursionChoice++;
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

        currentTag = BelladonnaButton.tag;



        if (BelladonnaExcursionChoice == 0)
        {
            BelladonnaExcursionChoice++;
            SceneManager.LoadScene(Belladonna1SceneName);
        }
        else
        {
            BelladonnaExcursionChoice++;
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

        currentTag = IvyButton.tag;

        if (IvyExcursionChoice == 0)
        {
            IvyExcursionChoice++;
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

        currentTag = XzciarButton.tag;


        if (XzciarExcursionChoice == 0)
        {
            XzciarExcursionChoice++;
            SceneManager.LoadScene(Xzciar1SceneName);
        }
        else
        {
            XzciarExcursionChoice++;
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

        currentTag = GaledricButton.tag;


        if (GaledricExcursionChoice == 0)
        {
            GaledricExcursionChoice++;
            SceneManager.LoadScene(Galedric1SceneName);
        }
        else
        {
            GaledricExcursionChoice++;
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

        currentTag = AetherButton.tag;



        if (AetherExcursionChoice == 0)
        {
            AetherExcursionChoice++;
            SceneManager.LoadScene(Aether1SceneName);
        }
        else
        {
            AetherExcursionChoice++;
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

        currentTag = CombatButton.tag;

        SceneManager.LoadScene(BattleSceneName);
       
    }

}
