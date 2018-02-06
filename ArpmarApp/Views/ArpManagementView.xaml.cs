using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ArpmarApp.Models;
using ArpmarCore.Domain;

namespace ArpmarApp.Views
{
    public partial class ArpManagementView : UserControl
    {
        private readonly Arp _arp = new Arp();

        public ArpManagementView()
        {
            InitializeComponent();

            PrepareInterfaces();
            GetAllArpEntries();
        }

        private void PrepareInterfaces()
        {
            InterfacesComboBox.Items.Clear();
            InterfacesComboBox.Items.Add("All");

            foreach (string @interface in _arp.Tables.Keys)
                InterfacesComboBox.Items.Add(@interface);

            InterfacesComboBox.SelectedIndex = 0;
        }

        private void FillTable(IEnumerable<ArpEntry> entries)
        {
            var tableEntries = entries.Select(x => new TableEntry(x));
            DataGrid.ItemsSource = tableEntries;
        }

        private void InterfacesComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
            => GetArpEntries((string) InterfacesComboBox.SelectedItem);

        private void GetArpEntries(string @interface)
        {
            if (@interface == "All")
            {
                GetAllArpEntries();
                return;
            }

            FillTable(_arp.Tables[@interface].Entries);
        }

        private void GetAllArpEntries()
        {
            var allEntries = new List<ArpEntry>();
            foreach (var tablesValue in _arp.Tables.Values)
                allEntries.AddRange(tablesValue.Entries);

            FillTable(allEntries);
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Service.IsAdministrator())
                    throw new Exception("Administrator permissions required");

                var entryWindow = new EntryWindow(EntryWindowType.Add, _arp)
                {
                    Owner = App.Current.MainWindow,
                    Delegate = this
                };
                entryWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                App.ShowMessageBox(ex.Message);
            }
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Service.IsAdministrator())
                    throw new Exception("Administrator permissions required");

                var item = (TableEntry) DataGrid.SelectedCells[0].Item;

                var entryWindow = new EntryWindow(EntryWindowType.Edit, _arp, item)
                {
                    Owner = App.Current.MainWindow,
                    Delegate = this
                };
                entryWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                App.ShowMessageBox(ex.Message);
            }
            finally
            {
                Refresh();
            }
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Service.IsAdministrator())
                    throw new Exception("Administrator permissions required");

                foreach (var selectedItem in DataGrid.SelectedItems)
                {
                    var tableEntry = (TableEntry) selectedItem;
                    if (tableEntry.Type.ToLower()[0] == 'd')
                        continue;

                    _arp.ExecuteDelete(tableEntry.Interface, tableEntry.IpAddress);
                }
            }
            catch (Exception ex)
            {
                App.ShowMessageBox(ex.Message);
            }
            finally
            {
                Refresh();
            }
        }

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
            => Refresh();

        public void Refresh()
        {
            DataGrid.SelectedItems.Clear();

            _arp.ExecuteDisplay();
            GetArpEntries((string) InterfacesComboBox.SelectedItem);
        }
    }
}