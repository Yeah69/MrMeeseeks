using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MrMeeseeks.Windows.HamburgerMenu
{
    [DefaultEvent("IsSelectedChanged")]
    public class HamburgerMenuItem : HeaderedContentControl
    {
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(
            typeof(HamburgerMenuItem),
            new FrameworkPropertyMetadata(false,
                FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal,
                OnIsSelectedChanged));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
            typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null));

        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(nameof(IconTemplate), typeof(DataTemplate), typeof(HamburgerMenuItem),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IconDataTemplateSelectorProperty =
            DependencyProperty.Register(nameof(IconDataTemplateSelector), typeof(DataTemplateSelector),
                typeof(HamburgerMenuItem), new PropertyMetadata(null));

        public static readonly DependencyProperty IconStringFormatProperty =
            DependencyProperty.Register(nameof(IconStringFormat), typeof(string), typeof(HamburgerMenuItem),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ContentHeaderProperty = DependencyProperty.Register(
            nameof(ContentHeader), typeof(object), typeof(HamburgerMenuItem), new PropertyMetadata(null, (o, args) =>
            {
                if (!(o is HamburgerMenuItem hamburgerMenuItem))
                    return;
                hamburgerMenuItem.OnContentHeaderChanged(args.OldValue, args.NewValue);
            }));

        public static readonly DependencyProperty ContentHeaderTemplateProperty = DependencyProperty.Register(
            nameof(ContentHeaderTemplate), typeof(DataTemplate), typeof(HamburgerMenuItem), new PropertyMetadata(null,
                (o, args) =>
                {
                    if (!(o is HamburgerMenuItem hamburgerMenuItem))
                        return;
                    hamburgerMenuItem.OnContentHeaderTemplateChanged((DataTemplate) args.OldValue,
                        (DataTemplate) args.NewValue);
                }));

        public static readonly DependencyProperty ContentHeaderTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ContentHeaderTemplateSelector), typeof(DataTemplateSelector), typeof(HamburgerMenuItem),
            new PropertyMetadata(null, (o, args) =>
            {
                if (!(o is HamburgerMenuItem hamburgerMenuItem))
                    return;
                hamburgerMenuItem.OnContentHeaderTemplateSelectorChanged((DataTemplateSelector) args.OldValue,
                    (DataTemplateSelector) args.NewValue);
            }));

        public static readonly DependencyProperty ContentHeaderStringFormatProperty = DependencyProperty.Register(
            nameof(ContentHeaderStringFormat), typeof(string), typeof(HamburgerMenuItem), new PropertyMetadata(null,
                (o, args) =>
                {
                    if (!(o is HamburgerMenuItem hamburgerMenuItem))
                        return;
                    hamburgerMenuItem.OnContentHeaderStringFormatChanged((string) args.OldValue,
                        (string) args.NewValue);
                }));

        private static readonly DependencyPropertyKey IconLengthPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IconLength), typeof(double), typeof(HamburgerMenuItem),
            new PropertyMetadata(0.0, null,
                (o, value) => o is HamburgerMenuItem hamburgerMenuItem
                    ? (object?) hamburgerMenuItem.HamburgerMenuParent?.IconLength
                    : (object) 0.0));

        public static readonly DependencyProperty IconLengthProperty = IconLengthPropertyKey.DependencyProperty;
        private BoolField _hamburgerMenuItemBoolFieldStore;

        static HamburgerMenuItem()
        {
            EventManager.RegisterClassHandler(typeof(HamburgerMenuItem), AccessKeyManager.AccessKeyPressedEvent,
                new AccessKeyPressedEventHandler(OnAccessKeyPressed));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem),
                new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(HamburgerMenuItem),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(HamburgerMenuItem),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
            AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(HamburgerMenuItem),
                new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
        }

        public double IconLength => (double) GetValue(IconLengthProperty);

        public string ContentHeaderStringFormat
        {
            get => (string) GetValue(ContentHeaderStringFormatProperty);
            set => SetValue(ContentHeaderStringFormatProperty, value);
        }

        public DataTemplateSelector ContentHeaderTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(ContentHeaderTemplateSelectorProperty);
            set => SetValue(ContentHeaderTemplateSelectorProperty, value);
        }

        public DataTemplate ContentHeaderTemplate
        {
            get => (DataTemplate) GetValue(ContentHeaderTemplateProperty);
            set => SetValue(ContentHeaderTemplateProperty, value);
        }

        public object ContentHeader
        {
            get => GetValue(ContentHeaderProperty);
            set => SetValue(ContentHeaderProperty, value);
        }

        public string IconStringFormat
        {
            get => (string) GetValue(IconStringFormatProperty);
            set => SetValue(IconStringFormatProperty, value);
        }

        public DataTemplateSelector IconDataTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(IconDataTemplateSelectorProperty);
            set => SetValue(IconDataTemplateSelectorProperty, value);
        }

        public DataTemplate IconTemplate
        {
            get => (DataTemplate) GetValue(IconTemplateProperty);
            set => SetValue(IconTemplateProperty, value);
        }

        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        [Bindable(true)]
        [Category("Appearance")]
        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        private HamburgerMenu? HamburgerMenuParent => ItemsControl.ItemsControlFromItemContainer(this) as HamburgerMenu;

        private static void OnIsSelectedChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var hamburgerMenuItem = d as HamburgerMenuItem;
            if ((bool) e.NewValue)
                hamburgerMenuItem?.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, hamburgerMenuItem));
            else
                hamburgerMenuItem?.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, hamburgerMenuItem));
        }

        protected virtual void OnSelected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(e);
        }

        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(e);
        }

        private void HandleIsSelectedChanged(RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if ((Equals(e.Source, this) || !IsSelected) && SetFocus())
                e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
            if (e.Handled || !Equals(e.NewFocus, this) || IsSelected || HamburgerMenuParent == null)
                return;
            SetValue(IsSelectedProperty, true);
            if (e.OldFocus != Keyboard.FocusedElement)
            {
                e.Handled = true;
            }
            else
            {
                if (!GetBoolField(BoolField.SetFocusOnContent))
                    return;
                HamburgerMenu hamburgerMenuParent = HamburgerMenuParent;
                var contentPresenter = hamburgerMenuParent?.SelectedContentPresenter;
                if (contentPresenter == null)
                    return;
                hamburgerMenuParent?.UpdateLayout();
                if (!contentPresenter.MoveFocus(new TraversalRequest(FocusNavigationDirection.First)))
                    return;
                e.Handled = true;
            }
        }

        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            SetFocus();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContent = newContent;
        }

        protected override void OnContentTemplateChanged(
            DataTemplate oldContentTemplate,
            DataTemplate newContentTemplate)
        {
            base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentTemplate = newContentTemplate;
        }

        protected override void OnContentTemplateSelectorChanged(
            DataTemplateSelector oldContentTemplateSelector,
            DataTemplateSelector newContentTemplateSelector)
        {
            base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentTemplateSelector = newContentTemplateSelector;
        }

        protected override void OnContentStringFormatChanged(
            string oldContentStringFormat,
            string newContentStringFormat)
        {
            base.OnContentStringFormatChanged(oldContentStringFormat, newContentStringFormat);
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentStringFormat = newContentStringFormat;
        }

        protected virtual void OnContentHeaderChanged(object oldContent, object newContent)
        {
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentHeader = newContent;
        }

        protected virtual void OnContentHeaderTemplateChanged(
            DataTemplate oldContentTemplate,
            DataTemplate newContentTemplate)
        {
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentHeaderTemplate = newContentTemplate;
        }

        protected virtual void OnContentHeaderTemplateSelectorChanged(
            DataTemplateSelector oldContentTemplateSelector,
            DataTemplateSelector newContentTemplateSelector)
        {
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentHeaderTemplateSelector = newContentTemplateSelector;
        }

        protected virtual void OnContentHeaderStringFormatChanged(
            string oldContentStringFormat,
            string newContentStringFormat)
        {
            if (!IsSelected)
                return;
            var hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentHeaderStringFormat = newContentStringFormat;
        }

        private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
        {
            if (e.Handled || e.Scope != null)
                return;
            var hamburgerMenuItem = sender as HamburgerMenuItem;
            if (e.Target == null)
            {
                e.Target = hamburgerMenuItem;
            }
            else
            {
                if (hamburgerMenuItem != null && hamburgerMenuItem.IsSelected)
                    return;
                e.Scope = hamburgerMenuItem;
                e.Handled = true;
            }
        }

        internal bool SetFocus()
        {
            var flag1 = false;
            if (!GetBoolField(BoolField.SettingFocus))
            {
                var focusedElement = Keyboard.FocusedElement as HamburgerMenuItem;
                var flag2 = Equals(focusedElement, this) || focusedElement == null ||
                            !Equals(focusedElement.HamburgerMenuParent, HamburgerMenuParent);
                SetBoolField(BoolField.SettingFocus, true);
                SetBoolField(BoolField.SetFocusOnContent, flag2);
                try
                {
                    flag1 = Focus() | flag2;
                }
                finally
                {
                    SetBoolField(BoolField.SettingFocus, false);
                    SetBoolField(BoolField.SetFocusOnContent, false);
                }
            }

            return flag1;
        }

        private bool GetBoolField(BoolField field)
        {
            return (uint) (_hamburgerMenuItemBoolFieldStore & field) > 0U;
        }

        private void SetBoolField(BoolField field, bool value)
        {
            if (value)
                _hamburgerMenuItemBoolFieldStore |= field;
            else
                _hamburgerMenuItemBoolFieldStore &= ~field;
        }

        [Flags]
        private enum BoolField
        {
            SetFocusOnContent = 16, // 0x00000010
            SettingFocus = 32 // 0x00000020
        }
    }
}