using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeraJet
{
    public class TeraConverter
    {
        public static string SecondsToMinutes(int timer)
        {
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string niceTime = string.Format("{00:00}:{1:00}", minutes, seconds);

            return niceTime;
        }
    }
}


