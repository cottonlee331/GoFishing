using System;
using UnityEngine;

// sizes.csv 대응.
[CreateAssetMenu(menuName = "Fishing/Size Table", fileName = "SizeTable")]
public class SizeTableSO : ScriptableObject
{
    [Serializable]
    public class Row
    {
        public SizeClass size;
        public int share;          // 등장 비율 (소 66 / 중 28 / 대 6)
        public float waitMin;
        public float waitMax;
        public int gaugeGain;
        public int points;          // 기본 점수
        //public float difficulty;     // 난이도 보정 d
        //public float shadowRatio;
    }

    public Row[] rows;

    // TODO: size로 Row 찾기
    public Row Get(SizeClass size)
    {
        foreach (var row in rows)
        {
            if (row.size == size)
                return row;
        }
        throw new Exception($"SizeTableSO.Get({size}) not found");
    }
}
