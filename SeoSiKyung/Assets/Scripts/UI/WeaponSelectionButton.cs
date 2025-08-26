using System.Collections.Generic;
using DataSet;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSelectionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Sprites")]
    public Sprite weapon;
    public Sprite background;

    [Header("Visuals")]
    private Image wpImage;
    private Image bgImage;
    private RectTransform rt;
    private Color hoverColor = new Color(1f, 1f, 1f, 1f);
    private Color normalColor = new Color(1f, 1f, 1f, 0.5f);
    private Color selectedColor = new Color(0.8f, 1f, 0.8f, 1f);
    private int pixelSize = 100;

    public int weaponIndex;

    bool isSelected;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        
        BuildButton();
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
        bgImage.raycastTarget = true;
    }
    
    // 마우스가 올라왔을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
        {
            bgImage.color = hoverColor;
            wpImage.color = hoverColor;
        }
    }

    // 마우스가 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            bgImage.color = normalColor;
            wpImage.color = normalColor;
        }
    }

    // 클릭했을 때
    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        bgImage.color = isSelected ? selectedColor : hoverColor;
        wpImage.color = hoverColor;
        SelectWindow.onClickWeapon?.Invoke(weaponIndex, isSelected);
    }

    public void SetSelected(bool value)
    {
        isSelected = value;
        bgImage.color = isSelected ? selectedColor : normalColor;
    }
}
