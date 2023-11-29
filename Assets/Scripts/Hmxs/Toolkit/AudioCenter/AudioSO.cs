using UnityEngine;

namespace Hmxs.Toolkit.AudioCenter
{
    [CreateAssetMenu(fileName = "AudioSO", menuName = "AudioSO", order = 0)]
    public class AudioSO : ScriptableObject
    {
        public void BgmPlay(string clipName)
        {
            AudioAsset asset = new AudioAsset(AudioType.BGM, clipName, isLoop: true);
            AudioCenter.Instance.AudioPlaySync(asset);
        }

        public void BgmPause(string clipName)
        {
            AudioCenter.Instance.AudioPause(AudioType.BGM, clipName);
        }
        
        public void BgmUnPause(string clipName)
        {
            AudioCenter.Instance.AudioUnPause(AudioType.BGM, clipName);
        }
        
        public void BgmStop(string clipName)
        {
            AudioCenter.Instance.AudioStop(AudioType.BGM, clipName);
        }
    }
}