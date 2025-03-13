using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupGame : MonoBehaviour
{
    Transform setupPanel;
    Button PointThiefButton, ClassicButton;
    private void Awake()
    {
        setupPanel = transform.Find("Setup");

        PointThiefButton = setupPanel.Find("pointthief").GetComponent<Button>();
        ClassicButton = setupPanel.Find("classic").GetComponent<Button>();

        setupPanel.gameObject.SetActive(true);

        PointThiefButton.onClick.RemoveAllListeners();
        ClassicButton.onClick.RemoveAllListeners();

        PointThiefButton.onClick.AddListener(() => { SelectGameMode(GameMode.PointThief); });
        ClassicButton.onClick.AddListener(() => { SelectGameMode(GameMode.Classic); });
    }

    public void SelectGameMode(GameMode gameMode)
    {
        GameManager.instance.gameMode = gameMode;
        GameManager.instance.StartUpGame();
        setupPanel.gameObject.SetActive(false);
    }
}
