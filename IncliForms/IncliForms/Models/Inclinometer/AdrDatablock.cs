using System;

namespace IncliForms.Models.Inclinometer
{
    [SQLite.Table("AdrDatablock")]
    public class AdrDatablock
    {
        /// <summary>
        /// A recursive block of data, seen in an AdrRecord
        /// </summary>
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; } = 0;
        /// <summary>
        /// The Record to which this datablock belongs
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Depth level
        /// </summary>
        public float Depth { get; set; } = 0;
        /// <summary>
        /// The A parameter
        /// </summary>
        public float Aplus { get; set; } = 0;
        /// <summary>
        /// The B parameter
        /// </summary>
        public float Bplus { get; set; } = 0;
        /// <summary>
        /// The rotated A parameter
        /// </summary>
        public float Aminus { get; set; } = 0;
        /// <summary>
        /// The rotated B parameter
        /// </summary>
        public float Bminus { get; set; } = 0;
        /// <summary>
        /// Delta = |Aplus - Aminus|
        /// </summary>
        public float DeltaA { get; set; }
        /// <summary>
        /// Delta = |Bplus - Bminus|
        /// </summary>
        public float DeltaB { get; set; }

        public void CalculateDeltas()
        {
            //if (Aplus == 0) throw new Exception("Aplus is null");
            //if (Bplus == 0) throw new Exception("Bplus is null");
            //if (Aminus == 0) throw new Exception("Aminus is null");
            //if (Bminus == 0) throw new Exception("Bminus is null");

            DeltaA = Math.Abs(Aplus + Aminus);
            DeltaB = Math.Abs(Bplus + Bminus);
        }

        public float AvA()
        {
            return (Aplus + Aminus) / 2;
        }

        public float AvB()
        {
            return (Bplus + Bminus) / 2;
        }

    }
}
