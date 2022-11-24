using System;

namespace TeraJet
{
    [Serializable]
    public class BaseResponse
    {
        public int code;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this, true);
        }
    }
}

