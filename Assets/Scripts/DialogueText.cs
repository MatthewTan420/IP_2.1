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

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        script = player.GetComponent<NewBehaviourScript>();
        dialCon = script.dialCon;
        dialText = script.dialText;
        dialCon.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            dialCon.SetActive(true);
            dialText.text = whatToSay;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            dialCon.SetActive(false);
        }
    }
}
