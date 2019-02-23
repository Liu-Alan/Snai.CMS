# Snai.CMS
## 内容管理后台

### 开发踩过的坑
    1. 注册HttpContext，用于在Controller之外的地方使用  
    services.AddHttpContextAccessor();  
    2. appsettings.json 中文乱码，如配置文件里有中文，保存时默认GB2312格式，改为UTF-8