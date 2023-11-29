using System.Linq;
using Fungus;
using UnityEngine;

namespace Procedure
{
    public class Final : MonoBehaviour
    {
        private Flowchart _flowchart;

        private void Start()
        {
            _flowchart = GetComponent<Flowchart>();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                ExecuteFungusBlock("Final");
            }
        }
        
        private void ExecuteFungusBlock(string blockName)
        {
            if (_flowchart.HasBlock(blockName))
            {
                if (_flowchart.GetExecutingBlocks().Any(block => block.BlockName == blockName))
                    return;

                _flowchart.ExecuteBlock(blockName);
            }
            else
                Debug.LogError($"{blockName} not exist");
        }
    }
}