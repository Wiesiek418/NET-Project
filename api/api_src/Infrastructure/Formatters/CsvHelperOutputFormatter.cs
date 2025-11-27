using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

public class CsvHelperOutputFormatter : TextOutputFormatter
{
    public CsvHelperOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (type == null) return false;

        return typeof(IEnumerable<object>).IsAssignableFrom(type)
               || !type.IsPrimitive;
    }

    public override async Task WriteResponseBodyAsync(
        OutputFormatterWriteContext context,
        Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;

        await using var writer = new StreamWriter(response.Body, selectedEncoding, leaveOpen: true);

        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        await using var csv = new CsvWriter(writer, csvConfig);

        // list or single object
        if (context.Object is IEnumerable<object> list)
            await csv.WriteRecordsAsync(list);
        else
            await csv.WriteRecordsAsync(new[] { context.Object });

        await writer.FlushAsync();
    }
}