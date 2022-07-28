using System;
using UnityEditor;
using UnityEngine;

namespace AronBoguta
{
    [RequireComponent(typeof(MeshRenderer))]
    public class SmallCubeController : CubeController
    {
        [SerializeField]  private float _timeToLive;
        public Action OnRelease;
        
        private float _lifeTime;
        private float _currentCutoff;
        
        private MeshRenderer _meshRenderer;

        private string CutoffPropertyName = "AlphaCutoffValue";
        private string AlbedoPropertyName = "BaseColor";
        private string EdgeColorPropertyName = "EdgeColor";
        private MaterialPropertyBlock _propBlock;

        private void Awake()
        {
            base.Awake();
            _propBlock = new MaterialPropertyBlock();
            _meshRenderer = GetComponent<MeshRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// The Update function updates rendering params each frame. Once the life of the object has ended,
        /// the Kill function is called which returns it to the object pool
        /// </summary>
        private void Update()
        {
            if (_timeToLive > 0)
            {
                _currentCutoff = Mathf.Min(1, _currentCutoff + Time.deltaTime / _lifeTime);
                _propBlock.SetFloat(CutoffPropertyName, _currentCutoff);
                _meshRenderer.SetPropertyBlock(_propBlock);
                _timeToLive -= Time.deltaTime; 
            }
            else
            {
                Kill();
            }
        }

        private void FixedUpdate()
        {
            base.FixedUpdate();
        }

        /// <summary>
        /// The Init function initializes class parameters with given values.
        /// It also listens for a change in the main cube's rotation speed to stay in synch.
        /// </summary>
        public void Init(CubeController boundingCube, Vector3 pos, Color col, float scale, float lifetime)
        {
            SetRenderingParams(col);
            boundingCube.RotationSpeedChanged += OnSpeedChanged;

            _lifeTime = lifetime;
            _timeToLive = _lifeTime;

            transform.position = pos;
            transform.localScale = new Vector3(scale, scale, scale);
        }

        public void ShootOutwards(Vector3 center, float forceFactor)
        {
            var direction = (transform.position - center).normalized;
            _rigidbody.AddForce(direction * forceFactor, ForceMode.Impulse);
        }

        public void Kill()
        {
            OnRelease.Invoke();
        }

        public void OnSpeedChanged(float speed)
        {
            _rotationSpeed = speed;
        }
        
        private void SetRenderingParams(Color col)
        {
            _meshRenderer.material.SetColor(AlbedoPropertyName, col);
            _meshRenderer.material.SetColor(EdgeColorPropertyName, col * 2);
            _currentCutoff = _meshRenderer.material.GetFloat(CutoffPropertyName);
            _meshRenderer.material.color = col;
        }
    }
}