using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingMenu : MonoBehaviour
{
  public void SetVolume(float volume) 
  {
    Debug.Log(volume);
  }

  public void SetFullscreen(bool isFullscreen)
  {
    Screen.fullScreen = isFullscreen;
  }
}
