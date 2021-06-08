using IncliForms.Models.Inclinometer;
using IncliForms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdrRecordView : ContentPage
    {
        AdrRecord Record;

        public AdrRecordView()
        {
            InitializeComponent();
        }

        public AdrRecordView(AdrRecord record)
        {
            InitializeComponent();
            Record = record;

            try
            {
                Title = record.SiteName;
                lblDateHarvested.Text = Record.DateHarvested.ToString();
                lblEndDepth.Text = record.EndDepth.ToString();
                lblStartDepth.Text = record.StartDepth.ToString();
                lblOperatorName.Text = record.OperatorName;
                lblSiteName.Text = record.SiteName;
            }
            catch
            {
                App.ToastShort("Data is Corrupt");
            }
        }

        private void cellExcel_Tapped(object sender, EventArgs e)
        {
            try
            {
                Launcher.OpenAsync
                           (new OpenFileRequest()
                           {
                               File = new ReadOnlyFile(Record.ExcelPath)
                           }
                       );
            }
            catch
            {
                App.ToastShort("File could not be opened");
            }
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            RecordAccess acc = new RecordAccess();

            bool res = await DisplayAlert("Delete Record", "This record will be deleted.", "Delete", "Cancel");

            if (res == false) return;

            await acc.DeleteById(Record.Id);

            await Navigation.PopAsync();
        }

        private void cellRpp_Tapped(object sender, EventArgs e)
        {
            try
            {
                Launcher.OpenAsync
                           (new OpenFileRequest()
                           {
                               File = new ReadOnlyFile(Record.RppPath)
                           }
                       );
            }
            catch
            {
                App.ToastShort("File could not be opened");
            }
        }

        private void cellCsv_Tapped(object sender, EventArgs e)
        {
            try
            {
                Launcher.OpenAsync
                           (new OpenFileRequest()
                           {
                               File = new ReadOnlyFile(Record.CsvPath)
                           }
                       );
            }
            catch
            {
                App.ToastShort("File could not be opened");
            }
        }
    }
}