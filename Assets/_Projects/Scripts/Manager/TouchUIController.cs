using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchUIController : MonoBehaviour
{
    public GameObject ui_Canvas;
    GraphicRaycaster ui_Raycaster;
    PointerEventData click_Data;
    List<RaycastResult> click_Results;

    List<GameObject> clicked_elements;

    Vector3 touch_Position;
    Touch touch;

    // Start is called before the first frame update
    void Start()
    {
        ui_Raycaster = ui_Canvas.GetComponent<GraphicRaycaster>();
        click_Data = new PointerEventData(EventSystem.current);
        click_Results = new List<RaycastResult>();
        clicked_elements = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            TouchUI();
        }
    }

    void TouchUI()
    {
        touch = Input.GetTouch(0);
        touch_Position = touch.position;
        DetectUI();
    }

    private void DetectUI()
    {
        GetUIElementsTouched();

        if(GamePlayController.Instance != null)
        {
            if (clicked_elements.Count > 0)
            {
                if(touch.phase == TouchPhase.Began)
                {
                    GameObject touch_UiGOJ = clicked_elements[0];
                    if (touch_UiGOJ.CompareTag("Shot"))
                    {
                        GamePlayController.Instance.isCanShoot = true;
                    }
                    else
                    {
                        GamePlayController.Instance.isCanShoot = false;
                    }
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                GamePlayController.Instance.isCanShoot = true;
            }
        }
    }

    private void GetUIElementsTouched()
    {
        click_Data.position = touch_Position;
        click_Results.Clear();
        ui_Raycaster.Raycast(click_Data, click_Results);

        clicked_elements.Clear();
        foreach (RaycastResult result in click_Results)
        {
            clicked_elements.Add(result.gameObject);
        }
    }
}
