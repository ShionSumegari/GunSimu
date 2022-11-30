using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class LinkToURL : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI privacyText;
    string myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = "";
        HandleMyText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void HandleMyText()
    {
        myText += "<color=black>To continue playing, you need to confirm that you agree BlackStarStudio's</color>";
        myText += "<color=blue><link=https://terajetgame.com/privacy-policy/> Terms of Service</link></color>";
        myText += "<color=black> and have read our</color>";
        myText += "<color=blue><link=https://terajetgame.com/privacy-policy/> Privacy Policy                                     </link></color>";
        privacyText.text = myText;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerClick)
        {
            int index = TMP_TextUtilities.FindIntersectingLink(privacyText, Input.GetTouch(0).position, null);
            if (index > -1)
            {
                Application.OpenURL(privacyText.textInfo.linkInfo[index].GetLinkID());
            }
        }
    }
}
