using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedGunController : MonoBehaviour
{
    [SerializeField] SliderBannerController sliderBanner;

    [SerializeField] float startGunPos = 0;
    [SerializeField] int gunHolderAmount;

    private float m_startGunPos;

    Transform currentGun;
    Transform previousGun;
    private void Awake()
    {
        m_startGunPos = startGunPos;
        StartCoroutine(SortGunHolder());
    }

    private void Update()
    {
        foreach (Transform gun in transform)
        {
            if (transform.GetComponent<RectTransform>().anchoredPosition.x <= -gun.GetComponent<GunHolder>().gunPos)
            {
                currentGun = gun;
            }
        }
        if (currentGun != previousGun)
        {
            sliderBanner.SetupSlider(currentGun.GetComponent<GunHolder>().gunType);
            previousGun = currentGun;
        }
    }


    IEnumerator SortGunHolder()
    {
        for(int i = 1;i <= gunHolderAmount; i++)
        {
            GameObject gHolder = Resources.Load<GameObject>("GunHolder/GunHolder_" + i);
            yield return new WaitUntil(() => gHolder != null);
            GameObject gunHolder = Instantiate(gHolder, transform);
            foreach(Transform gun in gunHolder.transform)
            {
                if (gun.GetComponent<Gun>().typeGun == Gun.TypeGun.PISTOLS)
                {
                    if(GameManager.Instance.userData._unlockPistolID.Exists(x => x == gun.GetComponent<Gun>().gunId))
                    {
                        gun.GetComponent<Gun>().locked = true;
                        gun.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        gun.GetComponent<Gun>().locked = false;
                        gun.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                if (gun.GetComponent<Gun>().typeGun == Gun.TypeGun.ASSAULTRIFLES)
                {
                    if (GameManager.Instance.userData._unlockGunAssaulID.Exists(x => x == gun.GetComponent<Gun>().gunId))
                    {
                        gun.GetComponent<Gun>().locked = true;
                        gun.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        gun.GetComponent<Gun>().locked = false;
                        gun.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                if (gun.GetComponent<Gun>().typeGun == Gun.TypeGun.SHOTGUNS)
                {
                    if (GameManager.Instance.userData._unlockShotGunID.Exists(x => x == gun.GetComponent<Gun>().gunId))
                    {
                        gun.GetComponent<Gun>().locked = true;
                        gun.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        gun.GetComponent<Gun>().locked = false;
                        gun.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                if (gun.GetComponent<Gun>().typeGun == Gun.TypeGun.MACHINEGUNS)
                {
                    if (GameManager.Instance.userData._unlockGunMachineID.Exists(x => x == gun.GetComponent<Gun>().gunId))
                    {
                        gun.GetComponent<Gun>().locked = true;
                        gun.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        gun.GetComponent<Gun>().locked = false;
                        gun.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                if (gun.GetComponent<Gun>().typeGun == Gun.TypeGun.SMGS)
                {
                    if (GameManager.Instance.userData._unlockGunSmgID.Exists(x => x == gun.GetComponent<Gun>().gunId))
                    {
                        gun.GetComponent<Gun>().locked = true;
                        gun.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        gun.GetComponent<Gun>().locked = false;
                        gun.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                if (gun.GetComponent<Gun>().typeGun == Gun.TypeGun.SNIPERRIFLES)
                {
                    if (GameManager.Instance.userData._unlockGunSniperID.Exists(x => x == gun.GetComponent<Gun>().gunId))
                    {
                        gun.GetComponent<Gun>().locked = true;
                        gun.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        gun.GetComponent<Gun>().locked = false;
                        gun.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
            gunHolder.name = gunHolder.GetComponent<GunHolder>().gunType.ToString();
            gunHolder.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_startGunPos, 0f);
            gunHolder.GetComponent<GunHolder>().gunPos = m_startGunPos - 1920 / 2;
            SetUpSliderBar(gunHolder.GetComponent<GunHolder>().gunType, m_startGunPos);
            gunHolder.GetComponent<GunHolder>().isInstantiate = true;
            int childCount = gunHolder.transform.childCount;
            int count;
            if (childCount % 2 == 0)
            {
                count = childCount / 2;
            }
            else
            {
                count = childCount / 2 + 1;
            }
            float posNextHolder = gunHolder.GetComponent<GridLayoutGroup>().cellSize.x + gunHolder.GetComponent<GridLayoutGroup>().spacing.x;
            m_startGunPos += posNextHolder * count;
            yield return new WaitForEndOfFrame();
        }
    }

    void SetUpSliderBar(GunHolder.GunType gunType, float m_gunPos)
    {
        foreach(Transform gunSlider in sliderBanner.transform)
        {
            if (gunSlider.GetComponent<GunHolder>().gunType == gunType)
            {
                gunSlider.GetComponent<GunHolder>().gunPos = m_gunPos;
                break;
            }
        }
    }
}
