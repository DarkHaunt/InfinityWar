using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Arena
{
    public class SpawnPlace : MonoBehaviour
    {
        [SerializeField] private Transform _townHallPointTransform;
        [SerializeField] private List<Transform> _barracksPointTransforms = new List<Transform>();



        public Vector3 TownHallSpawnPointPosition => _townHallPointTransform.position;
        public IEnumerable<Vector3> BarracksSpawnPointsPositions
        {
            get
            {
                foreach (var barrackTransform in _barracksPointTransforms)
                    yield return barrackTransform.position;
            }
        }
    } 
}
