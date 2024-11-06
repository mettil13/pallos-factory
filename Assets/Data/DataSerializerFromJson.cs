#if UNITY_EDITOR
using System;
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
        List<NamePeaker> dataNames = new List<NamePeaker>();
        for(int i = 0; i < dataList.Count; i++) {
            NamePeaker name = JsonUtility.FromJson<NamePeaker>(dataList[i]);
            dataNames.Add(name);
            try {
                //Debug.Log(name.name.Substring(0, 1).ToUpper() + name.name.Substring(1, name.name.Length - 1) + "SO");
                Type t = Type.GetType(name.name.Substring(0, 1).ToUpper() + name.name.Substring(1, name.name.Length - 1) + "SO");
                if(t == null) {
                    t = typeof(StructureSO);
                }

                if(name.name == "turret" || name.name == "autoCleaner") {
                    t = typeof(PalloBotSO);
                }


                var myScriptableObject = AssetDatabase.LoadAssetAtPath("Assets/Data/StructuresFromJson/" + name.name + ".asset", t);
                if(myScriptableObject == null) {
                    myScriptableObject = ScriptableObject.CreateInstance(t);
                    AssetDatabase.CreateAsset(myScriptableObject, "Assets/Data/StructuresFromJson/" + name.name + ".asset");
                }
                JsonUtility.FromJsonOverwrite(dataList[i], myScriptableObject);
                EditorUtility.SetDirty(myScriptableObject);
                AssetDatabase.SaveAssets();
            }
            catch {

            }
        }

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