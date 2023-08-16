using GoldCloud.Presentation.WebBase.Extensions;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddRemoteConfiguration();
builder.Configuration.AddUserSecrets<Program>();
// ���� KingMetal ȱʡ��Ϊ��
builder.ConfigurationKingMetalDefault();
// ���� ClusterClient ������
builder.ConfigurationClusterClientHandler();
// ���� Ӧ�÷���
builder.ConfigurationAppServices();

var app = builder.Build();
// ʹ�� KingMetal ȱʡ���á�
app.UseKingMetalDefaultConfiguration();
// ʹ�� KingMetal ��ܡ�
app.UseKingMetal();

await app.RunAsync();