using UnityEngine;

[CreateAssetMenu(fileName = "TeraGameConfig", menuName = "Terajet/SDKConfig", order = 1)]
public class TeraGameConfigSO : ScriptableObject
{
    public string notificationTitle = "We miss you!";
    public string notificationDesc = "Come back! Come back! Come back!";
    public bool isInternetConnectionRequired;

    public string SDKVersion = "v0.1.6.7";
}