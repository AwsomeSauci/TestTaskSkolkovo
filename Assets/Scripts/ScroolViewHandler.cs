using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScroolViewHandler : MonoBehaviour
{
    #region singleton
    private static ScroolViewHandler instance;


    private ScroolViewHandler()
    { }

    public static ScroolViewHandler getInstance()
    {
        if (instance == null)
            instance = new ScroolViewHandler();
        return instance;
    }
    #endregion

    #region Parameters
    [SerializeField] private GameObject scroolItem;

    private List<GameObject> scrollItems = new List<GameObject>();
    #endregion

    #region MainMetods
    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Awake()
    {
        ClearScroll();
    }
    #endregion

    #region PublicMetods
    public void ShowSequence(List<Vector3> sequence)
    {
        ClearScroll();
        LoadScroll(sequence);
    }
    #endregion

    #region PrivateMetods
    private void LoadScroll(List<Vector3> sequence)
    {
        for(int i = 0; i < sequence.Count; i++)
        {
            scrollItems.Add(Instantiate(scroolItem));
            scrollItems[i].SetActive(true);
            scrollItems[i].transform.SetParent(scroolItem.transform.parent);
            scrollItems[i].GetComponentInChildren<TextMeshProUGUI>().text = GetOutPutString(sequence[i]) ;
        }
    }

    private string GetOutPutString(Vector3 shift)
    {
        return "Сдвиг " + shift.x.ToString() + " " + shift.y.ToString() + " " + shift.z.ToString();
    }

    private void ClearScroll()
    {
        for(int i = 0; i < scrollItems.Count; i++)
        {
            Destroy(scrollItems[i]);
        }
        scrollItems.Clear();
    }
    #endregion
}
