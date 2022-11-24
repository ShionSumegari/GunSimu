using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
        Accelerometer.Instance.OnShake += ActionToRunWhenShakingDevice;
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        Accelerometer.Instance.OnShake -= ActionToRunWhenShakingDevice; 
    }

    private void ActionToRunWhenShakingDevice()
    {
        if (GamePlayController.Instance.isShake)
        {
            GamePlayController.Instance.ShakeGun();
        }
    }
}
