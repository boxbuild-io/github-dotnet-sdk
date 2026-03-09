using GitHub.Repos.Item.Item.Actions.Workflows.Item.Dispatches;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Moq;
using Xunit;

public class DispatchesRequestBuilderTests
{
    private static (DispatchesRequestBuilder builder, Mock<IRequestAdapter> mockAdapter) CreateBuilder()
    {
        var mockAdapter = new Mock<IRequestAdapter>();
        var mockSerializationWriterFactory = new Mock<ISerializationWriterFactory>();
        var mockSerializationWriter = new Mock<ISerializationWriter>();
        mockSerializationWriterFactory
            .Setup(f => f.GetSerializationWriter(It.IsAny<string>()))
            .Returns(mockSerializationWriter.Object);
        mockAdapter
            .Setup(a => a.SerializationWriterFactory)
            .Returns(mockSerializationWriterFactory.Object);

        var builder = new DispatchesRequestBuilder(new Dictionary<string, object>
        {
            { "baseurl", "https://api.github.com" },
            { "owner%2Did", "org" },
            { "repo%2Did", "repo" },
            { "workflow_id", "main.yml" },
        }, mockAdapter.Object);

        return (builder, mockAdapter);
    }

    [Fact]
    public async Task PostAsync_ReturnsDispatchResponse()
    {
        var (builder, mockAdapter) = CreateBuilder();

        var expectedResponse = new DispatchesPostResponse
        {
            WorkflowRunId = 22725043315,
            RunUrl = "https://api.github.com/repos/org/repo/actions/runs/22725043315",
            HtmlUrl = "https://github.com/org/repo/actions/runs/22725043315",
        };

        mockAdapter
            .Setup(a => a.SendAsync(
                It.IsAny<RequestInformation>(),
                It.IsAny<ParsableFactory<DispatchesPostResponse>>(),
                It.IsAny<Dictionary<string, ParsableFactory<IParsable>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var body = new DispatchesPostRequestBody
        {
            Ref = "main",
            AdditionalData = { ["return_run_details"] = true },
        };

        var result = await builder.PostAsync(body);

        Assert.NotNull(result);
        Assert.Equal(22725043315, result!.WorkflowRunId);
        Assert.Equal("https://api.github.com/repos/org/repo/actions/runs/22725043315", result.RunUrl);
        Assert.Equal("https://github.com/org/repo/actions/runs/22725043315", result.HtmlUrl);
    }

    [Fact]
    public async Task PostAsync_ThrowsOnNullBody()
    {
        var (builder, _) = CreateBuilder();
        await Assert.ThrowsAsync<ArgumentNullException>(() => builder.PostAsync(null!));
    }
}
