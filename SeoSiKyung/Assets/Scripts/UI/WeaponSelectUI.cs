using System.Collections.Generic;
using DataSet;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectUI : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite sword;
    public Sprite hammer;
    public Sprite spear;
    public Sprite crossbow;
    public Sprite firestaff;
    public Sprite icestaff;
    public Sprite background;
    public Sprite outline;

    [Header("Layout")]
    public int weaponCount = 3;
    public int pixelSize = 100;
    public int pixelGap = 5;
    public Vector2 padding = new Vector2(12f, 50f);

    private Image backImg;
    private Image wpImg;
    private GameObject[,] weaponUI = new GameObject[3, 2];
    private RectTransform rt;
    
    private List<WeaponData> selectedWeapons;
    private Dictionary<string, Sprite> weaponSprites;

    private int curWeapon = 1;
    private int prevWeapon = 1;

    void Awake()
    {
        rt = GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(padding.x, -padding.y);
        
        weaponSprites = new Dictionary<string, Sprite>
        {
            { "Sword", sword },
            { "Hammer", hammer },
            { "Spear", spear },
            { "Crossbow", crossbow },
            { "FireStaff", firestaff },
            { "IceStaff", icestaff }
        };
    }
    void Start()
    {
        BuildBackground();
    }

    void Update()
    {
        if (prevWeapon != curWeapon)
        {
            
        }
    }


    public void ChangeWeapon(int num)
    {
        curWeapon = num;
        if (prevWeapon != curWeapon)
        {
            weaponUI[prevWeapon - 1, 0].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            weaponUI[prevWeapon - 1, 1].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            weaponUI[curWeapon - 1, 0].GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
            weaponUI[curWeapon - 1, 1].GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            prevWeapon = curWeapon;
        }
    }
    
    
    
    void BuildBackground()
    {
        for (int i = 0; i < weaponCount; i++)
        {
            var bgGo = new GameObject($"BG_{i}", typeof(RectTransform), typeof(Image));
            bgGo.transform.SetParent(transform, false);

            backImg = bgGo.GetComponent<Image>();
            backImg.sprite = background;
            backImg.color = new Color(1, 1, 1, 0.5f);
            backImg.type = Image.Type.Sliced;
            backImg.raycastTarget = false;

            var bgRt = bgGo.GetComponent<RectTransform>();
            bgRt.localScale = Vector3.one;
            
            bgRt.anchorMin = bgRt.anchorMax = bgRt.pivot = new Vector2(0.5f, 0.5f);
            bgRt.sizeDelta = new Vector2(pixelSize, pixelSize);
            float x = i * (pixelSize + pixelGap);
            bgRt.anchoredPosition = new Vector2(padding.x + x, -padding.y);
            
            weaponUI[i, 0] = bgGo;
        }
    }

    public void BuildWeapons()
    {
        for (int i = 0; i < weaponCount; i++)
        {
            var wpGo = new GameObject($"WP_{i}", typeof(RectTransform), typeof(Image));
            wpGo.transform.SetParent(transform, false);

            wpImg = wpGo.GetComponent<Image>();
            if (weaponSprites.TryGetValue(selectedWeapons[i].weaponName, out var sprite))
                wpImg.sprite = sprite;
            
            wpImg.color = new Color(1, 1, 1, 0.5f);
            wpImg.type = Image.Type.Sliced;
            wpImg.raycastTarget = false;

            var wpRt = wpGo.GetComponent<RectTransform>();
            wpRt.localScale = Vector3.one;
            
            wpRt.anchorMin = wpRt.anchorMax = wpRt.pivot = new Vector2(0.5f, 0.5f);
            wpRt.sizeDelta = new Vector2(pixelSize, pixelSize);
            float x = i * (pixelSize + pixelGap);
            wpRt.anchoredPosition = new Vector2(padding.x + x, -padding.y);
            
            weaponUI[i, 1] = wpGo;
        }
        
    }

    public void SetWeaponList(List<WeaponData> weapon)
    {
        selectedWeapons = weapon;
    }
}
