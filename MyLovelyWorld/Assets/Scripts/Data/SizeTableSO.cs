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
        public float share;          // 등장 비율 (소 66 / 중 28 / 대 6)
        public float waitMin;
        public float waitMax;
        public float gaugeGain;
        //public float difficulty;     // 난이도 보정 d
        //public float shadowRatio;
    }

    public Row[] rows;

    // TODO: size로 Row 찾기
    public Row Get(SizeClass size) => throw new NotImplementedException();
}
