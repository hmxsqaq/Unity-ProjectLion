using System;
using Fungus;
using UnityEngine;

namespace Game.Element
{
    public class FinalBall : Interactable
    {
        [SerializeField] private string finalChartName;
        private Flowchart _flowchart;
        
        private void Start()
        {
            CanInteract = true;
            _flowchart = GameObject.Find("Flowchart").GetComponent<Flowchart>();
        }

        protected override void OnPlayerPressed()
        {
            if (!IsPlayerNear) return;
            ExecuteFungusBlock(finalChartName);
        }
        
        private void ExecuteFungusBlock(string blockName)
        {
            if (_flowchart.HasBlock(blockName)) 
                _flowchart.ExecuteBlock(blockName);
            else
                Debug.LogError($"{blockName} not exist");
        }
    }
}