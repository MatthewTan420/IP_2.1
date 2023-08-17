using UnityEngine.Audio;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public AudioMixer mainMixer;

    public void setVolume(float volume)
    {
        mainMixer.SetFloat("MyExposedParam", volume);
    }
}
