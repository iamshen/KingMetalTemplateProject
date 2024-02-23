# KingMetal Project Template

[![Build](https://github.com/iamshen/KingMetalTemplateProject/actions/workflows/nuget-publish.yml/badge.svg)](https://github.com/iamshen/KingMetalTemplateProject/actions/workflows/nuget-publish.yml)
[![Nuget](https://img.shields.io/nuget/v/KingMetal.Orleans.Solution.Template?label=NuGet)](https://www.nuget.org/packages/KingMetal.Orleans.Solution.Template)
[![Nuget](https://img.shields.io/nuget/dt/KingMetal.Orleans.Solution.Template?label=Downloads)](https://www.nuget.org/packages/KingMetal.Orleans.Solution.Template)


### Installation via dotnet new template

```bash
# 安装/更新
dotnet new install KingMetal.Orleans.Solution.Template
# 卸载
dotnet new uninstall KingMetal.Orleans.Solution.Template
```

### Create new project:

new solution

```bash
dotnet new kingmetal --name "Crm"
```

options:

```bash
--name: [string value] for project name
```


### Initialization SQL

[See](src/Infrastructure/KingMetalTemplateProject.Infrastructure.Database/SQL/README.md)

### Running

> Current Target Framework Version is `net7.0`

#### Set Startup projects:

Orleans Silo 

`KingMetalTemplateProject.Presentation.Host`

Orleans Client

`KingMetalTemplateProject.Presentation.Api`

### Configure

[See](CONFIG.md)