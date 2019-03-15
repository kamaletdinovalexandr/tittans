using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FlyWeight;
using Factory;

namespace GameEntitties {
    public class TitanUnit : Unit {
        
        protected override bool IsEnemyFound() {
            return false;
        }
    }
}
