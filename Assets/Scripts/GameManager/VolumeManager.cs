using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
  [System.Serializable]
  private class VolumeData
  {
    [SerializeField, Range(0.0f, 1.0f)] public float fxVolume;
    [SerializeField, Range(0.0f, 1.0f)] public float musicVolume;
  }

  [SerializeField] private Saver<VolumeData> _volumeSaver;
  [SerializeField] private AudioMixer _masterMixer;
  [SerializeField] private VolumeData _volumeData;
  [SerializeField] private Slider _fxSlider;
  [SerializeField] private Slider _musicSlider;

  [SerializeField] private float _volumeMultiplier = 30.0f;
  [SerializeField] private float _pauseCutoff = 200.0f;
  [SerializeField] private float _pauseCutoffSpeed;
  [SerializeField] private float _resumeCutoffSpeed;

  private float _targetCutoff;
  private float _currCutoff;
  private float _cutoffSpeed;

  private void Start()
  {
    VolumeData savedVolumeData = _volumeSaver.ReadFile();
    if (savedVolumeData != null) _volumeData = savedVolumeData;

    SetSFXVolume(_volumeData.fxVolume);
    SetMusicVolume(_volumeData.musicVolume);
    _fxSlider.SetValueWithoutNotify(_volumeData.fxVolume);
    _musicSlider.SetValueWithoutNotify(_volumeData.musicVolume);

    _targetCutoff = 22000.0f;
    _currCutoff = _targetCutoff;
  }

  public void SetSFXVolume(float volume)
  {
    _masterMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * _volumeMultiplier);
    _volumeData.fxVolume = volume;
  }

  public void SetMusicVolume(float volume)
  {
    _masterMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * _volumeMultiplier);
    _volumeData.musicVolume = volume;
  }

  public void SaveVolumeData()
  {
    _volumeSaver.WriteData(_volumeData);
  }

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
