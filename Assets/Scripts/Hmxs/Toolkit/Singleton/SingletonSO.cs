using UnityEngine;

namespace Hmxs.Toolkit.Singleton
{
    /// <summary>
    /// 懒汉式泛型单例-继承SO
    /// </summary>
    /// <typeparam name="T">继承该类的类名</typeparam>
    public class SingletonSO<T> : ScriptableObject where T : SingletonSO<T>
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] assets = Resources.LoadAll<T>($"");
                    if (assets == null || assets.Length < 1)
                    {
                        throw new System.Exception($"SingletonSO: Null {typeof(T)} SO instance in Resources directory");
                    }
                    if (assets.Length > 1)
                    {
                        Debug.LogWarning($"SingletonSO: Multiple {typeof(T)} SO instance in Resources directory");
                    }
                    _instance = assets[0];
                }
                return _instance;
            }
        }
    }
}