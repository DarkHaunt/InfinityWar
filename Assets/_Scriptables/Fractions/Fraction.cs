using UnityEngine;
using InfinityGame.Strategies.WarrioirSpawnStrategies;

namespace InfinityGame.Fractions
{
    using WarrioirSpawnSettings = WarrioirSpawner.SpawnData;

    [CreateAssetMenu(fileName = "Fraction", menuName = "Data/New Fraction", order = 52)]
    public class Fraction : ScriptableObject
    {
        [SerializeField] private string _tag;

        [Space(10f)]
        [SerializeField] private BuildingData _townHallBuildingData;

        [Space(10f)]
        [SerializeField] private BuildingData _barrackBuildingData;
        [SerializeField] private WarrioirsPickStrategy _warrioirPickStrategy;

        [Space(10f)]
        [SerializeField] private WarrioirSpawnSettings _warrioirSpawnSettings;


        public string Tag => _tag;
        public WarrioirsPickStrategy WarrioirPickStrategy => _warrioirPickStrategy;
        public WarrioirSpawnSettings WarrioirSpawnSettings => _warrioirSpawnSettings;
        public BuildingData BarrackBuildingData => _barrackBuildingData;
        public BuildingData TownHallBuildingData => _townHallBuildingData;


        [System.Serializable]
        public struct BuildingData
        {
            [SerializeField] private Sprite _buildingSprite;
            [SerializeField] private float _buildingHealthPoints;

            public Sprite BuildingSprite => _buildingSprite;
            public float BuildingHealthPoints => _buildingHealthPoints;
        }

        public struct FractionBuildingData
        {
            public readonly string FractionTag;
            public readonly string Name;
            public readonly BuildingData BuildingData;


            public FractionBuildingData(string fractionTag, string name, BuildingData buildingData)
            {
                FractionTag = fractionTag;
                Name = name;
                BuildingData = buildingData;
            }

        }
    }
}
