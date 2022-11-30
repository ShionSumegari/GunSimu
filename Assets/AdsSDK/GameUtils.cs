using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.EventSystems;

public static class GameUtils
{
    public static readonly string PLATFORM_ANDROID = "Android";
    public static readonly string PLATFORM_IOS = "iOS";
    public static readonly string PLATFORM_WEB = "Web";

    //public static void SavePlayerData(UserData data)
    //{
    //    BinaryFormatter formatter = new BinaryFormatter();

    //    string path = Application.persistentDataPath + "/player.terajet";

    //    FileStream stream = new FileStream(path, FileMode.Create);

    //    // PlayerData data = new PlayerData(player);

    //    formatter.Serialize(stream, data);
    //    stream.Close();
    //}

    //public static UserData LoadPlayerData()
    //{
    //    string path = Application.persistentDataPath + "/player.terajet";
    //    if (File.Exists(path))
    //    {
    //        Debug.Log("Saved file found in " + path);

    //        BinaryFormatter formatter = new BinaryFormatter();

    //        FileStream stream = new FileStream(path, FileMode.Open);

    //        UserData data = formatter.Deserialize(stream) as UserData;

    //        data.CheckNullableData();

    //        stream.Close();

    //        return data;
    //    }
    //    else
    //    {
    //        Debug.Log("Saved file not found in " + path + " Generate Zero Data");
    //        UserData data = new UserData();
    //        return data;
    //    }
    //}
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    //public static bool IsPointerOverUIElement()
    //{
    //    return IsPointerOverUIElement(GetEventSystemRaycastResults());
    //}
    /////Returns 'true' if we touched or hovering on Unity UI element.
    //public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    //{
    //    for (int index = 0; index < eventSystemRaysastResults.Count; index++)
    //    {
    //        RaycastResult curRaysastResult = eventSystemRaysastResults[index];
    //        if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
    //            return true;
    //    }
    //    return false;
    //}
    /////Gets all event systen raycast results of current mouse or touch position.
    //static List<RaycastResult> GetEventSystemRaycastResults()
    //{
    //    PointerEventData eventData = new PointerEventData(EventSystem.current);
    //    eventData.position = Input.mousePosition;
    //    List<RaycastResult> raysastResults = new List<RaycastResult>();
    //    EventSystem.current.RaycastAll(eventData, raysastResults);
    //    return raysastResults;
    //}
    #region SYSTEM_UTILS
    public static T[] AddItemToArray<T>(this T[] original, T itemToAdd)
    {
        T[] finalArray = new T[original.Length + 1];
        for (int i = 0; i < original.Length; i++)
        {
            finalArray[i] = original[i];
        }
        finalArray[finalArray.Length - 1] = itemToAdd;
        return finalArray;
    }

    public static T[] Reverse<T>(T[] array)
    {
        var result = new T[array.Length];
        int j = 0;
        for (int i = array.Length - 1; i >= 0; i--)
        {
            result[j] = array[i];
            j++;
        }
        return result;
    }
    #endregion
}
