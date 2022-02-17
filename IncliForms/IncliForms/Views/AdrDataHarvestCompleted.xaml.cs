using Android;
using ClosedXML.Excel;
using IncliForms.Models;
using IncliForms.Models.Inclinometer;
using IncliForms.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdrDataHarvestCompleted : ContentPage
    {
        AdrInclinometer Inclinometer;
        IEnumerable<AdrDatablock> Datalist;
        Models.Settings Settings;
        public AdrRecord Record { get; set; }
        /// <summary>
        /// The Directory to save the files in
        /// </summary>

        //private string Dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //private string Dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //private string Dir = "/AzarDagigRooz/IncliForms";

        private string Dir = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath, "Incliforms");

        /// <summary>
        /// The Name of the File
        /// </summary>
        private string FileName;

        /// \/ initialized in <see cref="CreateRecord"/>
        private string rppPath;
        private string exPath;
        private string csvPath;
        private int boreholeReadCount = 0;

        public AdrDataHarvestCompleted()
        {
            InitializeComponent();
        }

        public AdrDataHarvestCompleted(AdrInclinometer inclinometer, IEnumerable<AdrDatablock> datalist)
        {
            this.Inclinometer = inclinometer;
            this.Datalist = datalist;
            InitializeComponent();
            InitFiles();
        }

        private async void InitFiles()
        {

        askStorageRead:
            await Permissions.RequestAsync<Permissions.StorageRead>();

        askStorageWrite:
            await Permissions.RequestAsync<Permissions.StorageWrite>();


            if ((await Permissions.CheckStatusAsync<Permissions.StorageWrite>()) != PermissionStatus.Granted)
                goto askStorageRead;

            if ((await Permissions.CheckStatusAsync<Permissions.StorageWrite>()) != PermissionStatus.Granted)
                goto askStorageWrite;

            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);

            FileName = $"{Inclinometer.BoreholeName}#{Inclinometer.BoreholeNumber}";

            Dir = Path.Combine(Dir, $"{FileName} - ({Directory.GetDirectories(Dir).Length + 1})");

            await CreateRecord();
            //Task.WaitAll(
            //     GenerateExcel(),
            //     GenerateCSV(),
            //     GenerateRPP()
            //    );

            foreach (AdrDatablock data in Datalist)
            {
                switch (Settings.RecordUnit)
                {
                    case RecordUnit.cm:
                        break;
                    case RecordUnit.m:
                        break;
                    case RecordUnit.mm:
                        data.Aplus *= 100;
                        data.Bplus *= 100;
                        data.Aminus *= 100;
                        data.Bminus *= 100;

                        data.Aplus = (float)Math.Floor(data.Aplus);
                        data.Bplus = (float)Math.Floor(data.Bplus);
                        data.Aminus = (float)Math.Floor(data.Aminus);
                        data.Bminus = (float)Math.Floor(data.Bminus);

                        break;
                }
            }

            await GenerateExcel();
            await GenerateCSV();
            await GenerateRPP();
        }

        private async Task CreateRecord()
        {
            DatablockAccess datablockAccess = new DatablockAccess();
            RecordAccess recordAccess = new RecordAccess();
            SettingsAccess settingsAccess = new SettingsAccess();

            Settings = await settingsAccess.GetItemAsync();

            var date = DateTime.Now;

            rppPath = Path.Combine(Dir, FileName + ".rpp");
            exPath = Path.Combine(Dir, FileName + ".xlsx");
            csvPath = Path.Combine(Dir, FileName + ".csv");

            var all = await recordAccess.GetItemsAsync();

            boreholeReadCount = all.Where(i => i.InclinometerId == Inclinometer.Id).Count();

            Record = new AdrRecord()
            {
                DateHarvested = date,
                Description = $"Taken on ${date}, By ${Settings.OperatorName}",
                EndDepth = Inclinometer.EndDepth,
                InclinometerId = Inclinometer.Id,
                OperatorName = Settings.OperatorName,
                RotationalCorrA = Inclinometer.RotationalCorrA,
                RotationalCorrB = Inclinometer.RotationalCorrB,
                SensitivityFactorA = Inclinometer.SensitivityFactorA,
                SensitivityFactorB = Inclinometer.SensitivityFactorB,
                SiteName = $"{Settings.SiteName} - {Inclinometer.BoreholeName}#{Inclinometer.BoreholeNumber} - ({boreholeReadCount + 1})",
                StartDepth = Inclinometer.StartDepth,
                CsvPath = csvPath,
                ExcelPath = exPath,
                RppPath = rppPath,
            };

            var a = (await recordAccess.SaveItemAsync(Record));

            if (a != 1) throw new Exception("Err while saving Record");

            all = await recordAccess.GetItemsAsync();
            Record = all[all.Count - 1];

            foreach (var block in Datalist)
            {
                block.RecordId = Record.Id;
                block.CalculateDeltas();
                int o = await datablockAccess.SaveItemAsync(block);
                if (o != 1) throw new Exception("Err while adding datablock to Record");
            }

        }

        private async Task GenerateExcel()
        {
            await Task.Run(() =>
             {

                 using (var workbook = new XLWorkbook())
                 {
                     var worksheet = workbook.Worksheets.Add(FileName + "xlsx");

                     #region Headers
                     //worksheet.Cell("A1").SetValue("TIME");
                     //worksheet.Cell("B1").SetValue($"{Record.DateHarvested.TimeOfDay}   {Record.DateHarvested.Date}");

                     //worksheet.Cell("A2").SetValue("DIGITILT/SPIRAL");
                     //worksheet.Cell("B2").SetValue("D");

                     //worksheet.Cell("A3").SetValue("ENGLISH/METRIC");
                     //worksheet.Cell("B3").SetValue("M");

                     //worksheet.Cell("A4").SetValue("HOLE #");
                     //worksheet.Cell("B4").SetValue($"{Inclinometer.BoreholeNumber}");

                     //worksheet.Cell("A5").SetValue("PROJECT");
                     //worksheet.Cell("B5").SetValue($"{Inclinometer.BoreholeName}");

                     //worksheet.Cell("A6").SetValue("JOB DESC");
                     //worksheet.Cell("B6").SetValue($"{Settings.SiteName}");

                     //worksheet.Cell("A7").SetValue("DIR CODE");
                     //worksheet.Cell("B7").SetValue("0");

                     //worksheet.Cell("A8").SetValue("PROBE SER");
                     //worksheet.Cell("B8").SetValue($"{Inclinometer.ProbeSerial}");

                     //worksheet.Cell("A9").SetValue("OPERATOR");
                     //worksheet.Cell("B9").SetValue($"{Settings.OperatorName}");

                     //worksheet.Cell("A10").SetValue("START DEPTH");
                     //worksheet.Cell("B10").SetValue($"{Inclinometer.StartDepth}");

                     //worksheet.Cell("A11").SetValue("END DEPTH");
                     //worksheet.Cell("B11").SetValue($"{Inclinometer.EndDepth}");

                     //worksheet.Cell("A12").SetValue("INCREMENT");
                     //worksheet.Cell("B12").SetValue("0.5");

                     //worksheet.Cell("A13").SetValue("INSTR CONST");
                     //worksheet.Cell("B13").SetValue("50000");

                     //worksheet.Cell("A14").SetValue("ROTATIONAL CORR A");
                     //worksheet.Cell("B14").SetValue($"{Inclinometer.RotationalCorrA}");

                     //worksheet.Cell("A15").SetValue("ROTATIONAL CORR B");
                     //worksheet.Cell("B15").SetValue($"{Inclinometer.RotationalCorrB}");

                     //worksheet.Cell("A16").SetValue("SENSITIVITY FACTOR A");
                     //worksheet.Cell("B16").SetValue($"{Inclinometer.SensitivityFactorA}");

                     //worksheet.Cell("A17").SetValue("SENSITIVITY FACTOR B");
                     //worksheet.Cell("B17").SetValue($"{Inclinometer.SensitivityFactorB}");
                     #endregion

                     #region Headers
                     worksheet.Cell("A1").SetValue("Azar Dagig Rooz Inclinometer Data");

                     worksheet.Cell("A2").SetValue("File Version");
                     worksheet.Cell("B2").SetValue(Inclinometer.VersionName);

                     worksheet.Cell("A3").SetValue("File Type");
                     worksheet.Cell("B3").SetValue("Digital Inclinometer");

                     worksheet.Cell("A4").SetValue("Site");
                     worksheet.Cell("B4").SetValue($"{Record.SiteName}");

                     worksheet.Cell("A5").SetValue("Borehole");
                     worksheet.Cell("B5").SetValue($"{Inclinometer.BoreholeName}");

                     worksheet.Cell("A6").SetValue("Probe Serial");
                     worksheet.Cell("B6").SetValue($"{Inclinometer.ProbeSerial}");

                     worksheet.Cell("A7").SetValue("Reel Serial");
                     worksheet.Cell("B7").SetValue($"{Inclinometer.ReelSerial}");

                     worksheet.Cell("A8").SetValue("Reading Date");
                     worksheet.Cell("B8").SetValue($"{Record.DateHarvested.Date}");
                     worksheet.Cell("C8").SetValue($"{Record.DateHarvested.TimeOfDay}");

                     worksheet.Cell("A9").SetValue("Depth");
                     worksheet.Cell("B9").SetValue($"{Record.StartDepth}");
                     worksheet.Cell("C9").SetValue($"{Record.EndDepth}");

                     worksheet.Cell("A10").SetValue("Interval");
                     worksheet.Cell("B10").SetValue($"{0.5f}");

                     worksheet.Cell("A11").SetValue("Depth Unit");

                     string unit = "";
                     switch (Settings.RecordUnit)
                     {
                         case RecordUnit.cm:
                             unit = "centimeters";
                             break;
                         case RecordUnit.m:
                             unit = "meters";
                             break;
                         case RecordUnit.mm:
                             unit = "milimeters";
                             break;
                     }

                     worksheet.Cell("B11").SetValue($"{unit}");

                     worksheet.Cell("A12").SetValue("Reading Unit");
                     worksheet.Cell("B12").SetValue($"{unit}");

                     worksheet.Cell("A13").SetValue("Operator");
                     worksheet.Cell("B13").SetValue($"{Settings.OperatorName}");

                     worksheet.Cell("A14").SetValue("Comment:");
                     worksheet.Cell("B14").SetValue($"");

                     worksheet.Cell("A15").SetValue("Comment End");
                     worksheet.Cell("B15").SetValue($"");

                     worksheet.Cell("A16").SetValue("Offset");
                     worksheet.Cell("B16").SetValue($"{Inclinometer.SensitivityFactorA}");

                     worksheet.Cell("A18").SetValue("Depth");
                     worksheet.Cell("B18").SetValue("Face A+");
                     worksheet.Cell("C18").SetValue("Face A-");
                     worksheet.Cell("D18").SetValue("Face B+");
                     worksheet.Cell("E18").SetValue("Face B-");

                     #endregion


                     #region Blocks
                     int lineNumber = 20;
                     foreach (AdrDatablock data in Datalist)
                     {
                       
                         worksheet.Cell($"A{lineNumber}").SetValue($"+{data.Depth}");
                         worksheet.Cell($"B{lineNumber}").SetValue($"A0");
                         worksheet.Cell($"C{lineNumber}").SetValue($"{data.Aplus}");
                         worksheet.Cell($"D{lineNumber}").SetValue($"B0");
                         worksheet.Cell($"E{lineNumber}").SetValue($"{data.Bplus}");

                         worksheet.Cell($"A{lineNumber + 1}").SetValue("");
                         worksheet.Cell($"B{lineNumber + 1}").SetValue($"A180");
                         worksheet.Cell($"C{lineNumber + 1}").SetValue($"{data.Aminus}");
                         worksheet.Cell($"D{lineNumber + 1}").SetValue($"B180");
                         worksheet.Cell($"E{lineNumber + 1}").SetValue($"{data.Bminus}");

                         lineNumber += 2;
                     }
                     #endregion

                     workbook.SaveAs(Path.Combine(Dir, FileName + ".xlsx"));

                 }

             });
        }

        private async Task GenerateCSV()
        {
            await Task.Run(() =>
            {

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(FileName + "csv");

                    #region Headers
                    worksheet.Cell("A1").SetValue("Time");
                    worksheet.Cell("B1").SetValue($"{Record.DateHarvested.Date}");
                    worksheet.Cell("C1").SetValue($"{Record.DateHarvested.TimeOfDay}");

                    worksheet.Cell("A2").SetValue("Digital/Spiral");
                    worksheet.Cell("B2").SetValue("D");

                    worksheet.Cell("A3").SetValue("English/Metric");
                    worksheet.Cell("B3").SetValue("M");

                    worksheet.Cell("A4").SetValue("Hole #");
                    worksheet.Cell("B4").SetValue($"{Inclinometer.BoreholeNumber}");

                    worksheet.Cell("A5").SetValue("Project");
                    worksheet.Cell("B5").SetValue($"1");

                    worksheet.Cell("A6").SetValue("Job Desc");
                    worksheet.Cell("B6").SetValue($"{Settings.SiteName}");

                    worksheet.Cell("A7").SetValue("Dir Code");
                    worksheet.Cell("B7").SetValue($"0");

                    worksheet.Cell("A8").SetValue("Probe Serial Number");
                    worksheet.Cell("B8").SetValue($"{Inclinometer.ProbeSerial}");

                    worksheet.Cell("A9").SetValue("Operator");
                    worksheet.Cell("B9").SetValue($"{Settings.OperatorName}");

                    worksheet.Cell("A10").SetValue("Start Depth");
                    worksheet.Cell("B10").SetValue($"{Record.StartDepth}");

                    worksheet.Cell("A11").SetValue("End Depth");
                    worksheet.Cell("B11").SetValue($"{Record.EndDepth}");

                    worksheet.Cell("A12").SetValue("Increment");
                    worksheet.Cell("B12").SetValue($"0.5");

                    worksheet.Cell("A13").SetValue("Instr Const");
                    worksheet.Cell("B13").SetValue("50000");

                    worksheet.Cell("A14").SetValue("Rotational Corr A");
                    worksheet.Cell("B14").SetValue($"{Settings.RotationalCorrA}");

                    worksheet.Cell("A15").SetValue("Rotaional Corr B");
                    worksheet.Cell("B15").SetValue($"{Settings.RotationalCorrB}");

                    worksheet.Cell("A16").SetValue("Sensitivity Factor A");
                    worksheet.Cell("B16").SetValue($"{Settings.SensitivityFactorA}");

                    worksheet.Cell("A17").SetValue("Sensitivity Factor B");
                    worksheet.Cell("B17").SetValue($"{Settings.SensitivityFactorB}");

                    worksheet.Cell("A19").SetValue("Depth");
                    worksheet.Cell("B19").SetValue("Face A+");
                    worksheet.Cell("C19").SetValue("Face A-");
                    worksheet.Cell("D19").SetValue("Face B+");
                    worksheet.Cell("E19").SetValue("Face B-");

                    #endregion

                    #region Blocks
                    int lineNumber = 21;
                    foreach (AdrDatablock data in Datalist)
                    {

                      
                        worksheet.Cell($"A{lineNumber}").SetValue($"-{data.Depth}");
                        worksheet.Cell($"B{lineNumber}").SetValue($"{data.Aplus}");
                        worksheet.Cell($"C{lineNumber}").SetValue($"{data.Aminus}");
                        worksheet.Cell($"D{lineNumber}").SetValue($"{data.Bplus}");
                        worksheet.Cell($"E{lineNumber}").SetValue($"{data.Bminus}");

                        lineNumber++;
                    }
                    #endregion

                    workbook.SaveAs(Path.Combine(Dir, FileName + "csv" + ".xlsx"));

                    #region CSV

                    var lastCellAddress = worksheet.RangeUsed().LastCell().Address;
                    File.WriteAllLines(csvPath, worksheet.Rows(1, lastCellAddress.RowNumber)
                        .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                                .Select(cell =>
                                {
                                    var cellValue = cell.GetValue<string>();
                                    return cellValue.Contains(",") ? $"\"{cellValue}\"" : cellValue;
                                }))));

                    #endregion

                }

            });
        }

        private async Task GenerateRPP()
        {
            await Task.Run(() =>
            {
                Record.RppPath = rppPath;

                string monthName = DateTime.Today.ToString("MMM", CultureInfo.CurrentCulture);

                string time = DateTime.Now.TimeOfDay.ToString().Split('.')[0];

                string date = $"{ Record.DateHarvested.Date.Day:00} { monthName } { Record.DateHarvested.Date.Year }";

                string template =
                    $"TIME = {time}   {date}                                                 \r\n" +
                    $"DIGITILT/SPIRAL = D                                                    \r\n" +
                    $"ENGLISH/METRIC = M                                                     \r\n" +
                    //$"HOLE # = I{Inclinometer.BoreholeName}-{Inclinometer.BoreholeNumber}  \r\n" +
                    $"HOLE # = {Inclinometer.BoreholeName}                                   \r\n" +
                    $"PROJECT = 1                                                            \r\n" +
                    $"JOB DESC = {Settings.SiteName}                                         \r\n" +
                    $"DIR CODE = 0                                                           \r\n" +
                    $"PROBE SER # = {Inclinometer.ProbeSerial}                               \r\n" +
                    $"OPERATOR = {Settings.OperatorName}                                     \r\n" +
                    $"START DEPTH = {Inclinometer.StartDepth}                                \r\n" +
                    $"END DEPTH = {Inclinometer.EndDepth}                                    \r\n" +
                    $"INCREMENT = 0.5                                                        \r\n" +
                    $"INSTR CONST = 50000                                                    \r\n" +
                    $"ROTATIONAL CORR A = {Settings.RotationalCorrA}                         \r\n" +
                    $"ROTATIONAL CORR B = {Settings.RotationalCorrB}                         \r\n" +
                    $"SENSITIVITY FACTOR A = {Settings.SensitivityFactorA}                   \r\n" +
                    $"SENSITIVITY FACTOR B = {Settings.SensitivityFactorB}                   \r\n" +
                    $"\r\n \r\n";

                foreach (var data in Datalist)
                {
                     
                    string blockTemplate =
                    data.Depth.ToString().PadRight(6) +
                    "   A0 " +
                    data.Aplus.ToString().PadRight(8) +
                    "   B0 " +
                    data.Bplus.ToString().PadRight(8) +
                    "\r\n" +
                    "      " +
                    " A180 " +
                    data.Aminus.ToString().PadRight(8) +
                    " B180 " +
                    data.Bminus.ToString().PadRight(8) +
                    "\r\n";

                    template += blockTemplate;
                }

                //template = template.Replace($"\n", Environment.NewLine);

                System.IO.File.WriteAllText(rppPath, template);
            });
        }

        protected override bool OnBackButtonPressed()
        {
            btnMainMenu_Clicked(this, null);
            return base.OnBackButtonPressed();
        }

        protected override void OnDisappearing()
        {
            for (int i = 1; i <= Navigation.NavigationStack.Count - 2; i++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[i]);
            }
            base.OnDisappearing();
        }

        private void Excel_Tapped(object sender, EventArgs e)
        {
            try
            {
                Launcher.OpenAsync
                           (new OpenFileRequest()
                           {
                               File = new ReadOnlyFile(exPath)
                           }
                       );
            }
            catch
            {
                App.ToastShort("File not ready yet");
            }
        }

        private void Csv_Tapped(object sender, EventArgs e)
        {
            try
            {
                Launcher.OpenAsync
                           (new OpenFileRequest()
                           {
                               File = new ReadOnlyFile(csvPath)
                           }
                       );
            }
            catch
            {
                App.ToastShort("File not ready yet");
            }
        }

        private void Rpp_Tapped(object sender, EventArgs e)
        {
            try
            {
                Launcher.OpenAsync
                        (new OpenFileRequest()
                        {
                            File = new ReadOnlyFile(rppPath)
                        }
                    );
            }
            catch
            {
                App.ToastShort("File not ready yet");
            }
        }

        private async void Charts_Tapped(object sender, EventArgs e)
        {
            AdrGraph adrGraph = new AdrGraph(Record, Datalist.ToList());
            await Navigation.PushAsync(adrGraph);

            //AdrImageGraph adrGraph = new AdrImageGraph(Record, Datalist.ToList());
            //await Navigation.PushAsync(adrGraph);
        }

        private async void btnMainMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}