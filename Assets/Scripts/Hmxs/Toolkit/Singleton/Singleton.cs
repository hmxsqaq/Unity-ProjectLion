namespace Hmxs.Toolkit.Singleton
{
    /// <summary>
    /// 懒汉式泛型单例-不继承Mono（非线程安全）
    /// </summary>
    /// <typeparam name="T">继承该类的类名</typeparam>
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                _instance ??= new T();
                _instance.OnInstanceCreate();
                return _instance;
            }
        }
        
        /// <summary>
        /// 单例被第一次调用时调用该方法
        /// </summary>
        protected virtual void OnInstanceCreate() {}
    }
}