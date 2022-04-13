using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{

    public float textSpeedPrefs;

    [Header("Sound Sources")]
    public AudioSource soundSource;
    public AudioClip menuClick;
    public float clickTimer = 0.5f;
   

    public void LowTextPressed(bool isLow)
    {
        if (isLow == true)
        {
            soundSource.PlayOneShot(menuClick);
            PlayerPrefs.SetFloat("TextSpeed", textSpeedPrefs = 0.04f);
        }
    }


    public void MediumTextPressed(bool isMedium)
    {
        if (isMedium == true)
        {
            soundSource.PlayOneShot(menuClick);
            PlayerPrefs.SetFloat("TextSpeed", textSpeedPrefs = 0.02f);
        }
    }


    public void HighTextPressed(bool isHigh)
    {
        if (isHigh == true)
        {
            soundSource.PlayOneShot(menuClick);
            PlayerPrefs.SetFloat("TextSpeed", textSpeedPrefs = 0.01f);

        }
    }


}
