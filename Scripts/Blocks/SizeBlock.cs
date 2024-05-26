using System;
using UnityEngine;

namespace ExtendedButton.Scripts.Blocks
{
    [Serializable]
    public struct SizeBlock : IEquatable<SizeBlock>
    {
        [SerializeField] private Vector2 _normalSize;
        [SerializeField] private Vector2 _highlightedSize;
        [SerializeField] private Vector2 _pressedSize;
        [SerializeField] private Vector2 _selectedSize;
        [SerializeField] private Vector2 _disabledSize;
        [SerializeField] private float _fadeDuration;
        
        public Vector2 NormalSize
        {
            readonly get => _normalSize;
            private set => _normalSize = value;
        }

        
        public Vector2 HighlightedSize
        {
            readonly get => _highlightedSize;
            private set => _highlightedSize = value;
        }

        
        public Vector2 PressedSize
        {
            readonly get => _pressedSize;
            private set => _pressedSize = value;
        }

        
        public Vector2 SelectedSize
        {
            readonly get => _selectedSize;
            private set => _selectedSize = value;
        }


        public Vector2 DisabledSize
        {
            readonly get => _disabledSize;
            private set => _disabledSize = value;
        }


        public float FadeDuration
        {
            readonly get => _fadeDuration;
            private set => _fadeDuration = value;
        }


        public static SizeBlock DefaultSizeBlock;

        static SizeBlock()
        {
            DefaultSizeBlock = new SizeBlock
            {
                NormalSize = Vector2.one,
                HighlightedSize = Vector2.one * 1.1f,
                PressedSize = Vector2.one * 1.2f,
                SelectedSize = Vector2.one,
                DisabledSize = Vector2.one,
                FadeDuration = 0.1f
            };
        }

        public override bool Equals(object obj)
        {
            return obj is SizeBlock other && Equals(other);
        }

        public bool Equals(SizeBlock other)
        {
            return NormalSize.Equals(other.NormalSize) && 
                   HighlightedSize.Equals(other.HighlightedSize) &&
                   PressedSize.Equals(other.PressedSize) && 
                   SelectedSize.Equals(other.SelectedSize) &&
                   DisabledSize.Equals(other.DisabledSize) && 
                   FadeDuration.Equals(other.FadeDuration);
        }
        
        public static bool operator==(SizeBlock point1, SizeBlock point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator!=(SizeBlock point1, SizeBlock point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NormalSize, HighlightedSize, PressedSize, SelectedSize, DisabledSize, FadeDuration);
        }
    }
}