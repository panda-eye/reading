using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace Чтение.Managers
{
    [ContentProperty("OverlayContent")]
    public partial class Nav : MetroWindow
    {
        public Nav()
        {
            InitializeComponent();
            
            Loaded += Nav_Loaded;
            Closing += Nav_Closing;
            Navigated += Nav_Navigated;
            Navigating += Nav_Navigating;
        }

        private void Nav_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            //var page = ((sender as Nav).PageContent as Page);
            //if (page != null && page.Opacity != 0)
            //{
            //    e.Cancel = true;
            //    AnimationManager.ApplyNavigateAnimation(page, () =>
            //    {
            //        Navigate(e.Content);
            //    });
            //}
        }
        private void Nav_Navigated(object sender, NavigationEventArgs e)
        {
            //AnimationManager.ApplyNavigateAnimation(e.Content as Page, null, true);
        }
        void Nav_Loaded(object sender, RoutedEventArgs e)
        {
            PART_Frame.Navigated += PART_Frame_Navigated;
            PART_Frame.Navigating += PART_Frame_Navigating;
            PART_Frame.NavigationFailed += PART_Frame_NavigationFailed;
            PART_Frame.NavigationProgress += PART_Frame_NavigationProgress;
            PART_Frame.NavigationStopped += PART_Frame_NavigationStopped;
            PART_Frame.LoadCompleted += PART_Frame_LoadCompleted;
            PART_Frame.FragmentNavigation += PART_Frame_FragmentNavigation;

            //PART_BackButton.Click += PART_BackButton_Click;
            //PART_ForwardButton.Click += PART_ForwardButton_Click;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoForward)
                GoForward();
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_FragmentNavigation(object sender, FragmentNavigationEventArgs e)
        {
            FragmentNavigation?.Invoke(this, e);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            LoadCompleted?.Invoke(this, e);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationStopped(object sender, NavigationEventArgs e)
        {
            NavigationStopped?.Invoke(this, e);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationProgress(object sender, NavigationProgressEventArgs e)
        {
            NavigationProgress?.Invoke(this, e);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            NavigationFailed?.Invoke(this, e);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            Navigating?.Invoke(this, e);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void PART_BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanGoBack)
                GoBack();
        }
        [System.Diagnostics.DebuggerNonUserCode]
        void Nav_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PART_Frame.FragmentNavigation -= PART_Frame_FragmentNavigation;
            PART_Frame.Navigating -= PART_Frame_Navigating;
            PART_Frame.NavigationFailed -= PART_Frame_NavigationFailed;
            PART_Frame.NavigationProgress -= PART_Frame_NavigationProgress;
            PART_Frame.NavigationStopped -= PART_Frame_NavigationStopped;
            PART_Frame.LoadCompleted -= PART_Frame_LoadCompleted;
            PART_Frame.Navigated -= PART_Frame_Navigated;

            //PART_ForwardButton.Click -= PART_ForwardButton_Click;
            //PART_BackButton.Click -= PART_BackButton_Click;

            Loaded -= Nav_Loaded;
            Closing -= Nav_Closing;
            Navigating -= Nav_Navigating;
            Navigated -= Nav_Navigated;
        }

        [System.Diagnostics.DebuggerNonUserCode]
        void PART_Frame_Navigated(object sender, NavigationEventArgs e)
        {
            //(this as IUriContext).BaseUri..BaseUri(e.Uri);

            PageContent = PART_Frame.Content;

            //PART_BackButton.IsEnabled = CanGoBack;

            //PART_ForwardButton.IsEnabled = CanGoForward;

            Navigated?.Invoke(this, e);
        }

        public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register("OverlayContent", typeof(object), typeof(Nav));

        public object OverlayContent
        {
            get { return GetValue(OverlayContentProperty); }
            set { SetValue(OverlayContentProperty, value); }
        }

        public static readonly DependencyProperty PageContentProperty = DependencyProperty.Register("PageContent", typeof(object), typeof(Nav));

        public object PageContent
        {
            get { return GetValue(PageContentProperty); }
            private set { SetValue(PageContentProperty, value); }
        }
        
        public IEnumerable ForwardStack { get { return PART_Frame.ForwardStack; } }
        public IEnumerable BackStack { get { return PART_Frame.BackStack; } }
        public NavigationService NavigationService { get { return PART_Frame.NavigationService; } }
        public bool CanGoBack { get { return PART_Frame.CanGoBack; } }
        public bool CanGoForward { get { return PART_Frame.CanGoForward; } }

        //private Uri baseUri;

        //public Uri GetBaseUri()
        //{
        //    return baseUri;
        //}

        //void SetBaseUri(Uri value)
        //{
        //    baseUri = value;
        //}

        public Uri Source { get { return PART_Frame.Source; } set { PART_Frame.Source = value; } }
        
        [System.Diagnostics.DebuggerNonUserCode]
        public void AddBackEntry(CustomContentState state)
        {
            PART_Frame.AddBackEntry(state);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        public JournalEntry RemoveBackEntry()
        {
            return PART_Frame.RemoveBackEntry();
        }
        
        [System.Diagnostics.DebuggerNonUserCode]
        public void GoBack()
        {
            PART_Frame.GoBack();
        }
        [System.Diagnostics.DebuggerNonUserCode]
        public void GoForward()
        {
            PART_Frame.GoForward();
        }
        
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Object content)
        {
            return PART_Frame.Navigate(content);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Uri source)
        {
            return PART_Frame.Navigate(source);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Object content, Object extraData)
        {
            return PART_Frame.Navigate(content, extraData);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        public bool Navigate(Uri source, Object extraData)
        {
            return PART_Frame.Navigate(source, extraData);
        }
        [System.Diagnostics.DebuggerNonUserCode]
        public void StopLoading()
        {
            PART_Frame.StopLoading();
        }

        public event FragmentNavigationEventHandler FragmentNavigation;
        public event NavigatingCancelEventHandler Navigating;
        public event NavigationFailedEventHandler NavigationFailed;
        public event NavigationProgressEventHandler NavigationProgress;
        public event NavigationStoppedEventHandler NavigationStopped;
        public event NavigatedEventHandler Navigated;
        public event LoadCompletedEventHandler LoadCompleted;
    }
}