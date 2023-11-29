using Hmxs.Toolkit.Events;
using UnityEngine;

namespace Procedure
{
    public class StartProcedure : MonoBehaviour
    {
        private void Start()
        {
            EventCenter.Trigger(EventName.UI.SceneStartFadeEffect);
        }
    }
}