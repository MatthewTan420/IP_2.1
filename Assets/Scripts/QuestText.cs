using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestText : MonoBehaviour
{
    public NewBehaviourScript script;
    public Transform player;
    public GameObject questCon;
    public TextMeshProUGUI questText;
    public string whatsQuest;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        script = player.GetComponent<NewBehaviourScript>();
        questCon = script.questCon;
        questText = script.questText;
        questCon.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            questCon.SetActive(true);
            questText.text = whatsQuest;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            questCon.SetActive(false);
        }
    }
}
