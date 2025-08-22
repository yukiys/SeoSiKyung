using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite backSprite;
    public Sprite fillSprite;

    [Header("Layout")]
    public float offsetY;
    public float w = 0.5f;
    public float h = 0.26f;
    public float gap = 0.1f;
    public float padding = 0.15f;

    [Header("Color")]
    private Color fillColor = Color.red;
    private Color emptyColor = new Color(1, 1, 1, 0.25f);

    private Enemy enemy;
    private Canvas canvas;
    private RectTransform rt;
    Image BackImg;
    private int maxHp;
    private int cachedHp;
    readonly List<Image> segments = new List<Image>();
    private RectTransform content;

    void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        canvas = GetComponent<Canvas>();
        rt = GetComponent<RectTransform>();

        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
    }

    void Start()
    {
        maxHp = GameManager.instance.GetEnemyData(enemy.enemyName).maxHp;
        BuildBackGround();
        BuildSegments(maxHp);
        UpdateBar();
    }

    void LateUpdate()
    {
        transform.position = enemy.transform.position + Vector3.up * offsetY;
    
        if(enemy.Hp!=cachedHp) UpdateBar();
    }

    void BuildBackGround()
    {
        var bgGo = new GameObject("BG", typeof(RectTransform), typeof(Image));
        bgGo.transform.SetParent(transform, false);

        BackImg = bgGo.GetComponent<Image>();
        BackImg.sprite = backSprite;
        BackImg.type = Image.Type.Sliced;
        BackImg.raycastTarget = false;

        var bgRt = bgGo.GetComponent<RectTransform>();
        bgRt.localScale = Vector3.one;
        bgRt.anchorMin = bgRt.anchorMax = bgRt.pivot = new Vector2(0.5f, 0.5f);
    }

    void BuildSegments(int maxHp)
    {
        var contentGo = new GameObject("Content", typeof(RectTransform));
        contentGo.transform.SetParent(transform, false);

        content = contentGo.GetComponent<RectTransform>();
        content.anchorMin = content.anchorMax = content.pivot = new Vector2(0.5f, 0.5f);
        content.localScale = Vector3.one;

        segments.Clear();

        float totalFillWidth = maxHp * w + (maxHp - 1) * gap;
        float totalWidth = totalFillWidth + padding * 2f;
        float totalHeight = h + padding * 2f;

        BackImg.rectTransform.sizeDelta = new Vector2(totalWidth, totalHeight);

        content.sizeDelta = new Vector2(totalFillWidth, h);
        content.localPosition = Vector3.zero;

        float start = -0.5f * totalFillWidth;
        float x = start;

        for (int i = 0; i < maxHp; i++)
        {
            var fillGo = new GameObject($"Fill_{i}", typeof(RectTransform), typeof(Image));
            fillGo.transform.SetParent(content, false);

            var fillRt = fillGo.GetComponent<RectTransform>();
            fillRt.sizeDelta = new Vector2(w, h);
            fillRt.localPosition = new Vector2(x + 0.5f * w, 0f);

            var fillImg = fillGo.GetComponent<Image>();
            fillImg.sprite = fillSprite;
            fillImg.color = emptyColor;
            fillImg.raycastTarget = false;

            segments.Add(fillImg);

            x += w + gap;
        }

        rt.sizeDelta = BackImg.rectTransform.sizeDelta;
    }

    void UpdateBar()
    {
        int cur = enemy.Hp;
        cachedHp = cur;

        for (int i = 0; i < maxHp; i++)
        {
            var seg = segments[i];

            seg.color = i < cur ? fillColor : emptyColor;
        }
    }
}