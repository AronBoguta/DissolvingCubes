using System;
using System.Collections;
using UnityEngine;

namespace AronBoguta
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private CommandManager _commandManager;
        
        private bool _awaitingKeyPress = false;
        private KeyCode _lastKeyPressed;

        /// <summary>
        /// The Update function is checking whether any of the mapped keys in command manager have been pressed.
        /// This way we can avoid naive checking for any key press and then asking the command manager to handle it
        /// </summary>
        private void Update()
        {
            if (!_awaitingKeyPress)
            {
                foreach (var key in _commandManager.KeyToCommand.Keys)
                {
                    if (Input.GetKeyDown(key))
                    {
                        _commandManager.HandlePressedKey(key);
                    }
                }
            }
        }

        /// <summary>
        /// A coroutine that awaits key press. Sets the _awaitingKeyPress flag to true.
        /// Once a key has been pressed the callback passed in the parameter is invoked. 
        /// </summary>
        /// <param name="onFinishedCallback"></param>
        /// <returns></returns>
        public IEnumerator AwaitKeypress(Action<KeyCode> onFinishedCallback)
        {
            _awaitingKeyPress = true;
            while (_awaitingKeyPress)
            {
                yield return null;
            }
            
            onFinishedCallback(_lastKeyPressed);
        }

        /// <summary>
        /// Listens for key events
        /// </summary>
        private void OnGUI()
        {
            if (Event.current.isKey && Event.current.keyCode != KeyCode.None)
            {
                if (_awaitingKeyPress)
                {
                    _awaitingKeyPress = false;
                    _lastKeyPressed = Event.current.keyCode;
                }
            }
        }
    }

}
