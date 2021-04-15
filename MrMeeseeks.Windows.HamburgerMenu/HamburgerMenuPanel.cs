using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MrMeeseeks.Windows.HamburgerMenu
{
    public class HamburgerMenuPanel : Panel
    {
        private int _numHeaders;

        static HamburgerMenuPanel()
        {
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(HamburgerMenuPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(HamburgerMenuPanel), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            _numHeaders = 0;
            foreach (UIElement internalChild in InternalChildren)
            {
                if (internalChild.Visibility != Visibility.Collapsed)
                {
                    _numHeaders = _numHeaders + 1;
                    internalChild.Measure(constraint);
                    Size sizeWithoutMargin = GetDesiredSizeWithoutMargin(internalChild);
                    if (size.Width < sizeWithoutMargin.Width)
                        size.Width = sizeWithoutMargin.Width;
                    size.Height += sizeWithoutMargin.Height;
                }
            }
            return size;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            double y = 0.0;
            foreach (UIElement internalChild in InternalChildren)
            {
                if (internalChild.Visibility != Visibility.Collapsed)
                {
                    Size sizeWithoutMargin = GetDesiredSizeWithoutMargin(internalChild);
                    internalChild.Arrange(new Rect(0.0, y, arrangeSize.Width, sizeWithoutMargin.Height));
                    y += sizeWithoutMargin.Height;
                }
            }
            return arrangeSize;
        }

        protected override Geometry? GetLayoutClip(Size layoutSlotSize)
        {
            return null;
        }

        private Size GetDesiredSizeWithoutMargin(UIElement element)
        {
            Thickness thickness = (Thickness)element.GetValue(MarginProperty);
            return new Size
            {
                Height = Math.Max(0.0, element.DesiredSize.Height - thickness.Top - thickness.Bottom),
                Width = Math.Max(0.0, element.DesiredSize.Width - thickness.Left - thickness.Right)
            };
        }
    }
}
