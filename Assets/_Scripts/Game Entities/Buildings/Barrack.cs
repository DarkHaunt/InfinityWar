using InfinityGame.Fractions;
using InfinityGame.Spawning;



namespace InfinityGame.GameEntities
{
    public class Barrack : Building
    {
        private WarrioirSpawner _warriorSpawner;



        public override void Initialize(FractionInitData fractionData, FractionInitData.BuildingInitData fractionBuildingData)
        {
            base.Initialize(fractionData, fractionBuildingData);

            _warriorSpawner = gameObject.AddComponent<WarrioirSpawner>();
            _warriorSpawner.Initialize(fractionData);

            OnDie += () =>
            {
                //print("Barracks stop spawning");
                _warriorSpawner.DeactivateSpawner();
            };
        }
    }
}
