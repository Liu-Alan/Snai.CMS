var MA = {};

MA.Const = {
    userName: {
        empty: "请输入用户名"
    },

    password: {
        empty: "请输入密码",
        error: "密码至少6位，且必须是字母与(数字或特殊符号)组合"
    },

    rePassword: {
        empty: "请重复输入密码",
        error: "重复密码与密码不一样"
    },

    roleID: {
        empty: "请选择角色"
    },

    ajaxErr: "很抱歉，由于服务器繁忙，请您稍后再试",

    url: {
        doModifyAdmin: "/BackManage/ModifyAdmin",
        doAdminList: "/BackManage/AdminList"
    }
};

MA.Form = {
    id: null,
    userName: null,
    password: null,
    rePassword: null,
    roleID: null,
    state1: null,
    state2: null,
    btnSubmit: null,

    inti: function () {
        this.id = $("#id");
        this.userName = $("#userName");
        this.password = $("#password");
        this.rePassword = $("#rePassword");
        this.roleID = $("#roleID");
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

MA.layui = {
    form: null,
    layer: null
};

MA.UserName = {
    check: function () {
        var strUserName = MA.Form.userName.val();
        return Utils.String(strUserName).isNullOrEmptyTrim();
    },

    clear: function () {
        MA.Form.userName.val("");
        MA.Form.userName.focus();
    },

    onfocus: function () {
        MA.Form.focus(MA.Form.userName);
    },
    onblur: function (e) {
        MA.Form.blur(MA.Form.userName);
    },

    onkeydown: function () {
        if (this.check()) {
            MA.Form.userName.focus();
        } else {
            MA.Form.password.focus();
        }
    },

    bind: function () {
        MA.Form.userName.bind("focus", MA.UserName.onfocus);
        MA.Form.userName.bind("blur", MA.UserName.onblur);

        MA.Form.userName.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MA.UserName.onkeydown(); });
        });
    }
};

MA.Password = {
    check: function () {
        var password = MA.Form.password.val();
        if (Utils.String(password).isNullOrEmptyTrim()) {
            return 4;
        }

        var verifyValue = Utils.Validator.Password(password);

        return verifyValue;
    },

    clear: function () {
        MA.Form.password.val("");
        MA.Form.password.focus();
    },

    onfocus: function () {
        MA.Form.focus(MA.Form.password);
    },
    onblur: function (e) {
        MA.Form.blur(MA.Form.password);
    },

    onkeydown: function () {
        var passwordCheck = MA.Password.check();
        if ((MA.Form.id.val() <= 0 && passwordCheck > 0) || (MA.Form.id.val() >0 && passwordCheck > 0 && passwordCheck != 4)) {
            MA.Form.password.focus();
        } else {
            MA.Form.rePassword.focus();
        }
    },

    bind: function () {
        MA.Form.password.bind("focus", MA.Password.onfocus);
        MA.Form.password.bind("blur", MA.Password.onblur);
        MA.Form.password.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MA.Password.onkeydown(); });
        });
    }
};

MA.RePassword = {
    check: function () {
        var rePassword = MA.Form.rePassword.val();
        if (Utils.String(rePassword).isNullOrEmptyTrim()) {
            return 1;
        }

        var password = MA.Form.password.val();
        if (password != rePassword) {
            return 2;
        }

        return 0;
    },

    equals: function () {
        var rePassword = MA.Form.rePassword.val();
        var password = MA.Form.password.val();
        return password == rePassword;
    },

    clear: function () {
        MA.Form.rePassword.val("");
        MA.Form.rePassword.focus();
    },

    onfocus: function (e) {
        MA.Form.focus(MA.Form.rePassword);
    },
    onblur: function (e) {
        MA.Form.blur(MA.Form.rePassword);
    },

    onkeydown: function () {
        var idValue = MA.Form.id.val();
        var rePasswordCheck = MA.RePassword.check();
        var pwdEquals = MA.RePassword.equals();
        if ((idValue <= 0 && rePasswordCheck > 0)
            || (idValue > 0 && !pwdEquals)){
            MA.Form.rePassword.focus();
        } 
    },

    bind: function () {
        MA.Form.rePassword.bind("focus", MA.RePassword.onfocus);
        MA.Form.rePassword.bind("blur", MA.RePassword.onblur);
        MA.Form.rePassword.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MA.RePassword.onkeydown(); });
        });
    }
};

MA.RoleID = {
    check: function () {
        var roleID = MA.Form.roleID.val();

        return roleID <= 0 ? true : false;
    },

    clear: function () {
        MA.Form.roleID.val(0);
    }
};

MA.State = {
    clear: function () {
        MA.Form.state1.attr("checked", true);
    }
};

MA.BtnSubmit = {
    disable: function (obj) {
        $.disableButton(obj);
    },
    enable: function (obj) {
        $.enableButton(obj);
    },

    bind: function () {
        MA.Form.btnSubmit.bind("click", function () {
            return MA.onsubmit();
        });

        MA.Form.btnSubmit.bind("keypress", function (e) {
            var event = e || window.event;
            if (event.keyCode == 13) {
                MA.onsubmit();
            }
        });
    }
};

MA.checkInput = function () {
    if (MA.UserName.check()) {
        MA.Form.error(MA.Form.userName);
        MA.layui.layer.msg(MA.Const.userName.empty, { icon: 2 });
        MA.Form.userName.focus();
        return false;
    }

    var idValue = MA.Form.id.val();
    var passwordCheck = MA.Password.check();
    if ((idValue <= 0 && passwordCheck == 4)) {
        MA.Form.error(MA.Form.password);
        MA.layui.layer.msg(MA.Const.password.empty, { icon: 2 });
        MA.Form.password.focus();
        return false;
    }
    else if ((idValue <= 0 && passwordCheck > 0) || (idValue > 0 && passwordCheck > 0 && passwordCheck != 4)) {
        MA.Form.error(MA.Form.password);
        MA.layui.layer.msg(MA.Const.password.error, { icon: 2 });
        MA.Form.password.focus();
        return false;
    }

    var rePasswordCheck = MA.RePassword.check();
    var pwdEquals = MA.RePassword.equals();
    if ((idValue <= 0 && rePasswordCheck == 1)
        || (idValue > 0 && passwordCheck > 0 && passwordCheck != 4 && rePasswordCheck == 1)) {
        MA.Form.error(MA.Form.rePassword);
        MA.layui.layer.msg(MA.Const.rePassword.empty, { icon: 2 });
        MA.Form.rePassword.focus();
        return false;
    }
    else if (!pwdEquals) {
        MA.Form.error(MA.Form.rePassword);
        MA.layui.layer.msg(MA.Const.rePassword.error, { icon: 2 });
        MA.Form.rePassword.focus();
        return false;
    }

    if (MA.RoleID.check()) {
        MA.Form.error(MA.Form.roleID);
        MA.layui.layer.msg(MA.Const.roleID.empty, { icon: 2 });
        return false;
    }

    return true;
};

MA.onsubmit = function () {
    if (!MA.checkInput()) {
        return false;
    }

    MA.BtnSubmit.disable(MA.Form.btnSubmit);

    //请求参数
    var params = {
        id: MA.Form.id.val(),
        userName: MA.Form.userName.val(),
        password: MA.Form.password.val(),
        rePassword: MA.Form.rePassword.val(),
        roleID: MA.Form.roleID.val(),
        state: $("input[name='state']:checked").val()
    };

    var ajaxUrl = MA.Const.url.doModifyAdmin;

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
                MA.BtnSubmit.enable(MA.Form.btnSubmit);
                MA.Password.clear();
                MA.RePassword.clear();
                MA.layui.layer.msg(data.msg, { icon: 2 });
            } else {
                MA.layui.layer.msg(data.msg, { icon: 1 }, function () {
                    $.jump(MA.Const.url.doAdminList);
                });
                
            }
        },
        error: function (result, status) {
            MA.BtnSubmit.enable(MA.Form.btnSubmit);

            if (status == 'timeout') {
                alert(MA.Const.ajaxErr);
            } else if (result.responseText != "") {
                eval("exception = " + result.responseText);
                alert(exception.Message);
            }
        }
    });
};

MA.bind = function () {
    layui.use(['form', 'layer'], function () {
        MA.layui.form = layui.form;
        MA.layui.layer = layui.layer;

        MA.layui.form.on('radio(state)', function (data) {
            if (data.value == 1) {
                MA.Form.state1.attr("checked", true);
                MA.Form.state2.attr("checked", false);
            } else {
                MA.Form.state1.attr("checked", false);
                MA.Form.state2.attr("checked", true);
            }
        });
    });

    MA.Form.inti();
    MA.UserName.bind();
    MA.Password.bind();
    MA.RePassword.bind();
    MA.BtnSubmit.bind();
};

/*
 *  窗体事件
 */
$(document).ready(function (e) {
    MA.bind();
});
