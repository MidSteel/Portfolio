using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration.Scriptables
{
    [CreateAssetMenu(menuName = "Terrain Generation/Biome Data")]
    public class TerrainBiomeData : ScriptableObject
    {
        [SerializeField] private BiomeData[] _biomeData;

        public BiomeData[] BiomeData { get { return _biomeData; } }
    }

    [System.Serializable]
    public struct BiomeData 
    {
        public string name;
        public Texture2D texture;
        public float height;
    }
}