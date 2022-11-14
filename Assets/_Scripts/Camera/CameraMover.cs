using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


namespace InfinityGame.Camera
{
    using UnityCamera = UnityEngine.Camera;

    [RequireComponent(typeof(UnityCamera))]
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private CameraBorders _cameraBorders;
        [SerializeField] private UnityCamera _camera;



        private void ExecuteBorder()
        {
            Vector3 currentCameraLeftBottomCornerPosition =
                        _camera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));

            var borderDifference = Vector3.zero;

            if (currentCameraLeftBottomCornerPosition.x < _cameraBorders.LeftBottomCorner.x)
                borderDifference.x = _cameraBorders.LeftBottomCorner.x - currentCameraLeftBottomCornerPosition.x;

            if (currentCameraLeftBottomCornerPosition.y < _cameraBorders.LeftBottomCorner.y)
                borderDifference.y = _cameraBorders.LeftBottomCorner.y - currentCameraLeftBottomCornerPosition.y;

            transform.position += borderDifference;

            borderDifference = Vector3.zero;

            Vector3 currentCameraRightTopCornerPosition =
            _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

            if(currentCameraRightTopCornerPosition.x > _cameraBorders.RightTopCorner.x)
                borderDifference.x = currentCameraRightTopCornerPosition.x - _cameraBorders.RightTopCorner.x;

            if (currentCameraRightTopCornerPosition.y > _cameraBorders.RightTopCorner.y)
                borderDifference.y = currentCameraRightTopCornerPosition.y - _cameraBorders.RightTopCorner.y;

            transform.position -= borderDifference;
      
        }

        private void Update()
        {
            ExecuteBorder();
        }
    }
}
