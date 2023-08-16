# 定义参数
Param(
    # Nuget APIKey
    [string] $apikey
)

if ($apikey -eq $null -or $apikey -eq "")
{
    $apikey = "oy2crbz2sevvkqqzmpdaf42yv4e2uyzujpd4xi3nwl2dwm";
}

$templateName = "templates"
$templateSrc =     "./$templateName/content/src"
$templateTests =     "./$templateName/content/tests"
$templateConfig =     "./$templateName/content/.template.config"
$contentDirectory = "./$templateName/content"
$templateNuspec = "KingMetal.Template.nuspec"

# Clean up src, tests, config folders
if ((Test-Path -Path $templateSrc)) { Remove-Item ./$templateSrc -recurse -force }
if ((Test-Path -Path $templateTests)) { Remove-Item ./$templateTests -recurse -force }
if ((Test-Path -Path $templateConfig)) { Remove-Item ./$templateConfig -recurse -force }
if ((Test-Path -Path $contentDirectory)) { Remove-Item ./$contentDirectory -recurse -force }

# Create src, tests, config folders
if (!(Test-Path -Path $templateSrc)) { mkdir $templateSrc }
if (!(Test-Path -Path $templateTests)) { mkdir $templateTests }
if (!(Test-Path -Path $templateConfig)) { mkdir $templateConfig }
 
Write-Output "Copy KingMetal Project Template..."

# Copy the latest src and tests to content
Copy-Item ./src/* $templateSrc -recurse -force
Copy-Item ./tests/* $templateTests -recurse -force
Copy-Item ./.template.config/* $templateConfig -recurse -force

# Copy Solution Items
Copy-Item ./Common.props $contentDirectory -recurse -force
Copy-Item ./Directory.Build.props $contentDirectory -recurse -force
Copy-Item ./Directory.Packages.props $contentDirectory -recurse -force
Copy-Item ./Version.props $contentDirectory -recurse -force
Copy-Item ./.gitattributes $contentDirectory -recurse -force
Copy-Item ./.gitignore $contentDirectory -recurse -force
Copy-Item ./KingMetalTemplateProject.sln $contentDirectory -recurse -force
Copy-Item ./KingMetalTemplateProject.sln.DotSettings $contentDirectory -recurse -force
Copy-Item ./LICENSE.md $contentDirectory -recurse -force
Copy-Item ./CHANGELOG.md $contentDirectory -recurse -force
Copy-Item ./README.md $contentDirectory -recurse -force

# Copy nuspec
Write-Output "Copy KingMetal.Template.nuspec..."
Copy-item -Force -Recurse "$templateNuspec" -Destination $contentDirectory


######################################
# Step 2
$templateNuspecPath = "./templates/content/$templateNuspec"
nuget pack $templateNuspecPath -NoDefaultExcludes
######################################
# Step 3
$project_nupkg = Get-ChildItem -Filter *.nupkg;
Write-Output "publish $project_nupkg to nuget.org..."

$nupkg = $project_nupkg.FullName;

Write-Output "-----------------";
$nupkg;

# Step 4
dotnet nuget push $nupkg --skip-duplicate --api-key $apikey --source https://api.nuget.org/v3/index.json;

Write-Output "-----------------";

Write-Output "finish publish $project_nupkg to nuget.org...";

Remove-Item $project_nupkg -Force -recurse

Write-Warning "发布成功";