using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillToggleUnitPrefab : MonoBehaviour
{
    [SerializeField] SkillTableSO skillTable;
    [SerializeField] Image skillImg;
    [SerializeField] TMP_Text skillName;

    public int skillId {get; private set;}

    public event Action<int> ValueChanged;

    private void Reset(){
        skillImg = transform.Find("Icon").GetComponent<Image>();
        skillName = GetComponentInChildren<TMP_Text>();
    }

    public void InitPrefab(int id, ToggleGroup toggleGroup){
        skillId = id;
        skillImg.sprite = skillTable.Get(skillId).icon;
        skillName.text = skillTable.Get(skillId).displayName;
        GetComponent<Toggle>().group = toggleGroup;
        GetComponent<Toggle>().isOn = false;
    }
    
    public void OnValueChange(){
        if (GetComponent<Toggle>().isOn)
        {
            ValueChanged?.Invoke(skillId);
        }
    }
}
