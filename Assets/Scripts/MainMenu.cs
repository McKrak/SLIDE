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
        _MainMenuCanvasGO.SetActive(true);
        _OptionsCanvasGO.SetActive(false);
        _CreditsCanvasGO.SetActive(false);
        
        EventSystem.current.SetSelectedGameObject(_MainMenuButton);
    }

    //----Main Menu Buttons-----------------------------------------------------------

    public void PlayButton(){
        Debug.Log("Play Button Pressed");
    }

    public void LevelSelectButton(){
        Debug.Log("Level Select Button Pressed");
    }

    public void OptionsButton(){
        _MainMenuCanvasGO.SetActive(false);
        _OptionsCanvasGO.SetActive(true);
        _CreditsCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_OptionsButton);
    }

    public void CreditsButton(){
        _MainMenuCanvasGO.SetActive(false);
        _OptionsCanvasGO.SetActive(false);
        _CreditsCanvasGO.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_CreditsButton);

    }

    public void QuitButton(){
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }

    //----Options Buttons-----------------------------------------------------------

    public void OptionsBackButton(){
        _MainMenuCanvasGO.SetActive(true);
        _OptionsCanvasGO.SetActive(false);
        _CreditsCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_MainMenuButton);
    }

    //----Credits Buttons-----------------------------------------------------------

    public void CreditsBackButton(){
        _MainMenuCanvasGO.SetActive(true);
        _OptionsCanvasGO.SetActive(false);
        _CreditsCanvasGO.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_MainMenuButton);
    }

}
