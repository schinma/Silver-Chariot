using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playlist : MonoBehaviour
{
  public AudioClip[] waveFiles;
  private AudioSource thisAudioSource;

  public float volMin;
  public float volMax;

  public float pitchMin;
  public float pitchMax;
  private int nbSong;

  public bool playOnAwake;

  void Awake () {
    thisAudioSource = GetComponent<AudioSource>();
    nbSong = Random.Range( 0, waveFiles.Length );

    if ( playOnAwake )
    {
      Play();
    }

  }

  void Start (){
  }

  // Update is called once per frame
  void Update () {

  }

  public void Play() {
    if ( thisAudioSource != null && thisAudioSource.isActiveAndEnabled )
    {

      // play the sound
      thisAudioSource.PlayOneShot( waveFiles[nbSong] );
      nbSong++;
      if (nbSong >= waveFiles.Length)
        nbSong = 0;
    }
  }

  public void Stop() {
    thisAudioSource.Stop();
  }

  public void Pause() {
    thisAudioSource.Pause();
  }

  public void Unpause() {
    thisAudioSource.UnPause();
  }
}
