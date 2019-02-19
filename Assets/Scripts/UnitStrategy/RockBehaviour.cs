
public class RockBehaviour : AbstractCollideBehaviour {
    
    public RockBehaviour(Unit unit) : base(unit) { }
    
    public override void DoCollide(Unit unit) {
        if (unit.UnitPower == Power.scissors) {
			_unit.NearEnemyTransform = unit.transform;
		}
    }
}
