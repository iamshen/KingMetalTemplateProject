# KingMetal Project Template

### Installation via dotnet new template

```bash
dotnet new -i KingMetal.Orleans.Solution.Template::1.0.x
```

### Create new project:

```bash
dotnet new king-sln --name "HsCrm" --dbTPrefix "hc_" --appName "crm" --connectionString "Your ConnectionString"
```

Project template options:

```bash
--name: [string value] for project name
--dbTPrefix: [string value] The prefix of DataBase table(for example: [gi_]).
--connectionString: [string value] The ConnectionString of DataBase.
--appName: [string value] The short name of application(for example: [crm|order|cart...]).
```

### Running in Visual Studio

- Set Startup projects:
  - xxx.Presentation.Host
  - xxx.Presentation.Api
