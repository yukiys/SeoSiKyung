using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite blackHeart;
    public Sprite redHeart;

    [Header("Layout")]
    public float size = 32f;
    public float spacing = 4f;
    public Vector2 padding = new Vector2(12f, 12f);

    private List<Image> hearts = new List<Image>();
    private int maxHp;
    private RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();

        rt.anchorMin = new Vector2(0f, 1f);
        rt.anchorMax = new Vector2(0f, 1f);
        rt.pivot = new Vector2(0f, 1f);
        rt.anchoredPosition = new Vector2(padding.x, -padding.y);
    }

    void Start()
    {
        maxHp = GameManager.instance.maxHealth;
        BuildHearts(maxHp);
    }

    void Update()
    {
        int cur = GameManager.instance.health;

        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = (i < cur) ? redHeart : blackHeart;
        }
    }

    void BuildHearts(int maxHp)
    {
        for (int i = 0; i < maxHp; i++)
        {
            var go = new GameObject($"Heart_{i}", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(transform, false);

            var img = go.GetComponent<Image>();
            img.sprite = blackHeart;
            hearts.Add(img);

            var heart = go.GetComponent<RectTransform>();
            heart.sizeDelta = new Vector2(size, size);
            heart.anchorMin = heart.anchorMax = heart.pivot = new Vector2(0f, 1f);

            float x = i * (size + spacing);
            heart.anchoredPosition = new Vector2(x, 0f);
        }

        float width = maxHp * size + (maxHp - 1) * spacing;
        rt.sizeDelta = new Vector2(width, size);
    }
}