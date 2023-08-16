# KingMetal Project Template

### Installation via dotnet new template

```bash
dotnet new -i KingMetal.Orleans.Solution.Template
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

Olreans Client

`KingMetalTemplateProject.Presentation.Api`

### Configure

[See](Config.md)