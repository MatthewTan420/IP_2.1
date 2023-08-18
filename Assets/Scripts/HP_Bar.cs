/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: HP_Bar
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    /// <summary>
    /// This is the player's health bar
    /// </summary>

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    /// <summary>
    /// Set Player's max health
    /// </summary>
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    /// <summary>
    /// Update health if take damage or heal
    /// </summary>
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
