var MR = {};

MR.Const = {
    title: {
        empty: "请输入角色名"
    },

    ajaxErr: "很抱歉，由于服务器繁忙，请您稍后再试",

    url: {
        doModifyRole: "/BackManage/ModifyRole",
        doRoleList: "/BackManage/RoleList"
    }
};

MR.Form = {
    id: null,
    title: null,
    state1: null,
    state2: null,
    btnSubmit: null,

    inti: function () {
        this.id = $("#id");
        this.title = $("#title");
        this.state1 = $("input[name='state'][value=1]");
        this.state2 = $("input[name='state'][value=2]");
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

MR.layui = {
    form: null,
    layer: null
};

MR.Title = {
    check: function () {
        var strTitle = MR.Form.title.val();
        return Utils.String(strTitle).isNullOrEmptyTrim();
    },

    clear: function () {
        MR.Form.title.val("");
        MR.Form.title.focus();
    },

    onfocus: function () {
        MR.Form.focus(MR.Form.title);
    },
    onblur: function (e) {
        MR.Form.blur(MR.Form.title);
    },

    onkeydown: function () {
        if (this.check()) {
            MR.Form.title.focus();
        }
    },

    bind: function () {
        MR.Form.title.bind("focus", MR.Title.onfocus);
        MR.Form.title.bind("blur", MR.Title.onblur);

        MR.Form.title.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MR.Title.onkeydown(); });
        });
    }
};

MR.State = {
    clear: function () {
        MR.Form.state1.attr("checked", true);
    }
};

MR.BtnSubmit = {
    disable: function (obj) {
        $.disableButton(obj);
    },
    enable: function (obj) {
        $.enableButton(obj);
    },

    bind: function () {
        MR.Form.btnSubmit.bind("click", function () {
            return MR.onsubmit();
        });

        MR.Form.btnSubmit.bind("keypress", function (e) {
            var event = e || window.event;
            if (event.keyCode == 13) {
                MR.onsubmit();
            }
        });
    }
};

MR.checkInput = function () {
    if (MR.Title.check()) {
        MR.Form.error(MR.Form.title);
        MR.layui.layer.msg(MR.Const.title.empty, { icon: 2 });
        MR.Form.title.focus();
        return false;
    }

    return true;
};

MR.onsubmit = function () {
    if (!MR.checkInput()) {
        return false;
    }

    MR.BtnSubmit.disable(MR.Form.btnSubmit);

    //请求参数
    var params = {
        id: MR.Form.id.val(),
        title: MR.Form.title.val(),
        state: $("input[name='state']:checked").val()
    };

    var ajaxUrl = MR.Const.url.doModifyRole;

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
                MR.BtnSubmit.enable(MR.Form.btnSubmit);
                MR.layui.layer.msg(data.msg, { icon: 2 });
            } else {
                MR.layui.layer.msg(data.msg, { icon: 1 }, function () {
                    $.jump(MR.Const.url.doRoleList);
                });

            }
        },
        error: function (result, status) {
            MR.BtnSubmit.enable(MR.Form.btnSubmit);

            if (status == 'timeout') {
                alert(MR.Const.ajaxErr);
            } else if (result.responseText != "") {
                eval("exception = " + result.responseText);
                alert(exception.Message);
            }
        }
    });
};

MR.bind = function () {
    layui.use(['form', 'layer'], function () {
        MR.layui.form = layui.form;
        MR.layui.layer = layui.layer;

        MR.layui.form.on('radio(state)', function (data) {
            if (data.value == 1) {
                MR.Form.state1.attr("checked", true);
                MR.Form.state2.attr("checked", false);
            } else {
                MR.Form.state1.attr("checked", false);
                MR.Form.state2.attr("checked", true);
            }
        });
    });

    MR.Form.inti();
    MR.Title.bind();
    MR.BtnSubmit.bind();
};

/*
 *  窗体事件
 */
$(document).ready(function (e) {
    MR.bind();
});
