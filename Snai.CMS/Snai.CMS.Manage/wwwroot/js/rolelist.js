//当前页码
var curPage = 1;

layui.use(['table', 'form', 'layer'], function () {
    var table = layui.table;
    var form = layui.form;
    var layer = layui.layer;

    table.render({
        elem: '#roleList'
        , url: '/BackManage/RoleList/Data/' //数据接口
        , page: {
            layout: ['count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
        }
        , limit: 20 //每页显示条数
        , cols: [[ //表头
            { type: 'checkbox', fixed: true }
            , { field: 'id', title: 'ID', fixed: 'left', width: 60 }
            , { field: 'title', title: '角色名', width: 130 }
            , { field: 'state', title: '状态', templet: '#stateTpl', width: 120, sort: true }
            , { toolbar: '#roleBar', title: '操作', fixed: 'right', width: 150, align: 'center' } //这里的toolbar值是模板元素的选择器
        ]]
        , id: 'roleList'
        , toolbar: '#roleToolbar'
        , defaultToolbar: []
        , done: function (res, curr, count) {
            curPage = curr;
        }
    });

    //数据重载
    var dataReload = {
        reload: function () {
            table.reload('roleList', {
                where: {
                    title: $('#title').val()
                }
                , page: {
                    curr: curPage //重载页数
                }
            });
        }
    };

    //头工具栏事件
    table.on('toolbar(roleList)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        if (obj.event != 'add') {
            if (checkStatus.data.length <= 0) {
                layer.msg('请选择要操作的角色', { icon: 2 });
                return;
            }
        }
        switch (obj.event) {
            case 'add':
                $.jump('/BackManage/ModifyRole');
                break;
            case 'enable':
                var data = checkStatus.data;
                var ids = [];
                for (var i = 0; i < data.length; i++) {
                    ids.push(data[i].id);
                }
                //请求参数
                var params = {
                    ids: ids,
                    state: 1
                };

                var ajaxUrl = '/BackManage/UpdateRoleState';

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

                var ajaxUrl = '/BackManage/UpdateRoleState';

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
                layer.confirm('您确定要删除选择的角色吗？', {
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

                    var ajaxUrl = '/BackManage/DeleteRole';

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
    table.on('tool(roleList)', function (obj) {
        var data = obj.data;
        if (obj.event === 'edit') {
            $.jump('/BackManage/ModifyRole?id=' + data.id);
        } else if (obj.event === 'editright') {
            $.jump('/BackManage/ModifyRoleRight?id=' + data.id);
        }
    });

    //绑定重载事情
    $('#btnReload').bind("click", function () {
        curPage = 1;
        dataReload.reload();
    });
});