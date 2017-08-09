using AS = Microsoft.AnalysisServices.Tabular;
using System;
using System.Data.OleDb;

namespace Pentamic.SSBI.Models.DataModel
{
    public static class EnumExtension
    {
        public static AS.DataType ToDataType(this ColumnDataType dataType)
        {
            return (AS.DataType)Enum.Parse(typeof(AS.DataType), dataType.ToString());
        }

        public static ColumnDataType ToDataType(this OleDbType dataType)
        {
            if ((int)dataType == 146)
            {
                return ColumnDataType.DateTime;
            }
            switch (dataType)
            {
                case OleDbType.BigInt:
                case OleDbType.Integer:
                case OleDbType.SmallInt:
                case OleDbType.UnsignedTinyInt:
                case OleDbType.UnsignedSmallInt:
                case OleDbType.UnsignedInt:
                case OleDbType.UnsignedBigInt:
                    return ColumnDataType.Int64;
                case OleDbType.Binary:
                case OleDbType.VarBinary:
                    return ColumnDataType.Binary;
                case OleDbType.Boolean:
                    return ColumnDataType.Boolean;
                case OleDbType.BSTR:
                case OleDbType.Char:
                case OleDbType.Guid:
                case OleDbType.VarWChar:
                case OleDbType.WChar:
                case OleDbType.VarChar:
                    return ColumnDataType.String;
                case OleDbType.Currency:
                case OleDbType.Decimal:
                case OleDbType.Numeric:
                case OleDbType.VarNumeric:
                    return ColumnDataType.Decimal;
                case OleDbType.Double:
                    return ColumnDataType.Double;
                case OleDbType.Date:
                case OleDbType.DBDate:
                case OleDbType.DBTime:
                case OleDbType.DBTimeStamp:
                case OleDbType.Filetime:
                    return ColumnDataType.DateTime;
                case OleDbType.Variant:
                    return ColumnDataType.Variant;
                default:
                    throw new Exception("Type not found");
            }
        }

        public static AS.ModeType ToModeType(this ModeType modeType)
        {
            return (AS.ModeType)Enum.Parse(typeof(AS.ModeType), modeType.ToString());
        }

        public static AS.PartitionSourceType ToPartitionSourceType(this PartitionSourceType partitionSourceType)
        {
            return (AS.PartitionSourceType)Enum.Parse(typeof(AS.PartitionSourceType), partitionSourceType.ToString());
        }
    }
}
