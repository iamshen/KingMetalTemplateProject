# SQL 初始化流程

## 1. 创建模式

```postgresql
-- 创建 业务模式
CREATE SCHEMA gold_work;
-- 创建 集群模式
CREATE SCHEMA gold_cluster;
```

## 2. 设置默认模式

```postgresql
-- 设置方式为在当前所在的数据库执行
-- ALTER ROLE 用户名(例如postgres) SET search_path=gold_cluster
ALTER ROLE gold_xxx SET search_path=gold_cluster;

-- 验证（需要断开会话，重新链接）
show search_path ;
```

## 3. 创建集群表

SQL 初始化详情点击查看以下文件

[gold_cluster.sql](gold_cluster.sql)

## 4. 创建业务表

SQL 初始化详情点击查看以下文件

[gold_work.sql](gold_work.sql)
