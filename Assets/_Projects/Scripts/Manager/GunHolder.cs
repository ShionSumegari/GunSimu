using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GunHolder : MonoBehaviour
{
    public enum GunType
    {
        PISTOL,
        SHOTGUNS,
        ASSAULTRIFLES,
        SMGS,
        MACHINEGUNS,
        SNIPERRIFLES
    }

    public GunType gunType;
    public Vector2 sliderPos;
    public float gunPos;
    public bool isGunHolder;
    public bool isInstantiate;

    [SerializeField] GameObject container;

    private async void Start()
    {
        await WaitUntilInstantited();
        if (isGunHolder && isInstantiate)
        {
            float height = (container.GetComponent<RectTransform>().rect.height - 20) / 2;
            container.GetComponent<GridLayoutGroup>().cellSize = new
                Vector2(container.GetComponent<GridLayoutGroup>().cellSize.x, height);
        }
    }

    private async Task WaitUntilInstantited()
    {
        while (!isInstantiate)
        {
            await Task.Yield();
        }
        await Task.Delay(50);
    }
}
