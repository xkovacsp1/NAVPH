namespace Assets.Scripts.PowerUps
{
    public interface IPowerUpsBehaviour
    {
        void Move();
        bool IsActive();
        void TakeEffect();
    }
}