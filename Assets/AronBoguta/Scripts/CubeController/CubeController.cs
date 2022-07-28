using System;
using UnityEngine;

namespace AronBoguta
{
    [RequireComponent(typeof(Rigidbody))]
    public class CubeController : MonoBehaviour
    {
        [SerializeField] protected float _rotationSpeed;

        public Action<float> RotationSpeedChanged;
        protected Rigidbody _rigidbody;

        protected void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected void FixedUpdate()
        {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(Time.deltaTime * _rotationSpeed* Vector3.up));
        }

        /// <summary>
        /// This function is only used in editor to keep updating rotation speed when changed by the user
        /// </summary>
        private void OnValidate()
        {
            RotationSpeedChanged?.Invoke(_rotationSpeed);
        }
    }    
}
