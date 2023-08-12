using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class testScript : MonoBehaviour
{
    private NewBehaviourScript player;
    public int sceneToLoad;
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
