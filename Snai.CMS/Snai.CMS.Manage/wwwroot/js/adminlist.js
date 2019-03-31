//当前页码
var curPage = 1;

layui.use(['table', 'layer'], function () {
    var table = layui.table;
    var layer = layui.layer;

    table.render({
        elem: '#adminList'
        , url: '/BackManage/AdminList/Data/' //数据接口
        , page: {
            layout: ['count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
        }
        , limit: 20 //每页显示条数
        , cols: [[ //表头
            { type: 'checkbox', fixed: true }
            , { field: 'id', title: 'ID', fixed: 'left', width: 60 }
            , { field: 'userName', title: '用户名', width: 120 }
            , { field: 'roleTitle', title: '角色', width: 120 }
            , { field: 'state', title: '账号状态', templet: '#stateTpl', width: 120, sort: true }
            , { field: 'lockState', title: '登录状态', templet: '#lockStateTpl', width: 120, sort: true }
            , { toolbar: '#adminBar', title: '操作', fixed: 'right', width: 80, align: 'center' } //这里的toolbar值是模板元素的选择器
        ]]
        , id: 'adminList'
        , toolbar: '#adminToolbar'
        , defaultToolbar: []
        , done: function (res, curr, count) {
            curPage = curr;
        }
    });

    //数据重载
    var dataReload = {
        reload: function () {
            table.reload('adminList', {
                where: {
                    userName: $('#userName').val()
                    , roleID: $('#roleID').val()
                }
                , page: {
                    curr: curPage //重载页数
                }
            });
        }
    };

    //头工具栏事件
    table.on('toolbar(adminList)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        if (obj.event != 'add') {
            if (checkStatus.data.length <= 0) {
                layer.msg('请选择要操作的账号', { icon: 2 });
                return;
            }
        }
        switch (obj.event) {
            case 'add':
                $.jump('/BackManage/ModifyAdmin');
                break;
            case 'enable':
                var data = checkStatus.data;
                var ids = [];
                for(var i=0;i<data.length;i++)
                {
                    ids.push(data[i].id);
                }
                //请求参数
                var params = {
                    ids: ids,
                    state: 1
                };

                var ajaxUrl = '/BackManage/UpdateAdminState';

                //发送请求
                $.ajax({
                    url: ajaxUrl,
                    type: "POST",
                    cache: false,
                    async: true,
                    dataType: "json",
                    traditional: true,
                    data: params,
                    success: function (data, textStatus) {
                        if (!data.success) {
                            layer.msg(data.msg, { icon: 2 });
                        } else {
                            layer.msg(data.msg, { icon: 1 });
                            dataReload.reload();
                        }
                    },
                    error: function (result, status) {
                        if (status == 'timeout') {
                            alert('很抱歉，由于服务器繁忙，请您稍后再试');
                        } else if (result.responseText != "") {
                            eval("exception = " + result.responseText);
                            alert(exception.Message);
                        }
                    }
                });
                break;
            case 'disable':
                var data = checkStatus.data;
                var ids = [];
                for (var i = 0; i < data.length; i++) {
                    ids.push(data[i].id);
                }
                //请求参数
                var params = {
                    ids: ids,
                    state: 2
                };

                var ajaxUrl = '/BackManage/UpdateAdminState';

                //发送请求
                $.ajax({
                    url: ajaxUrl,
                    type: "POST",
                    cache: false,
                    async: true,
                    dataType: "json",
                    traditional: true,
                    data: params,
                    success: function (data, textStatus) {
                        if (!data.success) {
                            layer.msg(data.msg, { icon: 2 });
                        } else {
                            layer.msg(data.msg, { icon: 1 });
                            dataReload.reload();
                        }
                    },
                    error: function (result, status) {
                        if (status == 'timeout') {
                            alert('很抱歉，由于服务器繁忙，请您稍后再试');
                        } else if (result.responseText != "") {
                            eval("exception = " + result.responseText);
                            alert(exception.Message);
                        }
                    }
                });
                break;
            case 'unlock':
                var data = checkStatus.data;
                var ids = [];
                for (var i = 0; i < data.length; i++) {
                    ids.push(data[i].id);
                }
                //请求参数
                var params = {
                    ids: ids
                };

                var ajaxUrl = '/BackManage/UnlockAdmin';

                //发送请求
                $.ajax({
                    url: ajaxUrl,
                    type: "POST",
                    cache: false,
                    async: true,
                    dataType: "json",
                    traditional: true,
                    data: params,
                    success: function (data, textStatus) {
                        if (!data.success) {
                            layer.msg(data.msg, { icon: 2 });
                        } else {
                            layer.msg(data.msg, { icon: 1 });
                            dataReload.reload();
                        }
                    },
                    error: function (result, status) {
                        if (status == 'timeout') {
                            alert('很抱歉，由于服务器繁忙，请您稍后再试');
                        } else if (result.responseText != "") {
                            eval("exception = " + result.responseText);
                            alert(exception.Message);
                        }
                    }
                });
                break;
            case 'delete':
                layer.confirm('您确定要删除选择的账号吗？', {
                    btn: ['是', '否'] //按钮
                    , icon: 3
                    , title: '提示'
                }, function () {
                    var data = checkStatus.data;
                    var ids = [];
                    for (var i = 0; i < data.length; i++) {
                        ids.push(data[i].id);
                    }
                    //请求参数
                    var params = {
                        ids: ids
                    };

                    var ajaxUrl = '/BackManage/DeleteAdmin';

                    //发送请求
                    $.ajax({
                        url: ajaxUrl,
                        type: "POST",
                        cache: false,
                        async: true,
                        dataType: "json",
                        traditional: true,
                        data: params,
                        success: function (data, textStatus) {
                            if (!data.success) {
                                layer.msg(data.msg, { icon: 2 });
                            } else {
                                layer.msg(data.msg, { icon: 1 });
                                dataReload.reload();
                            }
                        },
                        error: function (result, status) {
                            if (status == 'timeout') {
                                alert('很抱歉，由于服务器繁忙，请您稍后再试');
                            } else if (result.responseText != "") {
                                eval("exception = " + result.responseText);
                                alert(exception.Message);
                            }
                        }
                    });
                }, function () {
                    
                });
                
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

    //绑定重载事情
    $('#btnReload').bind("click", function () {
        curPage = 1;
        dataReload.reload();
    });
});