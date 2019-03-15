using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
    public class PaperUnit : Unit {
        protected override void EnemyFoundBehaviour() {
            MoveBackwardIfScissors();
        }

        protected override bool IsEnemyFound() {
            return NearEnemies.Where(u => u != null && u.SelfPower == Power.scissors).Any();
        }

        private void MoveBackwardIfScissors() {
            var scissors = NearEnemies.Where(u => u != null && u.SelfPower == Power.scissors);

            if (scissors.Any()) {
                transform.position = Vector2.MoveTowards(transform.position, -Team.EnemyBasePosition, CurrentSpeed * Time.deltaTime);
            } 
        }
    }
}
