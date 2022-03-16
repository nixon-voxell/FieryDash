using UnityEngine;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
  [SerializeField] private AudioMixer _masterMixer;
  [SerializeField, Range(0.0f, 1.0f)] private float _initialSFXVolume;
  [SerializeField, Range(0.0f, 1.0f)] private float _initialMusicVolume;

  [SerializeField] private float _volumeMultiplier = 30.0f;
  [SerializeField] private float _pauseCutoff = 200.0f;
  [SerializeField] private float _pauseCutoffSpeed;
  [SerializeField] private float _resumeCutoffSpeed;

  private float _targetCutoff;
  private float _currCutoff;
  private float _cutoffSpeed;

  private void Start()
  {
    SetSFXVolume(_initialSFXVolume);
    SetMusicVolume(_initialMusicVolume);
    _targetCutoff = 22000.0f;
    _currCutoff = _targetCutoff;
  }

  public void SetSFXVolume(float volume)
    => _masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * _volumeMultiplier);

  public void SetMusicVolume(float volume)
    => _masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * _volumeMultiplier);

  public void EnablePauseCutoff()
  {
    _targetCutoff = _pauseCutoff;
    _cutoffSpeed = _pauseCutoffSpeed;
  }

  public void DisablePauseCutoff()
  {
    _targetCutoff = 22000.0f;
    _cutoffSpeed = _resumeCutoffSpeed;
  }

  private void Update()
  {
    _currCutoff = Mathf.Lerp(_currCutoff, _targetCutoff, Time.unscaledDeltaTime*_cutoffSpeed);
    _masterMixer.SetFloat("MusicCutoff", _currCutoff);
  }
}
