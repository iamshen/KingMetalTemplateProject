# 项目配置说明

## **Consul集群管理配置部分说明**

集群管理的 `Consul` 的存储的 `Key` 全部统一配置，`Orleans` 会自动新建子目录来管理没一个节点的信息。

例如: `GoldCloud/Clusters/{Environment}`，其中 `{Environment}` 为占位符，运行时会替换为当前的运行环境名称，例如 `Development` 等，配置如下：

```json
{
  "ConsulClusteringSiloOptions": {
    "AclClientToken": "Your Acl Client Token...",
    "Address": "http://127.0.0.1:8500",
    "KvRootFolder": "GoldCloud/Clusters/{Environment}"
  }
}

```

```json
{
  "ConsulClusteringClientOptions": {
    "AclClientToken": "Your Acl Client Token...",
    "KvRootFolder": "GoldCloud/Clusters/{Environment}",
    "Address": "http://127.0.0.1:8500"
  }
}
```


## **远程配置部分说明**

配置分为:
 - **公用配置**
 - **Host配置**
 - **Api配置** 

具体说明如下

### 1. 根目录

`GoldCloud/Configs/{Environment}` 为全部配置的根目录

### 2. 公用配置

`GoldCloud/Configs/{Environment}/Common`  全局公用配置根目录
`GoldCloud/Configs/{Environment}/Common/Public` 全局公用配置
`GoldCloud/Configs/{Environment}/Common/Api` 全局 Api 的公用配置
`GoldCloud/Configs/{Environment}/Common/Host` 全局 Host 的公用配置

### 3. Api & Host 专用配置

`GoldCloud/Configs/{Environment}/Host/{ApplicationName}`
`GoldCloud/Configs/{Environment}/Api/{ApplicationName}`
 
`{ApplicationName}` 是程序名称的占位符，运行运行时会替换为真实的程序名称。

## Api 的 `appsettings.json` 配置如下

```json
{
  "RemoteConfigOptions": {
    "Enable": true,
    "AppId": "GoldCloud/Configs/{Environment}",
    "MetaServer": "http://127.0.0.1:8500",
    "Namespaces": ["Common/Public", "Common/Api", "Api/{ApplicationName}"],
    "LocalCacheDir": "ConfigCache",
    "Secret": "YourSecret"
  }
}
```

## Host 的 `appsettings.json` 配置如下

```json
{
  "RemoteConfigOptions": {
    "Enable": true,
    "AppId": "GoldCloud/Configs/{Environment}",
    "MetaServer": "http://127.0.0.1:8500",
    "Namespaces": ["Common/Public", "Common/Host", "Host/{ApplicationName}"],
    "LocalCacheDir": "ConfigCache",
    "Secret": "YourSecret"
  }
}
```


## 具体 `Consul` 中的配置大致如下

### 公共配置

<details>
<summary> Common/Public </summary>

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "NLogProvider": {
      "ConsoleTarget": {
        "Enable": true,
        "LayoutItems": {} 
      },
      "FileTarget": {
        "Enable": true,
        "LogDirectory": "logs/logs",
        "ArchiveDirectory": "logs/archives",
        "LogFilePrefix": null,
        "LayoutItems": {} 
      },
      "ESTarget": {
        "Enable": false,
        "UserName": "elastic",
        "Password": "tO6AQrxU8UbNXSCHMLCd",
        "ApiKey": "hYIJeXDZSxmPy8zAUUj-5g", 
        "ApiKeyId": "6pCQS4EBp5-O1D1ypXQe",
        "ServerHost": "https://127.0.0.1:9200",
        "Index": "filebeat-7.16.0",
        "DocumentType": "_doc",
        "LayoutItems": {} 
      },
      "NLogNative": {}
    }
  },

  "AllowedHosts": "*",

  "RemoteApiConfigOptions": {
    "BaseUrl": "https://develop.api.inglod.net",
    "GithubUrl": "https://api.github.com"
   }
}
```

</details>

<details>
<summary>Common/Api</summary>

```json
{
  "ConsulClusteringClientOptions": {
    "AclClientToken": "Your Acl Client Token...",
    "KvRootFolder": "GoldCloud/Clusters/{Environment}",
    "Address": "http://127.0.0.1:8500"
  },
  
  "SwaggerOptions": {
    "Endpoints": [
      {
        "Title": "Default API （内部调用）",
        "Name": "default"
      },
      {
        "Title": "Admin API （管理后台）",
        "Name": "admin"
      },
      {
        "Title": "Merchant API （商户后台）",
        "Name": "mch"
      },
      {
        "Title": "Device API （终端设备）",
        "Name": "device"
      },
      {
        "Title": "Mobile API （移动平台）",
        "Name": "mobile"
      },
      {
        "Title": "Open API （公共开放）",
        "Name": "open"
      }
    ],
    "RoutePrefix": "docs",
    "IsHideSchemas": false,
    "Enabled": true
  },
  
  "WebBaseOptions": {
    "EnableRequestInfoFilter": true,
    "EnableAuthorizeFilter": true
  }
}
```

</details>

<details>
<summary>Common/Host</summary>

```json
{
    "ConsulClusteringSiloOptions": {
        "AclClientToken": "Your Acl Client Token...",
        "Address": "http://127.0.0.1:8500",
        "KvRootFolder": "GoldCloud/Clusters/{Environment}"
    },
    "RabbitMQOptions": {
        "ChannelPoolSize": 20,
        "ConnectionPoolSize": 20,
        "HostNames": "127.0.0.1:5672",
        "MaxBatchPushCount": 100,
        "MaxPrefetchCount": 2000,
        "Password": "YourPassword",
        "UserName": "YourUserName"
    },
    "EndpointOptions": {
        "AdvertisedIPAddress": "127.0.0.1"
    },
    "AdoNetReminderTableOptions": {
        "Invariant": "Npgsql"
    },
    "EventStorageOptions": {
        "MaxArchiveBatchSaveSize": 100,
        "MaxBatchSaveSize": 100,
        "Schema": "KingMetal_Event"
    },
    "SnapshotStorageOptions": {
        "MaxArchiveBatchSaveSize": 100,
        "MaxBatchSaveSize": 100,
        "Schema": "KingMetal_Snapshot",
        "VersionInterval": 100
    },
    "CommandStorageOptions": {
        "MaxArchiveBatchSaveSize": 100,
        "MaxBatchSaveSize": 100,
        "Schema": "KingMetal_Command"
    },
    "ObserverStateStorageOptions": {
        "MaxBatchSaveSize": 100,
        "Schema": "KingMetal_Observer"
    },
    "UniqueValueStorageOptions": {
        "MaxBatchSaveSize": 100,
        "Name": "UniqueValue",
        "Partitions": 50,
        "Schema": "KingMetal_UniqueValue"
    },
    "UniqueIdStorageOptions": {
        "MaxBatchSaveSize": 100,
        "Partitions": 20,
        "Schema": "KingMetal_UniqueId"
    },
    "TransactionStorageOptions": {
        "MaxBatchSaveSize": 100,
        "Partitions": 10,
        "Schema": "KingMetal_Transaction"
    },
    "AuditingStorageOptions": {
        "DelaySubmit": 5,
        "MaxBatchSaveSize": 100,
        "RecordPartitions": 20,
        "Schema": "KingMetal_Auditing",
        "StatisticalPartitions": 20
    },
    "UniqueIdOptions": {
        "StartSerialNumber": 0
    },
    "KingMetalOptions": {
        "MaxReadEventSize": 2000,
        "DeactivateSnapshot": true,
        "PersistenceCommand": true,
        "SnapshotVersionInterval": 500,
        "MaxEventParallelism": 10,
        "ConsumersTypeFullNames": []
    },
    "KingMetalTransactionOptions": {
        "Retain": true,
        "LocalTimeout": 61000
    },
    "GrainAuditingOptions": {
        "Enable": false
    }
}

```
</details>

### Host配置

<details>

<summary>Gcp.YourApp.Host</summary>

```json
{
  "ClusterOptions": {
      "ClusterId": "GC_Cluster_AppName",
      "ServiceId": "GC_Service_AppName"
  },
  "DashboardOptions": {
      "Port": 33301
  },
  "EndpointOptions": {
      "SiloPort": 34401,
      "GatewayPort": 35501
  },
  "RabbitMQOptions": {
      "VirtualHost": "/goldcloud/appName",
      "ExchangeName": "zgjy.goldcloud.appName"
  },
  "AppDataBaseOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "AdoNetReminderTableOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "AuditingStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "CommandStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "EventStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "ObserverStateStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "SnapshotStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "TransactionStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "UniqueValueStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },
  "UniqueIdStorageOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  }
}
```

</details>

### Api配置

<details>

<summary> Gcp.YourApp.Api </summary>


```json
{
  "urls": "http://*:5001",
  
  "ClusterOptions": {
    "ClusterId": "GC_Cluster_AppName",
    "ServiceId": "GC_Service_AppName"
  },

  "AppDataBaseOptions": {
    "ConnectionString": "Server=127.0.0.1;Port=5432;Database=db_name_1;User Id=db_user_1;Password=YourPassword;Pooling=true;MaxPoolSize=100;"
  },

  "ApiResourceOptions": {
    "ResourceName": "your_api",
    "ResourceDisplayName": "Your Api Service",
    "AutoRegisterApiResource": true,
    "IdentityServerApiUrl": "https://www.yourApi.com"
  }
}
```

</details>