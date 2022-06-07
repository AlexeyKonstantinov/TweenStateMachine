using TweensStateMachine.TweenTemplates;
using UnityEngine;
using UnityEngine.UI;

namespace TweensStateMachine
{
    public static class TweenAnimatorUIExtensions
    {
        public static TweenTemplate TTAnchorPos(this RectTransform target, Vector2 endValue, float duration)
        {
            return new TweenTemplateVector2(() => target.anchoredPosition, x => target.anchoredPosition = x, endValue, duration);
        }
        
        public static TweenTemplate TTAnchorPosX(this RectTransform target, float endValue, float duration)
        {
            return new TweenTemplateFloat(() => target.anchoredPosition.x, x => target.anchoredPosition = new Vector2(x, target.anchoredPosition.y), endValue, duration);
        }
        
        public static TweenTemplate TTAnchorPosY(this RectTransform target, float endValue, float duration)
        {
            return new TweenTemplateFloat(() => target.anchoredPosition.y, y => target.anchoredPosition = new Vector2(target.anchoredPosition.x, y), endValue, duration);
        }
        
        public static TweenTemplate TTFade(this CanvasGroup target, float endValue, float duration)
        {
            return new TweenTemplateFloat(() => target.alpha, x => target.alpha = x, endValue, duration);
        }
        
        public static TweenTemplate TTFade(this Image image, float endValue, float duration)
        {
            return new TweenTemplateColor(() => image.color, value => image.color = value, new Color(image.color.r, image.color.g, image.color.b, endValue), duration);
        }
        
        public static TweenTemplate TTColor(this Image image, Color endValue, float duration)
        {
            return new TweenTemplateColor(() => image.color, value => image.color = value, endValue, duration);
        }
    }
}