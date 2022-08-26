using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    #region Parameters
    [SerializeField] private int unitType = 0;
    [SerializeField] private float moveSpeed = 1;
    /// <summary>
    /// materials for unit
    /// </summary>
    [SerializeField] private Material discretMaterial;
    [SerializeField] private Material analogMaterial;
    [SerializeField] private Material selectedMaterial;

    private List<Vector3> shiftSequence = new List<Vector3>();

    private Vector3 targetPos = Vector3.zero;// the unit's current follow point
    private Material defaultMaterial;// material of unit
    private bool needMove = false;
    private bool unitSelected = false;
    private int collisionCheckMode = 0;
    #endregion

    #region MainMetods

    private void Update()
    {
        if (needMove)
        {
            if (transform.position == targetPos) needMove = false;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collisionCheckMode)
        {
            case 0:
                break;
            case 1:
                ExceptionHandler.getInstance().ShowException();
                break;
            case 2:
                needMove = false;
                break;
        }
    }
    #endregion

    #region PublicMetods
    public void ShowSequence()
    {
        ScroolViewHandler.getInstance().ShowSequence(shiftSequence);
    }

    public void SetCheckMode(int mode)
    {
        collisionCheckMode = mode;
    }

    public bool GetSelected()
    {
        return unitSelected;
    }

    public void AddToSequence(Vector3 sequence)
    {
        this.shiftSequence.Add(sequence);
    }

    public void StartSequence()
    {
        if (shiftSequence.Count > 0)
        {
            StopAllCoroutines();
            StartCoroutine(SequenceController());
        }
    }

    public void ClearSequence()
    {
        shiftSequence.Clear();
    }

    public void ClickSelect()
    {
        if (!unitSelected)
        {
            unitSelected = true;
            SetCurrentMaterial(selectedMaterial);
        }
        else
        {
            unitSelected = false;
            SetCurrentMaterial(defaultMaterial);
        }
    }

    public void SetType(int mod)
    {
        unitType = mod;
        SetDefaultMaterial();
    }

    public void SetCurrentMaterial(Material material)
    {
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    // initialization moving a unit depending on its type
    public void Move(Vector3 distantionMove)
    {
        switch (unitType)
        {
            case 1:
                MoveDiscret(distantionMove);
                break;
            case 0:
                MoveSmooth(distantionMove);
                break;
        }
    }
    #endregion

    #region PrivateMetods

    private void SetDefaultMaterial()
    {
        if (unitType == 0)
        {
            defaultMaterial = analogMaterial;
        }
        else
        {
            defaultMaterial = discretMaterial;
        }
        SetCurrentMaterial(defaultMaterial);
    }   

    private void MoveDiscret(Vector3 moveDist)
    {
        needMove = true;
        transform.position = new Vector3(transform.position.x + moveDist.x, transform.position.y + moveDist.y, transform.position.z + moveDist.z);
        needMove = false;
    }

    private void MoveSmooth(Vector3 moveDist)
    {
        needMove = true;
        targetPos = new Vector3(transform.position.x + moveDist.x, transform.position.y + moveDist.y, transform.position.z + moveDist.z);
    }
    #endregion

    #region Coruntines

    private IEnumerator SequenceController()
    {
        for (int i = 0; i < shiftSequence.Count; i++)
        {
            Move(shiftSequence[i]);
            yield return new WaitWhile(() => needMove);
        }
    }
    #endregion
}
