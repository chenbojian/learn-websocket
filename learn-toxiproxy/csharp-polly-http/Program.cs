// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Polly;
using Polly.Retry;

Console.WriteLine("Hello, World!");

var resiliencePipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions())
    .AddTimeout(TimeSpan.FromSeconds(10))
    .Build();

var httpServerProcess = new Process();
httpServerProcess.StartInfo.FileName = "npx";
httpServerProcess.StartInfo.Arguments = "http-server";
httpServerProcess.Start();

var toxiProxyServerProcess = new Process();
toxiProxyServerProcess.StartInfo.FileName = "toxiproxy-server";
toxiProxyServerProcess.Start();

var toxiProxyCliProcess = new Process();
toxiProxyCliProcess.StartInfo.FileName = "toxiproxy-cli";
toxiProxyCliProcess.StartInfo.Arguments = "create -l localhost:8081 -u localhost:8080 npx-http-server";
toxiProxyCliProcess.Start();
await toxiProxyCliProcess.WaitForExitAsync();

async Task AddToxic()
{
    var process = new Process();
    process.StartInfo.FileName = "toxiproxy-cli";
    process.StartInfo.Arguments = "toxic add -t reset_peer npx-http-server";
    process.Start();
    await process.WaitForExitAsync();
}

async Task RemoveToxic()
{
    var process = new Process();
    process.StartInfo.FileName = "toxiproxy-cli";
    process.StartInfo.Arguments = "toxic remove -n reset_peer_downstream npx-http-server";
    process.Start();
    await process.WaitForExitAsync();
}

Thread.Sleep(5000);

await AddToxic();
int retry = 0;
await resiliencePipeline.ExecuteAsync(async (token) =>
{
    if (retry == 3)
    {
        await RemoveToxic();
    }
    Console.WriteLine($"\x1b[36m=========> Trying the {retry++} time {DateTime.UtcNow}\x1b[0m");
    var httpClient = new HttpClient();
    var result = await httpClient.GetStringAsync("http://localhost:8081", token);
    Console.WriteLine($"\x1b[36m=========> Success the {retry - 1} time {DateTime.UtcNow}\x1b[0m");
});
