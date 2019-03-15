using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
    public class TowerUnit : Unit {
        protected override void EnemyFoundBehaviour() {
            SlowDownNearestEnemy();
        }

        protected override bool IsEnemyFound() {
            return NearEnemies.Where(u => u != null && (u.SelfPower != Power.tower || u.SelfPower != Power.mine)).Any();
        }

        private void SlowDownNearestEnemy() {
            var enemies = NearEnemies.Where(u => u != null && (u.SelfPower != Power.tower || u.SelfPower != Power.mine));
            enemies.ToList().ForEach(u => u.CurrentSpeed = 0.5f);
        }
    }
}
