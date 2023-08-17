using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    /// <summary>
    /// The index of the scene to load to.
    /// </summary>
    public int sceneToLoad;

    /// <summary>
    /// The interact function called by the player.
    /// </summary>
    public void Switch()
    {
        // use the SceneManager to load the specified scene index.
        SceneManager.LoadScene(sceneToLoad);
    }

    // May move this to the player script since it didnt take static into account
    public void ReloadScene()
    {
        // use the SceneManager to load the specified scene index.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }

}
