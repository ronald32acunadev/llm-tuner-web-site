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
