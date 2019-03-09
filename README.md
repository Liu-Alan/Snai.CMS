# Snai.CMS
## 内容管理后台

### 开发踩过的坑
    1. 注册HttpContext，用于在Controller之外的地方使用  
    services.AddHttpContextAccessor();  
    2. appsettings.json 中文乱码，如配置文件里有中文，保存时默认GB2312格式，改为UTF-8  
    3. View 文件不编译 <MvcRazorCompileOnPublish>false</MvcRazorCompileOnPublish>  
    4. View 使用Model  
    @{
        ViewData.Model = new NoUserRoleModel()
        {
            PageTitle = "没有权限访问",
            WebTitle = "CMS管理后台"
        };
    }


### 菜单层级
    后台配置  
    -------管理员管理  
    ----------------管理员
    ----------------------添/删/改等管理员