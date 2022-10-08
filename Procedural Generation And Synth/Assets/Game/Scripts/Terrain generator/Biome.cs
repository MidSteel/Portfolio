using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Terrain/Biome")]
public class Biome : ScriptableObject
{
    public TerrainType[] _biomes;
}
