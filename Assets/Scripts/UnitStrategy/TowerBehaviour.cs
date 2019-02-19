
public class TowerBehaviour : AbstractCollideBehaviour {
    
    public TowerBehaviour(Unit unit) : base(unit) { }
    
    public override void DoCollide(Unit unit) {
        if (unit.UnitPower == Power.tower)
            return;
        
        unit.AlternateSpeed = 0.2f;
    }
}