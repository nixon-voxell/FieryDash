using UnityEngine;
using Voxell;
using Voxell.Inspector;

[System.Serializable]
public class Saver<T> where T : class
{
  [SerializeField, StreamingAssetFilePath] private protected string _filepath;

  public T ReadFile()
  {
    string jsonData = FileUtilx.ReadStreamingAssetFileText(_filepath);
    T content = JsonUtility.FromJson<T>(jsonData);
    return content;
  }

  public void WriteData(T data)
  {
    string jsonData = JsonUtility.ToJson(data);
    FileUtilx.WriteStreamingAssetFileText(_filepath, jsonData);
  }
}