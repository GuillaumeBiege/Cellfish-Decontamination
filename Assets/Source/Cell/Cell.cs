using System;
using System.Collections.Generic;




[Serializable]
public class Cell : Unit
{
    public enum TYPECell
    {
        Blue,
        Yellow,
        Purple,
        Count
    }

    public TYPECell typeCell = TYPECell.Count;
    protected GameManager GM = null;
    protected Board board = null;

    public bool AsMoved = false;
    public bool AsFinishedTurn = false;

    public int MoveRange = 4;
    public int AttackPower = 1;

    // Start is called before the first frame update
    protected void Awake()
    {
        GM = FindObjectOfType<GameManager>();
        board = FindObjectOfType<Board>();
    }

    public virtual void BeginTurn()
    {
        AsMoved = false;
        AsFinishedTurn = false;
    }

    public virtual void Attack()
    {

    }

    public virtual List<Box> GetAttackRange()
    {
        return new List<Box>();
    }

}
