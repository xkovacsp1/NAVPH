namespace Assets.Scripts.Enemy
{
    public class EnemyBehaviourContext
    {
        public IEnemyBehaviour EnemyBehavior { get; }

        public EnemyBehaviourContext(IEnemyBehaviour enemyBehavior)
        {
            EnemyBehavior = enemyBehavior;
        }

        public void Act()
        {
            if (!EnemyBehavior.IsActive()) return;
            EnemyBehavior?.Attack();
            EnemyBehavior?.Move();
        }
    }
}