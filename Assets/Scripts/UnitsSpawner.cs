using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsSpawner : MonoBehaviour
{
    #region singleton
    private static UnitsSpawner instance;

    private UnitsSpawner()
    { }

    public static UnitsSpawner getInstance()
    {
        if (instance == null)
            instance = new UnitsSpawner();
        return instance;
    }
    #endregion

    #region Parameters
    [SerializeField] private GameObject unitPrefab;

    private UnitController crntUnit;
    #endregion

    #region MainMetods
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    #region PublicMetods
    public void SpawnNewUnit(Vector3 position, int type)
    {
        crntUnit = Instantiate(unitPrefab, position, Quaternion.identity).GetComponent<UnitController>();
        crntUnit.SetType(type);
        UnitsCommander.getInstance().AddUnit(crntUnit);
    }

    public void SpawnUnits(UnitData[] _units)
    {
        for (int i = 0; i < _units.Length; i++)
        {
            SpawnNewUnit(new Vector3(_units[i].x, _units[i].y, _units[i].z), _units[i].type);
        }
    }
    #endregion
}
