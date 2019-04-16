# Snai.CMS
## 内容管理后台

### 目前进度（已完成）
    总后台大概分为 登录、修改密码，账号管理，菜单管理，角色权限管理
    ... 完成 登录、修改密码
    2019-4-6 完成 账号管理（账号列表，添加，删除，修改，禁/启用，解锁）
    2019-4-9 完成 菜单管理（菜单列表，添加，删除，修改，禁/启用）
    2019-4-17 完成 角色权限管理（角色列表，添加，删除，修改，禁/启用，分配权限）

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
    6. 加基类控制器 ControllerBase : Controller，抽出GetLayoutModel()，再通过泛型 ToT<T>(ref T t) 方法，得到子类页面Model，  
        简化控制器取 LayoutModel Model值
    7. layui重新渲染后的单选按钮，选择后直接用 $("input[name='state']:checked").val() 是取不值的（用layui的表单取值不确定是否  
        能取到）后面用layui监听事件，监听按钮选择修改原单选按钮选中状态，然后再用 $("input[name='state']:checked").val() 取值
        MA.layui.form.on('radio(state)', function (data) {
            if (data.value == 1) {
                MA.Form.state1.attr("checked", true);
                MA.Form.state2.attr("checked", false);
            } else {
                MA.Form.state1.attr("checked", false);
                MA.Form.state2.attr("checked", true);
            }
        });
    8. 对于checkbox提交，用 jquery 组合成数组提交
        var moduleIDs =[];   
        $("input[name='moduleIDs']:checked").each(function(){   
            moduleIDs.push($(this).val());   
        });

        //请求参数
        var params = {
            roleID: MRR.Form.roleID.val(),
            moduleIDs: moduleIDs 
        };
    9. 对于分配权限时checkbox选择与联动选择，也用的是layui监听事件MRR.layui.form.on('checkbox(moduleIDs)', function (data) {}); 来设置原checkbox的值  
        对于联动后重新渲染checkbox用MRR.layui.form.render('checkbox')，要注意用prop而不用attr，$(this).prop("checked", false)，否则已经做过选择复选框联动无效
### 菜单层级
    后台配置  
    -------管理员管理  
    ----------------账号
    ----------------------添/删/改等账号
