using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISpriteAnimSheet : MonoBehaviour
{
    [SerializeField] Sprite[] spriteSheet;
    public float frameDelay = 0.1f;
    Image image;
    Coroutine animCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();

        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
        }
        animCoroutine = StartCoroutine(AnimLoading());
    }

    IEnumerator AnimLoading()
    {
        int i = 0;
        while (spriteSheet.Length > 0)
        {
            image.sprite = spriteSheet[i];
            i++;
            if (i >= spriteSheet.Length) i = 0;
            yield return new WaitForSeconds(frameDelay);
        }
    }
}
