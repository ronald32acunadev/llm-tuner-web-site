namespace llm_tuner_web_site.Models;

public class CliCommand
{
    public string Flag { get; set; } = "";
    public string Description { get; set; } = "";
    public string Category { get; set; } = "General";
    public string DefaultValue { get; set; } = "-";
    public string Example { get; set; } = "";
}

public class LoadProfileInfo
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Tagline { get; set; } = "";
    public string Description { get; set; } = "";
    public string KvTypes { get; set; } = "";
    public string TargetVariant { get; set; } = "";
    public string TradeOff { get; set; } = "";
    public string RecommendedCondition { get; set; } = "";
    public bool IsDefault { get; set; }
}

public class ExecutionModeInfo
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Command { get; set; } = "";
    public string BinTarget { get; set; } = "";
    public string Badge { get; set; } = "";
}

public class DocOption
{
    public string Flag { get; set; } = "";
    public string Description { get; set; } = "";
    public string DefaultValue { get; set; } = "";
    public string Group { get; set; } = "";
}

public class DocExample
{
    public string Caption { get; set; } = "";
    public string Code { get; set; } = "";
}

public class DocSection
{
    public string Id { get; set; } = "";
    public string Group { get; set; } = "";
    public string Title { get; set; } = "";
    public List<string> Paragraphs { get; set; } = new();
    public List<DocExample> Examples { get; set; } = new();
    public List<DocOption> Options { get; set; } = new();
    public string ImagePath { get; set; } = "";
    public string ImageAlt { get; set; } = "";
}

public class DocGroup
{
    public string Id { get; set; } = "";
    public string Label { get; set; } = "";
}
