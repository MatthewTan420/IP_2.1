/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: WinTest
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTest : MonoBehaviour
{
    /// <summary>
    /// Player wins when reach the end point
    /// </summary>
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
