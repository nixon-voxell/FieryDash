using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAnimation : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = Vector2.zero;
    }
    public void Open() 
    {
        transform.LeanScale(Vector2.one, 0.3f);
        Debug.Log("open");
    }
    public void Close()
    {
        transform.LeanScale(Vector2.zero, 0.5f).setEaseInBack();
        Debug.Log("Close");
    }
}
