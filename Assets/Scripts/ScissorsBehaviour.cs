
public class ScissorsBehaviour : AbstractCollideBehaviour {
   
    public ScissorsBehaviour(Unit unit) : base(unit) { }
    
    public override void DoCollide(Unit unit) {
        if (unit.UnitPower != Power.paper)
            return;
        
        _unit.NearEnemyTransform = unit.transform;
        _unit.RunAway = true;
    }    
 }