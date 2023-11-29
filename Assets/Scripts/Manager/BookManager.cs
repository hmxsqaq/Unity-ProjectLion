using Hmxs.Toolkit.AudioCenter;
using UnityEngine;
using AudioType = Hmxs.Toolkit.AudioCenter.AudioType;

namespace Manager
{
    public class BookManager : MonoBehaviour
    {
        public void Continue()
        {
            Time.timeScale = 1;
        }

        public void Audio()
        {
            AudioCenter.Instance.AudioPlaySync(new AudioAsset(AudioType.UI, "UI点击音效"));
        }
    }
}