using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingMenu : MonoBehaviour
{
    public SetVolume (float volume);
    public void SetVolumn (float volumn) 
    {
        Debug.Log(volume);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
