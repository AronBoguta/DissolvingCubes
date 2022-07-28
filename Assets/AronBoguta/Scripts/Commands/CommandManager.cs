using System.Collections.Generic;
using UnityEngine;

namespace AronBoguta
{
    public class CommandManager : MonoBehaviour, ICommandManager
    {
        public Dictionary<KeyCode, ICommand> KeyToCommand
        {
            get {  return _keyToCommand; }
            set { _keyToCommand = value; }
        }
        private Dictionary<KeyCode, ICommand> _keyToCommand = new Dictionary<KeyCode, ICommand>();

        public void BindCommandToKey(KeyCode key, ICommand command)
        {
            _keyToCommand[key] = command;
        }

        public void HandlePressedKey(KeyCode key)
        {
            ICommand mappedCommand;
            if (KeyToCommand.TryGetValue(key, out mappedCommand))
            {
                mappedCommand.Execute();
            }
        }
        
    }   
}
