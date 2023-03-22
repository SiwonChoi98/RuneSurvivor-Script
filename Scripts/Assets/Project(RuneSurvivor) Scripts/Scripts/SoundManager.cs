using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource _audioSource;

    public AudioClip fireLevelHitSound;
    public AudioClip waterLevel1HitSound;
    public AudioClip lightningLevel1HitSound;
    public AudioClip rockLevel1HitSound;
    public AudioClip coinSound;
    public AudioClip storeSound;
    public AudioClip lightninglightningLevel3HitSound;
    public AudioClip buttonClickSound;
    public AudioSource backGroundSound;
    public AudioSource deathPanelSound;
    public AudioClip healSound;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void FireLevelHitSound()
    {
        _audioSource.PlayOneShot(fireLevelHitSound);
    }
    public void WaterLevel1HitSound()
    {
        _audioSource.PlayOneShot(waterLevel1HitSound);
    }
    public void LightningLevel1HitSound()
    {
        _audioSource.PlayOneShot(lightningLevel1HitSound);
    }
    public void RockLevel1HitSound()
    {
        _audioSource.PlayOneShot(rockLevel1HitSound);
    }
    public void CoinSound()
    {
        _audioSource.PlayOneShot(coinSound);
    }
    public void StoreSound()
    {
        _audioSource.PlayOneShot(storeSound);
    }
    public void LightninglightningLevel3HitSound()
    {
        _audioSource.PlayOneShot(lightninglightningLevel3HitSound);
    }
    public void ButtonClickSound()
    {
        _audioSource.PlayOneShot(buttonClickSound);
    }
    public void HealSound()
    {
        _audioSource.PlayOneShot(healSound);
    }
}
