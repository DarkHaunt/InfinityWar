using UnityEngine;



namespace InfinityGame.Camera
{
    public class CameraBorders : MonoBehaviour
    {
        [SerializeField] private Transform _leftBottomBorderCornerObject;
        [SerializeField] private Transform _rightTopBorderCornerObject;



        public Vector2 LeftBottomCorner => _leftBottomBorderCornerObject.position;
        public Vector2 RightTopCorner => _rightTopBorderCornerObject.position;
    } 
}
