using Costscape.Common;
using Costscape.Models;
using Costscape.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Costscape.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BudgetPage : Page
    {
        private NavigationHelper navigationHelper;

        public BudgetViewModel ViewModel { get { return (BudgetViewModel)DataContext; } }

        public BudgetPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            ViewModel.NewBudgetSectionCreated += ViewModel_NewBudgetSectionCreated;
            ViewModel.NewBudgetItemCreated += ViewModel_NewBudgetItemCreated;
            ViewModel.BudgetItemEdited += ViewModel_BudgetItemEdited;

            var budget = e.Parameter as Budget;

            if (budget == null) return;

            await ViewModel.LoadData(budget);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

            ViewModel.NewBudgetSectionCreated -= ViewModel_NewBudgetSectionCreated;
            ViewModel.NewBudgetItemCreated -= ViewModel_NewBudgetItemCreated;
            ViewModel.BudgetItemEdited -= ViewModel_BudgetItemEdited;
        }

        #endregion

        private void ViewModel_NewBudgetSectionCreated(object sender, EventArgs e)
        {
            AddSectionFlyout.Hide();
        }

        private void ViewModel_BudgetItemEdited(object sender, EventArgs e)
        {
            EditItemFlyout.Hide();
        }

        private void ViewModel_NewBudgetItemCreated(object sender, EventArgs e)
        {
            AddItemFlyout.Hide(); 
        }

        private void AddSectionFlyout_Closed(object sender, object e)
        {
            ViewModel.NewBudgetSection = null;
        }

        private void RadDataBoundListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedBudgetItem = e.AddedItems.FirstOrDefault() as BudgetItem;
            EditItemFlyout.ShowAt(this);
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            var btn = (sender as Button);
            var section = (btn.DataContext as BudgetSection);
            ViewModel.SelectedBudgetSection = section;
            AddItemFlyout.ShowAt(this);
        }

        private void BtnCancelItemAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewBudgetSection = null;
            AddItemFlyout.Hide();
        }

        private void BtnCancelSectionAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewBudgetItem = null;
            AddSectionFlyout.Hide();
        }

        private void BtnCancelItemEdit_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedBudgetItem = null;
            EditItemFlyout.Hide();
        }

        private void ItemNameBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            EditItem((sender as Border).DataContext as BudgetItem);
        }

        private void MnEditItem_Click(object sender, RoutedEventArgs e)
        {
            EditItem((sender as MenuFlyoutItem).DataContext as BudgetItem);
        }

        private void EditItem(BudgetItem item)
        {
            ViewModel.SelectedBudgetItem = item;
            EditItemFlyout.ShowAt(this);
        }

        private void MnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuFlyoutItem).DataContext as BudgetItem;

            if (ViewModel.RemoveItemCommand.CanExecute(item))
                ViewModel.RemoveItemCommand.Execute(item);
        }

        private void MnMoveToAnotherSection_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
