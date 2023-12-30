using System.Reflection;
using System.Text;
using RazorLight;

namespace GatherUs.Core.Mailing.SetUp;

public static class MailingHelper
{
    public static async Task<string> GenerateTemplate<TData>(MailType type, TData data)
    {
        var template = await ExtractMailContent(type);

        var engine = new RazorLightEngineBuilder().Build();

        var key = EnumToFileBindings.GetFileName(type);
        var result = await engine.CompileRenderStringAsync(key, template, data);
        return result;
    }

    private static async Task<string> ExtractMailContent(MailType type)
    {
        var fileName = EnumToFileBindings.GetFileName(type);

        var path = Assembly.GetExecutingAssembly()
            .GetManifestResourceNames()
            .Single(p => p.EndsWith($"Mailing.Templates.{fileName}.cshtml"));

        var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);

        using var reader = new StreamReader(resourceStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}
