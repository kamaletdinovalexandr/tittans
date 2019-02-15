
public class TitanBehaviour : AbstractCollideBehaviour {
    
    public TitanBehaviour(Unit unit) : base(unit) { }
    
    public override void DoCollide(Unit unit) {
        if (unit.UnitPower == Power.titan)
            return;
        
        _unit.NearEnemyTransform = unit.transform;
        _unit.RunAway = false;
    }
    
}