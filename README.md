# BaGet 
一个轻量级Nuget服务器  


## docker部署

### 修改配置文件
目录：src\BaGet\appsettings.json  
ApiKey：推送包使用的Api Key，默认是123456  
AllowPackageOverwrites：是否允许替换现有包，true表示允许替换  
使用sqlite作为数据库，文件路径：/data/baget/data/baget.db  
包存储路径：/data/baget/data/packages/  
http协议时,会使用v2版本推送，Legacy需要设置为true

```json
{
  "ApiKey": "123456",
  "PackageDeletionBehavior": "Unlist",
  "AllowPackageOverwrites": true,
  "Database": {
    "Type": "Sqlite",
    "ConnectionString": "Data Source=/data/baget/data/baget.db"
  },
  "Storage": {
    "Type": "FileSystem",
    "Path": "/data/baget/data/packages/"
  },
  "Search": {
    "Type": "Database"
  },
  "Mirror": {
    "Enabled": false,
    //开启v2版本推送，通常http协议时会使用
    "Legacy": true,
    "PackageSource": "https://api.nuget.org/v3/index.json"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

挂载目录：/data/baget/data  
```command
docker build -t baget:latest .
docker run -d -p 8085:8080 -p 8086:443 --name baget -v /data/baget/data:/data/baget/data baget:latest
```
