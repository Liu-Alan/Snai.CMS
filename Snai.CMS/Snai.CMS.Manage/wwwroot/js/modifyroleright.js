var MRR = {};

MRR.Const = {
    moduleIDs: {
        empty: "请选择需要的权限"
    },

    ajaxErr: "很抱歉，由于服务器繁忙，请您稍后再试",

    url: {
        doModifyRoleRight: "/BackManage/ModifyRoleRight",
        doRoleList: "/BackManage/RoleList"
    }
};

MRR.Form = {
    roleID: null,
    btnSubmit: null,

    inti: function () {
        this.roleID = $("#roleID");
        this.btnSubmit = $("#btnSubmit");
    },

    focus: function (obj) {
        obj.select();
    },

    blur: function (obj) {
    },

    error: function (obj) {
        obj.addClass("layui-form-danger");
    }
};

MRR.layui = {
    form: null,
    layer: null
};

MRR.ModuleIDs = {
    check: function () {
        var strModuleIDs = $("input[name='moduleIDs']:checked").val();
        if (strModuleIDs == 'undefined' || strModuleIDs == '' || strModuleIDs.length <= 0) {
            return true;
        } else {
            return false;
        }
    },

    clear: function () {
        $("input[name='moduleIDs']").each(function () {
            $(this).attr("checked", false);
        }); 
    }
};

MRR.BtnSubmit = {
    disable: function (obj) {
        $.disableButton(obj);
    },
    enable: function (obj) {
        $.enableButton(obj);
    },

    bind: function () {
        MRR.Form.btnSubmit.bind("click", function () {
            return MRR.onsubmit();
        });

        MRR.Form.btnSubmit.bind("keypress", function (e) {
            var event = e || window.event;
            if (event.keyCode == 13) {
                MRR.onsubmit();
            }
        });
    }
};

MRR.checkInput = function () {
    if (MRR.ModuleIDs.check()) {
        MRR.layui.layer.msg(MRR.Const.moduleIDs.empty, { icon: 2 });
        return false;
    }

    return true;
};

MRR.onsubmit = function () {
    if (!MRR.checkInput()) {
        return false;
    }

    MRR.BtnSubmit.disable(MRR.Form.btnSubmit);

    //请求参数
    var params = {
        roleID: MRR.Form.roleID.val(),
        moduleIDs: $("input[name='moduleIDs']:checked").val()
    };

    var ajaxUrl = MRR.Const.url.doModifyRoleRight;

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
                MRR.BtnSubmit.enable(MRR.Form.btnSubmit);
                MRR.layui.layer.msg(data.msg, { icon: 2 });
            } else {
                MRR.layui.layer.msg(data.msg, { icon: 1 }, function () {
                    $.jump(MRR.Const.url.doRoleList);
                });

            }
        },
        error: function (result, status) {
            MRR.BtnSubmit.enable(MRR.Form.btnSubmit);

            if (status == 'timeout') {
                alert(MRR.Const.ajaxErr);
            } else if (result.responseText != "") {
                eval("exception = " + result.responseText);
                alert(exception.Message);
            }
        }
    });
};

MRR.bind = function () {
    layui.use(['form', 'layer'], function () {
        MRR.layui.form = layui.form;
        MRR.layui.layer = layui.layer;

        MRR.layui.form.on('checkbox(moduleIDs)', function (data) {
            if (data.elem.checked) {
                $("input[name='moduleIDs'][value=" + data.value + "]").attr("checked", true);
                //往上
                var parentID1 = $("input[name='moduleIDs'][value=" + data.value + "]").attr("data-parentID");
                if (parentID1 != 0) {
                    $("input[name='moduleIDs'][value=" + parentID1 + "]").attr("checked", true);
                    var parentID2 = $("input[name='moduleIDs'][value=" + parentID1 + "]").attr("data-parentID");
                    if (parentID2 != 0) {
                        $("input[name='moduleIDs'][value=" + parentID2 + "]").attr("checked", true);
                        var parentID3 = $("input[name='moduleIDs'][value=" + parentID2 + "]").attr("data-parentID");
                    }
                    if (parentID3 != 0) {
                        $("input[name='moduleIDs'][value=" + parentID3 + "]").attr("checked", true);
                    }
                }
                //往下
                $("input[name='moduleIDs'][data-parentID=" + data.value + "]").each(function () {
                    $(this).attr("checked", true);
                    $("input[name='moduleIDs'][data-parentID=" + $(this).val() + "]").each(function () {
                        $(this).attr("checked", true);
                        $("input[name='moduleIDs'][data-parentID=" + $(this).val() + "]").each(function () {
                            $(this).attr("checked", true);
                        });
                    });
                }); 
            } else {
                $("input[name='moduleIDs'][value=" + data.value + "]").attr("checked", false);
                //往下
                $("input[name='moduleIDs'][data-parentID=" + data.value + "]").each(function () {
                    $(this).attr("checked", false);
                    $("input[name='moduleIDs'][data-parentID=" + $(this).val() + "]").each(function () {
                        $(this).attr("checked", false);
                        $("input[name='moduleIDs'][data-parentID=" + $(this).val() + "]").each(function () {
                            $(this).attr("checked", false);
                        });
                    });
                });
            }

            MRR.layui.form.render('checkbox');
        });
    });

    MRR.Form.inti();
    MRR.BtnSubmit.bind();
};

/*
 *  窗体事件
 */
$(document).ready(function (e) {
    MRR.bind();
});
