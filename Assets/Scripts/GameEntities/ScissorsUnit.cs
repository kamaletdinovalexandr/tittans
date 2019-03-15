using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
    public class ScissorsUnit : Unit {
       
        protected override bool IsEnemyFound() {
            return false;
        }
    }
}
