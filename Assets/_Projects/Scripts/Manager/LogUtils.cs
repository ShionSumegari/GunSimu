using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogUtils
{
    static string TAG = "GunSimulator: ";
    static bool isLogged = true;

    public static void Log(object mess)
    {
        if (isLogged)
            Debug.Log(TAG + mess);
    }
}
