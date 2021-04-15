using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation;
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
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:HamburgerMenuItem/>
    ///
    /// </summary>
    [DefaultEvent("IsSelectedChanged")]
    public class HamburgerMenuItem : HeaderedContentControl
    {
        private BoolField _hamburgerMenuItemBoolFieldStore;
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnIsSelectedChanged));
        static HamburgerMenuItem()
        {
            EventManager.RegisterClassHandler(typeof(HamburgerMenuItem), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(OnAccessKeyPressed));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));
            //HamburgerMenuItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(HamburgerMenuItem));
            KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(KeyboardNavigationMode.Local));
            //IsEnabledProperty.OverrideMetadata(typeof(HamburgerMenuItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
            //UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(HamburgerMenuItem), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
            AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
        }

        [Bindable(true)]
        [Category("Appearance")]
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HamburgerMenuItem? hamburgerMenuItem = d as HamburgerMenuItem;
            bool newValue = (bool)e.NewValue;
            //HamburgerMenuBlah hamburgerMenuParent = hamburgerMenuItem?.HamburgerMenuParent;
            //if (hamburgerMenuParent != null)
            //    hamburgerMenuParent.RaiseIsSelectedChangedAutomationEvent(hamburgerMenuItem, newValue);
            if (newValue)
                hamburgerMenuItem?.OnSelected(new RoutedEventArgs(Selector.SelectedEvent, hamburgerMenuItem));
            else
                hamburgerMenuItem?.OnUnselected(new RoutedEventArgs(Selector.UnselectedEvent, hamburgerMenuItem));
            //if (newValue)
            //    BindingOperations.SetBinding(hamburgerMenuItem, KeyboardNavigation.DirectionalNavigationMarginProperty, new Binding("Margin")
            //    {
            //        Source = hamburgerMenuItem
            //    });
            //else
            //    BindingOperations.ClearBinding(hamburgerMenuItem, KeyboardNavigation.DirectionalNavigationMarginProperty);
            //hamburgerMenuItem.UpdateVisualState();
        }

        protected virtual void OnSelected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(true, e);
        }

        protected virtual void OnUnselected(RoutedEventArgs e)
        {
            HandleIsSelectedChanged(false, e);
        }

        private void HandleIsSelectedChanged(bool newValue, RoutedEventArgs e)
        {
            RaiseEvent(e);
        }

        //internal override void ChangeVisualState(bool useTransitions)
        //{
        //    if (!IsEnabled)
        //        VisualStateManager.GoToState(this, "Disabled", useTransitions);
        //    else if (IsMouseOver)
        //        VisualStateManager.GoToState(this, "MouseOver", useTransitions);
        //    else
        //        VisualStateManager.GoToState(this, "Normal", useTransitions);
        //    if (IsSelected)
        //        VisualStates.GoToState(this, (useTransitions ? 1 : 0) != 0, "Selected", "Unselected");
        //    else
        //        VisualStateManager.GoToState(this, "Unselected", useTransitions);
        //    if (IsKeyboardFocused)
        //        VisualStateManager.GoToState(this, "Focused", useTransitions);
        //    else
        //        VisualStateManager.GoToState(this, "Unfocused", useTransitions);
        //    base.ChangeVisualState(useTransitions);
        //}

        //protected override AutomationPeer OnCreateAutomationPeer()
        //{
        //    return new TabItemWrapperAutomationPeer(this);
        //}

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if ((e.Source == this || !IsSelected) && SetFocus())
                e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnPreviewGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewGotKeyboardFocus(e);
            if (e.Handled || e.NewFocus != this || (IsSelected || HamburgerMenuParent == null))
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
                if (hamburgerMenuParent == null)
                    return;
                ContentPresenter? contentPresenter = hamburgerMenuParent.SelectedContentPresenter;
                if (contentPresenter == null)
                    return;
                hamburgerMenuParent.UpdateLayout();
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
            HamburgerMenu? hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            //if (newContent == BindingExpressionBase.DisconnectedItem)
            //    newContent = null;
            hamburgerMenuParent.SelectedContent = newContent;
        }

        protected override void OnContentTemplateChanged(DataTemplate oldContentTemplate, DataTemplate newContentTemplate)
        {
            base.OnContentTemplateChanged(oldContentTemplate, newContentTemplate);
            if (!IsSelected)
                return;
            HamburgerMenu? hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentTemplate = newContentTemplate;
        }

        protected override void OnContentTemplateSelectorChanged(DataTemplateSelector oldContentTemplateSelector, DataTemplateSelector newContentTemplateSelector)
        {
            base.OnContentTemplateSelectorChanged(oldContentTemplateSelector, newContentTemplateSelector);
            if (!IsSelected)
                return;
            HamburgerMenu? hamburgerMenuParent = HamburgerMenuParent;
            if (hamburgerMenuParent == null)
                return;
            hamburgerMenuParent.SelectedContentTemplateSelector = newContentTemplateSelector;
        }

        private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
        {
            if (e.Handled || e.Scope != null)
                return;
            HamburgerMenuItem? hamburgerMenuItem = sender as HamburgerMenuItem;
            if (e.Target == null)
            {
                e.Target = hamburgerMenuItem;
            }
            else
            {
                if (hamburgerMenuItem?.IsSelected ?? false)
                    return;
                e.Scope = hamburgerMenuItem;
                e.Handled = true;
            }
        }

        internal bool SetFocus()
        {
            bool flag1 = false;
            if (!GetBoolField(BoolField.SettingFocus))
            {
                HamburgerMenuItem? focusedElement = Keyboard.FocusedElement as HamburgerMenuItem;
                bool flag2 = focusedElement == this || focusedElement == null || focusedElement.HamburgerMenuParent != HamburgerMenuParent;
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

        private HamburgerMenu? HamburgerMenuParent => ItemsControl.ItemsControlFromItemContainer(this) as HamburgerMenu;

        private bool GetBoolField(BoolField field)
        {
            return (uint)(_hamburgerMenuItemBoolFieldStore & field) > 0U;
        }

        private void SetBoolField(BoolField field, bool value)
        {
            if (value)
                _hamburgerMenuItemBoolFieldStore = _hamburgerMenuItemBoolFieldStore | field;
            else
                _hamburgerMenuItemBoolFieldStore = _hamburgerMenuItemBoolFieldStore & ~field;
        }

        //internal override DependencyObjectType DTypeThemeStyleKey
        //{
        //    get
        //    {
        //        return _dType;
        //    }
        //}

        [Flags]
        private enum BoolField
        {
            SetFocusOnContent = 16,
            SettingFocus = 32,
            DefaultValue = 0,
        }
    }
}
