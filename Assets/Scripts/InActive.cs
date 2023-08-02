using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InActive : MonoBehaviour
{
    public GameObject inactive;

    // Start is called before the first frame update
    void Start()
    {
        inactive.SetActive(false);
    }
}
