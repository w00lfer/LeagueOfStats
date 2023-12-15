// using System.Text.Json;
// using System.Text.Json.Serialization;
// using NodaTime;
// using NodaTime.Serialization.SystemTextJson;
// using NodaTime.Text;
// using NodaTime.Utility;
//
// namespace LeagueOfStats.API.Infrastructure.Converters;
//
// public class LocalDateConverter : JsonConverter<LocalDate>
// {
//     public override LocalDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         return new LocalDate(NodaConverters.LocalDateConverter.Read(ref reader, typeToConvert, options).);
//         return new LocalDate(NodaConverters.LocalDateConverter.Read(ref reader, typeToConvert, options));
//     }
//
//     public override void Write(Utf8JsonWriter writer, LocalDate value, JsonSerializerOptions options)
//     {
//         NodaConverters.LocalDateConverter.Write(writer, value, options);
//     }
// }