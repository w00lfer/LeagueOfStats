using System.ComponentModel;
using System.Globalization;
using NodaTime.Text;

namespace LeagueOfStats.API.Common.Converters;

public class InstantTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return (sourceType == typeof(string));
    }

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? cultureInfo, object value)
    {
        return InstantPattern.General.Parse(value.ToString()).Value;
    }
}