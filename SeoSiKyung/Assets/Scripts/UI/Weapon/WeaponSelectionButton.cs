using System.Collections.Generic;
using System.Text;
using DataSet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelectionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sprites")]
    Sprite weapon;
    public Sprite background;

    [Header("Visuals")]
    private Image wpImage;
    private Image bgImage;
    private RectTransform rt;
    private Text infoText;
    private GameObject infoGo;

    private Color hoverColor = new Color(1f, 1f, 1f, 1f);
    private Color normalColor = new Color(1f, 1f, 1f, 0.5f);
    private Color selectedColor = new Color(0.8f, 1f, 0.8f, 1f);
    private int pixelSize = 100;

    public int weaponIndex;
    bool isSelected;

    static readonly Dictionary<string, Sprite> IconCache = new Dictionary<string, Sprite>();

    void Awake()
    {
        rt = GetComponent<RectTransform>();

        EnsureHitArea();

        var data = GameManager.instance.weaponDataList[weaponIndex];
        weapon = Resources.Load<Sprite>(data.path);

        BuildButton();
        BuildInfo(data);

        transform.SetAsLastSibling();
    }

    void EnsureHitArea()
    {
        var hit = GetComponent<Image>();
        if (hit == null) hit = gameObject.AddComponent<Image>();
        hit.raycastTarget = true;
        hit.color = new Color(0, 0, 0, 0);
        
        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(pixelSize, pixelSize);
    }

    void BuildButton()
    {
        Debug.Log("button");
        var bgGo = new GameObject("BG", typeof(RectTransform), typeof(Image));
        bgGo.transform.SetParent(transform, false);

        bgImage = bgGo.GetComponent<Image>();
        bgImage.sprite = background;
        bgImage.color = normalColor;
        bgImage.type = Image.Type.Sliced;

        var bgRt = bgGo.GetComponent<RectTransform>();
        bgRt.localScale = Vector3.one;
        
        bgRt.anchorMin = bgRt.anchorMax = bgRt.pivot = new Vector2(0.5f, 0.5f);
        bgRt.sizeDelta = new Vector2(pixelSize, pixelSize);
        
        
        
        var wpGo = new GameObject("WP", typeof(RectTransform), typeof(Image));
        wpGo.transform.SetParent(transform, false);

        wpImage = wpGo.GetComponent<Image>();
        wpImage.sprite = weapon;
            
        wpImage.color = normalColor;
        wpImage.type = Image.Type.Sliced;
        wpImage.raycastTarget = false;

        var wpRt = wpGo.GetComponent<RectTransform>();
        wpRt.localScale = Vector3.one;
            
        wpRt.anchorMin = wpRt.anchorMax = wpRt.pivot = new Vector2(0.5f, 0.5f);
        wpRt.sizeDelta = new Vector2(pixelSize, pixelSize);
        
        wpImage.raycastTarget = false;
        bgImage.raycastTarget = false;
    }

    void BuildInfo(WeaponData data)
    {
        infoGo = new GameObject("INFO", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        infoGo.transform.SetParent(transform, false);

        var infoRt = infoGo.GetComponent<RectTransform>();
        infoRt.anchorMin = Vector2.zero;
        infoRt.anchorMax = Vector2.one;
        infoRt.offsetMin = new Vector2(6, 6);
        infoRt.offsetMax = new Vector2(-6, -6);
        infoRt.pivot = new Vector2(0.5f, 0.5f);

        infoText = infoGo.GetComponent<Text>();
        infoText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        infoText.raycastTarget = false;
        infoText.color = Color.white;
        infoText.alignment = TextAnchor.UpperLeft;
        infoText.horizontalOverflow = HorizontalWrapMode.Wrap;
        infoText.verticalOverflow = VerticalWrapMode.Truncate;
        infoText.resizeTextForBestFit = true;
        infoText.resizeTextMinSize = 10;
        infoText.resizeTextMaxSize = 22;
        infoText.lineSpacing = 1.0f;

        var sb = new StringBuilder();
        sb.AppendLine($"{data.title}");
        sb.Append($"타입 : {data.attackType}\n내구도 : {data.maxDurability}");
        sb.AppendLine();
        sb.AppendLine(string.Join("\n", data.description));

        infoText.text = sb.ToString();

        infoGo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoGo.SetActive(true);
        wpImage.enabled = false;

        if (!isSelected) bgImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoGo.SetActive(false);
        wpImage.enabled = true;

        if (!isSelected) bgImage.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        bgImage.color = isSelected ? selectedColor : hoverColor;
        
        SelectWindow.onClickWeapon?.Invoke(weaponIndex, isSelected);
    }

    public void SetSelected(bool value)
    {
        isSelected = value;
        bgImage.color = isSelected ? selectedColor : normalColor;
    }
}