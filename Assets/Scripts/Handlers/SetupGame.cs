using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupGame : MonoBehaviour
{
    TMP_Text titulo;
    Transform setupPanel, btnPanel, menuPanel, settingPanel;
    GameMode gameModeSelected;

    private void OnEnable()
    {
        GameEventsManager.instance.gameEvents.onSetupOpen += SelectGameMode;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.gameEvents.onSetupOpen -= SelectGameMode;
    }

    private void Awake()
    {
        setupPanel = transform.Find("Setup");
        btnPanel = setupPanel.Find("BOTONES");
        menuPanel = setupPanel.Find("MENUS");
        settingPanel = menuPanel.Find("Scroll View").Find("Viewport").Find("Content");
        titulo = setupPanel.Find("Titulo").GetComponent<TMP_Text>();

        for (int i = 0; i < btnPanel.childCount; i++)
        {
            int index = i;
            Button btn = btnPanel.GetChild(i).GetComponent<Button>();

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => { ChosenGameMode(index); });
        }

        menuPanel.Find("ACCEPT").GetComponent<Button>().
            onClick.AddListener(() => { AcceptSettings(); });

        menuPanel.Find("CANCEL").GetComponent<Button>().
            onClick.AddListener(() => { SelectGameMode(); });

        SelectGameMode();
    }

    public void SelectGameMode()
    {
        titulo.text = "Elige el modo de juego";

        setupPanel.gameObject.SetActive(true);
        btnPanel.gameObject.SetActive(true);
        menuPanel.gameObject.SetActive(false);
    }

    public void ChosenGameMode(int index)
    {
        GameMode[] gamemodeArray = ((GameMode[])Enum.GetValues(typeof(GameMode)));

        gameModeSelected = gamemodeArray[index];

        titulo.text = "Personaliza tu partida";

        btnPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);
        ActivateSettingMenu(index);
    }

    /// <summary>
    /// Llamada al pulsar el boton de aceptar
    /// </summary>
    public void AcceptSettings()
    {
        //Acepta los valores de cada GameRule activo
        foreach(GameRule gr in FindObjectsOfType<GameRule>())
            gr.AcceptValues();

        GameManager.instance.gameMode = gameModeSelected;
        GameManager.instance.StartUpGame();
        setupPanel.gameObject.SetActive(false);
    }

    void ActivateSettingMenu(int index)
    {
        foreach (Transform t in settingPanel) 
        {
            t.gameObject.SetActive(t.GetSiblingIndex() == index);
        }
    }
}
