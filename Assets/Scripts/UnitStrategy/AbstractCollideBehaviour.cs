
public abstract class AbstractCollideBehaviour : ICollideBehaviour {
    protected readonly Unit _unit;

    public AbstractCollideBehaviour(Unit unit) {
        _unit = unit;
    }

    public abstract void DoCollide(Unit unit);

}