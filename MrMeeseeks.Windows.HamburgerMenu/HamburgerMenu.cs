using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MrMeeseeks.Windows.HamburgerMenu
{
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(HamburgerMenuItem))]
    [TemplatePart(Name = "PART_SelectedContentHost", Type = typeof(ContentPresenter))]
    public class HamburgerMenu : MultiSelector
    {
        private const string SelectedContentHostTemplateName = "PART_SelectedContentHost";
        private const string HamburgerButtonTemplateName = "PART_HamburgerButton";

        private static readonly DependencyPropertyKey SelectedContentPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContent), typeof(object), typeof(HamburgerMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentProperty =
            SelectedContentPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey SelectedContentTemplatePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContentTemplate), typeof(DataTemplate),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentTemplateProperty =
            SelectedContentTemplatePropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey SelectedContentTemplateSelectorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContentTemplateSelector), typeof(DataTemplateSelector),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentTemplateSelectorProperty =
            SelectedContentTemplateSelectorPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey SelectedContentStringFormatPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContentStringFormat), typeof(string),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentStringFormatProperty =
            SelectedContentStringFormatPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(HamburgerMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateSelectorProperty =
            DependencyProperty.Register(nameof(ContentTemplateSelector), typeof(DataTemplateSelector),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentStringFormatProperty =
            DependencyProperty.Register(nameof(ContentStringFormat), typeof(string), typeof(HamburgerMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IconLengthProperty = DependencyProperty.Register(nameof(IconLength),
            typeof(double), typeof(HamburgerMenu), new PropertyMetadata(0.0, (o, args) =>
            {
                if (!(o is HamburgerMenu hamburgerMenu))
                    return;
                foreach (DependencyObject dependencyObject in hamburgerMenu.Items.OfType<HamburgerMenuItem>())
                    dependencyObject.CoerceValue(IconLengthProperty);
            }));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
            typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(nameof(IconTemplate), typeof(DataTemplate), typeof(HamburgerMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IconTemplateSelectorProperty =
            DependencyProperty.Register(nameof(IconTemplateSelector), typeof(DataTemplateSelector),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty IconStringFormatProperty =
            DependencyProperty.Register(nameof(IconStringFormat), typeof(string), typeof(HamburgerMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(nameof(IsPaneOpen),
            typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false));

        public static readonly DependencyProperty IsOverlayingProperty =
            DependencyProperty.Register(nameof(IsOverlaying), typeof(bool), typeof(HamburgerMenu),
                new PropertyMetadata(false));

        public static readonly DependencyProperty ClosePaneOnItemSelectionProperty =
            DependencyProperty.Register(nameof(ClosePaneOnItemSelection), typeof(bool), typeof(HamburgerMenu),
                new PropertyMetadata(false));

        private static readonly DependencyPropertyKey SelectedContentHeaderPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContentHeader), typeof(object), typeof(HamburgerMenu),
                new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentHeaderProperty =
            SelectedContentHeaderPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey SelectedContentHeaderTemplatePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContentHeaderTemplate), typeof(DataTemplate),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentHeaderTemplateProperty =
            SelectedContentHeaderTemplatePropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey SelectedContentHeaderTemplateSelectorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContentHeaderTemplateSelector),
                typeof(DataTemplateSelector), typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentHeaderTemplateSelectorProperty =
            SelectedContentHeaderTemplateSelectorPropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey SelectedContentHeaderStringFormatPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(SelectedContentHeaderStringFormat), typeof(string),
                typeof(HamburgerMenu), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedContentHeaderStringFormatProperty =
            SelectedContentHeaderStringFormatPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsContentHeaderSetPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsContentHeaderSet), typeof(bool), typeof(HamburgerMenu),
                new PropertyMetadata(false));

        public static readonly DependencyProperty IsContentHeaderSetProperty =
            IsContentHeaderSetPropertyKey.DependencyProperty;

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu),
                new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
            IsTabStopProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(HamburgerMenu),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
        }

        public string IconStringFormat
        {
            get => (string) GetValue(IconStringFormatProperty);
            set => SetValue(IconStringFormatProperty, value);
        }

        public DataTemplateSelector IconTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(IconTemplateSelectorProperty);
            set => SetValue(IconTemplateSelectorProperty, value);
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

        public bool IsContentHeaderSet
        {
            get => (bool) GetValue(IsContentHeaderSetProperty);
            private set => SetValue(IsContentHeaderSetPropertyKey, value);
        }

        public object? SelectedContentHeader
        {
            get => GetValue(SelectedContentHeaderProperty);
            internal set => SetValue(SelectedContentHeaderPropertyKey, value);
        }

        public DataTemplate? SelectedContentHeaderTemplate
        {
            get => (DataTemplate) GetValue(SelectedContentHeaderTemplateProperty);
            internal set => SetValue(SelectedContentHeaderTemplatePropertyKey, value);
        }

        public DataTemplateSelector? SelectedContentHeaderTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(SelectedContentHeaderTemplateSelectorProperty);
            internal set => SetValue(SelectedContentHeaderTemplateSelectorPropertyKey, value);
        }

        public string? SelectedContentHeaderStringFormat
        {
            get => (string) GetValue(SelectedContentHeaderStringFormatProperty);
            internal set => SetValue(SelectedContentHeaderStringFormatPropertyKey, value);
        }

        public bool ClosePaneOnItemSelection
        {
            get => (bool) GetValue(ClosePaneOnItemSelectionProperty);
            set => SetValue(ClosePaneOnItemSelectionProperty, value);
        }

        public bool IsOverlaying
        {
            get => (bool) GetValue(IsOverlayingProperty);
            set => SetValue(IsOverlayingProperty, value);
        }

        public bool IsPaneOpen
        {
            get => (bool) GetValue(IsPaneOpenProperty);
            set => SetValue(IsPaneOpenProperty, value);
        }

        public double IconLength
        {
            get => (double) GetValue(IconLengthProperty);
            set => SetValue(IconLengthProperty, value);
        }

        public string ContentStringFormat
        {
            get => (string) GetValue(ContentStringFormatProperty);
            set => SetValue(ContentStringFormatProperty, value);
        }

        public DataTemplateSelector ContentTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(ContentTemplateSelectorProperty);
            set => SetValue(ContentTemplateSelectorProperty, value);
        }

        public DataTemplate ContentTemplate
        {
            get => (DataTemplate) GetValue(ContentTemplateProperty);
            set => SetValue(ContentTemplateProperty, value);
        }

        public string? SelectedContentStringFormat
        {
            get => (string) GetValue(SelectedContentStringFormatProperty);
            internal set => SetValue(SelectedContentStringFormatPropertyKey, value);
        }

        public DataTemplateSelector? SelectedContentTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(SelectedContentTemplateSelectorProperty);
            internal set => SetValue(SelectedContentTemplateSelectorPropertyKey, value);
        }

        public DataTemplate? SelectedContentTemplate
        {
            get => (DataTemplate) GetValue(SelectedContentTemplateProperty);
            internal set => SetValue(SelectedContentTemplatePropertyKey, value);
        }

        public object? SelectedContent
        {
            get => GetValue(SelectedContentProperty);
            internal set => SetValue(SelectedContentPropertyKey, value);
        }

        internal ContentPresenter? SelectedContentPresenter =>
            GetTemplateChild("PART_SelectedContentHost") as ContentPresenter;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            CanSelectMultipleItems = false;
            ItemContainerGenerator.StatusChanged += OnGeneratorStatusChanged;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (Template.FindName("PART_HamburgerButton", this) is Button name)
                name.Click += (RoutedEventHandler) ((sender, args) => IsPaneOpen = !IsPaneOpen);
            UpdateSelectedContent();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (IsKeyboardFocusWithin)
                GetSelectedHamburgerMenuItem()?.SetFocus();
            if (ClosePaneOnItemSelection)
                IsPaneOpen = false;
            UpdateSelectedContent();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.Action != NotifyCollectionChangedAction.Remove || SelectedIndex != -1)
                return;
            var startIndex = e.OldStartingIndex + 1;
            if (startIndex > Items.Count)
                startIndex = 0;
            FindNextHamburgerMenuItem(startIndex, -1)?.SetValue(IsSelectedProperty, true);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var direction = 0;
            var startIndex = -1;
            switch (e.Key)
            {
                case Key.Tab:
                    if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        startIndex =
                            ItemContainerGenerator.IndexFromContainer(
                                ItemContainerGenerator.ContainerFromItem(SelectedItem));
                        direction = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift ? 1 : -1;
                    }

                    break;
                case Key.End:
                    direction = -1;
                    startIndex = Items.Count;
                    break;
                case Key.Home:
                    direction = 1;
                    startIndex = -1;
                    break;
            }

            var hamburgerMenuItem = FindNextHamburgerMenuItem(startIndex, direction);
            if (hamburgerMenuItem != null && !Equals(hamburgerMenuItem, SelectedItem))
                e.Handled = hamburgerMenuItem.SetFocus();
            if (e.Handled)
                return;
            base.OnKeyDown(e);
        }

        private HamburgerMenuItem? FindNextHamburgerMenuItem(
            int startIndex,
            int direction)
        {
            HamburgerMenuItem? hamburgerMenuItem1 = null;
            if ((uint) direction > 0U)
            {
                var index1 = startIndex;
                for (var index2 = 0; index2 < Items.Count; ++index2)
                {
                    index1 += direction;
                    if (index1 >= Items.Count)
                        index1 = 0;
                    else if (index1 < 0)
                        index1 = Items.Count - 1;
                    if (ItemContainerGenerator.ContainerFromIndex(index1) is HamburgerMenuItem hamburgerMenuItem2 &&
                        hamburgerMenuItem2.IsEnabled && hamburgerMenuItem2.Visibility == Visibility.Visible)
                    {
                        hamburgerMenuItem1 = hamburgerMenuItem2;
                        break;
                    }
                }
            }

            return hamburgerMenuItem1;
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is HamburgerMenuItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HamburgerMenuItem();
        }

        private void OnGeneratorStatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return;
            if (HasItems && SelectedItem == null)
                SetValue(SelectedIndexProperty, 0);
            UpdateSelectedContent();
        }

        private HamburgerMenuItem? GetSelectedHamburgerMenuItem()
        {
            object selectedItem = SelectedItem;
            if (selectedItem == null)
                return null;
            HamburgerMenuItem? hamburgerMenuItem = selectedItem as HamburgerMenuItem;
            if (hamburgerMenuItem == null)
            {
                hamburgerMenuItem = ItemContainerGenerator.ContainerFromIndex(SelectedIndex) as HamburgerMenuItem;
                if (hamburgerMenuItem == null || !Equals(selectedItem, ItemContainerGenerator.ItemFromContainer(hamburgerMenuItem)))
                    hamburgerMenuItem = ItemContainerGenerator.ContainerFromItem(selectedItem) as HamburgerMenuItem;
            }
            return hamburgerMenuItem;
        }

        private void UpdateSelectedContent()
        {
            SelectedContent = null;
            SelectedContentTemplate = null;
            SelectedContentTemplateSelector = null;
            SelectedContentStringFormat = null;
            SelectedContentHeader = null;
            SelectedContentHeaderTemplate = null;
            SelectedContentHeaderTemplateSelector = null;
            SelectedContentHeaderStringFormat = null;
            IsContentHeaderSet = false;
            var hamburgerMenuItem = GetSelectedHamburgerMenuItem();
            if (hamburgerMenuItem == null)
                return;
            SelectedContent = hamburgerMenuItem.Content;
            SelectedContentHeader = hamburgerMenuItem.ContentHeader;
            var contentPresenter = SelectedContentPresenter;
            if (contentPresenter != null)
            {
                contentPresenter.HorizontalAlignment = hamburgerMenuItem.HorizontalContentAlignment;
                contentPresenter.VerticalAlignment = hamburgerMenuItem.VerticalContentAlignment;
            }

            if (hamburgerMenuItem.ContentTemplate != null || hamburgerMenuItem.ContentTemplateSelector != null ||
                hamburgerMenuItem.ContentStringFormat != null)
            {
                SelectedContentTemplate = hamburgerMenuItem.ContentTemplate;
                SelectedContentTemplateSelector = hamburgerMenuItem.ContentTemplateSelector;
                SelectedContentStringFormat = hamburgerMenuItem.ContentStringFormat;
            }
            else
            {
                SelectedContentTemplate = ContentTemplate;
                SelectedContentTemplateSelector = ContentTemplateSelector;
                SelectedContentStringFormat = ContentStringFormat;
            }

            if (hamburgerMenuItem.ContentHeaderTemplate != null ||
                hamburgerMenuItem.ContentHeaderTemplateSelector != null ||
                hamburgerMenuItem.ContentHeaderStringFormat != null)
            {
                SelectedContentHeaderTemplate = hamburgerMenuItem.ContentHeaderTemplate;
                SelectedContentHeaderTemplateSelector = hamburgerMenuItem.ContentHeaderTemplateSelector;
                SelectedContentHeaderStringFormat = hamburgerMenuItem.ContentHeaderStringFormat;
            }

            IsContentHeaderSet = SelectedContentHeader != null || SelectedContentHeaderStringFormat != null ||
                                 SelectedContentHeaderTemplate != null || SelectedContentHeaderTemplateSelector != null;
        }
    }
}