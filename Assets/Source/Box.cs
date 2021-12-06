using System;
using UnityEngine;


[Serializable]
public class Box : MonoBehaviour
{
    public uint pox = 0;
    public uint poy = 0;

    public Unit UnitOnThis = null;

    public GameObject Spawn = null;
    public GameObject SelectQuad = null;
    public GameObject AOEQuad = null;
    public GameObject AttackQuad = null;
    public SpriteRenderer CadrillageQuad = null;

    public Color colorNormal = Color.black;
    public Color colorEnableCell = Color.white;

    public Box() { }
    public Box(uint _pox, uint _poy)
    {
        pox = _pox;
        poy = _poy;
        UnitOnThis = null;
    }

    private void Start()
    {
        SelectQuad.SetActive(false);
        CadrillageQuad.color = colorNormal;
    }

    private void Update()
    {
        if (UnitOnThis != null)
        {
            Cell cell = UnitOnThis as Cell;
            if (cell != null)
            {
                if (!cell.AsFinishedTurn)
                {
                    CadrillageQuad.color = colorEnableCell;
                }
                else
                {
                    CadrillageQuad.color = colorNormal;
                }
            }
        }
        else
        {
            CadrillageQuad.color = colorNormal;
        }

    }

    public Vector3 GetWorldPos()
    {
        return Spawn.transform.position;
    }

    public void SetSelected()
    {
        SelectQuad.SetActive(true);
    }

    public void SetUnselected()
    {
        SelectQuad.SetActive(false);
    }

    public void SetAOE()
    {
        AOEQuad.SetActive(true);
    }

    public void SetUnAOE()
    {
        AOEQuad.SetActive(false);
    }

    public void SetAttack()
    {
        AttackQuad.SetActive(true);
    }

    public void SetUnAttack()
    {
        AttackQuad.SetActive(false);
    }
}
