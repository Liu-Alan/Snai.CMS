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
        this.id = $("#roleID");
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
        MRR.Form.title.val("");
        MRR.Form.title.focus();
    },

    onfocus: function () {
        MRR.Form.focus(MRR.Form.title);
    },
    onblur: function (e) {
        MRR.Form.blur(MRR.Form.title);
    },

    onkeydown: function () {
        if (this.check()) {
            MRR.Form.title.focus();
        }
    },

    bind: function () {
        MRR.Form.title.bind("focus", MRR.Title.onfocus);
        MRR.Form.title.bind("blur", MRR.Title.onblur);

        MRR.Form.title.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MRR.Title.onkeydown(); });
        });
    },

    select: function (value) {
        var parentID1 = -1;
        parentID1 = $("select[name='parentID'] option[value=" + value + "]").attr("data-parentID");
        if (parentID1 == 0) {
            MRR.Form.controller.attr("disabled", true);
            MRR.Form.action.attr("disabled", true);
        } else {
            MRR.Form.controller.attr("disabled", false);
            MRR.Form.action.attr("disabled", false);
        }
    }
};

MRR.State = {
    clear: function () {
        MRR.Form.state1.attr("checked", true);
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
    if (MRR.Title.check()) {
        MRR.Form.error(MRR.Form.title);
        MRR.layui.layer.msg(MRR.Const.title.empty, { icon: 2 });
        MRR.Form.title.focus();
        return false;
    }

    var sortCheck = MRR.Sort.check();
    if (sortCheck == 1) {
        MRR.Form.error(MRR.Form.sort);
        MRR.layui.layer.msg(MRR.Const.sort.empty, { icon: 2 });
        MRR.Form.sort.focus();
        return false;
    }
    else if (sortCheck == 2) {
        MRR.Form.error(MRR.Form.sort);
        MRR.layui.layer.msg(MRR.Const.sort.error, { icon: 2 });
        MRR.Form.sort.focus();
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
        id: MRR.Form.id.val(),
        title: MRR.Form.title.val(),
        parentID: MRR.Form.parentID.val(),
        controller: MRR.Form.controller.val(),
        action: MRR.Form.action.val(),
        sort: MRR.Form.sort.val(),
        state: $("input[name='state']:checked").val()
    };

    var ajaxUrl = MRR.Const.url.doModifyModule;

    //发送请求
    $.ajax({
        url: ajaxUrl,
        type: "POST",
        cache: false,
        async: true,
        dataType: "json",
        data: params,
        success: function (data, textStatus) {
            if (!data.success) {
                MRR.BtnSubmit.enable(MRR.Form.btnSubmit);
                MRR.layui.layer.msg(data.msg, { icon: 2 });
            } else {
                MRR.layui.layer.msg(data.msg, { icon: 1 }, function () {
                    $.jump(MRR.Const.url.doModuleList);
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

        MRR.layui.form.on('radio(state)', function (data) {
            if (data.value == 1) {
                MRR.Form.state1.attr("checked", true);
                MRR.Form.state2.attr("checked", false);
            } else {
                MRR.Form.state1.attr("checked", false);
                MRR.Form.state2.attr("checked", true);
            }
        });

        MRR.layui.form.on('select(parentID)', function (data) {
            MRR.ParentID.select(data.value);
        });
    });

    MRR.Form.inti();
    MRR.Title.bind();
    MRR.Controller.bind();
    MRR.Action.bind();
    MRR.Sort.bind();
    MRR.BtnSubmit.bind();
    MRR.ParentID.select(MRR.Form.parentID.val());
};

/*
 *  窗体事件
 */
$(document).ready(function (e) {
    MRR.bind();
});
