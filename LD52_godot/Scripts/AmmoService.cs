namespace LD52.Scripts
{
    public class AmmoService
    {
        private int maximumAmmo = GameConfig.maxAmmo;
        public int MaximumAmmo => maximumAmmo;
        private int currentAmmo = GameConfig.maxAmmo;
        public int CurrentAmmo => currentAmmo;
        public void AddAmmo()
        {
            if (CurrentAmmo < MaximumAmmo)
                currentAmmo = CurrentAmmo + 1;
        }
        public void Used()
        {
            if (currentAmmo > 0)
                currentAmmo--;
        }
    }
}