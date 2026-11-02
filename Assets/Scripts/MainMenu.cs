using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour{

    [Header("Set Up")] // Input the canvas of the menu you want active. (If we want a submenu for the options, we would put a new variable for the canvas of the options menu)
    [SerializeField] private GameObject _MainMenuCanvasGO;
    [SerializeField] private GameObject _LevelSelectCanvasGO;
    [SerializeField] private GameObject _OptionsCanvasGO;
    [SerializeField] private GameObject _CreditsCanvasGO;

    [Header("Default Selected Button")] // Used so that a controler can navigate the menus
    [SerializeField] private GameObject _MainMenuButton;
    [SerializeField] private GameObject _LevelSelectButton;
    [SerializeField] private GameObject _OptionsButton;
    [SerializeField] private GameObject _CreditsButton;

    void Start(){
        OpenMainMenu();
    }

    //----QOL Functions-----------------------------------------------------------

    private void OpenMainMenu(){
        _MainMenuCanvasGO.SetActive(true);
        _LevelSelectCanvasGO.SetActive(false);
        _OptionsCanvasGO.SetActive(false);
        _CreditsCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_MainMenuButton);
    }

    private void OpenLevelSelect(){
        _MainMenuCanvasGO.SetActive(false);
        _LevelSelectCanvasGO.SetActive(true);
        _OptionsCanvasGO.SetActive(false);
        _CreditsCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_LevelSelectButton);
    }

    private void OpenOptionsMenu(){
        _MainMenuCanvasGO.SetActive(false);
        _LevelSelectCanvasGO.SetActive(false);
        _OptionsCanvasGO.SetActive(true);
        _CreditsCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_OptionsButton);
    }

    private void OpenCredits(){
        _MainMenuCanvasGO.SetActive(false);
        _LevelSelectCanvasGO.SetActive(false);
        _OptionsCanvasGO.SetActive(false);
        _CreditsCanvasGO.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_CreditsButton);
    }

    //----Main Menu Buttons-----------------------------------------------------------

    public void PlayButton(){
        Debug.Log("Play Button Pressed");
    }

    public void LevelSelectButton(){
        OpenLevelSelect();
    }

    public void OptionsButton(){
        OpenOptionsMenu();
    }

    public void CreditsButton(){
        OpenCredits();
    }

    public void QuitButton(){
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }

    //----Generic Buttons-----------------------------------------------------------
    
    public void BackButtonMainMenu(){
        OpenMainMenu();
    }

}
