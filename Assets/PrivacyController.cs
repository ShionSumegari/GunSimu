using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrivacyController : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (toggle.isOn)
        {
            GetComponent<Button>().interactable = true;
        }
        else if (!toggle.isOn)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
