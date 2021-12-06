public class PurpleCell : Cell
{
    
    private void Start()
    {
        typeCell = TYPECell.Purple;
        AttackPower = 3;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Attack()
    {
        base.Attack();

    }
}
