using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AronBoguta
{
    public class PipelineVerify : MonoBehaviour
    {
        [SerializeField] private GameObject pipelinePrompt;

        void Start()
        {
            if (UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline == null)
            {
                pipelinePrompt.SetActive(true);
            }
        }
    }
}
