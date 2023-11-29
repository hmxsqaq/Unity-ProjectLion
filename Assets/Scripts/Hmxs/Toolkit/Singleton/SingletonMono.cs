using UnityEngine;

namespace Hmxs.Toolkit.Singleton
{
    /// <summary>
    /// 懒汉式泛型单例-继承Mono（非线程安全）
    /// 不会因切换场景被销毁
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject
                    {
                        name = typeof(T).ToString()
                    };
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<T>();
                    _instance.OnInstanceCreate();
                }
                return _instance;
            }
        }
        
        protected virtual void OnInstanceCreate() {}
    }
}