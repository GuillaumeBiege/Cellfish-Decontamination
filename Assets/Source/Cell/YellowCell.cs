using System.Collections.Generic;

public class YellowCell : Cell
{
    private void Start()
    {
        typeCell = TYPECell.Yellow;
        AttackPower = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Attack()
    {
        base.Attack();

        List<Box> attackRange = new List<Box>();

        //left
        Box box = board.GetBoxAtCoordinate(CurrentBox.pox - 1, CurrentBox.poy);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        //Right
        box = board.GetBoxAtCoordinate(CurrentBox.pox + 1, CurrentBox.poy);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        //Up
        box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy - 1);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        //Down
        box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy + 1);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        foreach (Box item in attackRange)
        {
            RedCell red = item.UnitOnThis as RedCell;
            if (red != null)
            {
                red.Kill();
            }
        }

    }

    public override List<Box> GetAttackRange()
    {
        List<Box> attackRange = new List<Box>();

        //left
        Box box = board.GetBoxAtCoordinate(CurrentBox.pox - 1, CurrentBox.poy);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        //Right
        box = board.GetBoxAtCoordinate(CurrentBox.pox + 1, CurrentBox.poy);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        //Up
        box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy - 1);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        //Down
        box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy + 1);
        if (box != null)
        {
            if (!attackRange.Contains(box))
            {
                attackRange.Add(box);
            }

        }

        return attackRange;

    }
}
