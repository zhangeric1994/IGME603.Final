using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour {
    private GameData data;
    private string dataPath;

    private static SaveSystem saveSystem;

    public static SaveSystem getInstance() { return saveSystem; }

    void Awake(){
        saveSystem = this;
        data = new GameData();
    }
    void Start(){
        dataPath = Path.Combine(Application.persistentDataPath, "GameData.txt");
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.S)){
            Debug.Log("Press S");
            SaveData(data, dataPath);
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("Press L");
            data = LoadData(dataPath);
        }
    }

    static void SaveData(GameData data, string dataPath){
        string jsonString = JsonUtility.ToJson(data);

        using (StreamWriter streamWriter = File.CreateText(dataPath)){
            streamWriter.Write(jsonString);
        }
    }

    static GameData LoadData(string dataPath){
        using (StreamReader streamReader = File.OpenText (dataPath)){
            string jsonString = streamReader.ReadToEnd ();
            return JsonUtility.FromJson<GameData> (jsonString);
        }
    }

    public void SetData(GameData _data){
        data = _data;
    }

    public GameData GetData(){
        return data;
    }
}
