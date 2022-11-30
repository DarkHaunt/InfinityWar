using InfinityGame.FractionsData;
using InfinityGame.Spawning;



namespace InfinityGame.GameEntities.Buildings
{
    /// <summary>
    /// A building, which spawns fraction warriors
    /// </summary>
    public class Barrack : Building
    {
        private WarrioirSpawner _warriorSpawner;



        public override void Initialize(FractionInitData fractionData, FractionInitData.BuildingInitData fractionBuildingData)
        {
            base.Initialize(fractionData, fractionBuildingData);

            _warriorSpawner = gameObject.AddComponent<WarrioirSpawner>();
            _warriorSpawner.Initialize(fractionData);

            OnDie += _warriorSpawner.DeactivateSpawner;
        }
    }
}
