using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

namespace TeraJet
{
    public class SimpleTeraVFX
    {
        public static void PlayGotHitFX(Material targetMaterial , Color oldColor, float blinkIntensity = 10f, float blinkDuration = 0.1f)
        {
            ChangeMaterial(targetMaterial, oldColor, Color.white, blinkIntensity, blinkDuration);
        }

        public static async void ChangeMaterial(Material targetMaterial, Color oldColor, Color targetColor, float blinkIntensity, float blinkDuration)
        {
            float blinkTimer = blinkDuration;
            do
            {
                blinkTimer -= Time.deltaTime;
                float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
                float intensity = lerp * blinkIntensity;
                targetMaterial.color = targetColor * intensity;
                await Task.Delay((int)(Time.deltaTime * 1000f));
            } while (targetMaterial.color.a >= 1f);
            targetMaterial.color = oldColor;
        }

    }
}

