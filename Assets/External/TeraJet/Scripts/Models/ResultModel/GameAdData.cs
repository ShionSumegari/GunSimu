using System;

namespace TeraJet
{
    [Serializable]
    public class GameAdData
    {
        public int version_code;
        public string current_date;
        public string ad_type;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}

