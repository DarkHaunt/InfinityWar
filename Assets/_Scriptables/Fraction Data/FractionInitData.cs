using UnityEngine;
using InfinityGame.Spawning;



namespace InfinityGame.FractionsData
{
    using WarrioirSpawnSettings = WarrioirSpawner.SpawnerInitData;



    /// <summary>
    /// Data, that determines fraction parameters in the game
    /// </summary>
    [CreateAssetMenu(fileName = "Fraction", menuName = "Data/New Fraction", order = 52)]
    public class FractionInitData : ScriptableObject
    {
        [SerializeField] private string _fractionTag;

        [Range(1, 50)]
        [SerializeField] private int _warrioirMaxCount;

        [Header("--- Townhall Settings ---")]
        [SerializeField] private BuildingInitData _townHallBuildingData;

        [Header("--- Barrack Settings ---")]
        [SerializeField] private BuildingInitData _barrackBuildingData;

        [Header("--- Barrack Spawn Settings ---")]
        [SerializeField] private WarrioirSpawnSettings _barracksWarrioirSpawnSettings;



        public string FractionTag => _fractionTag;
        public int WarrioirMaxLimit => _warrioirMaxCount;
        public WarrioirSpawnSettings BarracksWarrioirSpawnSettings => _barracksWarrioirSpawnSettings;
        public BuildingInitData BarrackBuildingData => _barrackBuildingData;
        public BuildingInitData TownHallBuildingData => _townHallBuildingData;



        [System.Serializable]
        public struct BuildingInitData
        {
            [SerializeField] private string _name;
            [SerializeField] private Sprite _buildingSprite;
            [SerializeField] private float _buildingHealthPoints;


            public string Name => _name;
            public Sprite BuildingSprite => _buildingSprite;
            public float BuildingHealthPoints => _buildingHealthPoints;
        }
    }
}
