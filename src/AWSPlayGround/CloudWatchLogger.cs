using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;

namespace AWSPlayGround;

public class CloudWatchLogger
{

    private IAmazonCloudWatchLogs _awsCloudClient;
    private string _logGroup;
    private CloudWatchLogger(string logGroup)
    {
        var awsCreds = new BasicAWSCredentials("AKIAWYR53V5ME32K3OOX", "hu4Ap996E3JOVEOnnPGsgIIAk7J62CdJupI274Tu");
        _awsCloudClient = new AmazonCloudWatchLogsClient(awsCreds, new AmazonCloudWatchLogsConfig() { AuthenticationRegion = "us-east-1"});
        _logGroup = logGroup;
    }

    public static async Task<CloudWatchLogger> GetLoggerAsync(string logGroup)
    {
        var logger = new CloudWatchLogger(logGroup);
        await logger.CreateLoggerAsync();
        return logger;
    }

    private async Task CreateLoggerAsync()
    {
        _ = await _awsCloudClient.CreateLogGroupAsync(new CreateLogGroupRequest()
        {
            LogGroupName = _logGroup
        });
    }
}