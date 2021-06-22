namespace IncliForms.Models
{
    [SQLite.Table("Settings")]
    public class Settings
    {
        /// <summary>
        /// Global Settings for the application
        /// </summary>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; } = 0;
        /// <summary>
        /// Dam Name
        /// </summary>
        public string SiteName { get; set; } = "Site Name";
        /// <summary>
        /// The name of the operator, using this device
        /// </summary>
        public string OperatorName { get; set; } = "Operator Name";
        /// <summary>
        /// Record unit
        /// </summary>
        public RecordUnit RecordUnit { get; set; } = RecordUnit.cm;
        /// <summary>
        /// The interval at which the program should check for new data in the buffer
        /// </summary>
        public int RefreshInterval { get; set; } = 256;

        #region DefaultCalibration
        /// <summary>
        /// ROTATIONAL CORR A
        /// </summary>
        public float RotationalCorrA { get; set; } = 1;
        /// <summary>
        /// ROTATIONAL CORR B
        /// </summary>
        public float RotationalCorrB { get; set; } = 1;
        /// <summary>
        /// SENSITIVITY FACTOR A
        /// </summary>
        public float SensitivityFactorA { get; set; } = 0;
        /// <summary>
        /// SENSITIVITY FACTOR B
        /// </summary>
        public float SensitivityFactorB { get; set; } = 0;
        /// <summary>
        /// Battery = this * Battery
        /// </summary>
        public float BatteryMultiplier { get; set; } = 1;
        /// <summary>
        /// Battery = this + Battery
        /// </summary>
        public float BatteryOffset { get; set; } = 0;
        #endregion

    }

    /// <summary>
    /// Record Unit
    /// </summary>
    public enum RecordUnit
    {
        /// <summary>
        /// Centimeters 1*
        /// </summary>
        cm,
        /// <summary>
        /// Meters 0.01*
        /// </summary>
        m,
        /// <summary>
        /// Milimeters 10
        /// </summary>
        mm,
    }
}
