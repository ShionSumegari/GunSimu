using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int _qualitySettingsIndex;

    public UserData(UserData data)
    {
        _qualitySettingsIndex = data._qualitySettingsIndex;
    }
    public UserData()
    {
        _qualitySettingsIndex = 2;
    }
}
