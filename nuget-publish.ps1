# 定义参数
Param(
    # Nuget APIKey
    [string] $apikey
)

if ($apikey -eq $null -or $apikey -eq "")
{
    # 请从配置中读取
    $apikey = "xxx"; 
}

# 加载 .nuspec 文件
[xml]$nuspec = Get-Content -Path "KingMetal.Template.nuspec"

# 从 .nuspec 文件中读取当前版本号
$currentVersion = $nuspec.package.metadata.version
$versionParts = $currentVersion.Split('.')

# 提取 major, minor, build 和 revision 部分
$major = $versionParts[0]
$minor = $versionParts[1]
$build = $versionParts[2]
$revision = [int]$versionParts[3]

# 计算新的 build 号，以 2024年1月1日为起点
$startDate = Get-Date "2024-01-01"
$today = Get-Date
$newBuild = ($today - $startDate).Days

# 如果 build 号发生变化，重置 revision 为 0，否则自增 revision
if ($newBuild -ne $build) {
    $build = $newBuild
    $revision = 0
} else {
    $revision++
}

# 组合新的版本号
$newVersion = "$major.$minor.$build.$revision"
# 更新 .nuspec 文件中的 version 节点
$nuspec.package.metadata.version = $newVersion
# 读取 CHANGELOG.md 文件内容，使用 UTF-8 编码
$releaseNotes = Get-Content -Path "CHANGELOG.md" -Raw -Encoding UTF8
# 更新 releaseNotes 节点
$nuspec.package.metadata.releaseNotes = $releaseNotes
# 保存更改后的 .nuspec 文件
$nuspec.Save("KingMetal.Template.nuspec")

# 接下来，打包 NuGet 包

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
Copy-Item ./CONFIG.md $contentDirectory -recurse -force

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