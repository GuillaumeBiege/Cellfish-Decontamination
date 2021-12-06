using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RedCell : Unit
{
    GameManager GM = null;

    Board board = null;

    public GameObject deathParticule = null;


    public int MaxHealth = 1;
    public int CurrentHealth = 1;

    public int Level = 1;

    public bool IsInitialised = false;

    public List<Box> ReproductionBoxes = new List<Box>();

    public int Age = 0;

    public int Gen = 0;

    Vector3 InitialScale = new Vector3(0.25f, 0.25f, 0.25f);

    public Canvas canva;
    public Text TextHealth;

    private void Start()
    {
        board = FindObjectOfType<Board>();
        GM = FindObjectOfType<GameManager>();

        transform.localScale = (InitialScale * Level);
    }

    private void Update()
    {
        //Health limit
        CurrentHealth = (CurrentHealth < 0) ? 0 : CurrentHealth;
        CurrentHealth = (CurrentHealth > MaxHealth) ? MaxHealth : CurrentHealth;


        //Death condition
        if (CurrentHealth <= 0)
        {
            Kill();
        }

        canva.transform.forward = Camera.main.transform.forward;

        if (CurrentHealth != MaxHealth)
        {
            TextHealth.text = CurrentHealth.ToString() + " / " + MaxHealth.ToString();
        }
        else
        {
            TextHealth.text = "";
        }
        
    }

    public void PlayTurn()
    {

        {
            //left
            Box box = board.GetBoxAtCoordinate(CurrentBox.pox - 1, CurrentBox.poy);
            if (box != null && box.UnitOnThis == null)
            {
                if (!ReproductionBoxes.Contains(box))
                {
                    ReproductionBoxes.Add(box);
                }

            }

            //Right
            box = board.GetBoxAtCoordinate(CurrentBox.pox + 1, CurrentBox.poy);
            if (box != null && box.UnitOnThis == null)
            {
                if (!ReproductionBoxes.Contains(box))
                {
                    ReproductionBoxes.Add(box);
                }
            }

            //Up
            box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy - 1);
            if (box != null && box.UnitOnThis == null)
            {
                if (!ReproductionBoxes.Contains(box))
                {
                    ReproductionBoxes.Add(box);
                }
            }

            //Down
            box = board.GetBoxAtCoordinate(CurrentBox.pox, CurrentBox.poy + 1);
            if (box != null && box.UnitOnThis == null)
            {
                if (!ReproductionBoxes.Contains(box))
                {
                    ReproductionBoxes.Add(box);
                }
            }
        }

        //Evolution
        Age++;
        if (Age >= 2)
        {
            Evolve();
            Age = 1;
        }

        transform.localScale = (InitialScale * Level);


        //Reproduction
        int Willpop = UnityEngine.Random.Range(0, (30 / Level));
        if (Level >= 2)
        {
            int nextReproBox = UnityEngine.Random.Range(0, ReproductionBoxes.Count - 1);
            for (int i = 0; i < ReproductionBoxes.Count; i++)
            {
                int temp = (nextReproBox + i) % ReproductionBoxes.Count;
                if (ReproductionBoxes[temp].UnitOnThis == null)
                {
                    GM.CreateRedCell(Gen, ReproductionBoxes[temp]);
                    break;
                }
            }
        }


    }

    void Evolve()
    {
        if (Level >= 3)
        {
            return;
        }

        MaxHealth += 2;
        CurrentHealth += 2;

        Level++;
    }

    public void CauseDamage(int _qt)
    {
        Debug.Log("Cause Damage");
        CurrentHealth -= _qt;
    }

    public void Kill()
    {
        CurrentBox.UnitOnThis = null;
        GM.redCells.Remove(this);

        GameObject go = Instantiate<GameObject>(deathParticule);
        go.transform.position = transform.position;

        Destroy(gameObject);
    }
}
