using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain/Terrain Type")]
public class TerrainType : ScriptableObject
{
    public TerrainTypeData[] terrainTypes;

    [ContextMenu("Sort Array")]
    public void SortTerrainDataArray()
    {
        terrainTypes = terrainTypes.OrderBy(x => x.typeNumber).ToArray();
    }
}

[System.Serializable]
public struct TerrainTypeData
{
    public int typeNumber;
    public Color typeColor;
    public float typeHeight;
}
