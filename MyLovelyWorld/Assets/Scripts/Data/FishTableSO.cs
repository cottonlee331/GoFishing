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
        public SizeClass size;
        public string displayName;
        public int basePoints;
        public int weight;
    }

    public Row[] rows;

    // TODO: id로 Row 찾기
    public Row Get(int id) => throw new NotImplementedException();
}
