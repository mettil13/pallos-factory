#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataSerializerFromJson : MonoBehaviour
{
    public TextAsset textAssetData;
    public List<PlaceableSO> placeablesSO;

    private void Start() {
        DeserializeJSONToSO(textAssetData.text);
    }

    public void DeserializeJSONToSO(string json) {
        List<string> dataList = FromJSONListToStringList(json);
        List<Dictionary<string, string>> rawData = new List<Dictionary<string, string>>();
        List<NamePeaker> dataNames = new List<NamePeaker>();
        foreach(string str in dataList) {
            rawData.Add(JsonUtility.FromJson<Dictionary<string, string>>(str));
            dataNames.Add(JsonUtility.FromJson<NamePeaker>(str));
        }

        Debug.Log(dataNames[0].name);

        StructureSO myScriptableObject = ScriptableObject.CreateInstance<StructureSO>();
        AssetDatabase.CreateAsset(myScriptableObject, "Assets/NewSO.asset"); // TODO: mark it as dirty
        JsonUtility.FromJsonOverwrite(dataList[0], myScriptableObject);
    }

    private List<string> FromJSONListToStringList(string json) {
        if (json[0]  != '[' || json[json.Length - 1] != ']') {
            return new List<string> { json };
        }
        
        List<string> list = new List<string>();
        int curledBracketCounter = 0;
        int startOfNewObject = 1;
        for(int i = 1; i < json.Length; i++) {
            if (json[i] == '{') {
                if(curledBracketCounter == 0) {
                    startOfNewObject = i;
                }
                curledBracketCounter++;
            }
            else if (json[i] == '}') {
                curledBracketCounter--;
                if(curledBracketCounter == 0) {
                    string obj = json.Substring(startOfNewObject, i - startOfNewObject + 1);
                    list.Add(obj);
                }
            }
        }
        return list;
    }
}
[System.Serializable]
public class NamePeaker {
    public string name;
}
#endif