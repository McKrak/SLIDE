using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour{

    [Header("Set Up")] // Input the canvas of the menu you want active. (If we want a submenu for the options, we would put a new variable for the canvas of the options menu)
    [SerializeField] private GameObject _MainMenuCanvasGO;
    [SerializeField] private GameObject _LevelSelectCanvasGO;
    [SerializeField] private GameObject _OptionsCanvasGO;

    [Header("Default Selected Button")] // Used so that a controler can navigate the menus
    [SerializeField] private GameObject _MainMenuButton;
    [SerializeField] private GameObject _LevelSelectButton;
    [SerializeField] private GameObject _OptionsButton;

    void Start(){
        _MainMenuCanvasGO.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(_MainMenuButton);
    }

    //---------------------------------------------------------------

    public void PlayButton(){
        Debug.Log("Play Button Pressed");
    }

    public void LevelSelectButton(){
        Debug.Log("Level Select Button Pressed");
    }

    public void OptionsButton(){
        Debug.Log("Options Button Pressed");
    }

    public void CreditsButton(){
        Debug.Log("Credits Button Pressed");
    }

    public void QuitButton(){
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }

}
