using System;

namespace TeraJet
{
    [Serializable]
    public class BasePost
    {
        public string package_name;

        public string platform_name;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}

