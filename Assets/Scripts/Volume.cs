/*
 * Author: 
 * Date: 
 * Description: 
 */

using UnityEngine.Audio;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public AudioMixer mainMixer;

    /// <summary>
    /// adjust the audio for the game
    /// </summary>
    public void setVolume(float volume)
    {
        mainMixer.SetFloat("MyExposedParam", volume);
    }
}
