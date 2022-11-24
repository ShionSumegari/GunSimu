using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeraJet
{
    [Serializable]
    public class TeraGameConfig
    {
        public string notificationTitle;
        public string notificationDesc;

        public bool isInternetConnectionRequired;

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }

        public TeraGameConfig()
        {
            notificationTitle = "We miss you, hero";
            notificationDesc = "Come back and fight!!";
            isInternetConnectionRequired = false;
        }

        public TeraGameConfig(TeraGameConfig data)
        {
            notificationTitle = data.notificationTitle;
            notificationDesc = data.notificationDesc;
            isInternetConnectionRequired = data.isInternetConnectionRequired;
        }
    }
}


