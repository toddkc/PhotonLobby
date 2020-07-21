using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// using PUN I randomly get " Scene (is loading)" in the editor hierarchy which breaks saving scenes.  this can fix that.
/// </summary>

public class CloseAllScenes : MonoBehaviour
{
    private void OnEnable()
    {
        //CloseScenes();
    }
    private void OnDisable()
    {
        CloseScenes();
    }

    private void CloseScenes()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!IsActiveScene(scene)) CloseScene(scene);
        }
    }

    private bool IsActiveScene(Scene scene)
    {
        return scene == activeScene;
    }

    private Scene activeScene
    {
        get { return SceneManager.GetActiveScene(); }
    }

    private void CloseScene(Scene scene)
    {
        SceneManager.UnloadSceneAsync(scene.buildIndex);
    }
}
