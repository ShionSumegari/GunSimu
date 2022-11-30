using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int _qualitySettingsIndex;

    public List<int> _unlockPistolID;

    public List<int> _unlockShotGunID;

    public List<int> _unlockGunAssaulID;

    public List<int> _unlockGunMachineID;

    public List<int> _unlockGunSmgID;

    public List<int> _unlockGunSniperID;

    public UserData(UserData data)
    {
        _qualitySettingsIndex = data._qualitySettingsIndex;
        _unlockPistolID = data._unlockPistolID;
        _unlockShotGunID = data._unlockShotGunID;
        _unlockGunAssaulID = data._unlockGunAssaulID;
        _unlockGunMachineID = data._unlockGunMachineID;
        _unlockGunSmgID = data._unlockGunSmgID;
        _unlockGunSniperID = data._unlockGunSniperID;
    }
    public UserData()
    {
        _qualitySettingsIndex = 2;
        
        _unlockPistolID = new List<int>();
        _unlockPistolID.Add(2);
        _unlockPistolID.Add(4);
        _unlockPistolID.Add(6);
        _unlockPistolID.Add(8);

        _unlockShotGunID = new List<int>();
        _unlockShotGunID.Add(2);

        _unlockGunAssaulID = new List<int>();
        _unlockGunAssaulID.Add(2);

        _unlockGunMachineID = new List<int>();
        _unlockGunMachineID.Add(1);

        _unlockGunSmgID = new List<int>();
        _unlockGunSmgID.Add(1);

        _unlockGunSniperID = new List<int>();
        _unlockGunSniperID.Add(1);
    }
}
