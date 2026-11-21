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
        var fish = fishTable.Get(fishId);
        Debug.Log(fish+" "+fish.displayName);
        SizeClass size = fish.size;
        fishImage.sprite = fish.icon;
        int score = sizeTable.Get(size).points+fish.basePoints;
        textobj.text = $"{fish.displayName}\n{score}점";
        if(isFever) textobj.text += $" X{config.feverMult}";
    }
}
