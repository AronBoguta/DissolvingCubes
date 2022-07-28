using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AronBoguta
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private int _spawnCount;
        [SerializeField] private float _maxSmallCubeSize = 0.4f;
        [SerializeField] private float _maxCubeLifetime = 4;
        [SerializeField] private float _forceFactor = 2;
        [SerializeField] private CubeController _boundingCube;

        [SerializeField] private Button _assignButton;
        [SerializeField] private InputField _currentKeyInputField;
        [SerializeField] private GameObject _promptPanel;
        
        [SerializeField] private CommandManager _commandManager;
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private ParticleSystem _particleSystem;


        private SmallCubePool _cubePool;
        private float _lowerBound;
        private float _upperBound;
        private KeyCode _currentKey;
        private List<SmallCubeController> activeCubes = new List<SmallCubeController>();
        private bool _isAwaitingKeyPress = false;

        private void Awake()
        {
            //This call would be typically replaced by DI
            _cubePool = Object.FindObjectOfType<SmallCubePool>();
            _assignButton.onClick.AddListener(OnAssignButton);

            _lowerBound = (-_boundingCube.transform.localScale.x + _maxSmallCubeSize) / 2;
            _upperBound = (_boundingCube.transform.localScale.x - _maxSmallCubeSize) / 2;
            
            
            for (int i = 0; i < _spawnCount; i++)
            {
                var cube = SpawnCubeFromPool();
            }
        }

        private void OnAssignButton()
        {
            if (!_isAwaitingKeyPress)
            {
                ShowPromptPanel();
                _isAwaitingKeyPress = true;
                StartCoroutine(_inputManager.AwaitKeypress(OnKeyReceived));
            }
        }

        private void OnKeyReceived(KeyCode key)
        {
            _currentKey = key;
            _isAwaitingKeyPress = false;
            HidePromptPanel();
            _commandManager.BindCommandToKey(_currentKey, new FireOutwardsCommand(activeCubes, _boundingCube.transform.position, _forceFactor, _particleSystem));
            _currentKeyInputField.SetTextWithoutNotify(_currentKey.ToString());
        }

        public void ShowPromptPanel()
        {
            _promptPanel.SetActive(true);
        }

        public void HidePromptPanel()
        {
            _promptPanel.SetActive(false);
        }

        private SmallCubeController SpawnCubeFromPool()
        {
            var cube = _cubePool.Get(c =>
            {
                var position = GetRandomPosInBoundingBox();
                c.Init(_boundingCube, position, Random.ColorHSV(), Random.value * _maxSmallCubeSize, Random.Range(1, _maxCubeLifetime));
            });
            cube.OnRelease = () =>
            {
                _cubePool.Release(cube);
                if (!activeCubes.Remove(cube))
                {
                    Debug.LogError("Failed to release cube from list");
                }
                cube = SpawnCubeFromPool();
            };

            activeCubes.Add(cube);
            return cube;
        }
        
        private Vector3 GetRandomPosInBoundingBox()
        {
            return  _boundingCube.transform.rotation * new Vector3(RandomValInBounds(), RandomValInBounds(), RandomValInBounds())  
                    + _boundingCube.transform.position;
        }

        private float RandomValInBounds()
        {
            return Random.Range(_lowerBound, _upperBound);
        }
    }
}

