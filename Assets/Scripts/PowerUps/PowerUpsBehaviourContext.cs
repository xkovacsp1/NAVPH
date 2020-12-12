namespace Assets.Scripts.PowerUps
{
    public class PowerUpsBehaviourContext
    {
        public IPowerUpsBehaviour PowerUpBehavior { get; set; }

        public PowerUpsBehaviourContext(IPowerUpsBehaviour powerUpBehavior)
        {
            PowerUpBehavior = powerUpBehavior;
        }

        public void Act()
        {
            if (!PowerUpBehavior.IsActive()) return;
            PowerUpBehavior?.Move();
            PowerUpBehavior?.TakeEffect();
        }
    }
}