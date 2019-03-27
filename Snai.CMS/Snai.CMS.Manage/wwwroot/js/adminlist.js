layui.use('table', function () {
    var table = layui.table;

    table.render({
        elem: '#adminList'
        , url: '/BackManage/AdminList/Data/' //数据接口
        , page: {
            layout: ['prev', 'page', 'next', 'skip', 'count'] //自定义分页布局
        }
        , limit: 20 //每页显示条数
        , cols: [[ //表头
            { type: 'checkbox', fixed: true }
            , { field: 'id', title: 'ID', fixed: 'left', width: 60 }
            , { field: 'userName', title: '用户名', width: 120 }
            , { field: 'roleTitle', title: '角色', width: 120 }
            , { field: 'state', title: '账号状态', width: 120, sort: true }
            , { field: 'lockDes', title: '登录状态', width: 120, sort: true }
            , { toolbar: '#adminBar', title: '操作', fixed: 'right', width: 80, align: 'center' } //这里的toolbar值是模板元素的选择器
        ]]
        , toolbar: '#adminToolbar'
        , defaultToolbar: []
    });

    //头工具栏事件
    table.on('toolbar(adminList)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'add':
                $.jump('/BackManage/ModifyAdmin');
                break;
            case 'enable':
                var data = checkStatus.data;
                layer.msg('选中了：' + data.length + ' 个');
                break;
            case 'disable':
                layer.msg(checkStatus.isAll ? '全选' : '未全选');
                break;
            case 'unlock':
                layer.msg(checkStatus.isAll ? '全选' : '未全选');
                break;
            case 'delete':
                layer.msg(checkStatus.isAll ? '全选' : '未全选');
                break;
        };
    });

    //监听行工具事件
    table.on('tool(adminList)', function (obj) {
        var data = obj.data;
        if (obj.event === 'edit') {
            $.jump('/BackManage/ModifyAdmin?id=' + data.id);
        } 
    });

});