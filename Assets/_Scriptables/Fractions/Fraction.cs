using UnityEngine;
using InfinityGame.Strategies.WarrioirSpawnStrategies;

namespace InfinityGame.Fractions
{
    using WarrioirSpawnSettings = WarrioirSpawner.SpawnData;
    using FractionType = FractionHandler.FractionType;

    [CreateAssetMenu(fileName = "Fraction", menuName = "Data/New Fraction", order = 52)]
    public class Fraction : ScriptableObject
    {
        [SerializeField] private FractionType _fraction;
        [SerializeField] private int _warrioirMaxCount;

        [Space(10f)]
        [SerializeField] private BuildingData _townHallBuildingData;

        [Space(10f)]
        [SerializeField] private BuildingData _barrackBuildingData;
        [SerializeField] private WarrioirsPickStrategy _warrioirPickStrategy;

        [Space(10f)]
        [SerializeField] private WarrioirSpawnSettings _warrioirSpawnSettings;



        public FractionType FractionType => _fraction;
        public int WarrioirMaxLimit => _warrioirMaxCount;
        public WarrioirsPickStrategy WarrioirPickStrategy => _warrioirPickStrategy;
        public WarrioirSpawnSettings WarrioirSpawnSettings => _warrioirSpawnSettings;
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

        public struct FractionBuildingData
        {
            public readonly FractionType Fraction;
            public readonly string Name;
            public readonly BuildingData BuildingData;


            public FractionBuildingData(FractionType fraction, BuildingData buildingData)
            {
                Fraction = fraction;
                BuildingData = buildingData;
                Name = buildingData.Name;
            }

        }
    }
}
