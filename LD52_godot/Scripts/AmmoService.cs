namespace LD52.Scripts
{
    public class AmmoService
    {
        private int maximumAmmo = GameConfig.maxAmmo;
        public int MaximumAmmo => maximumAmmo;
        private int currentAmmo = GameConfig.maxAmmo;
        public int CurrentAmmo => currentAmmo;
        public bool AddAmmo()
        {
            if (CurrentAmmo >= MaximumAmmo)
                return false;
            currentAmmo = CurrentAmmo + 1;
            return true;
        }
        public void Used()
        {
            if (currentAmmo > 0)
                currentAmmo--;
        }
    }
}