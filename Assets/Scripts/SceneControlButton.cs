using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneControlButton : MonoBehaviour
{
    enum TargetScene 
    {
        Next,
        Previous,
        MainMenu
    }

    [SerializeField] private TargetScene _targetScene;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.RemoveAllListeners();

        switch (_targetScene)
        {
            case TargetScene.MainMenu:
                _button.onClick.AddListener(() => SceneController.LoadMainScene());
                break;

            case TargetScene.Next:
                _button.onClick.AddListener(() => SceneController.LoadNextScene());
                break;

            case TargetScene.Previous:
                _button.onClick.AddListener(() => SceneController.LoadPreviousScene());
                break;
        }
    }
}
