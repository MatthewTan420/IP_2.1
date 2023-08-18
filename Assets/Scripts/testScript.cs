/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: testScript
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testScript : MonoBehaviour
{
    private NewBehaviourScript player;
    public int sceneToLoad;

    /// <summary>
    /// Teleport player to next scene
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(sceneToLoad);
            player = FindObjectOfType<NewBehaviourScript>();
            player.isSwitched = true;
        }
    }
}
