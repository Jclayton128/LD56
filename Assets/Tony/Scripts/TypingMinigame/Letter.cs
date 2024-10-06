using UnityEngine;
using TMPro;
using DG.Tweening;

namespace BeeGame.TypingGame
{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Letter : MonoBehaviour
    {

        private static Color UntypedColor = Color.black;
        private static Color TypedColor = Color.clear;
        private static Color MistakeColor = Color.red;

        private TextMeshProUGUI tmp;
        
        private void Awake()
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ShowUntyped(char c)
        {
            gameObject.SetActive(true);
            tmp.text = c.ToString();
            tmp.color = UntypedColor;
            KillTweens();
            tmp.color = Color.clear;
            tmp.DOColor(UntypedColor, 0.25f);
            tmp.transform.localScale = Vector3.one / 2;
            tmp.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBounce);
        }

        public void ShowTyped()
        {
            tmp.DOColor(TypedColor, 0.5f);
            tmp.transform.DOScale(Vector3.one / 2, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        public void ShowMistake()
        {
            tmp.color = MistakeColor;
            Vector3 v = Vector3.zero;
            DOTween.Shake(
                () => v,
                x => tmp.margin = new Vector4(x.x, 0, 0, 0),
                0.5f, 10, 10, 45).OnComplete(() =>
                {
                    tmp.color = UntypedColor;
                });
        }

        private void KillTweens()
        {
            DOTween.Kill(tmp);
            DOTween.Kill(tmp.transform);
            tmp.margin = Vector4.zero;
        }

    }

}
