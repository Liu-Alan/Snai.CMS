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

    state: {
        empty: "请选择状态"
    },

    ajaxErr: "很抱歉，由于服务器繁忙，请您稍后再试",

    url: {
        doMA: "/BackManage/ModifyAdmin"
    }
};

MA.Form = {
    id: null,
    userName: null,
    password: null,
    rePassword: null,
    roleID: null,
    state: null,
    btnSubmit: null,

    inti: function () {
        this.id = $("#id");
        this.userName = $("#userName");
        this.password = $("#password");
        this.rePassword = $("#rePassword");
        this.roleID = $("#roleID");
        this.state = $("#state");
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
        if (passwordCheck > 0) {
            MA.Form.password.focus();
        } else {
            var rePasswordCheck = MA.RePassword.check();
            if (rePasswordCheck > 0) {
                MA.Form.rePassword.focus();
            }
            else {
                MA.Form.btnSubmit.trigger('click');
            }
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
        var rePasswordCheck = MA.RePassword.check();
        if (rePasswordCheck > 0) {
            MA.Form.rePassword.focus();
        } else {
            MA.Form.btnSubmit.trigger('click');
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
    if (MA.OldPassword.check()) {
        MA.Form.error(MA.Form.oldPassword);
        MA.layui.layer.msg(MA.Const.oldPassword.empty, { icon: 2 });
        MA.Form.oldPassword.focus();
        return false;
    }

    var passwordCheck = MA.Password.check();
    if (passwordCheck == 4) {
        MA.Form.error(MA.Form.password);
        MA.layui.layer.msg(MA.Const.password.empty, { icon: 2 });
        MA.Form.password.focus();
        return false;
    }
    else if (passwordCheck > 0) {
        MA.Form.error(MA.Form.password);
        MA.layui.layer.msg(MA.Const.password.error, { icon: 2 });
        MA.Form.password.focus();
        return false;
    }

    var rePasswordCheck = MA.RePassword.check();
    if (rePasswordCheck == 1) {
        MA.Form.error(MA.Form.rePassword);
        MA.layui.layer.msg(MA.Const.rePassword.empty, { icon: 2 });
        MA.Form.rePassword.focus();
        return false;
    }
    if (rePasswordCheck == 2) {
        MA.Form.error(MA.Form.rePassword);
        MA.layui.layer.msg(MA.Const.rePassword.error, { icon: 2 });
        MA.Form.rePassword.focus();
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
        oldPassword: MA.Form.oldPassword.val(),
        password: MA.Form.password.val(),
        rePassword: MA.Form.rePassword.val()
    };

    var ajaxUrl = MA.Const.url.doMA;

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
                MA.RePassword.clear();
                MA.Password.clear();
                MA.OldPassword.clear();
                MA.layui.layer.msg(data.msg, { icon: 2 });
            } else {
                MA.BtnSubmit.enable(MA.Form.btnSubmit);
                MA.RePassword.clear();
                MA.Password.clear();
                MA.OldPassword.clear();
                MA.layui.layer.msg(data.msg, { icon: 1 });
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
    layui.use('form', function () {
        MA.layui.form = layui.form;
    });

    layui.use('layer', function () {
        MA.layui.layer = layui.layer;
    });

    MA.Form.inti();
    MA.OldPassword.bind();
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
