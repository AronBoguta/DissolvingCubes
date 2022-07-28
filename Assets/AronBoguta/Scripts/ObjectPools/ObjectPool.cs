using System;
using System.Collections.Generic;
using UnityEngine;

namespace AronBoguta
{
    public class ObjectPool<T> : MonoBehaviour where T: MonoBehaviour
    {
        [SerializeField] protected GameObject _objPrefab;
        private Stack<T> _pool;

        private readonly int _initialCount = 10;

        private bool _initialized = false;

        protected void Awake()
        {
            Init();
        }

        /// <summary>Retrieves an item from stack. If the stack is empty it is grown by instantiating
        /// a new element</summary>
        public T Get(Action<T> initFunc)
        {
            if (_pool == null)
            {
                Init();
            }
            
            T item = _pool.Peek();

            if (item != null)
            {
                item = _pool.Pop();
                item.gameObject.SetActive(true);
            }
            else
            {
                item = GameObject.Instantiate(_objPrefab, transform, false).GetComponent<T>();
            }
            initFunc?.Invoke(item);
            
            return item;
        }
        
        /// <summary>
        ///  The Release function returns an object to pool. There is no hard requirement on the object being 
        ///  acquired from the pool in the first place.
        /// </summary>
        /// <param name="item"></param>
        public void Release(T item)
        {
            item.gameObject.SetActive(false);
            _pool.Push(item);
        }

        private void Init()
        {
            if (!_initialized)
            {
                _pool = new Stack<T>(_initialCount);
                for (int i = 0; i < _initialCount; i++)
                {
                    var newObj = GameObject.Instantiate(_objPrefab, transform, false);
                    newObj.SetActive(false);
                    _pool.Push(newObj.GetComponent<T>());
                }

                _initialized = true;
            }
        }
    }
}