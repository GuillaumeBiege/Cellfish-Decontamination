using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ConfigPrefab = null;
    [SerializeField] Configurator Config = null;

    [SerializeField] GameObject Mainmenu = null;
    [SerializeField] Button Play = null;
    [SerializeField] Button Quit = null;

    [SerializeField] GameObject Selectionmenu = null;
    [SerializeField] Button PlayLevelOne = null;
    [SerializeField] Button PlayLevelTwo = null;
    [SerializeField] Button PlayLevelThree = null;
    [SerializeField] Button Back = null;

    // Start is called before the first frame update
    void Start()
    {
        Config = FindObjectOfType<Configurator>();
        if (Config == null)
        {
            Config = Instantiate<GameObject>(ConfigPrefab).GetComponent<Configurator>();
        }

        //Main menu
        Play.onClick.AddListener(PlayFunction);
        Quit.onClick.AddListener(QuitFunction);

        //Selection Menu
        PlayLevelOne.onClick.AddListener(PlayLevel1);
        PlayLevelTwo.onClick.AddListener(PlayLevel2);
        PlayLevelThree.onClick.AddListener(PlayLevel3);
        Back.onClick.AddListener(BackFunction);
    }

    private void OnDestroy()
    {
        //Main menu
        Play.onClick.RemoveAllListeners();
        Quit.onClick.RemoveAllListeners();

        //Selection Menu
        PlayLevelOne.onClick.RemoveAllListeners();
        PlayLevelTwo.onClick.RemoveAllListeners();
        PlayLevelThree.onClick.RemoveAllListeners();
        Back.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Main menu
    public void PlayFunction()
    {
        Mainmenu.SetActive(false);
        Selectionmenu.SetActive(true);
    }

    public void QuitFunction()
    {
        Application.Quit();
    }

    //Selection Menu
    public void BackFunction()
    {
        Mainmenu.SetActive(true);
        Selectionmenu.SetActive(false);
    }

    public void PlayLevel1()
    {
        SceneManager.LoadScene(1);
        Config.SetCurrentLevel(1);
    }

    public void PlayLevel2()
    {
        SceneManager.LoadScene(1);
        Config.SetCurrentLevel(2);
    }

    public void PlayLevel3()
    {
        SceneManager.LoadScene(1);
        Config.SetCurrentLevel(3);
    }

    
}
