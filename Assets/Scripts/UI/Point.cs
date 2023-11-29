using Game.Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Point : MonoBehaviour
    {
        [SerializeField] private int all = 10;
        
        private TextMeshProUGUI _textMesh;

        private void Start()
        {
            _textMesh = GetComponent<TextMeshProUGUI>();
            PlayerInfo.OnPointChange += TextUpdate;
        }

        private void OnDestroy()
        {
            PlayerInfo.OnPointChange -= TextUpdate;
        }

        private void TextUpdate(int number)
        {
            number = number > all ? all : number;
            _textMesh.text = $"{number}/{all}";
        }
    }
}