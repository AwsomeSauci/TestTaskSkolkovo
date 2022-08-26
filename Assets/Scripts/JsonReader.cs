using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonReader : MonoBehaviour
{
    #region singleton
    private static JsonReader instance;

    private JsonReader()
    { }

    public static JsonReader getInstance()
    {
        if (instance == null)
            instance = new JsonReader();
        return instance;
    }
    #endregion

    #region Parameters
    private UnitData[] units;

    private const string fileName = "/" + "Items.json";
    private string path = Application.streamingAssetsPath + fileName;
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
    public void ReadJson()
    {
        string asd = File.ReadAllText(path);
        units = JsonHelper.FromJson<UnitData>(asd);
    }

    public void SpawnFromJson()
    {
        ReadJson();
        UnitsSpawner.getInstance().SpawnUnits(units);
    }
    #endregion
}
