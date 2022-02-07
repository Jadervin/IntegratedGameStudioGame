using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;



public class CharacterNameInput : MonoBehaviour
{
    public TMP_InputField field;
    public string nextSceneName;

    public void ReadUserInput()
    {
        CharacterNameScript.characterName = field.text;

        StartCoroutine(WaitforSound());
        SceneManager.LoadScene(nextSceneName);
    }


    IEnumerator WaitforSound()
    {
        yield return new WaitForSeconds(2f);
    }
}
