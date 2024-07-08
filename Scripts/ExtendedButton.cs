using System;
using DG.Tweening;
using ExtendedButton.Scripts.Blocks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExtendedButton.Scripts
{
    [AddComponentMenu("UI/ExtendedButton", 30)]
    public class ExtendedButton : Button
    {
        [SerializeField] private ExtendedButtonTransitions _transitions;

        [SerializeField] private ColorBlock _imageColors = ColorBlock.defaultColorBlock;
        [SerializeField] private SizeBlock _imageSizes = SizeBlock.DefaultSizeBlock;
        [SerializeField] private SpriteState _imageSprites;

        [SerializeField] private TextMeshProUGUI _textElement;
        [SerializeField] private ColorBlock _textElementColors = ColorBlock.defaultColorBlock;
        [SerializeField] private SizeBlock _textElementSizes = SizeBlock.DefaultSizeBlock;

        public ExtendedButtonTransitions Transitions
        {
            get => _transitions;
            private set => _transitions = value;
        }

        public ColorBlock ImageColors
        {
            get => _imageColors;
            private set => _imageColors = value;
        }


        public SizeBlock ImageSizes
        {
            get => _imageSizes;
            private set => _imageSizes = value;
        }


        public SpriteState ImageSprites
        {
            get => _imageSprites;
            private set => _imageSprites = value;
        }

        public TextMeshProUGUI TextElement
        {
            get => _textElement;
            private set => _textElement = value;
        }

        public ColorBlock TextElementColors
        {
            get => _textElementColors;
            private set => _textElementColors = value;
        }

        public SizeBlock TextElementSizes
        {
            get => _textElementSizes;
            private set => _textElementSizes = value;
        }
        
        [Flags]
        public enum ExtendedButtonTransitions
        {
            None = 0,
            ImageColor = 1 << 0,
            ImageSize = 1 << 1,
            ImageSprite = 1 << 2,
            TextColor = 1 << 3,
            TextSize = 1 << 4
        }
        
        protected override void OnValidate()
        {
            base.OnValidate();
            
            TextElement ??= GetComponentInChildren<TextMeshProUGUI>();
            image ??= GetComponentInChildren<Image>();

            if (Transitions.HasFlag(ExtendedButtonTransitions.TextColor))
                TextElement.color = Color.white;

            var navigationTemp = navigation;
            navigationTemp.mode = Navigation.Mode.None;
            navigation = navigationTemp;
            
            transition = Transition.None;
        }

        protected override void Awake()
        {
            base.Awake();
            transition = Transition.None;
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if (!gameObject.activeInHierarchy)
                return;

            if (Transitions.Equals(ExtendedButtonTransitions.None))
                return;

            Color imageColor;
            Vector2 imageSize;

            Sprite transitionSprite;

            Color textElementColor;
            Vector2 textElementSize;

            switch (state)
            {
                default:
                case SelectionState.Normal:
                    imageColor = ImageColors.normalColor;
                    imageSize = ImageSizes.NormalSize;
                    transitionSprite = null;

                    textElementColor = TextElementColors.normalColor;
                    textElementSize = ImageSizes.NormalSize;

                    break;

                case SelectionState.Highlighted:
                    imageColor = ImageColors.highlightedColor;
                    imageSize = ImageSizes.HighlightedSize;
                    transitionSprite = ImageSprites.highlightedSprite;

                    textElementSize = ImageSizes.HighlightedSize;
                    textElementColor = TextElementColors.highlightedColor;

                    break;

                case SelectionState.Pressed:
                    imageColor = ImageColors.pressedColor;
                    imageSize = ImageSizes.PressedSize;
                    transitionSprite = ImageSprites.pressedSprite;

                    textElementSize = ImageSizes.PressedSize;
                    textElementColor = TextElementColors.pressedColor;

                    break;

                case SelectionState.Selected:
                    imageColor = ImageColors.selectedColor;
                    imageSize = ImageSizes.SelectedSize;
                    transitionSprite = ImageSprites.selectedSprite;

                    textElementSize = ImageSizes.SelectedSize;
                    textElementColor = TextElementColors.selectedColor;

                    break;

                case SelectionState.Disabled:
                    imageColor = ImageColors.disabledColor;
                    imageSize = ImageSizes.DisabledSize;
                    transitionSprite = ImageSprites.disabledSprite;

                    textElementSize = ImageSizes.DisabledSize;
                    textElementColor = TextElementColors.disabledColor;

                    break;
            }

            image.DOKill();
            
            //TODO: it might be worth adding someone to check if the text element is null in the editor. 
            if (TextElement != null) TextElement.DOKill(); 

            if (Transitions.HasFlag(ExtendedButtonTransitions.ImageColor))
                StartColorTween(image, imageColor * _imageColors.colorMultiplier, ImageColors.fadeDuration, instant);
            
            if (Transitions.HasFlag(ExtendedButtonTransitions.ImageSize))
                image.transform.DOScale(imageSize, ImageSizes.FadeDuration);

            if (Transitions.HasFlag(ExtendedButtonTransitions.ImageSprite))
                image.overrideSprite = transitionSprite;

            if (Transitions.HasFlag(ExtendedButtonTransitions.TextColor))
                StartColorTextTween(TextElement, textElementColor * _textElementColors.colorMultiplier, TextElementColors.fadeDuration, instant);

            if (Transitions.HasFlag(ExtendedButtonTransitions.TextSize))
                TextElement.transform.DOScale(textElementSize, TextElementSizes.FadeDuration);
        }

        private void StartColorTween(Graphic graphic, Color targetColor, float duration, bool instant)
        {
            if (graphic == null)
                return;

            graphic.CrossFadeColor(targetColor, instant ? 0f : duration, true, true);
        }

        private void StartColorTextTween(TextMeshProUGUI textElement, Color targetColor, float duration, bool instant)
        {
            if (textElement == null)
                return;

            textElement.CrossFadeColor(targetColor, instant ? 0f : duration, true, true);
        }
    }
}