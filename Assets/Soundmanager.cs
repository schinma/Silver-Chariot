using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmanager : MonoBehaviour
{
  public AudioClip[] _foodPick;
  public AudioClip[] _wallHit;
  private AudioSource thisAudioSource;

  public float volMin;
  public float volMax;

  public float pitchMin;
  public float pitchMax;

  public bool playOnAwake;

  void Awake () {
    thisAudioSource = GetComponent<AudioSource>();
  }

  public void playFoodPick() {
    thisAudioSource.PlayOneShot(_foodPick[Random.Range( 0, _foodPick.Length)]);
  }

  public void playWallHit() {
    thisAudioSource.PlayOneShot(_wallHit[Random.Range( 0, _wallHit.Length)]);
  }
}
