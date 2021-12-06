using System.Collections.Generic;
using UnityEngine;



public class BlueCell : Cell
{
    public List<Box> InfluanceRange = new List<Box>();

    public GameObject VFXBlueAttack = null;

    private void Start()
    {
        typeCell = TYPECell.Blue;
        AttackPower = 1;
    }

    public override void BeginTurn()
    {
        base.BeginTurn();

        FillInfluanceRange();


        foreach (Box item in InfluanceRange)
        {
            Debug.Log(item.UnitOnThis);
            RedCell red = item.UnitOnThis as RedCell;
            if (red != null)
            {
                //Debug.Log("Moka");
                red.CauseDamage(AttackPower);
            }

            GameObject go = Instantiate<GameObject>(VFXBlueAttack);
            go.transform.position = transform.position;

        }


    }


    void FillInfluanceRange()
    {
        InfluanceRange.Clear();

        //left
        Box box = board.GetBoxAtCoordinate(CurrentBox.pox - 1, CurrentBox.poy);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }

        }

        //left Up
        box = board.GetBoxAtCoordinate(CurrentBox.pox - 1, CurrentBox.poy + 1);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }

        }

        //left Down
        box = board.GetBoxAtCoordinate(CurrentBox.pox - 1, CurrentBox.poy - 1);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }

        }

        //Right
        box = board.GetBoxAtCoordinate(CurrentBox.pox + 1, CurrentBox.poy);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }
        }

        //Right up
        box = board.GetBoxAtCoordinate(CurrentBox.pox + 1, CurrentBox.poy - 1);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }
        }

        //Right down
        box = board.GetBoxAtCoordinate(CurrentBox.pox + 1, CurrentBox.poy + 1);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }
        }

        //Up
        box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy - 1);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }
        }

        //Down
        box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy + 1);
        if (box != null)
        {
            if (!InfluanceRange.Contains(box))
            {
                InfluanceRange.Add(box);
            }
        }
    }
}
