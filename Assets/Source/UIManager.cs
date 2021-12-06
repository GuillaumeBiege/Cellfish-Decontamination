using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Ref
    GameManager GM = null;

    [SerializeField] GameObject MainHUD = null;
    [SerializeField] Text TurnText = null;

    [SerializeField] GameObject menuSelect = null;
    [SerializeField] Button buttonNextTurn = null;

    [SerializeField] GameObject menuMove = null;
    [SerializeField] Button buttonUndoMove = null;
    [SerializeField] Button buttonAttackMode = null;
    [SerializeField] Button buttonBackToSelect = null;


    [SerializeField] GameObject menuAttack = null;
    [SerializeField] Button buttonBack = null;
    [SerializeField] Button buttonSkipAttack = null;


    [SerializeField] GameObject menuGameOver = null;
    [SerializeField] Text GameOverText = null;
    [SerializeField] Text GameOverTextEnd = null;
    [SerializeField] Button buttonNextLevel = null;
    [SerializeField] Button buttonRestart = null;
    [SerializeField] Button buttonBackToMenu = null;

    [SerializeField] GameObject ImageNextTurn = null;





    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();

        buttonNextTurn.onClick.AddListener(NextTurn);
        buttonUndoMove.onClick.AddListener(UndoMove);
        buttonAttackMode.onClick.AddListener(AttackMode);
        buttonBackToSelect.onClick.AddListener(BackToSelect);
        buttonBack.onClick.AddListener(Back);
        buttonSkipAttack.onClick.AddListener(SkipAttack);
        buttonNextLevel.onClick.AddListener(NextLevel);
        buttonRestart.onClick.AddListener(Restart);
        buttonBackToMenu.onClick.AddListener(BackToMenu);
    }

    private void OnDestroy()
    {
        buttonNextTurn.onClick.RemoveAllListeners();
        buttonUndoMove.onClick.RemoveAllListeners();
        buttonAttackMode.onClick.RemoveAllListeners();
        buttonBackToSelect.onClick.AddListener(BackToSelect);
        buttonBack.onClick.RemoveAllListeners();
        buttonSkipAttack.onClick.RemoveAllListeners();
        buttonNextLevel.onClick.RemoveAllListeners();
        buttonRestart.onClick.RemoveAllListeners();
        buttonBackToMenu.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        TurnText.text = "Turn " + GM.NbTurn.ToString() + " / " + GM.TurnTimer.ToString();

        switch (GM.selectionMode)
        {
            case GameManager.TYPESelectionMode.SelectionMode:
                MainHUD.SetActive(true);
                menuSelect.SetActive(true);
                menuMove.SetActive(false);
                menuAttack.SetActive(false);
                menuGameOver.SetActive(false);


                break;
            case GameManager.TYPESelectionMode.MoveMode:
                MainHUD.SetActive(true);
                menuSelect.SetActive(false);
                menuMove.SetActive(true);
                menuAttack.SetActive(false);
                menuGameOver.SetActive(false);

                break;
            case GameManager.TYPESelectionMode.AttackMode:
                MainHUD.SetActive(true);
                menuSelect.SetActive(false);
                menuMove.SetActive(false);
                menuAttack.SetActive(true);
                menuGameOver.SetActive(false);

                break;

            case GameManager.TYPESelectionMode.GameOverMode:
                MainHUD.SetActive(false);
                menuSelect.SetActive(false);
                menuMove.SetActive(false);
                menuAttack.SetActive(false);
                menuGameOver.SetActive(true);

                GameOverText.text = (GM.IsPlayerVictorious) ? "Victory !" : "Defeat !";

                if (!GM.IsPlayerVictorious)
                {
                    buttonNextLevel.gameObject.SetActive(false);
                }

                Configurator config = FindObjectOfType<Configurator>();
                if (config != null && config.currentLevel >= 3 && GM.IsPlayerVictorious)
                {
                    GameOverTextEnd.gameObject.SetActive(true);
                    buttonNextLevel.gameObject.SetActive(false);
                }

                break;
            default:
                break;
        }

        ImageNextTurn.SetActive(GM.AsAllCellFinished);
    }

    //Button Function
    public void NextTurn()
    {
        GM.IncrementTurn();
    }

    public void UndoMove()
    {
        GM.UndoMove();
    }

    public void BackToSelect()
    {
        GM.BackToSelectMode();
    }

    public void AttackMode()
    {
        GM.SetAttackMode();
    }

    public void Back()
    {
        GM.BackToMoveMode();
    }

    public void SkipAttack()
    {
        GM.CancelAttackMode();
    }

    public void NextLevel()
    {
        Configurator config = FindObjectOfType<Configurator>();
        if (config != null)
        {
            config.IncreaseLevel();
        }
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }


}
