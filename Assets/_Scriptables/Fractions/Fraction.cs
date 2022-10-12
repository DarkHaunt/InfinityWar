using UnityEngine;


[CreateAssetMenu(fileName = "Fraction", menuName = "Data/New Fraction", order = 52)]
public class Fraction : ScriptableObject
{
    [SerializeField] private string _tag;

    [Header("Town Hall Settings")]
    [Space(10f)]
    [SerializeField] private Sprite _townHallSprite;
    [SerializeField] private float _townHallHealth;

    [Header("Barrack Settings")]
    [Space(10f)]
    [SerializeField] private Sprite _barrackSprite;
    [SerializeField] private float _barrackHealth;
    [SerializeField] private WarrioirsPickStrategy _warrioirPickStrategy;
    [SerializeField] private WarrioirSpawner.SpawnData _barracksSpawSettings;




    public string Tag => _tag;
    public Sprite TownHallSprite => _townHallSprite;
    public float TownHallHealth => _townHallHealth;
    public Sprite BarrackSprite => _barrackSprite;
    public float BarrackHealth => _barrackHealth;
    public WarrioirsPickStrategy WarrioirPickStrategy => _warrioirPickStrategy;
    public WarrioirSpawner.SpawnData BarracksSpawSettings => _barracksSpawSettings;
}
