using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUnitPrefab : MonoBehaviour
{
    [SerializeField] GameConfigSO config;
    [SerializeField] FishTableSO fishTable;
    [SerializeField]SizeTableSO sizeTable;
    [SerializeField] TMP_Text textobj;
    [SerializeField] Image fishImage;

    private void Reset(){
        textobj = GetComponentInChildren<TMP_Text>();
    }
    public void InitPrefab(int fishId, bool isFever){
        Debug.Log(fishTable.Get(fishId)+" "+fishTable.Get(fishId).displayName);
        SizeClass size = fishTable.Get(fishId).size;
        int score = sizeTable.Get(size).points+fishTable.Get(fishId).basePoints;
        textobj.text = fishTable.Get(fishId).displayName;
        textobj.text += "\n"+score.ToString()+"점";
        if(isFever) textobj.text += (" X" + config.feverMult.ToString());
    }
}
