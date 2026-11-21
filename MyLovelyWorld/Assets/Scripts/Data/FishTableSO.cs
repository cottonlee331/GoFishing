using System;
using UnityEngine;

// fish.csv 대응.
[CreateAssetMenu(menuName = "Fishing/Fish Table", fileName = "FishTable")]
public class FishTableSO : ScriptableObject
{
    [Serializable]
    public class Row
    {
        public int id;
        public Sprite icon;
        public SizeClass size;
        public string displayName;
        public int basePoints;
        public int weight;
    }

    public Row[] rows;

    // TODO: id로 Row 찾기
    public Row Get(int id){
        foreach (var row in rows)
        {
            if (row.id == id)
                return row;
        }
        throw new Exception($"FishTableSO.Get({id}) not found");
    }

    public Row[] GetBySize(SizeClass size){
        return Array.FindAll(rows, row => row.size == size);
    }
}
