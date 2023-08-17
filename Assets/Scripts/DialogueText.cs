using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueText : MonoBehaviour
{
    public NewBehaviourScript script;
    public Transform player;
    public GameObject dialCon;
    public TextMeshProUGUI dialText;
    public string whatToSay;
    public bool isQuest = true;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        script = player.GetComponent<NewBehaviourScript>();
        dialCon = script.dialCon;
        dialText = script.dialText;
        dialCon.SetActive(false);
    }

    /// <summary>
    /// This opens the quest/dialogue tag
    /// </summary>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && isQuest)
        {
            dialCon.SetActive(true);
            dialText.text = whatToSay;
            Invoke(nameof(Inactive), 5.0f);
            isQuest = false;
        }
    }

    void Inactive()
    {
        dialCon.SetActive(false);
    }
}
