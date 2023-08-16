using GoldCloud.Presentation.WebBase.Extensions;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddRemoteConfiguration();
builder.Configuration.AddUserSecrets<Program>();
// 配置 KingMetal 缺省行为。
builder.ConfigurationKingMetalDefault();
// 配置 ClusterClient 处理器
builder.ConfigurationClusterClientHandler();
// 配置 应用服务。
builder.ConfigurationAppServices();

var app = builder.Build();
// 使用 KingMetal 缺省配置。
app.UseKingMetalDefaultConfiguration();
// 使用 KingMetal 框架。
app.UseKingMetal();

await app.RunAsync();