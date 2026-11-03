using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour{
    
    [SerializeField] private TextMeshProUGUI _levelNameText;

    public LevelData levelData {get; set;}

    private Button _button;

    private Image _image;

    private void Awake(){
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    public void Setup(LevelData level, bool isUnlocked){
        
        levelData = level;
        _levelNameText.SetText(level.LevelID);

        _button.interactable = isUnlocked;

        if (isUnlocked){
            _button.onClick.AddListener(LoadLevel);

        }


    }

    public void Unlock(){
        
        _button.interactable = true;
        _button.onClick.AddListener(LoadLevel);

    }

    public void LoadLevel(){
        
        SceneManager.LoadScene(levelData.Scene);
    }

}
