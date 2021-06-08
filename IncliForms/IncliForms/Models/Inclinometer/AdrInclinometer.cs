using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace IncliForms.Models.Inclinometer
{
    [SQLite.Table("AdrInclinometer")]
    public class AdrInclinometer
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// The version of the Driver Software associated with this device
        /// </summary>
        public readonly string VersionName = "0.1 Alpha";
        /// <summary>
        /// The name of the device model
        /// </summary>
        public readonly string ModelName = "AD-600 Inclinometer";
        /// <summary>
        /// INSTR CONST 
        /// </summary>
        public readonly int InstrConst = 50000;
        /// <summary>
        /// Probe's Serial number
        /// </summary>
        public int ProbeSerial { get; set; }
        /// <summary>
        /// Reel's Serial number
        /// </summary>
        public int ReelSerial { get; set; }
        /// <summary>
        /// The UUID used for Bluetooth Connectivity
        /// </summary>
        public string BluetoothUUID { get; set; }
        /// <summary>
        /// A description for the user to distinguish between devices
        /// </summary>
        public string BoreholeName { get; set; }
        /// <summary>
        /// HOLE #
        /// </summary>
        public string BoreholeNumber { get; set; }
        /// <summary>
        /// The Entire Depth of the borehole
        /// </summary>
        public float BoreholeDepth { get; set; }
        /// <summary>
        /// The depth at which the reading was Started, the value is more than EndDepth
        /// </summary>
        public float StartDepth { get; set; }
        /// <summary>
        /// The depth at which the reading was Ended, the value is less than StartDepth
        /// </summary>
        public float EndDepth { get; set; } = 0.5f;
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
    }
}
