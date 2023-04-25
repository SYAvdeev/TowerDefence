using Data.Config;
using UnityEngine;

namespace Presentation
{
    public class GameplayScaler : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private SpriteRenderer _backgroundSprite;
        [SerializeField] private MainConfig _mainConfig;

        public float CalculateScale()
        {
            float sizeY = _backgroundSprite.size.y;
            _camera.orthographicSize = sizeY / 2f;
            return sizeY / _mainConfig.GameplayConfig.MapYSize;
        }
    }
}