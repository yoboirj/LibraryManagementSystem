namespace LibraryManagementSystem.Features.Helpers
{
    public class ThemePalos
    {
        private bool _isDarkMode;
        public bool IsDarkMode 
        { 
            get => _isDarkMode; 
            set 
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnThemeChanged?.Invoke();
                }
            } 
        }
        public event Action? OnThemeChanged;

        public void ToggleDarkMode()
        {
            IsDarkMode = !IsDarkMode;
        }

        public void SetDarkMode(bool isDarkMode)
        {
            IsDarkMode = isDarkMode;
        }
    }
}
