using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
