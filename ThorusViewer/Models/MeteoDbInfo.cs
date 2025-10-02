using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ThorusViewer.Models;

public enum MeteoDbStatus
{
    Absent,
    Offline,
    Online
}

public class CalendarRange
{
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime Start { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime End { get; set; }

    public int Length { get; set; }
}

public class MeteoDbInfo
{
    public string Name { get; set; }
    public int Dbi { get; set; }
    public CalendarRange CalendarRange { get; set; }
    public int DataCount => CalendarRange?.Length ?? 0;

    [JsonConverter(typeof(JsonStringEnumConverter<MeteoDbStatus>))]
    public MeteoDbStatus Status { get; set; }

    public override string ToString()
    {
        return Status switch
        {
            MeteoDbStatus.Online => $"{Name} [ACTIVE] [{CalendarRange.Start:yyyy-MM-dd}..{CalendarRange.End:yyyy-MM-dd}]",
            MeteoDbStatus.Offline => $"{Name} [{CalendarRange.Start:yyyy-MM-dd}..{CalendarRange.End:yyyy-MM-dd}]",
            _ => Name
        };
    }
}

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    public CustomDateTimeConverter()
    {
    }
    public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
    {
        writer.WriteStringValue(date.ToString("yyyy-MM-dd"));
    }
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}