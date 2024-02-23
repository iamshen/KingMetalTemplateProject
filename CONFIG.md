# 项目配置说明

## **Consul集群管理配置部分说明**

集群管理的 `Consul` 的存储的 `Key` 全部统一配置，`Orleans` 会自动新建子目录来管理没一个节点的信息。

例如: `Golden/Clusters/{Environment}`，其中 `{Environment}` 为占位符，运行时会替换为当前的运行环境名称，例如 `Development` 等，配置如下：

```json
{
  "ConsulClusteringSiloOptions": {
    "AclClientToken": "Your Acl Client Token...",
    "Address": "http://127.0.0.1:8500",
    "KvRootFolder": "Golden/Clusters/{Environment}"
  }
}

```

```json
{
  "ConsulClusteringClientOptions": {
    "AclClientToken": "Your Acl Client Token...",
    "KvRootFolder": "Golden/Clusters/{Environment}",
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

`Golden/Configs/{Environment}` 为全部配置的根目录

### 2. 公用配置

`Golden/Configs/{Environment}/Common`  全局公用配置根目录
`Golden/Configs/{Environment}/Common/Public` 全局公用配置
`Golden/Configs/{Environment}/Common/Api` 全局 Api 的公用配置
`Golden/Configs/{Environment}/Common/Host` 全局 Host 的公用配置

### 3. Api & Host 专用配置

`Golden/Configs/{Environment}/Host/{ApplicationName}`
`Golden/Configs/{Environment}/Api/{ApplicationName}`
 
`{ApplicationName}` 是程序名称的占位符，运行运行时会替换为真实的程序名称。

## Api 的 `appsettings.json` 配置如下

```json
{
  "RemoteConfigOptions": {
    "Enable": true,
    "AppId": "Golden/Configs/{Environment}",
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
    "AppId": "Golden/Configs/{Environment}",
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
            "Default": "Warning",
            "Override": {
                "Microsoft": "Information",
                "Microsoft.Hosting.Lifetime": "Information",
                "Microsoft.AspNetCore.Hosting.Diagnostics": "Warning",
                "Orleans": "Warning",
                "Orleans.ClientOptionsLogger": "Warning",
                "Orleans.Runtime.SiloLifecycleSubject": "Warning",
                "Orleans.Runtime.Catalog": "Warning",
                "Orleans.Runtime.SiloOptionsLogger": "Warning"
            }
        },
        "NLogProvider": {
            "NLogNative": {
                "Enable": true,
                "throwConfigExceptions": false,
                "extensions": [
                    {
                        "assembly": "NLog.Targets.Seq"
                    },
                    {
                        "assembly": "NLog.Extensions.Logging"
                    },
                    {
                        "assembly": "NLog.Web.AspNetCore"
                    }
                ],
                "variables": {
                    "log-directory": "${basedir}/logs/${shortdate}"
                },
                "targets": {
                    "async": true,
                    "all-file": {
                        "type": "File",
                        "fileName": "${log-directory}/${shortdate}.json",
                        "layout": {
                            "type": "JsonLayout",
                            "Attributes": [
                                {
                                    "name": "timestamp",
                                    "layout": "${date:format=o}"
                                },
                                {
                                    "name": "level",
                                    "layout": "${level}"
                                },
                                {
                                    "name": "logger",
                                    "layout": "${logger}"
                                },
                                {
                                    "name": "message",
                                    "layout": "${message:raw=true}"
                                },
                                {
                                    "name": "properties",
                                    "encode": false,
                                    "layout": {
                                        "type": "JsonLayout",
                                        "includeallproperties": "true"
                                    }
                                }
                            ]
                        }
                    },
                    "own-console": {
                        "type": "LimitingWrapper",
                        "interval": "00:00:01",
                        "target": {
                            "type": "ColoredConsole",
                            "layout": "${longdate}|${level:uppercase=true}|${message}|${callsite}|${all-event-properties}",
                            "rowHighlightingRules": [
                                {
                                    "condition": "level == LogLevel.Error",
                                    "foregroundColor": "Red"
                                },
                                {
                                    "condition": "level == LogLevel.Fatal",
                                    "foregroundColor": "Red",
                                    "backgroundColor": "White"
                                }
                            ],
                            "wordHighlightingRules": [
                                {
                                    "regex": "on|off",
                                    "foregroundColor": "DarkGreen"
                                },
                                {
                                    "condition": "level == LogLevel.Debug",
                                    "text": "[TEST]",
                                    "foregroundColor": "Blue"
                                }
                            ]
                        }
                    },
                    "seq": {
                        "type": "BufferingWrapper",
                        "bufferSize": 200,
                        "flushTimeout": 2000,
                        "slidingTimeout": false,
                        "target": {
                            "type": "Seq",
                            "serverUrl": "http://192.168.8.112:5341",
                            "apiKey": "3Vh3ymLCD7Lv7ExuXIq8",
                            "properties": [
                                {
                                    "name": "Source",
                                    "value": "${Logger}"
                                },
                                {
                                    "name": "ThreadId",
                                    "value": "${ThreadId}",
                                    "as": "number"
                                },
                                {
                                    "name": "MachineName",
                                    "value": "${MachineName}"
                                },
                                {
                                    "name": "requestParams",
                                    "layout": "${aspnet-request:queryString}"
                                },
                                {
                                    "name": "requestFormParams",
                                    "layout": "${aspnet-request-form}"
                                },
                                {
                                    "name": "ApplicationName",
                                    "value": "${var:ApplicationName}"
                                }
                            ]
                        }
                    }
                },
                "rules": [
                    {
                        "logger": "*",
                        "minLevel": "Info",
                        "writeTo": "seq"
                    },
                    {
                        "logger": "*",
                        "minLevel": "Info",
                        "writeTo": "own-console",
                        "filterDefaultAction": "Log",
                        "filters": {
                            "whenRepeated": {
                                "layout": "${message}",
                                "action": "Ignore"
                            }
                        }
                    },
                    {
                        "logger": "*",
                        "minLevel": "Error",
                        "writeTo": "all-file"
                    }
                ]
            }
        }
    },

  "AllowedHosts": "*",
    "ClusterOptions": {
        "ClusterId": "Gp_Cluster",
        "ServiceId": "Gp_Service"
    },
    "SmsTemplateOptions": {
        // 发送短信验证码模板
        "ValidateCode": "SMS_464345042",
        // 付款成功通知模板
        "SmsPayment": "SMS_464370092",
        // 熔炼完成通知
        "SmsSmelted": "SMS_463629248",
        // 支付尾款通知
        "SmsBalance": "SMS_463664180",
        // 客户提货通知
        "SmsDelivery": "SMS_463629250"
    },
 
    "MiniprogramOptions": {
        "AppId": "wxe3ceed38cec88e52",
        "AppSecret": "79c3df4fa5bbf08c6cb0580358362ad6",
        "ProgramState": "formal",
        // 订阅消息模板ID1 -回收订单状态变更通知
        "TemplateId1": "efXNtebMePAjpI9HIT30shxT-2et0gCst6G_i0adUbY",
        // 订阅消息模板ID1的跳转页面
        "TemplateId1Page": "/pages/order/info/info?id={0}&orderState=3",
        // 订阅消息模板ID1的参数
        "TemplateId1Dic": {
            // 订阅消息参数: 订单编号
            "OrderNoKey": "character_string1",
            // 订阅消息参数: 订单状态
            "OrderStatusKey": "phrase3",
            // 订阅消息参数: 下单时间
            "CreatedTimeKey": "time12",
            // 订阅消息参数: 温馨提示
            "RemarkKey": "thing8"
        },
        // 订阅消息模板ID2 -回收订单黄金检测完成通知
        "TemplateId2": "9YNAdy4Ux1Bnm6s9nnO-7IU-9MOr5lUAFSbU9_M5Tsk",
        // 订阅消息模板ID2的跳转页面
        "TemplateId2Page": "/pages/order/info/info?id={0}&orderState=3",
        // 订阅消息模板ID1的参数
        "TemplateId2Dic": {
            // 订阅消息参数: 订单编号
            "OrderNoKey": "character_string2",
            // 订阅消息参数: 检测机构
            "ReportOrgKey": "thing5",
            // 订阅消息参数: 检测报告
            "ReportMsgKey": "thing6",
            // 订阅消息参数: 温馨提示 
            "RemarkKey": "thing7"
        },
        // 小程序注册Url
        "RegisterPage": "https://reverse.dunhuanggold.com/?mp=mz&de=mp&fn=cn&ut=2&da={0}"
    },

    "RemoteApiConfigOptions": {
        "BaseUrl": "https://develop.api.com",
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
    "KvRootFolder": "Golden/Clusters/{Environment}",
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
        "KvRootFolder": "Golden/Clusters/{Environment}"
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
      "VirtualHost": "/Golden/appName",
      "ExchangeName": "kingmeta.golden.appName"
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