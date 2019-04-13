using System.Collections.Generic;
using UnityEngine;

public class TableDataManager
{
    public static readonly TableDataManager singleton = new TableDataManager();

    private Dictionary<string, TableDataPage> dataPages = new Dictionary<string, TableDataPage>();

    private TableDataManager()
    {
        Initialize();
    }

    /// <summary>
    /// Read all data from tables
    /// </summary>
    public void Initialize()
    {
        foreach (TableDataPage dataPage in Resources.LoadAll<TableDataPage>("TableData"))
            dataPages.Add(dataPage.name, dataPage);
    }

    /// <summary>
    /// Get data at a specific index in the given data page
    /// </summary>
    /// <typeparam name="T"> The type of the data </typeparam>
    /// <param name="pageName"> The name of the data page containing the desired data entry </param>
    /// <param name="index"> The index of the data entry to get </param>
    /// <returns></returns>
    public T GetData<T>(string pageName, int index) where T : TableDataEntry
    {
        return ((TableDataPage<T>)dataPages[pageName])[index];
    }
}
