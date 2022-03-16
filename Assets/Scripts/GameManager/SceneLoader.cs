using UnityEngine;
using UnityEngine.SceneManagement;
using Voxell.Inspector;

public class SceneLoader : MonoBehaviour
{
  [Scene, SerializeField] private string _gameplayScene;

  [SerializeField] private GameManager _gameManager;
  public static GameManager GameManager;

  private void Awake()
  {
    GameManager = _gameManager;
    LeanTween.reset();

    string[] loadedScenes = new string[SceneManager.sceneCount];
    for (int s=0; s < SceneManager.sceneCount; s++)
    {
      Scene scene = SceneManager.GetSceneAt(s);
      loadedScenes[s] = scene.name;
    }

    if (!IsSceneLoaded(_gameplayScene, in loadedScenes))
    SceneManager.LoadSceneAsync(_gameplayScene, LoadSceneMode.Additive);
  }

  private bool IsSceneLoaded(string scene, in string[] loadedScenes)
  {
    for (int s=0; s < loadedScenes.Length; s++)
      if (loadedScenes[s] == scene) return true;

    return false;
  }
}