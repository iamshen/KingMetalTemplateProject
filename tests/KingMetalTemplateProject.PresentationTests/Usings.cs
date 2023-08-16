// Orleans
global using Orleans;
global using Orleans.TestingHost;
global using Orleans.Hosting;
global using Orleans.Streams;

// System
global using Xunit;
global using System.Globalization;

// Infrastructure
global using GoldCloud.Infrastructure.Common.Options;

// Microsoft
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Hosting.Internal;
global using Microsoft.Extensions.Logging;

// KingMetal
global using KingMetal.Domains.UniqueValueService.Grains;

// KingMetalTemplateProject
global using KingMetalTemplateProject.PresentationTests;
global using KingMetalTemplateProject.Domain.Observers;
global using KingMetalTemplateProject.Infrastructure.Database.Extensions;
