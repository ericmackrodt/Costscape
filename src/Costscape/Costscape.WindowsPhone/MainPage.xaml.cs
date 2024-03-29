﻿using Costscape.Models;
using Costscape.ViewModels;
using Costscape.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Costscape
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get { return DataContext as MainViewModel; } }

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.BudgetCreated += ViewModel_BudgetCreated;
        }

        private void ViewModel_BudgetCreated(object sender, Budget e)
        {
            NewBudgetFlyout.Hide();
            Frame.Navigate(typeof(BudgetPage), e);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.

            if (e.NavigationMode == NavigationMode.New)
                await ViewModel.LoadData(null);
        }

        private void BudgetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = BudgetList.SelectedItem as Budget;
            if (item != null)
                Frame.Navigate(typeof(BudgetPage), item);
            BudgetList.SelectedItem = null;
        }

        private void NewBudgetFlyout_Closed(object sender, object e)
        {
            ViewModel.NewBudget = null;
        }

        private void BtnCancelAddBudget_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewBudget = null;
            NewBudgetFlyout.Hide();
        }
    }
}
