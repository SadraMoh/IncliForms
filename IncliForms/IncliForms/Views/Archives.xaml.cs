using IncliForms.Models.Inclinometer;
using IncliForms.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Archives : ContentPage
    {
        ObservableCollection<RecordVm> records = new ObservableCollection<RecordVm>();

        class RecordVm : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            public int Id { get; set; }
            public string Name { get; set; }
            public string Date { get; set; }
            public object Raw { get; set; }

            public RecordVm()
            {

            }

            public RecordVm(AdrRecord record)
            {
                Raw = record;
                Name = record.SiteName;
                Date = record.DateHarvested.ToString();
            }
        }

        public Archives()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            InitRecords();
        }

        private async void InitRecords()
        {
            records.Clear();
            listMain.ItemsSource = null;
            RecordAccess acc = new RecordAccess();
            List<AdrRecord> task = await acc.GetItemsAsync();
            foreach (var item in task.OrderByDescending(x => x.DateHarvested))
                records.Add(new RecordVm(item));

            listMain.ItemsSource = records;
        }

        private void Refresh_Clicked(object sender, EventArgs e)
        {
            InitRecords();
        }

        private async void Clear_Clicked(object sender, EventArgs e)
        {
            RecordAccess acc = new RecordAccess();

            bool res = await DisplayAlert("Clear all records", "Are you sure you want to delete all of the records captured untill now? This cannot be undone.", "Delete", "Cancel");

            if (res == false) return;

            await acc.Clear();
            InitRecords();
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            var cell = (ViewCell)sender;
            RecordVm RecordVm = (RecordVm)cell.BindingContext;
            var record = (AdrRecord)RecordVm.Raw;
            await Navigation.PushAsync(new AdrRecordView(record));
        }


    }
}