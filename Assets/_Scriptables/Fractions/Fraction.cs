using UnityEngine;
using InfinityGame.Spawning;

namespace InfinityGame.Fractions
{
    using WarrioirSpawnSettings = WarrioirSpawner.SpawnData;

    [CreateAssetMenu(fileName = "Fraction", menuName = "Data/New Fraction", order = 52)]
    public class Fraction : ScriptableObject
    {
        [SerializeField] private string _fractionTag;
        [SerializeField] private int _warrioirMaxCount;

        [Space(10f)]
        [SerializeField] private BuildingData _townHallBuildingData;

        [Space(10f)]
        [SerializeField] private BuildingData _barrackBuildingData;

        [Space(10f)]
        [SerializeField] private WarrioirSpawnSettings _barracksWarrioirSpawnSettings;



        public string FractionTag => _fractionTag;
        public int WarrioirMaxLimit => _warrioirMaxCount;
        public WarrioirSpawnSettings BarracksWarrioirSpawnSettings => _barracksWarrioirSpawnSettings;
        public BuildingData BarrackBuildingData => _barrackBuildingData;
        public BuildingData TownHallBuildingData => _townHallBuildingData;



        [System.Serializable]
        public struct BuildingData
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
