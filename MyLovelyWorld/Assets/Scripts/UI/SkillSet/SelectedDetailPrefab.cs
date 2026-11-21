using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedDetailPrefab : MonoBehaviour
{
    [SerializeField] SkillTableSO skillTable;

    [SerializeField] Image skillIcon;
    [SerializeField] TMP_Text skillDescription;

    private void Reset(){
        skillIcon = transform.Find("Icon").GetComponent<Image>();
        skillDescription = GetComponentInChildren<TMP_Text>();
    }

    public void SetSkillDetail(int id)
    {
        var skillInfo = skillTable.Get(id);

        skillIcon.sprite = skillInfo.icon; 
        skillDescription.text =  $"<{skillInfo.displayName}>\n{skillInfo.description}";
    }
}
