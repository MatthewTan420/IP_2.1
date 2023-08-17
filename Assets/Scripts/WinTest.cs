using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTest : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Invoke(nameof(LoadMenu), 4.0f);
        }
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
