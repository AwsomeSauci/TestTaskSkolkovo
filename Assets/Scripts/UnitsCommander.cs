using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class UnitsCommander : MonoBehaviour
{
    #region singleton
    private static UnitsCommander instance;


    private UnitsCommander()
    { }

    public static UnitsCommander getInstance()
    {
        if (instance == null)
            instance = new UnitsCommander();
        return instance;
    }
    #endregion

    #region Parameters
    /// <summary>
    /// Input fields for move coordinates
    /// </summary>
    [SerializeField] private TMP_InputField InputX;
    [SerializeField] private TMP_InputField InputY;
    [SerializeField] private TMP_InputField InputZ;
    [SerializeField] private TMP_Dropdown DropDownCollisionMode;

    private static WaitForSeconds startDelay = new WaitForSeconds(1);

    private List<UnitController> units = new List<UnitController>();

    private RaycastHit hit;
    private Vector3 coordinatesShift = Vector3.zero;
    private bool showSequenceMode = false;
    private static string planeTag = "Plane";
    private static string unitTag = "Unit";
    #endregion

    #region MainMetods
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        StartCoroutine(Starter());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.tag == unitTag)
            {
                if (!showSequenceMode)
                hit.collider.gameObject.GetComponent<UnitController>().ClickSelect();
                else
                    hit.collider.gameObject.GetComponent<UnitController>().ShowSequence();
            }
        }
        if (Input.GetMouseButtonDown(1) &&
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.tag == planeTag)
            {
                UnitsSpawner.getInstance().SpawnNewUnit(new Vector3(hit.point.x, 0.5f, hit.point.z), 0);
                SetCollisionCheckMode();
            }

        }
        if (Input.GetMouseButtonDown(2) &&
               Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.tag == planeTag)
            {
                UnitsSpawner.getInstance().SpawnNewUnit(new Vector3(hit.point.x, 0.5f, hit.point.z), 1);
                SetCollisionCheckMode();
            }

        }
    }
    #endregion

    #region PublicMetods
    public void ActivateshowSequenceMode()
    {
        showSequenceMode=true;
    }

    public void DeactivateshowSequenceMode()
    {
        showSequenceMode = false;
    }

    public void SetCollisionCheckMode()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].SetCheckMode(DropDownCollisionMode.value);
        }
    }

    // adding a created unit to an array of units
    public void AddUnit(UnitController unit)
    {
        units.Add(unit);
    }

    public void UnSelectAll()
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].GetSelected())
            {
                units[i].ClickSelect();
            }
        }
    }

    // adding an action to a sequence for selected units
    public void PushSequence()
    {
        GetShift();
        for (int i = 0; i < units.Count; i++)
        {
            if (units[i].GetSelected())
            {
                units[i].AddToSequence(coordinatesShift);
            }
        }
    }

    // starting a sequence of unit actions
    public void StartSequence()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].StartSequence();
        }
    }

    // clearing the sequence of unit actions
    public void ClearSequence()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].ClearSequence();
        }
    }
    #endregion

    #region PrivateMetods
    private void GetShift()
    {
        if (!float.TryParse(InputX.text, out coordinatesShift.x))
        {
            coordinatesShift.x = 0;
        }
        if (!float.TryParse(InputZ.text, out coordinatesShift.z))
        {
            coordinatesShift.z = 0;
        }
        if (!float.TryParse(InputY.text, out coordinatesShift.y))
        {
            coordinatesShift.y = 0;
        }
    }
    #endregion

    #region Coruntines
    // waiting for class initialization and loading units from json
    public IEnumerator Starter()
    {
        yield return startDelay;
        JsonReader.getInstance().SpawnFromJson();
    }
    #endregion
}


