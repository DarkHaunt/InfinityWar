using UnityEngine;

namespace InfinityGame.Fractions
{
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
        [SerializeField] private WarrioirSpawner.SpawnData _warrioirSpawnSettings;


        public string Tag => _tag;
        public WarrioirsPickStrategy WarrioirPickStrategy => _warrioirPickStrategy;
        public WarrioirSpawner.SpawnData WarrioirSpawnSettings => _warrioirSpawnSettings;
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
    }
}
