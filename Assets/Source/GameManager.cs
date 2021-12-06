using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum TYPESelectionMode
    {
        SelectionMode,
        MoveMode,
        AttackMode,
        GameOverMode
    }

    public TYPESelectionMode selectionMode = TYPESelectionMode.SelectionMode;
    Board board = null;

    [SerializeField] Box HighlightBox = null;
    Box OldHighlightBox = null;
    [SerializeField] Box OldCellPos = null;

    public Unit SelectedUnit = null;
    public Cell SelectedCell = null;

    public bool AsAllCellFinished = false;

    public List<Box> MoveRangeBoxes = new List<Box>();
    public List<Box> AttackRangeBoxes = new List<Box>();

    public bool IsUptadeEnable = true;

    //Victory/Defeat Conditions
    public int TurnTimer = 5;
    public bool IsPlayerVictorious = false;

    //VFX Attack
    public GameObject VFXPurpleAttack = null;
    public GameObject VFXYellowAttack = null;


    private void Awake()
    {
        board = FindObjectOfType<Board>();

        Configurator config = FindObjectOfType<Configurator>();
        if (config != null)
        {
            TurnTimer = config.GetCurrentTimer();
        }
    }

    private void Start()
    {
        


    }

    private void Update()
    {
        if (IsUptadeEnable)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hito = new RaycastHit();

            switch (selectionMode)
            {
                case TYPESelectionMode.SelectionMode:

                    SelectionMode(ray, hito);

                    break;
                case TYPESelectionMode.MoveMode:

                    MoveMode(ray, hito);


                    break;
                case TYPESelectionMode.AttackMode:

                    AttackMode(ray, hito);

                    break;

                case TYPESelectionMode.GameOverMode:

                    GameOverMode();

                    break;

                default:
                    break;
            }

            if (HighlightBox != OldHighlightBox && HighlightBox != null)
            {
                HighlightBox.SetSelected();

                if (OldHighlightBox != null)
                {
                    OldHighlightBox.SetUnselected();
                    OldHighlightBox = HighlightBox;
                }
            }
            else if (HighlightBox == null)
            {
                if (OldHighlightBox != null)
                {
                    OldHighlightBox.SetUnselected();
                    OldHighlightBox = HighlightBox;
                }
            }
        }

        //Victory/Defeat Condition
        if (redCells != null && redCells.Count <= 0)
        {
            IsPlayerVictorious = true;
            selectionMode = TYPESelectionMode.GameOverMode;

            foreach (Box item in MoveRangeBoxes)
            {
                item.SetUnAOE();
            }
            MoveRangeBoxes.Clear();

            foreach (Box item in AttackRangeBoxes)
            {
                item.SetUnAttack();
            }
            AttackRangeBoxes.Clear();

            SelectedUnit = null;
            SelectedCell = null;
            HighlightBox = null;
        }
        else if (NbTurn > TurnTimer && redCells != null && redCells.Count > 0)
        {
            IsPlayerVictorious = false;
            selectionMode = TYPESelectionMode.GameOverMode;

            foreach (Box item in MoveRangeBoxes)
            {
                item.SetUnAOE();
            }
            MoveRangeBoxes.Clear();

            foreach (Box item in AttackRangeBoxes)
            {
                item.SetUnAttack();
            }
            AttackRangeBoxes.Clear();

            SelectedUnit = null;
            SelectedCell = null;
            HighlightBox = null;
        }

        AsAllCellFinished = true;
        foreach (Cell item in cells)
        {
            if (item.AsFinishedTurn == false)
            {
                AsAllCellFinished = false;
            }
        }


#if UNITY_EDITOR
        ///////////////DEBUG
        if (Input.GetKeyDown(KeyCode.J))
        {
            RedCell red = HighlightBox.UnitOnThis as RedCell;
            if (red != null)
            {
                red.Kill();
            }
        }
        ///////////////DEBUG
#endif



    }

    //Turn Gestion
    public int NbTurn = 1;
    public bool IsPlayerTurn = true;
    public List<Cell> cells = new List<Cell>();
    public void IncrementTurn()
    {
        IsPlayerTurn = false;

        RedcellTurn();
    }

    void FinishTurn()
    {
        IsPlayerTurn = true;
        NbTurn++;

        foreach (Cell cell in cells)
        {
            cell.BeginTurn();
        }
    }

    void SelectionMode(Ray _ray, RaycastHit _hito)
    {
        //Box highlight
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hito))
        {
            uint pox = (uint)Mathf.RoundToInt(_hito.point.x + 0.5f);
            uint poy = (uint)Mathf.RoundToInt(_hito.point.z + 0.5f);

            HighlightBox = board.GetBoxAtCoordinate(pox, poy);

            if (OldHighlightBox == null)
            {
                OldHighlightBox = HighlightBox;
                if (HighlightBox != null)
                {
                    HighlightBox.SetSelected();
                }

            }
        }

        //Select a cell
        if (Input.GetMouseButtonDown(0))
        {
            if (HighlightBox != null && HighlightBox.UnitOnThis != null)
            {
                SelectedCell = null;
                SelectedCell = HighlightBox.UnitOnThis as Cell;
                if (SelectedCell != null && !SelectedCell.AsFinishedTurn)
                {
                    selectionMode = TYPESelectionMode.MoveMode;
                    HighlightBox = null;
                    OldCellPos = SelectedCell.CurrentBox;
                }
            }
        }
    }

    void MoveMode(Ray _ray, RaycastHit _hito)
    {
        foreach (Box item in AttackRangeBoxes)
        {
            item.SetUnAttack();
        }
        AttackRangeBoxes.Clear();

        //Display move range
        if (SelectedCell != null && !SelectedCell.AsMoved)
        {
            MoveRangeBoxes = board.GetMoveRange(SelectedCell.CurrentBox, SelectedCell.MoveRange);
            foreach (Box item in MoveRangeBoxes)
            {
                item.SetAOE();
            }
        }

        //Box highlight
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!SelectedCell.AsMoved && Physics.Raycast(_ray, out _hito))
        {
            uint pox = (uint)Mathf.RoundToInt(_hito.point.x + 0.5f);
            uint poy = (uint)Mathf.RoundToInt(_hito.point.z + 0.5f);

            if (MoveRangeBoxes.Contains(board.GetBoxAtCoordinate(pox, poy)))
            {
                HighlightBox = board.GetBoxAtCoordinate(pox, poy);

                if (OldHighlightBox == null)
                {
                    OldHighlightBox = HighlightBox;
                    HighlightBox.SetSelected();
                }
            }
            else
            {
                HighlightBox = null;
            }
        }

        //Move cell
        if (Input.GetMouseButtonDown(0))
        {
            if (HighlightBox != null && HighlightBox.UnitOnThis == null)
            {
                if (SelectedCell != null)
                {
                    board.PlaceUnit(SelectedCell, HighlightBox);
                    SelectedCell.AsMoved = true;
                    foreach (Box item in MoveRangeBoxes)
                    {
                        item.SetUnAOE();
                    }
                    HighlightBox = null;

                    if (SelectedCell.typeCell == Cell.TYPECell.Blue)
                    {
                        SetAttackMode();
                        CancelAttackMode();
                    }
                }
            }
        }

        
    }

    void AttackMode(Ray _ray, RaycastHit _hito)
    {
        foreach (Box item in AttackRangeBoxes)
        {
            item.SetUnAttack();
        }
        AttackRangeBoxes.Clear();

        switch (SelectedCell.typeCell)
        {
            case Cell.TYPECell.Blue:

                CancelAttackMode();
                break;
            case Cell.TYPECell.Yellow:

                foreach (Box item in AttackRangeBoxes)
                {
                    item.SetUnAttack();
                }
                AttackRangeBoxes.Clear();

                //Mouse pos
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(_ray, out _hito);

                Vector3 cellToMouse = _hito.point - SelectedCell.transform.position;

                float dotProdFront = Vector3.Dot(cellToMouse, (SelectedCell.transform.forward + SelectedCell.transform.right).normalized);
                float dotProdRight = Vector3.Dot(cellToMouse, (SelectedCell.transform.forward - SelectedCell.transform.right).normalized);

                int direction2 = 1;
                //Front
                if (dotProdFront >= 0 && dotProdRight >= 0)
                {
                    direction2 = 1;
                    Box box;
                    for (int i = 1; i < 9; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox, SelectedCell.CurrentBox.poy + (uint)i);
                        if (box != null)
                        {
                            if (box.UnitOnThis != null && (box.UnitOnThis as RedCell) == null)
                            {
                                break;
                            }
                            else
                            {
                                AttackRangeBoxes.Add(box);
                                box.SetAttack();
                            }
                        }
                    }

                }

                //Down
                else if (dotProdFront < 0 && dotProdRight < 0)
                {
                    direction2 = 2;
                    Box box;
                    for (int i = 1; i < 9; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox, SelectedCell.CurrentBox.poy - (uint)i);
                        if (box != null)
                        {
                            if (box.UnitOnThis != null && (box.UnitOnThis as RedCell) == null)
                            {
                                break;
                            }
                            else
                            {
                                AttackRangeBoxes.Add(box);
                                box.SetAttack();
                            }
                        }
                    }
                }
                //Left
                else if (dotProdFront < 0 && dotProdRight >= 0)
                {
                    direction2 = 3;
                    Box box;
                    for (int i = 1; i < 9; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox - (uint)i, SelectedCell.CurrentBox.poy);
                        if (box != null)
                        {
                            if (box.UnitOnThis != null && (box.UnitOnThis as RedCell) == null)
                            {
                                break;
                            }
                            else
                            {
                                AttackRangeBoxes.Add(box);
                                box.SetAttack();
                            }
                        }
                    }
                }

                //Right
                else if (dotProdFront >= 0 && dotProdRight < 0)
                {
                    direction2 = 4;
                    Box box;
                    for (int i = 1; i < 9; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox + (uint)i, SelectedCell.CurrentBox.poy);
                        if (box != null)
                        {
                            if (box.UnitOnThis != null && (box.UnitOnThis as RedCell) == null)
                            {
                                break;
                            }
                            else
                            {
                                AttackRangeBoxes.Add(box);
                                box.SetAttack();
                            }
                        }
                    }
                }

                //Causing Damage
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(_ray, out _hito);
                if (Input.GetMouseButtonDown(0) && AttackRangeBoxes != null && AttackRangeBoxes.Count > 0 && _hito.collider.gameObject.GetComponent<Box>() != null)
                {
                    foreach (Box item in AttackRangeBoxes)
                    {
                        if (item != null)
                        {
                            RedCell red = item.UnitOnThis as RedCell;
                            if (red != null)
                            {
                                red.Kill();
                            }
                        }

                    }

                    GameObject go = Instantiate<GameObject>(VFXYellowAttack);

                    //VFX pop
                    switch (direction2)
                    {
                        case 1:
                            go.transform.position = SelectedCell.transform.position;
                            go.transform.rotation = Quaternion.Euler(0f, 270f, 0f);

                            break;
                        case 2:
                            go.transform.position = SelectedCell.transform.position;
                            go.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                            break;
                        case 3:
                            go.transform.position = SelectedCell.transform.position;
                            go.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                            break;
                        case 4:
                            go.transform.position = SelectedCell.transform.position;
                            go.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                            break;
                        default:
                            break;
                    }

                    CancelAttackMode();
                }


                break;
            case Cell.TYPECell.Purple:
                foreach (Box item in AttackRangeBoxes)
                {
                    item.SetUnAttack();
                }
                AttackRangeBoxes.Clear();

                //Mouse pos
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(_ray, out _hito);

                Vector3 cellToMouse2 = _hito.point - SelectedCell.transform.position;

                float dotProdFront2 = Vector3.Dot(cellToMouse2, (SelectedCell.transform.forward + SelectedCell.transform.right).normalized);
                float dotProdRight2 = Vector3.Dot(cellToMouse2, (SelectedCell.transform.forward - SelectedCell.transform.right).normalized);

                int direction = 1;
                //Front
                if (dotProdFront2 >= 0 && dotProdRight2 >= 0)
                {
                    direction = 1;
                    Box box;
                    for (int i = 1; i < 4; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox, SelectedCell.CurrentBox.poy + (uint)i);
                        if (box != null)
                        {
                            AttackRangeBoxes.Add(box);
                            box.SetAttack();

                            
                        }
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox + 1, SelectedCell.CurrentBox.poy + (uint)2);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox - 1, SelectedCell.CurrentBox.poy + (uint)2);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }

                }

                //Down
                else if (dotProdFront2 < 0 && dotProdRight2 < 0)
                {
                    direction = 2;
                    Box box;
                    for (int i = 1; i < 4; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox, SelectedCell.CurrentBox.poy - (uint)i);
                        if (box != null)
                        {
                            AttackRangeBoxes.Add(box);
                            box.SetAttack();
                        }
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox + 1, SelectedCell.CurrentBox.poy - (uint)2);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox - 1, SelectedCell.CurrentBox.poy - (uint)2);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }
                }
                //Left
                else if (dotProdFront2 < 0 && dotProdRight2 >= 0)
                {
                    direction = 3;
                    Box box;
                    for (int i = 1; i < 4; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox - (uint)i, SelectedCell.CurrentBox.poy);
                        if (box != null)
                        {
                            AttackRangeBoxes.Add(box);
                            box.SetAttack();
                        }
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox - (uint)2, SelectedCell.CurrentBox.poy - (uint)1);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox - (uint)2, SelectedCell.CurrentBox.poy + (uint)1);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }
                }

                //Right
                else if (dotProdFront2 >= 0 && dotProdRight2 < 0)
                {
                    direction = 4;
                    Box box;
                    for (int i = 1; i < 4; i++)
                    {
                        box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox + (uint)i, SelectedCell.CurrentBox.poy);
                        if (box != null)
                        {
                            AttackRangeBoxes.Add(box);
                            box.SetAttack();
                        }
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox + (uint)2, SelectedCell.CurrentBox.poy - (uint)1);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }
                    box = board.GetBoxAtCoordinate(SelectedCell.CurrentBox.pox + (uint)2, SelectedCell.CurrentBox.poy + (uint)1);
                    if (box != null)
                    {
                        AttackRangeBoxes.Add(box);
                        box.SetAttack();
                    }
                }

                //Causing Damage
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(_ray, out _hito);
                if (Input.GetMouseButtonDown(0) && AttackRangeBoxes != null && AttackRangeBoxes.Count > 0 && _hito.collider.gameObject.GetComponent<Box>() != null)
                {
                    foreach (Box item in AttackRangeBoxes)
                    {
                        if (item != null)
                        {
                            RedCell red = item.UnitOnThis as RedCell;
                            if (red != null)
                            {
                                red.CauseDamage(SelectedCell.AttackPower);

                                
                            }
                        }
                    }

                    GameObject go = Instantiate<GameObject>(VFXPurpleAttack);

                    //VFX pop
                    switch (direction)
                    {
                        case 1:
                            go.transform.position = SelectedCell.transform.position + new Vector3(0f, 0f, 2f);
                            break;
                        case 2:
                            go.transform.position = SelectedCell.transform.position - new Vector3(0f, 0f, 2f);
                            break;
                        case 3:
                            go.transform.position = SelectedCell.transform.position - new Vector3(2f, 0f, 0f);
                            break;
                        case 4:
                            go.transform.position = SelectedCell.transform.position + new Vector3(2f, 0f, 0f);
                            break;
                        default:
                            break;
                    }

                    CancelAttackMode();
                }


                break;
            case Cell.TYPECell.Count:
                break;
            default:
                break;
        }
    }

    void GameOverMode()
    {

    }


    //RedCells IA
    public GameObject prefabRedCell = null;

    public List<RedCell> redCells = new List<RedCell>();
    public List<RedCell> TempRedCells = new List<RedCell>();

    [SerializeField] int MaxPopPerTurn = 1;
    [SerializeField] int CurrentPopNb = 0;
    void RedcellTurn()
    {
        foreach (RedCell cell in redCells)
        {
            cell.PlayTurn();
        }

        foreach (RedCell cell1 in TempRedCells)
        {
            redCells.Add(cell1);
        }

        TempRedCells.Clear();

        CurrentPopNb = 0;
        FinishTurn();
    }

    public void CreateRedCell(int _gen, Box _box)
    {
        if (CurrentPopNb < MaxPopPerTurn && IsBoxInBlueInfluanceRange(_box) == false)
        {
            GameObject go = Instantiate<GameObject>(prefabRedCell);
            RedCell redCell = go.GetComponent<RedCell>();
            redCell.Gen = _gen + 1;
            go.name = "Red_Gen_" + redCell.Gen.ToString();
            board.PlaceUnit(redCell, _box);

            TempRedCells.Add(redCell);

            CurrentPopNb++;
        }

    }

    public bool IsBoxInBlueInfluanceRange(Box _box)
    {
        BlueCell[] blueCells = FindObjectsOfType<BlueCell>();
        if (blueCells != null)
        {
            for (int i = 0; i < blueCells.Length; i++)
            {
                if (blueCells[i].InfluanceRange.Contains(_box))
                    return true;
            }
        }

        return false;
    }



    //SelectMode function
    //MoveMode function
    public void UndoMove()
    {
        if (SelectedCell.AsMoved == true)
        {
            if (SelectedCell != null)
            {
                board.PlaceUnit(SelectedCell, OldCellPos);

                foreach (Box item in MoveRangeBoxes)
                {
                    item.SetAOE();
                }
            }
            SelectedCell.AsMoved = false;
        }

    }

    public void SetAttackMode()
    {
        selectionMode = TYPESelectionMode.AttackMode;
        foreach (Box item in MoveRangeBoxes)
        {
            item.SetUnAOE();
        }
        HighlightBox = null;
    }

    public void BackToSelectMode()
    {
        UndoMove();
        selectionMode = TYPESelectionMode.SelectionMode;
        SelectedCell = null;
        foreach (Box item in MoveRangeBoxes)
        {
            item.SetUnAOE();
        }
        HighlightBox = null;
    }

    //AttackMode function
    public void BackToMoveMode()
    {
        selectionMode = TYPESelectionMode.MoveMode;
    }

    public void CancelAttackMode()
    {
        selectionMode = TYPESelectionMode.SelectionMode;
        SelectedCell.AsFinishedTurn = true;
        SelectedCell = null;

        foreach (Box item in AttackRangeBoxes)
        {
            item.SetUnAttack();
        }
        AttackRangeBoxes.Clear();
    }

    
}
