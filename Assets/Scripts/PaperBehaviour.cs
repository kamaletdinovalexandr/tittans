
public class PaperBehaviour : AbstractCollideBehaviour {
    
    public PaperBehaviour(Unit unit) : base(unit) { }
    
    public override void DoCollide(Unit unit) {
        if (unit.UnitPower != Power.rock)
            return;
        
        _unit.NearEnemyTransform = unit.transform;
        _unit.RunAway = false;
    }
}
