using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] Image _progressImage;
    [SerializeField] float _raiseTime = 0.5f;
    Coroutine _raiseCoroutine;
    private void Start()
    {
        //_progressImage.fillAmount = 0;
        StartCoroutine(LoadDataFromServer());
    }

    IEnumerator LoadDataFromServer()
    {
        yield return new WaitUntil(() => { return GameManager.Instance.isPlayerDataLoaded; });
        //FirebaseManager.Instance.Initialize();
        LogUtils.Log("User's Data Loaded");

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.allowSceneActivation = false;



        while (!asyncOperation.isDone)
        {
            LogUtils.Log("Loading Progress: " + (asyncOperation.progress * 100) + "%");
            if (_raiseCoroutine != null) StopCoroutine(_raiseCoroutine);
           // _raiseCoroutine = StartCoroutine(RaiseProgress(asyncOperation.progress * (1f - _progressImage.fillAmount)));
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    //IEnumerator RaiseProgress(float progress)
    //{
    //    float currentAmount = _progressImage.fillAmount;
    //    var elapsedTime = 0f;
    //    while (elapsedTime < _raiseTime)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        _progressImage.fillAmount = Mathf.MoveTowards(currentAmount, currentAmount + progress, elapsedTime / _raiseTime);
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
}
