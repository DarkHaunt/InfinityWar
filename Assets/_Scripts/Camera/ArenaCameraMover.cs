using UnityEngine;



namespace InfinityGame.Camera
{
    using UnityCamera = UnityEngine.Camera;

    [RequireComponent(typeof(UnityCamera))]
    public class ArenaCameraMover : MonoBehaviour
    {
        [SerializeField] private CameraBorders _cameraBorders;
        [SerializeField] private UnityCamera _camera;

        private Vector2 _screenFirstPixelPosition; // Left bottom pixel
        private Vector2 _screenLastPixelPosition; // Right top pixel



        /// <summary>
        /// Checks if camera out of borders and makes to fit in them, if so
        /// </summary>
        private void ToFitInBorders()
        {
            Vector3 currentCameraLeftBottomCornerPosition =
                        _camera.ScreenToWorldPoint(_screenFirstPixelPosition);

            if (_cameraBorders.IsPointOutOfLeftBottomBorder(currentCameraLeftBottomCornerPosition, out Vector2 leftBottomDeviation))
                transform.Translate(leftBottomDeviation);

            Vector3 currentCameraRightTopCornerPosition =
            _camera.ScreenToWorldPoint(_screenLastPixelPosition);

            if (_cameraBorders.IsPointOutOfRightTopBorder(currentCameraRightTopCornerPosition, out Vector2 rightTopDeviation))
                transform.Translate(rightTopDeviation);
        }



        private void Awake()
        {
            _screenFirstPixelPosition = new Vector2(0f, 0f);
            _screenLastPixelPosition = new Vector2(Screen.width, Screen.height);
        }

        private void Update()
        {
            ToFitInBorders();
        }
    }
}
