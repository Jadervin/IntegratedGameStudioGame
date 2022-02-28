using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource audioSource;

    public void Pause()
    {
        audioSource.Pause();
    }




    public void UnPause()
    {

        audioSource.UnPause();

    }
}
