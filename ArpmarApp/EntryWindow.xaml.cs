using System.Net;
using System.Windows;
using ArpmarApp.Models;
using ArpmarApp.Views;
using ArpmarCore.Domain;

namespace ArpmarApp
{
    public enum EntryWindowType
    {
        Add,
        Edit
    }

    public partial class EntryWindow : Window
    {
        private readonly EntryWindowType _type;
        private readonly Arp _arp;
        private readonly TableEntry _originalTableEntry;

        public ArpManagementView Delegate { get; set; }

        public EntryWindow(EntryWindowType type, Arp arp, TableEntry tableEntry = null)
        {
            InitializeComponent();

            _type = type;
            _arp = arp;
            _originalTableEntry = tableEntry;

            PrepareInterfacesComboBox();
            PrepareTexts(tableEntry);
        }

        private void PrepareTexts(TableEntry tableEntry)
        {
            if (_type == EntryWindowType.Add)
            {
                Title = "Add Entry";
                ActionButton.Content = "Add";
            }
            else
            {
                Title = "Edit Entry";
                ActionButton.Content = "Save";
            }

            if (tableEntry == null)
                return;

            InterfacesComboBox.SelectedIndex = InterfacesComboBox.Items.IndexOf(tableEntry.Interface);
            IpTextBox.Text = tableEntry.IpAddress;
            MacTextBox.Text = tableEntry.MacAddress;
        }

        private void PrepareInterfacesComboBox()
        {
            InterfacesComboBox.Items.Clear();

            foreach (string @interface in _arp.Tables.Keys)
                InterfacesComboBox.Items.Add(@interface);

            InterfacesComboBox.SelectedIndex = 0;
        }

        private void ActionButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(IpTextBox.Text) || !IPAddress.TryParse(IpTextBox.Text, out _))
            {
                App.ShowMessageBox("Invalid IP address!");
                return;
            }

            if (string.IsNullOrEmpty(MacTextBox.Text))
            {
                App.ShowMessageBox("Invalid MAC address!");
                return;
            }

            var @interface = (string) InterfacesComboBox.SelectedItem;

            if (_type == EntryWindowType.Add)
            {
                _arp.ExecuteAdd(@interface, IpTextBox.Text, MacTextBox.Text);
            }
            else
            {
                _arp.ExecuteDelete(_originalTableEntry.Interface, _originalTableEntry.IpAddress);
                _arp.ExecuteAdd(@interface, IpTextBox.Text, MacTextBox.Text);
            }

            Delegate?.Refresh();
            Close();
        }
    }
}