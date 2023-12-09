using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [ExecuteInEditMode]
    public class ScaleEnforcer : MonoBehaviour
    {
        public bool overrideEditorValue = true;
        private RectTransform _rect;
        private CanvasScaler sc;

        private void Awake()
        {
            sc = GetComponent<CanvasScaler>();
            _rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!overrideEditorValue) return;
            var screenRatio = (float) Screen.width / Screen.height;
            // if rect width is greater than height
            // if (_rect.rect.width > _rect.rect.height)
            if (screenRatio > 0.54f)
                // set the scale to the height
                sc.matchWidthOrHeight = 1;
            else
                sc.matchWidthOrHeight = 0;
        }
    }
}