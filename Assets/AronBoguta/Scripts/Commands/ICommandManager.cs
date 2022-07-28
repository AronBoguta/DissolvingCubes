using System.Collections.Generic;
using UnityEngine;

namespace AronBoguta
{
    public interface ICommandManager
    {
        /// <summary>
        /// Maps a specific command to a key
        /// </summary>
        Dictionary<KeyCode, ICommand> KeyToCommand { get; set; }
        
        /// <summary>
        /// Stores a new mapping key->command in the dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="command"></param>
        void BindCommandToKey(KeyCode key, ICommand command);
        void HandlePressedKey(KeyCode key);
    }
}

