// UIDropCurvePrimeTween.cs (修改版)
using UnityEngine;
using PrimeTween;
using System;

[DisallowMultipleComponent]
public class UIDropCurvePrimeTween : MonoBehaviour
{
    [Header("目標 UI 物件")]
    [SerializeField] RectTransform item;

    [Header("三角形頂點 (以 (0,0) 為起點)")]
    public Vector3 topLeft = new Vector3(-200, 300, 0);
    public Vector3 topRight = new Vector3(200, 300, 0);

    [Header("曲線與時間參數")]
    [SerializeField] float duration = 0.9f;
    [Range(0f, 2f)][SerializeField] float arcHeightFactor = 0.6f;

    [Header("收尾回彈 (固定方向)")]
    [SerializeField] bool settleBounce = true;
    [Tooltip("回彈時，沿 X 軸的水平越界距離（像素，正值往右，負值往左）")]
    [SerializeField] float settleHorizontal = 40f;
    [Tooltip("回彈時，向下壓的距離（像素）")]
    [SerializeField] float settleVertical = 18f;
    [SerializeField] float settleDuration = 0.12f;

    public Vector3 bottomApex => item.anchoredPosition;
    Tween mainTween;

    [ContextMenu("PlayOnce")]
    public void PlayOnce(Action callBack = null)
    {
        PlayInternal(callBack);
    }

    void OnDisable()
    {
        mainTween.Stop();
    }

    void PlayInternal(Action callBack = null)
    {
        if (item == null)
        {
            Debug.LogError("[UIDropCurvePrimeTween] 請指定 item RectTransform。");
            return;
        }

        // 起點 = (0,0)
        Vector3 start = bottomApex;

        // 在三角形內隨機取一點
        Vector3 target = RandomPointInTriangle(topLeft, topRight, bottomApex);

        // 三角形高度 (用 apex 到頂邊中點)
        float triHeight = Vector3.Distance(bottomApex, (topLeft + topRight) * 0.5f);
        float arc = triHeight * arcHeightFactor;

        // 控制點
        Vector3 midDir = (target - start) * 0.45f;
        Vector3 control = start + midDir + Vector3.up * arc;

        // 設定起始位置
        item.anchoredPosition = start;

        // 主動畫
        mainTween = Tween.Custom(0f, 1f, duration, (t) => {
            item.anchoredPosition = QuadraticBezier(start, control, target, t);
        }, Ease.OutCubic);

        // ▼ 固定方向回彈 ▼
        mainTween.OnComplete(() => {
            if (!settleBounce) { callBack?.Invoke(); return; }

            // 固定方向：X 軸偏移 + 往下
            Vector3 overshoot = target + new Vector3(settleHorizontal, -settleVertical, 0f);

            Tween.UIAnchoredPosition(item, overshoot, settleDuration, Ease.InCubic)
                 .OnComplete(() => {
                     Tween.UIAnchoredPosition(item, target, settleDuration, Ease.OutCubic)
                          .OnComplete(() => callBack?.Invoke());
                 });
        });
    }

    // ─────────── 工具方法 ───────────
    static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return (u * u) * p0 + 2f * u * t * p1 + (t * t) * p2;
    }

    static Vector3 RandomPointInTriangle(Vector3 a, Vector3 b, Vector3 c)
    {
        float r1 = UnityEngine.Random.value;
        float r2 = UnityEngine.Random.value;
        float sr1 = Mathf.Sqrt(r1);
        return (1f - sr1) * a + sr1 * (1f - r2) * b + sr1 * r2 * c;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomApex);
        Gizmos.DrawLine(bottomApex, topLeft);

        Vector3 start = bottomApex;
        Vector3 target = Vector3.Lerp(topLeft, topRight, 0.7f);
        float triHeight = Vector3.Distance(bottomApex, (topLeft + topRight) * 0.5f);
        float arc = triHeight * arcHeightFactor;
        Vector3 control = start + (target - start) * 0.45f + Vector3.up * arc;

        Gizmos.color = Color.red;
        Vector3 prev = start;
        for (int i = 1; i <= 30; i++)
        {
            float t = i / 30f;
            Vector3 p = QuadraticBezier(start, control, target, t);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }
#endif
}
