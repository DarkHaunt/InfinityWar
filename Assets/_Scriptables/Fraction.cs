using System.Collections;
using System.Collections.Generic;
using InfinityGame.SpawnStrategies;
using UnityEngine;


[CreateAssetMenu(fileName = "Fraction", menuName = "Data/New Fraction", order = 52)]
public class Fraction : ScriptableObject
{
    [SerializeField] private string _name;

    [SerializeField] private Sprite _townHallSprite;
    [SerializeField] private float _townHallHealth;

    [SerializeField] private Sprite _barrackSprite;
    [SerializeField] private float _barrackHealth;

    [SerializeField] private IWarrioirChoseStrategy.SpawnType _spawnTypea;
    [SerializeField] private WarrioirSpawner.SpawnData _barracksSpawnData;

    [SerializeField] private string _tag;

    [SerializeField] private List<Warrior> _fractionWarriours;




    public string Name => _name;
    public Sprite TownHallSprite => _townHallSprite;
    public float TownHallHealth => _townHallHealth;
    public Sprite BarrackSprite => _barrackSprite;
    public float BarrackHealth => _barrackHealth;
    public List<Warrior> FractionWarriors => _fractionWarriours;
    public IWarrioirChoseStrategy.SpawnType SpawnStrategyType => _spawnTypea;
    public WarrioirSpawner.SpawnData BarracksSpawnData => _barracksSpawnData;
    public string Tag => _tag;
}
