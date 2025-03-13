using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Combination : MonoBehaviour
{
    TMP_Text titleText, infoText;
    GameObject reborde;
    Transform cardTransform;
    
    List<Sprite> cardSprites;

    public Sprite openSprite, closeSprite;
    public float XTarget = 1234;

    RectTransform infoRect;
    GameObject extraPoints, extrabutton;
    Button MinusButton, PlusButton;
    TMP_Text ExtraText;
    [SerializeField] bool toggleInfo;

    Toggle SelectToggle;

    public bool openRight;
    public GameObject cardPrefab;

    [Header("Data")]
    public Combinations CombinationType;
    [SerializeField] CardCombination cardCombination;
    public int extraCardsAdded;

    public CombiChosePanel chosePanelScript;
    // Start is called before the first frame update
    void Awake()
    {
        cardCombination = AllCombinationData.GetData(CombinationType);
        cardSprites = GameManager.instance.CardSprites;
        FindComponents();
        SetInfo();

        reborde.SetActive(SelectToggle.isOn);
        infoRect.anchoredPosition = Vector2.zero;

        toggleInfo = false;
        MinusButton.onClick.RemoveAllListeners();
        PlusButton.onClick.RemoveAllListeners();

        MinusButton.onClick.AddListener(() => { AddExtraCards(false); });
        PlusButton.onClick.AddListener(() => { AddExtraCards(true); });

    }

    private void OnEnable()
    {
        //Cambia las cartas del mes solo a la combinacion de estaciones
        if(CombinationType == Combinations.Estaciones)
        {
            for (int i = 0; i < cardTransform.childCount; i++)
            {
                Transform t = cardTransform.GetChild(i);
                int month = Mathf.Clamp(GameManager.instance.round - 1, 0, 11);

                t.GetComponent<Image>().sprite = cardSprites[i + (month * 4)];
            }
        }
    }

    /// <summary>
    /// Asigna todas las variables a sus componentes en el juego
    /// </summary>
    public void FindComponents()
    {
        infoRect = transform.Find("Info").GetComponent<RectTransform>();
        chosePanelScript = FindObjectOfType<CombiChosePanel>();
        cardTransform = transform.Find("Cards");
        titleText = transform.Find("Title").GetComponent<TMP_Text>();
        reborde = transform.Find("reborde").gameObject;

        infoText = infoRect.Find("Info").GetComponent<TMP_Text>();
        extraPoints = infoRect.Find("extrapts").gameObject;
        extrabutton = transform.Find("openinfo").gameObject;


        MinusButton = extraPoints.transform.Find("rest").GetComponent<Button>();
        PlusButton = extraPoints.transform.Find("add").GetComponent<Button>();
        ExtraText = extraPoints.transform.Find("extra").GetComponentInChildren<TMP_Text>(true);

        SelectToggle = transform.Find("selectButton").GetComponent<Toggle>();        
    }


    public void SetExtraPoints(int p)
    {
        extraCardsAdded = p;
        ExtraText.text = p.ToString();
    }

    /// <summary>
    /// Cierra la informacion
    /// </summary>
    public void CloseInfoButton()
    {
        toggleInfo = true;
        ToggleInfoButton(true);
    }

    /// <summary>
    /// Selecciona esta combinacion
    /// </summary>
    public void ToggleSelectedCombi()
    {
        reborde.SetActive(SelectToggle.isOn);
        chosePanelScript.SelectCombination(cardCombination, extraCardsAdded, SelectToggle.isOn);
    }

    /// <summary>
    /// Añade o quita cartas extra
    /// </summary>
    /// <param name="add"></param>
    public void AddExtraCards(bool add)
    {
        extraCardsAdded = Mathf.Clamp(extraCardsAdded + (add ? 1 : -1), 0, 9);
        ExtraText.text = extraCardsAdded.ToString();

        if (SelectToggle.isOn)
        {
            chosePanelScript.UpdateCombination(cardCombination, extraCardsAdded);
        }
    }

    /// <summary>
    /// Instancia las cartas, escribe la informacion, etc etc
    /// </summary>
    void SetInfo()
    {
        titleText.text = $"{cardCombination.title}   <color=yellow>{cardCombination.points} pt</color>";
        infoText.text = cardCombination.info;

        extraPoints.SetActive(cardCombination.canAddExtra);
        extraCardsAdded = 0;
        ExtraText.text = extraCardsAdded.ToString();

        for (int i = 0; i < cardCombination.cardSpritesIndex.Count; i++)
        {
            Transform card = Instantiate(cardPrefab).transform;
            card.SetParent(cardTransform, false);

            card.GetComponent<Image>().sprite =
                cardSprites[cardCombination.cardSpritesIndex[i]];
        }
    }

    /// <summary>
    /// Se llama cuando se pulsa el boton de informacion
    /// </summary>
    /// <param name="forceInstant">Si el movimiento es instantaneo o no</param>
    public void ToggleInfoButton(bool forceInstant)
    {
        toggleInfo = !toggleInfo;

        if(toggleInfo)
            transform.SetAsLastSibling();

        extrabutton.transform.GetChild(0).GetComponent<Image>().sprite = toggleInfo ? openSprite : closeSprite;

        StopAllCoroutines();

        StartCoroutine(MoveInfoPanel((toggleInfo ? XTarget * (openRight ? 1 : -1) : 0), forceInstant));


    }

    /// <summary>
    /// Mueve el panel de la informacion hacia los lados
    /// </summary>
    /// <param name="targetX">A que direccion moverse</param>
    /// <param name="forceInsant">Si el movimiento es instantaneo o no</param>
    /// <returns></returns>
    IEnumerator MoveInfoPanel(float targetX, bool forceInsant)
    {
        bool right = infoRect.anchoredPosition.x - targetX < 0;
        float speed = 3000;

        if (!forceInsant)
        {
            if (right) 
            {
                while (infoRect.anchoredPosition.x - targetX < -0.1f)
                {
                    infoRect.anchoredPosition += Vector2.right * speed * Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (infoRect.anchoredPosition.x - targetX > 0.1f)
                {
                    infoRect.anchoredPosition += Vector2.right *  -1 * speed * Time.deltaTime;
                    yield return null;
                }
            }
        }
        else
        {
            infoRect.anchoredPosition = new Vector2(targetX, 0);
        }
        
    }
}
