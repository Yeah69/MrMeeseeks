using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MrMeeseeks.Windows.HamburgerMenu
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:HamburgerMenuBlah"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:HamburgerMenuBlah;assembly=HamburgerMenuBlah"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:HamburgerMenuBlah/>
    ///
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(HamburgerMenuItem))]
    [TemplatePart(Name = "PART_SelectedContentHost", Type = typeof(ContentPresenter))]
    public class HamburgerMenu : MultiSelector
    {
        public static readonly DependencyProperty SelectedContentProperty = DependencyProperty.Register(
            nameof(SelectedContent),
            typeof(object),
            typeof(HamburgerMenu),
            new PropertyMetadata(default(object)));

        public static readonly DependencyProperty SelectedContentTemplateProperty = DependencyProperty.Register(
            nameof(SelectedContentTemplate),
            typeof(DataTemplate),
            typeof(HamburgerMenu),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty SelectedContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(SelectedContentTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(HamburgerMenu),
            new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty SelectedContentStringFormatProperty = DependencyProperty.Register(
            nameof(SelectedContentStringFormat),
            typeof(string),
            typeof(HamburgerMenu),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            nameof(ContentTemplate),
            typeof(DataTemplate),
            typeof(HamburgerMenu),
            new PropertyMetadata(default(DataTemplate)));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            nameof(ContentTemplateSelector),
            typeof(DataTemplateSelector),
            typeof(HamburgerMenu),
            new PropertyMetadata(default(DataTemplateSelector)));

        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register(
            nameof(ContentStringFormat),
            typeof(string),
            typeof(HamburgerMenu),
            new PropertyMetadata(default(string)));

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
            set => SetValue(SelectedContentStringFormatProperty, value);
        }

        public DataTemplateSelector? SelectedContentTemplateSelector
        {
            get => (DataTemplateSelector) GetValue(SelectedContentTemplateSelectorProperty);
            set => SetValue(SelectedContentTemplateSelectorProperty, value);
        }

        public DataTemplate? SelectedContentTemplate
        {
            get => (DataTemplate) GetValue(SelectedContentTemplateProperty);
            set => SetValue(SelectedContentTemplateProperty, value);
        }

        public object? SelectedContent
        {
            get => GetValue(SelectedContentProperty);
            set => SetValue(SelectedContentProperty, value);
        }
        private const string SelectedContentHostTemplateName = "PART_SelectedContentHost";

        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu)));
            IsTabStopProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(false));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
        }

        //internal override void ChangeVisualState(bool useTransitions)
        //{
        //    if (!IsEnabled)
        //        VisualStates.GoToState(this, (useTransitions ? 1 : 0) != 0, "Disabled", "Normal");
        //    else
        //        VisualStateManager.GoToState(this, "Normal", useTransitions);
        //    base.ChangeVisualState(useTransitions);
        //}

        //protected override AutomationPeer OnCreateAutomationPeer()
        //{
        //    return new TabControlAutomationPeer(this);
        //}

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            //this.CanSelectMultiple = false;
            this.CanSelectMultipleItems = false;
            ItemContainerGenerator.StatusChanged += OnGeneratorStatusChanged;
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateSelectedContent();
        }
        
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (IsKeyboardFocusWithin)
            {
                GetSelectedHamburgerMenuItem()?.SetFocus();
            }
            UpdateSelectedContent();
            //if (!AutomationPeer.ListenerExists(AutomationEvents.SelectionPatternOnInvalidated) && !AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) && (!AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) && !AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection)))
            //    return;
            //TabControlAutomationPeer peerForElement = UIElementAutomationPeer.CreatePeerForElement(this) as TabControlAutomationPeer;
            //if (peerForElement == null)
            //    return;
            //peerForElement.RaiseSelectionEvents(e);
        }
        
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);
            if (e.Action != NotifyCollectionChangedAction.Remove || SelectedIndex != -1)
                return;
            int startIndex = e.OldStartingIndex + 1;
            if (startIndex > Items.Count)
                startIndex = 0;
            HamburgerMenuItem? nextHamburgerMenuItem = FindNextHamburgerMenuItem(startIndex, -1);
            nextHamburgerMenuItem?.SetValue(IsSelectedProperty, true);
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            int direction = 0;
            int startIndex = -1;
            switch (e.Key)
            {
                case Key.Tab:
                    if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        startIndex = ItemContainerGenerator.IndexFromContainer(ItemContainerGenerator.ContainerFromItem(SelectedItem));
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
            HamburgerMenuItem? nextHamburgerMenuItem = FindNextHamburgerMenuItem(startIndex, direction);
            if (nextHamburgerMenuItem != null && nextHamburgerMenuItem != SelectedItem)
                e.Handled = nextHamburgerMenuItem.SetFocus();
            if (e.Handled)
                return;
            base.OnKeyDown(e);
        }

        private HamburgerMenuItem? FindNextHamburgerMenuItem(int startIndex, int direction)
        {
            HamburgerMenuItem? hamburgerMenuItem1 = null;
            if (direction != 0)
            {
                int index1 = startIndex;
                for (int index2 = 0; index2 < Items.Count; ++index2)
                {
                    index1 += direction;
                    if (index1 >= Items.Count)
                        index1 = 0;
                    else if (index1 < 0)
                        index1 = Items.Count - 1;
                    HamburgerMenuItem? hamburgerMenuItem2 = ItemContainerGenerator.ContainerFromIndex(index1) as HamburgerMenuItem;
                    if (hamburgerMenuItem2 != null && hamburgerMenuItem2.IsEnabled && hamburgerMenuItem2.Visibility == Visibility.Visible)
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

        internal ContentPresenter? SelectedContentPresenter => GetTemplateChild("PART_SelectedContentHost") as ContentPresenter;

        private void OnGeneratorStatusChanged(object? sender, EventArgs? e)
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
            if (SelectedIndex < 0)
            {
                SelectedContent = null;
                SelectedContentTemplate = null;
                SelectedContentTemplateSelector = null;
                SelectedContentStringFormat = null;
            }
            else
            {
                HamburgerMenuItem? selectedHamburgerMenuItem = GetSelectedHamburgerMenuItem();
                if (selectedHamburgerMenuItem == null)
                    return;
                //FrameworkElement parent = VisualTreeHelper.GetParent(selectedHamburgerMenuItem) as FrameworkElement;
                //if (parent != null)
                //{
                //    KeyboardNavigation.SetTabOnceActiveElement(parent, selectedHamburgerMenuItem);
                //    KeyboardNavigation.SetTabOnceActiveElement(this, parent);
                //}
                SelectedContent = selectedHamburgerMenuItem.Content;
                ContentPresenter? contentPresenter = SelectedContentPresenter;
                if (contentPresenter != null)
                {
                    contentPresenter.HorizontalAlignment = selectedHamburgerMenuItem.HorizontalContentAlignment;
                    contentPresenter.VerticalAlignment = selectedHamburgerMenuItem.VerticalContentAlignment;
                }
                if (selectedHamburgerMenuItem.ContentTemplate != null || selectedHamburgerMenuItem.ContentTemplateSelector != null || selectedHamburgerMenuItem.ContentStringFormat != null)
                {
                    SelectedContentTemplate = selectedHamburgerMenuItem.ContentTemplate;
                    SelectedContentTemplateSelector = selectedHamburgerMenuItem.ContentTemplateSelector;
                    SelectedContentStringFormat = selectedHamburgerMenuItem.ContentStringFormat;
                }
                else
                {
                    SelectedContentTemplate = ContentTemplate;
                    SelectedContentTemplateSelector = ContentTemplateSelector;
                    SelectedContentStringFormat = ContentStringFormat;
                }
            }
        }
    }
}
