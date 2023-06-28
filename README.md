# BaGet
Nuget服务器

修改配置文件src\BaGet\appsettings.json
ApiKey为推送包使用的Api Key，默认是123456
使用sqlite作为数据库，文件路径：/data/baget/data/baget.db
包存储路径：/data/baget/data/packages/

docker部署，挂载/data/baget/data目录即可
docker build -t baget:latest .
docker run -d -p 8085:80 -p 8086:443 --name baget -v /data/baget/data:/data/baget/data baget:latest
