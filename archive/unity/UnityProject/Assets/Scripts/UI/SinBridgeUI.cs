using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Child‑friendly visual that shows: "Sin (separation) -> Jesus covers / bridges -> Entrance to the Kingdom".
/// - Attach to a GameObject under a Canvas (or use the Editor menu to create a ready setup).
/// - Inspector fields let you tweak colors, labels, timings and optional sprites.
/// - Call Play() to run the short animated sequence.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class SinBridgeUI : MonoBehaviour
{
    [Header("Layout")]
    public RectTransform container; // optional. if null, uses this RectTransform
    public Vector2 panelSize = new Vector2(680, 180);

    [Header("Labels")]
    public string humanLabel = "You";
    public string barrierLabel = "Sin — separation";
    public string bridgeLabel = "Jesus covers";
    public string kingdomLabel = "Kingdom";

    [Header("Colors")]
    public Color humanColor = new Color(0.2f, 0.6f, 1f, 1f);
    public Color barrierColor = new Color(0.9f, 0.2f, 0.2f, 1f);
    public Color bridgeColor = new Color(1f, 0.85f, 0.2f, 1f);
    public Color kingdomColor = new Color(0.2f, 0.9f, 0.4f, 1f);

    [Header("Timing")]
    public float startDelay = 0.4f;
    public float bridgeDropDuration = 0.9f;
    public float barrierCoverFade = 0.5f;
    public float walkDuration = 1.2f;
    public float endGlowDuration = 0.5f;

    [Header("Optional art")]
    public Sprite crossSprite;   // optional cross to display on the bridge
    public Sprite gateSprite;    // optional gate icon for the Kingdom

    // Internal references
    RectTransform leftRT, middleRT, rightRT;
    RectTransform humanRT, barrierRT, bridgeRT, kingdomRT;
    Image humanImg, barrierImg, bridgeImg, kingdomImg;
    Text humanText, barrierText, bridgeText, kingdomText;

    bool built = false;

    void Reset()
    {
        container = GetComponent<RectTransform>();
    }

    void Start()
    {
        if (container == null) container = GetComponent<RectTransform>();
        Build();
        StartCoroutine(PlaySequence());
    }

    /// <summary>
    /// Create the UI structure if it doesn't already exist.
    /// This is idempotent so you can call it from the Editor menu or at runtime.
    /// </summary>
    public void Build()
    {
        if (built) return;
        built = true;

        if (container == null) container = GetComponent<RectTransform>();
        container.sizeDelta = panelSize;

        // Root columns
        leftRT = CreateColumn("SinBridge_Left");
        middleRT = CreateColumn("SinBridge_Middle");
        rightRT = CreateColumn("SinBridge_Right");

        // Set anchors & positions for three columns
        float w = panelSize.x;
        float columnWidth = w / 3f;
        SetupColumnRect(leftRT, new Vector2(0f, 0.5f), new Vector2(0.0f, 1f), new Vector2(0, 0), columnWidth);
        SetupColumnRect(middleRT, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1f), new Vector2(0, 0), columnWidth);
        SetupColumnRect(rightRT, new Vector2(1f, 0.5f), new Vector2(1f, 1f), new Vector2(0, 0), columnWidth);

        // Human (left)
        humanRT = CreateBox(leftRT, "HumanBox", new Vector2(120, 120), humanColor);
        humanText = CreateLabel(humanRT.transform, humanLabel);

        // Barrier (middle)
        barrierRT = CreateBox(middleRT, "BarrierBox", new Vector2(120, 120), barrierColor);
        barrierText = CreateLabel(barrierRT.transform, barrierLabel);

        // Kingdom (right)
        kingdomRT = CreateBox(rightRT, "KingdomBox", new Vector2(120, 120), kingdomColor);
        kingdomText = CreateLabel(kingdomRT.transform, kingdomLabel);

        // Bridge (invisible initially)
        bridgeRT = CreateBox(container, "BridgeBox", new Vector2(220, 44), bridgeColor);
        bridgeRT.SetAsLastSibling();
        bridgeImg = bridgeRT.GetComponent<Image>();
        bridgeImg.color = new Color(bridgeColor.r, bridgeColor.g, bridgeColor.b, 0f);
        bridgeText = CreateLabel(bridgeRT.transform, bridgeLabel);
        bridgeText.fontSize = 20;
        bridgeText.color = Color.black;

        // Optionally try to place cross / gate sprites if provided (fallback to text label)
        if (crossSprite != null)
        {
            var go = new GameObject("BridgeCross", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(bridgeRT, false);
            var img = go.GetComponent<Image>();
            img.sprite = crossSprite;
            img.preserveAspect = true;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
            rt.anchoredPosition = new Vector2(-bridgeRT.sizeDelta.x / 2 + 28, 0);
            rt.sizeDelta = new Vector2(40, 40);
        }

        if (gateSprite != null)
        {
            var go = new GameObject("KingdomGate", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            go.transform.SetParent(kingdomRT, false);
            var img = go.GetComponent<Image>();
            img.sprite = gateSprite;
            img.preserveAspect = true;
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.2f);
            rt.anchorMax = new Vector2(0.5f, 0.2f);
            rt.anchoredPosition = new Vector2(0, -10);
            rt.sizeDelta = new Vector2(44, 44);
        }
    }

    RectTransform CreateColumn(string name)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(container, false);
        var rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(panelSize.x / 3f, panelSize.y);
        return rt;
    }

    void SetupColumnRect(RectTransform rt, Vector2 anchor, Vector2 pivot, Vector2 pos, float width)
    {
        rt.anchorMin = new Vector2(anchor.x, 0f);
        rt.anchorMax = new Vector2(anchor.x, 1f);
        rt.pivot = pivot;
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(width, panelSize.y);
    }

    RectTransform CreateBox(Transform parent, string name, Vector2 size, Color color)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = Vector2.zero;
        var img = go.GetComponent<Image>();
        img.color = color;
        img.raycastTarget = false;
        // rounded look: try to use default sprite if available (silhouette will be replaced in production art)
        return rt;
    }

    Text CreateLabel(Transform parent, string text)
    {
        var go = new GameObject("Label", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0f);
        rt.anchorMax = new Vector2(0.5f, 0f);
        rt.pivot = new Vector2(0.5f, 0f);
        rt.anchoredPosition = new Vector2(0, -28);
        rt.sizeDelta = new Vector2(200, 40);
        var t = go.GetComponent<Text>();
        t.text = text;
        t.alignment = TextAnchor.MiddleCenter;
        t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.fontSize = 20;
        t.color = Color.white;
        t.raycastTarget = false;
        return t;
    }

    /// <summary>
    /// Run the short sequence: drop bridge -> cover barrier -> walk across -> glow kingdom.
    /// </summary>
    public IEnumerator PlaySequence()
    {
        // initial states
        bridgeImg = bridgeImg ?? bridgeRT.GetComponent<Image>();
        humanImg = humanImg ?? humanRT.GetComponent<Image>();
        barrierImg = barrierImg ?? barrierRT.GetComponent<Image>();
        kingdomImg = kingdomImg ?? kingdomRT.GetComponent<Image>();

        // hide bridge
        SetAlpha(bridgeImg, 0f);

        yield return new WaitForSeconds(startDelay);

        // Bridge appears (fade in + drop)
        Vector2 startPos = new Vector2(0, panelSize.y / 2 + 30);
        Vector2 targetPos = new Vector2(0, 0);
        bridgeRT.anchoredPosition = startPos;
        float t = 0f;
        while (t < bridgeDropDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.SmoothStep(0f, 1f, t / bridgeDropDuration);
            bridgeRT.anchoredPosition = Vector2.Lerp(startPos, targetPos, f);
            SetAlpha(bridgeImg, f);
            yield return null;
        }
        SetAlpha(bridgeImg, 1f);

        // cover (barrier fades to a muted color and text changes)
        Color origBarrier = barrierImg.color;
        Color toColor = Color.Lerp(origBarrier, Color.gray, 0.6f);
        t = 0f;
        while (t < barrierCoverFade)
        {
            t += Time.deltaTime;
            float f = t / barrierCoverFade;
            barrierImg.color = Color.Lerp(origBarrier, toColor, f);
            yield return null;
        }
        barrierText.text = "Covered";

        // walk: move human across the bridge toward kingdom
        Vector2 humanStart = humanRT.anchoredPosition;
        // compute world target anchored position (move horizontally toward kingdom column)
        Vector3 worldStart = humanRT.TransformPoint(humanRT.rect.center);
        Vector3 worldEnd = kingdomRT.TransformPoint(kingdomRT.rect.center);
        Vector2 anchoredEnd = new Vector2(kingdomRT.anchoredPosition.x - leftRT.anchoredPosition.x, humanStart.y);

        t = 0f;
        Vector2 moveStart = humanRT.anchoredPosition;
        Vector2 moveEnd = new Vector2(kingdomRT.anchoredPosition.x, humanRT.anchoredPosition.y);
        while (t < walkDuration)
        {
            t += Time.deltaTime;
            float f = Mathf.SmoothStep(0f, 1f, t / walkDuration);
            humanRT.anchoredPosition = Vector2.Lerp(moveStart, moveEnd, f);
            yield return null;
        }
        humanRT.anchoredPosition = moveEnd;

        // kingdom glow (scale pulse)
        float pulseT = 0f;
        Vector3 origScale = kingdomRT.localScale;
        while (pulseT < endGlowDuration)
        {
            pulseT += Time.deltaTime;
            float f = Mathf.Sin((pulseT / endGlowDuration) * Mathf.PI);
            kingdomRT.localScale = Vector3.Lerp(origScale, origScale * 1.12f, f);
            yield return null;
        }
        kingdomRT.localScale = origScale;

        yield return null;
    }

    void SetAlpha(Graphic g, float a)
    {
        if (g == null) return;
        var c = g.color;
        c.a = a;
        g.color = c;
    }

    /// <summary>
    /// Public trigger to replay the visual.
    /// </summary>
    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlaySequence());
    }
}
