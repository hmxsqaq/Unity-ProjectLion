using Fungus;
using Hmxs.Toolkit.AudioCenter;
using Hmxs.Toolkit.Events;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Hmxs.Toolkit.AudioCenter.AudioType;

namespace UI
{
    public class StartSceneButton : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Flowchart flowchart;

        private void Start()
        {
            startButton.onClick.AddListener(() =>
            {
                AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.UI, "UI点击音效"));
                flowchart.ExecuteBlock("SceneLoad");
            });
            
            quitButton.onClick.AddListener((() =>
            {
                AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.UI, "UI点击音效"));
                Application.Quit();
            }));
        }
    }
}