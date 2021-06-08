using System;

namespace IncliForms.Models.Inclinometer
{
    [SQLite.Table("AdrRecord")]
    public class AdrRecord
    {
        /// <summary>
        /// A record of data, harvested using an Incinometer
        /// </summary>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// The Device that recorded this data
        /// </summary>
        public int InclinometerId { get; set; }
        /// <summary>
        /// The date at which this record of data was harvested
        /// </summary>
        public DateTime DateHarvested { get; set; }
        /// <summary>
        /// A description for the user to distinguish between devices
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// $"{Settings.SiteName} - {BoreholeName}#{BoreholeNumber} - ({boreholeReadCount + 1})"
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// The name of the operator, using this device
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// The depth at which the reading was Started, the value is more than EndDepth
        /// </summary>
        public float StartDepth { get; set; }
        /// <summary>
        /// The depth at which the reading was Ended, the value is less than StartDepth
        /// </summary>
        public float EndDepth { get; set; }
        /// <summary>
        /// The length of every step in meters
        /// </summary>
        public readonly float IntervalLength = 0.5f;
        /// <summary>
        /// Unit always set to metric
        /// </summary>
        public readonly char Unit = 'M';
        /// <summary>
        /// DIGITILT/SPIRAL
        /// </summary>
        public readonly char Digital = 'D';
        //- Calibration
        /// <summary>
        /// ROTATIONAL CORR A
        /// </summary>
        public float RotationalCorrA { get; set; } = 0;
        /// <summary>
        /// ROTATIONAL CORR B
        /// </summary>
        public float RotationalCorrB { get; set; } = 0;
        /// <summary>
        /// SENSITIVITY FACTOR A
        /// </summary>
        public float SensitivityFactorA { get; set; } = 0;
        /// <summary>
        /// SENSITIVITY FACTOR B
        /// </summary>
        public float SensitivityFactorB { get; set; } = 0;
        /// <summary>
        /// Path to the Excel File
        /// </summary>
        public string ExcelPath { get; set; }
        /// <summary>
        /// Path to the Csv File
        /// </summary>
        public string CsvPath { get; set; }
        /// <summary>
        /// Path to the Rpp File
        /// </summary>
        public string RppPath { get; set; }
    }
}
