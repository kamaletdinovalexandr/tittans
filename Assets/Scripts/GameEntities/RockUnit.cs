using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
    public class RockUnit : Unit {
        protected override void EnemyFoundBehaviour() {
            AttackScissors();
        }

        protected override bool IsEnemyFound() {
            return NearEnemies.Where(u => u != null && u.SelfPower == Power.scissors).Any();
        }

        private void AttackScissors() {
            var scissors = NearEnemies.Where(u => u != null && u.SelfPower == Power.scissors);
            transform.position = Vector2.MoveTowards(transform.position, 
                                                     scissors.First().transform.position, 
                                                     CurrentSpeed * Time.deltaTime);
        }
    }
}
