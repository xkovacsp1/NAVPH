
 namespace Assets.Scripts.PowerUps
 {
     public class PowerUpsBehaviourContext
     {
         public IPowerUpsBehaviour EnemyBehavior { get; }

         public PowerUpsBehaviourContext(IPowerUpsBehaviour enemyBehavior)
         {
             EnemyBehavior = enemyBehavior;
         }

         public void Act()
         {
             if (!EnemyBehavior.IsActive()) return;
             EnemyBehavior.Move();
         }
     }
 }

