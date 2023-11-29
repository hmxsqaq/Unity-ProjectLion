using System;

namespace Game.Player
{
    public static class PlayerInfo
    {
        #region Health
        
        private static int _health = 1;
        
        public static int Health
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;
                _health = value;
                OnHealthChange?.Invoke();
            }
        }

        public static Action OnHealthChange;
        
        #endregion
        
        #region Point
        
        private static int _point = 0;
        
        public static int Point
        {
            get => _point;
            set
            {
                if (_point == value)
                    return;
                _point = value;
                OnPointChange?.Invoke(_point);
            }
        }

        public static Action<int> OnPointChange;
        
        #endregion
    }
}