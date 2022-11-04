using InfinityGame.Fractions;
using InfinityGame.Spawning;

namespace InfinityGame.GameEntities
{
    public class Barrack : Building
    {
        private WarrioirSpawner _warriorSpawner;



        public void Initialize(Fraction fraction, Fraction.BuildingData fractionBuildingData)
        {
            Initialize(fraction.FractionType, fractionBuildingData);

            _warriorSpawner = gameObject.AddComponent<WarrioirSpawner>();
            _warriorSpawner.Initialize(fraction);

            OnZeroHealth += _warriorSpawner.DeactivateSpawning;
        }
    }
}