using Microsoft.Kiota.Abstractions.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace GitHub.Repos.Item.Item.Actions.Workflows.Item.Dispatches;

/// <summary>
/// Response from workflow dispatch when return_run_details is true.
/// </summary>
public class DispatchesPostResponse : IParsable
{
    /// <summary>The workflow run ID.</summary>
    public long? WorkflowRunId { get; set; }

    /// <summary>The API URL for the workflow run.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
    public string? RunUrl { get; set; }
#nullable restore
#else
    public string RunUrl { get; set; }
#endif

    /// <summary>The HTML URL for the workflow run.</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
    public string? HtmlUrl { get; set; }
#nullable restore
#else
    public string HtmlUrl { get; set; }
#endif

    public static DispatchesPostResponse CreateFromDiscriminatorValue(IParseNode parseNode)
    {
        _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
        return new DispatchesPostResponse();
    }

    public IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
    {
        return new Dictionary<string, Action<IParseNode>>
        {
            { "workflow_run_id", n => { WorkflowRunId = n.GetLongValue(); } },
            { "run_url", n => { RunUrl = n.GetStringValue(); } },
            { "html_url", n => { HtmlUrl = n.GetStringValue(); } },
        };
    }

    public void Serialize(ISerializationWriter writer)
    {
        _ = writer ?? throw new ArgumentNullException(nameof(writer));
        writer.WriteLongValue("workflow_run_id", WorkflowRunId);
        writer.WriteStringValue("run_url", RunUrl);
        writer.WriteStringValue("html_url", HtmlUrl);
    }
}
