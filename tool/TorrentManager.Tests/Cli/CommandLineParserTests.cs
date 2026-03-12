namespace TorrentManager.Tests.Cli;

using TorrentManager.Cli;

public sealed class
CommandLineParserTests(ITestOutputHelper output)
{
    [Fact]
    public void Parse_UsesDefaultDbPath_WhenDbOptionMissing()
    {
        var args = new[] { "export", "by_category", "A%" };

        var parsed = CommandLineParser.Parse(args);

        // 输出解析结果以便调试
        output.WriteLine(parsed.ToString());

        Assert.Equal(CommandLineParser.DefaultDbPath, parsed.Options["db"]);
        Assert.Equal("export", parsed.Positionals[0]);
        Assert.Equal("by_category", parsed.Positionals[1]);
        Assert.Equal("A%", parsed.Positionals[2]);
    }

    [Fact]
    public void Parse_ParsesDbAndPathOptions()
    {
        var args = new[] { "export", "by_save_path", "%\\music\\%", "--path", "_out", "--db", "custom.db" };

        var parsed = CommandLineParser.Parse(args);
        output.WriteLine(parsed.ToString());

        Assert.Equal(3, parsed.Positionals.Count);
        Assert.Equal(2, parsed.Options.Count);

        Assert.Equal("export", parsed.Positionals[0]);
        Assert.Equal("by_save_path", parsed.Positionals[1]);
        Assert.Equal("%\\music\\%", parsed.Positionals[2]);

        Assert.Equal("custom.db", parsed.Options["db"]);
        Assert.Equal("_out", parsed.Options["path"]);
    }

    [Fact]
    public void Parse_Throws_WhenOptionValueIsMissing()
    {
        var args = new[] { "export", "by_category", "A%", "--db" };

        var ex = Assert.Throws<ArgumentException>(() => CommandLineParser.Parse(args));

        Assert.Contains("缺少值", ex.Message);
    }
}
