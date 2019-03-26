layui.use('table', function () {
    var table = layui.table;

    table.render({
        elem: '#adminlist'
        , height: 312
        , url: '/BackManage/AdminList/Data/' //数据接口
        , page: true //开启分页
        , cols: [[ //表头
            { field: 'id', title: 'ID', width: 80, fixed: 'left' }
            , { field: 'username', title: '用户名', width: 120 }
            , { field: 'roletitle', title: '角色', width: 120 }
            , { field: 'state', title: '账号状态', width: 120, sort: true }
            , { field: 'lockdes', title: '登录状态', width: 120, sort: true }
        ]]
    });

});