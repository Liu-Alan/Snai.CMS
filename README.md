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
    5. Razor里的代码块html标签跨了代码段时，视图会报错
    如：下面这种写法就会报错 
        @if(...){
            <li class='layui-nav-item'>
        }
            </li>
        正确写法
        @if(...){
            <li class='layui-nav-item'></li>
        }
        或
        @if(...){
            @Html.Raw("<li class='layui-nav-item'>")
        }
            @Html.Raw("</li>")
    6. 加基类控制器 ControllerBase : Controller，抽出GetLayoutModel()，通过泛型 T ToT<T>() 方法，得到子类页面Model，简化控制器取 LayoutModel Model值


### 菜单层级
    后台配置  
    -------管理员管理  
    ----------------管理员
    ----------------------添/删/改等管理员