
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PreBoarder
{
    public int[,] content =
     {
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,1,0,0,0,0,0,0,0 },
        {0,0,0,0,2,2,0,0,0,0 },
        {0,0,0,0,3,3,2,0,7,0 },
        {0,0,0,0,3,4,3,0,0,0 },
        {0,0,5,0,0,2,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,6,0,0,0,0,1,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
    };
}

public class Board : MonoBehaviour
{
    //ref
    GameManager GM = null;

    //Prefabs
    public GameObject prefabBox = null;
    public GameObject prefabBloc = null;
    public GameObject prefabRedcell = null;

    public GameObject prefabBluecell = null;
    public GameObject prefabYellowcell = null;
    public GameObject prefabPurplecell = null;

    const uint MaxSizeBoardX = 10;
    const uint MaxSizeBoardY = 10;

    public int[,] PreBoard =
    {
        {0,0,0,0,0,0,0,0,0,1 },
        {0,0,1,0,0,0,0,0,0,0 },
        {0,0,0,0,2,2,0,0,0,0 },
        {0,0,0,0,3,3,2,0,7,0 },
        {0,0,0,0,3,4,3,0,0,0 },
        {0,0,5,0,0,2,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,6,0,0,0,0,1,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
        {0,0,0,0,0,0,0,0,0,0 },
    };


    public Box[,] boxes = new Box[MaxSizeBoardX, MaxSizeBoardY];

    string fileName = "1";


    private void Awake()
    {
        GM = FindObjectOfType<GameManager>();


    }

    // Start is called before the first frame update
    void Start()
    {
        Configurator config = FindObjectOfType<Configurator>();
        if (config != null)
        {
            fileName = config.GetCurrentLevel();
        }
        

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        print("Streaming Assets Path: " + Application.streamingAssetsPath);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");

        foreach (FileInfo file in allFiles)
        {
            if (file.Name.Contains(fileName))
            {
                Debug.Log(file.FullName);
                StreamReader reader = new StreamReader(file.FullName);
                string content = reader.ReadToEnd();
                Debug.Log(content);
                reader.Close();


                string[] lines = content.Split("\n"[0]);

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] linedata = (lines[i].Trim()).Split(","[0]);

                    for (int j = 0; j < linedata.Length; j++)
                    {
                        PreBoard[i, j] = int.Parse(linedata[j]);
                        Debug.Log(linedata[j]);
                    }

                }


                //PreBoarder moka = JsonUtility.FromJson<PreBoarder>(content);
                //PreBoard = moka.content;

                //for (int i = 0; i < 10; i++)
                //{
                //    for (int j = 0; j < 10; j++)
                //    {
                //        PreBoard[i, j] = moka.content[i, j];
                //    }
                //}

                break;
            }
        }

        for (int y = 0; y < MaxSizeBoardY; y++)
        {
            for (int x = 0; x < MaxSizeBoardX; x++)
            {
                GameObject go = Instantiate(prefabBox, gameObject.transform);
                go.name = "Box_" + x.ToString() + "_" + y.ToString();
                go.transform.position = new Vector3(x, 0f, y);

                Box box = go.GetComponent<Box>();
                if (box != null)
                {
                    box.pox = (uint)x;
                    box.poy = (uint)y;

                    boxes[x, y] = box;
                }

                if (PreBoard[x, y] == 1)
                {
                    GameObject ga = Instantiate(prefabBloc);
                    Unit unit = ga.GetComponent<Unit>();
                    if (unit != null)
                    {
                        PlaceUnit(unit, (uint)x, (uint)y);
                    }
                }

                //RedCell
                if (PreBoard[x, y] >= 2 && PreBoard[x, y] <= 4)
                {
                    GameObject ga = Instantiate(prefabRedcell);
                    RedCell red = ga.GetComponent<RedCell>();
                    if (red != null)
                    {
                        if (PreBoard[x, y] == 3)
                        {
                            red.Level = 2;
                            red.CurrentHealth = 3;
                            red.MaxHealth = 3;
                        }
                        else if (PreBoard[x, y] == 4)
                        {
                            red.Level = 3;
                            red.CurrentHealth = 5;
                            red.MaxHealth = 5;
                        }
                        PlaceUnit(red, (uint)x, (uint)y);

                        GM.redCells.Add(red);
                    }
                }

                //BlueCell
                if (PreBoard[x, y] == 5)
                {
                    GameObject ga = Instantiate(prefabBluecell);
                    Unit unit = ga.GetComponent<Unit>();
                    if (unit != null)
                    {
                        PlaceUnit(unit, (uint)x, (uint)y);
                        GM.cells.Add(unit as Cell);
                    }
                }

                //YellowCell
                if (PreBoard[x, y] == 6)
                {
                    GameObject ga = Instantiate(prefabYellowcell);
                    Unit unit = ga.GetComponent<Unit>();
                    if (unit != null)
                    {
                        PlaceUnit(unit, (uint)x, (uint)y);
                        GM.cells.Add(unit as Cell);
                    }
                }

                //PurpleCell
                if (PreBoard[x, y] == 7)
                {
                    GameObject ga = Instantiate(prefabPurplecell);
                    Unit unit = ga.GetComponent<Unit>();
                    if (unit != null)
                    {
                        PlaceUnit(unit, (uint)x, (uint)y);
                        GM.cells.Add(unit as Cell);
                    }
                }
            }
        }
    }

    public bool PlaceUnit(Unit _unitToPlace, uint _pox, uint _poy)
    {
        if (_pox < 0 || _pox >= MaxSizeBoardX || _poy < 0 || _poy >= MaxSizeBoardY)
        {
            Debug.LogWarning("Invalid new coordinate for the Unit (outside the Board) !");
            return false;
        }


        if (boxes[_pox, _poy].UnitOnThis != null)
        {
            Debug.LogWarning("Invalid there is already a unit at those coordinate !");
            return false;
        }

        if (_unitToPlace.CurrentBox != null)
        {
            _unitToPlace.CurrentBox.UnitOnThis = null;
        }

        boxes[_pox, _poy].UnitOnThis = _unitToPlace;
        _unitToPlace.CurrentBox = boxes[_pox, _poy];

        return true;
    }

    public bool PlaceUnit(Unit _unitToPlace, Box _box)
    {
        if (_box.pox < 0 || _box.pox >= MaxSizeBoardX || _box.poy < 0 || _box.poy >= MaxSizeBoardY)
        {
            Debug.LogWarning("Invalid new coordinate for the Unit (outside the Board) !");
            return false;
        }


        if (boxes[_box.pox, _box.poy].UnitOnThis != null)
        {
            Debug.LogWarning("Invalid there is already a unit at those coordinate !");
            return false;
        }

        if (_unitToPlace.CurrentBox != null)
        {
            _unitToPlace.CurrentBox.UnitOnThis = null;
        }

        boxes[_box.pox, _box.poy].UnitOnThis = _unitToPlace;
        _unitToPlace.CurrentBox = boxes[_box.pox, _box.poy];

        return true;
    }

    public Box GetBoxAtCoordinate(uint _pox, uint _poy)
    {
        if (_pox < 0 || _pox >= MaxSizeBoardX || _poy < 0 || _poy >= MaxSizeBoardY)
        {
            Debug.LogWarning("Invalid new coordinate for the Unit (outside the Board) !");
            return null;
        }

        return boxes[_pox, _poy];
    }


    public List<Box> GetMoveRange(Box _initialBox, int _Range)
    {
        if (_initialBox == null)
        {
            return null;
        }

        List<Box> result = new List<Box>();

        RecurciveRange(_Range, result, _initialBox, _initialBox);

        return result;
    }

    void RecurciveRange(int _Range, List<Box> _list, Box _box, Box _initialBox)
    {
        int distance = Math.Abs((int)_box.pox - (int)_initialBox.pox) + Math.Abs((int)_box.poy - (int)_initialBox.poy);
        if (distance >= _Range)
        {
            return;
        }

        Box temp = GetBoxAtCoordinate(_box.pox - 1, _box.poy);
        if (temp != null && !_list.Contains(temp) && temp.UnitOnThis == null)
        {
            _list.Add(temp);
            RecurciveRange(_Range, _list, temp, _initialBox);
        }

        temp = GetBoxAtCoordinate(_box.pox + 1, _box.poy);
        if (temp != null && !_list.Contains(temp) && temp.UnitOnThis == null)
        {
            _list.Add(temp);
            RecurciveRange(_Range, _list, temp, _initialBox);
        }

        temp = GetBoxAtCoordinate(_box.pox, _box.poy - 1);
        if (temp != null && !_list.Contains(temp) && temp.UnitOnThis == null)
        {
            _list.Add(temp);
            RecurciveRange(_Range, _list, temp, _initialBox);
        }

        temp = GetBoxAtCoordinate(_box.pox, _box.poy + 1);
        if (temp != null && !_list.Contains(temp) && temp.UnitOnThis == null)
        {
            _list.Add(temp);
            RecurciveRange(_Range, _list, temp, _initialBox);
        }


        return;
    }
}
