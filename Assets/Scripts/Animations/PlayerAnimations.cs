using DG.Tweening;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Transform graphics;

    private Tween currentTween;

    void Awake()
    {
        if (graphics == null)
            graphics = transform;
    }

    void KillTween()
    {
        currentTween?.Kill();
    }

    public void Jump()
    {
        KillTween();

        currentTween = graphics
            .DOScale(new Vector3(0.24f, 0.18f, 0.21f), 0.08f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                graphics.DOScale(new Vector3(0.21f, 0.21f, 0.21f), 0.10f)
                    .SetEase(Ease.OutBack);
            });
    }

    public void Impact()
    {
        KillTween();

        Sequence seq = DOTween.Sequence();

        seq.Append(graphics.DOScale(new Vector3(0.24f, 0.18f, 0.21f), 0.05f));
        seq.Append(graphics.DOScale(new Vector3(0.18f, 0.24f, 0.21f), 0.06f));
        seq.Append(graphics.DOScale(new Vector3(0.21f, 0.21f, 0.21f), 0.08f));

        seq.SetEase(Ease.OutQuad);

        currentTween = seq;
    }

    public void MoveBounce()
    {
        if (currentTween != null && currentTween.IsActive())
            return;

        currentTween = graphics
            .DOScale(new Vector3(0.22f, 0.20f, 0.21f), 0.18f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void StopMoveBounce()
    {
        KillTween();
        graphics.DOScale(new Vector3(0.21f, 0.21f, 0.21f), 0.1f);
    }
}