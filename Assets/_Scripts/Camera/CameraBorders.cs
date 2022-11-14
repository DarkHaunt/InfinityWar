using UnityEngine;



namespace InfinityGame.Camera
{
    public class CameraBorders : MonoBehaviour
    {
        [SerializeField] private Transform _leftBottomBorderCornerObject;
        [SerializeField] private Transform _rightTopBorderCornerObject;



        public Vector2 LeftBottomCorner => _leftBottomBorderCornerObject.position;
        public Vector2 RightTopCorner => _rightTopBorderCornerObject.position;



        public bool IsPointOutOfLeftBottomBorder(Vector2 point, out Vector2 deviation)
        {
            deviation = Vector2.zero;

            bool isPointXLessThanBorderX = point.x < LeftBottomCorner.x;
            bool isPointYLessThanBorderY = point.y < LeftBottomCorner.y;

            if (isPointXLessThanBorderX)
                deviation.x = LeftBottomCorner.x - point.x;

            if (isPointYLessThanBorderY)
                deviation.y = LeftBottomCorner.y - point.y;


            return isPointXLessThanBorderX || isPointYLessThanBorderY;
        }


        public bool IsPointOutOfRightTopBorder(Vector2 point, out Vector2 deviation)
        {
            deviation = Vector2.zero;

            bool isPointXMoreThanBorderX = point.x > RightTopCorner.x;
            bool isPointYMoreThanBorderY = point.y > RightTopCorner.y;

            if (isPointXMoreThanBorderX)
                deviation.x = RightTopCorner.x - point.x;

            if (isPointYMoreThanBorderY)
                deviation.y = RightTopCorner.y - point.y;


            return isPointXMoreThanBorderX || isPointYMoreThanBorderY;
        }
    }
}
