namespace TorrentManager.Tests.Cli;

using TorrentManager.Cli;

public sealed class CommandLineParserTests
{
    [Fact]
    public void Parse_UsesDefaultDbPath_WhenDbOptionMissing()
    {
        var args = new[] { "export", "by_category", "A%" };

        var parsed = CommandLineParser.Parse(args);

        // 输出解析结果以便调试
        parsed.PrintInfo();

        Assert.Equal(CommandLineParser.DefaultDbPath, parsed.DbPath);
        Assert.Equal("export", parsed.Positionals[0]);
        Assert.Equal("by_category", parsed.Positionals[1]);
        Assert.Equal("A%", parsed.Positionals[2]);
    }

    [Fact]
    public void Parse_ParsesDbAndPathOptions()
    {
        var args = new[] { "export", "by_save_path", "%\\music\\%", "--path", "_out", "--db", "custom.db" };

        var parsed = CommandLineParser.Parse(args);

        Assert.Equal("custom.db", parsed.DbPath);
        Assert.Equal("_out", parsed.Options["path"]);
        Assert.Equal("custom.db", parsed.Options["db"]);
    }

    [Fact]
    public void Parse_Throws_WhenOptionValueIsMissing()
    {
        var args = new[] { "export", "by_category", "A%", "--db" };

        var ex = Assert.Throws<ArgumentException>(() => CommandLineParser.Parse(args));

        Assert.Contains("缺少值", ex.Message);
    }
}
