# ChangeLog

这个项目所有值得注意的变化都将记录在这个文件中。

# Version 1.0.11  - 2023-08-17

- 新增单元测试用例
- 升级到 `dotnet 7.0`
- 集群管理方式、配置中心都变更为 `Consul`
- 不再使用 `nlogsettings.config` 文件配置日志
- 新增应用层 `Application` 集中管理应用服务，也便于单元测试

# Version 1.0.5  - 2023-03-28

- Host Nlog 日志记录配置方式修改为使用 `nlogsettings.config` 文件

# Version 1.0.0  - 2023-01-31

- Initial release.
