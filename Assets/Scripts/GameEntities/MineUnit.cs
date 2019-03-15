using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
    public class MineUnit : Unit {
        protected override void EnemyFoundBehaviour() {
            DetectEnemyAndAttack();
        }

        protected override bool IsEnemyFound() {
            return NearEnemies.Where(u => u != null && (u.SelfPower != Power.tower || u.SelfPower != Power.mine)).Any();
        }

        private bool DetectEnemyAndAttack() {
            var enemies = NearEnemies.Where(u => u != null && (u.SelfPower != Power.tower || u.SelfPower != Power.mine));
            if (!enemies.Any()) {
                CurrentSpeed = UnitFactory.Instance.GetSetupForUnit(SelfPower).DefaultSpeed;
                return false;
            }

            var enemy = enemies.First();
            CurrentSpeed = 2 * enemy.CurrentSpeed;
            transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, CurrentSpeed * Time.deltaTime);
            return true;
        }
    }
}
