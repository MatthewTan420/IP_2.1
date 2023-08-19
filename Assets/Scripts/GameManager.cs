using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    private NewBehaviourScript activePlayer;
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += SpawnPlayerOnScreenLoad;
            instance = this;
        }
    }

    private void SpawnPlayerOnScreenLoad(Scene curScene, Scene next)
    {
        spawn spawnpoint = FindObjectOfType<spawn>();
        if (activePlayer == null)
        {
            GameObject newPlayer = Instantiate(playerPrefab, spawnpoint.transform.position, Quaternion.identity);
            activePlayer = newPlayer.GetComponent<NewBehaviourScript>();
        }
        else
        {
            activePlayer.transform.position = spawnpoint.transform.position;
            activePlayer.transform.rotation = spawnpoint.transform.rotation;
        }
    }
}
