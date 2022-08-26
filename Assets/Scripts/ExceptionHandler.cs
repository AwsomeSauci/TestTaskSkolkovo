using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionHandler : MonoBehaviour
{
    #region singleton
    private static ExceptionHandler instance;

    private ExceptionHandler()
    { }

    public static ExceptionHandler getInstance()
    {
        if (instance == null)
            instance = new ExceptionHandler();
        return instance;
    }
    #endregion

    #region Parameters
    [SerializeField] private GameObject exceptionText;

    private WaitForSeconds delay = new WaitForSeconds(5);
    #endregion

    #region MainMetods
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region PublicMetods
    public void ShowException()
    {
        StopAllCoroutines();
        StartCoroutine(ExceptionTimer());
    }
    #endregion

    #region Coruntines
    private IEnumerator ExceptionTimer()
    {
        exceptionText.SetActive(true);
        yield return delay;
        exceptionText.SetActive(false);
    }
    #endregion
}
