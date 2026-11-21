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
    public void InitPrefab(int fishId, float mult, int add){
        var fish = fishTable.Get(fishId);
        Debug.Log(fish+" "+fish.displayName);
        SizeClass size = fish.size;
        fishImage.sprite = fish.icon;
        int score = sizeTable.Get(size).points+fish.basePoints;
        textobj.text = $"{fish.displayName}\n{score}";
        if(mult!=1) textobj.text += $" X{mult}";
        if(add!=0) textobj.text += $" +{add}";
    }
}
