using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource SelectSource;
    public AudioSource CollisionSource;
    public AudioSource SortBallBlast;
    public AudioSource LevelWinSound;
    public AudioSource CoinClaimSound;
    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySelectSound()
    {
        SelectSource.Play();
    }

    public void PlayCollisionSound()
    {
        CollisionSource.Play();
    }
    public void SortBallBlastSound()
    {
        SortBallBlast.Play();
    }
    public void PlayLevelWinSound()
    {
        LevelWinSound.Play();
    }
    public void PlayCoinClaimSound()
    {
        CoinClaimSound.Play();
    }
}
