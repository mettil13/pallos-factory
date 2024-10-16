using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DataSerializerFromJson : MonoBehaviour
{
    public TextAsset textAssetData;
    public List<Dictionary<string, string>> rawData = new List<Dictionary<string, string>>();

    private void Start() {
        DeserializeJSONToSO(textAssetData.text);
    }

    public void DeserializeJSONToSO(string json) {
        List<string> dataList = FromJSONListToStringList(json);
        foreach(string str in dataList) {
            rawData.Add(JsonUtility.FromJson<Dictionary<string, string>>(str));
        }

        StructureSO myScriptableObject = ScriptableObject.CreateInstance<StructureSO>();
        AssetDatabase.CreateAsset(myScriptableObject, "Assets/NewSO.asset");
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
