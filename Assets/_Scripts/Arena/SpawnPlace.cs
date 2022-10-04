using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.Arena
{
    public class SpawnPlace : MonoBehaviour
    {
        [SerializeField] private Transform _townHallPointTransform;
        [SerializeField] private List<Transform> _barracksPointTransforms = new List<Transform>();


        public Vector3 TownHallSpawnPointPosition => _townHallPointTransform.position;
        public List<Vector3> BarracksSpawnPointsTransforms
        {
            get
            {
                var positions = new List<Vector3>(_barracksPointTransforms.Count);

                foreach (var transform in _barracksPointTransforms)
                    positions.Add(transform.position);

                return positions;
            }
        }

    } 
}
