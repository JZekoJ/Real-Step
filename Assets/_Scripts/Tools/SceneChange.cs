using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// Function use to pass to a scene to an other
    /// </summary>
    /// <param name="sceneName"></param>


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
}