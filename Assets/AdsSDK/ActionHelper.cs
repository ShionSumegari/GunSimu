using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionHelper
{
    public static IEnumerator StartAction(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (action != null) action.Invoke();
    }
}
