var UpPwd = {};

UpPwd.Const = {
    oldPassword: {
        empty: "请输入旧密码"
    },

    password: {
        empty: "请输入新密码",
        error: "新密码至少6位，且必须是字母与(数字或特殊符号)组合"
    },

    rePassword: {
        empty: "请重复输入新密码",
        error: "重复新密码与新密码不一样"
    },

    ajaxErr: "很抱歉，由于服务器繁忙，请您稍后再试",

    url: {
        doUpPwd: "/Home/DoUpdatePassword"
    }
};

UpPwd.Form = {
    oldPassword: null,
    password: null,
    rePassword: null,
    btnSubmit: null,

    inti: function () {
        this.oldPassword = $("#oldPassword");
        this.password = $("#password");
        this.rePassword = $("#rePassword");
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

UpPwd.layui = {
    form: null,
    layer: null
};

UpPwd.OldPassword = {
    check: function () {
        var strOldPassword = UpPwd.Form.oldPassword.val();
        return Utils.String(strOldPassword).isNullOrEmptyTrim();
    },

    clear: function () {
        UpPwd.Form.oldPassword.val("");
        UpPwd.Form.oldPassword.focus();
    },

    onfocus: function () {
        UpPwd.Form.focus(UpPwd.Form.oldPassword);
    },
    onblur: function (e) {
        UpPwd.Form.blur(UpPwd.Form.oldPassword);
    },

    onkeydown: function () {
        if (this.check()) {
            UpPwd.Form.oldPassword.focus();
        } else {
            var passwordCheck = UpPwd.Password.check();
            if (passwordCheck > 0) {
                UpPwd.Form.password.focus();
            }
            else {
                var rePasswordCheck = UpPwd.RePassword.check();
                if (rePasswordCheck > 0) {
                    UpPwd.Form.rePassword.focus();
                }
                else {
                    UpPwd.Form.btnSubmit.trigger('click');
                }
            }
        }
    },

    bind: function () {
        UpPwd.Form.oldPassword.bind("focus", UpPwd.OldPassword.onfocus);
        UpPwd.Form.oldPassword.bind("blur", UpPwd.OldPassword.onblur);

        UpPwd.Form.oldPassword.bind("keydown", function (e) {
            $.enterSubmit(e, function () { UpPwd.OldPassword.onkeydown(); });
        });

        if (this.check()) {
            UpPwd.Form.oldPassword.focus();
        }
        else {
            UpPwd.Form.password.focus();
        }
    }
};

UpPwd.Password = {
    check: function () {
        var password = UpPwd.Form.password.val();
        if (Utils.String(password).isNullOrEmptyTrim()) {
            return 4;
        }

        var verifyValue = Utils.Validator.Password(password);

        return verifyValue;
    },

    clear: function () {
        UpPwd.Form.password.val("");
        UpPwd.Form.password.focus();
    },

    onfocus: function () {
        UpPwd.Form.focus(UpPwd.Form.password);
    },
    onblur: function (e) {
        UpPwd.Form.blur(UpPwd.Form.password);
    },

    onkeydown: function () {
        var passwordCheck = UpPwd.Password.check();
        if (passwordCheck>0) {
            UpPwd.Form.password.focus();
        } else {
            var rePasswordCheck = UpPwd.RePassword.check();
            if (rePasswordCheck > 0) {
                UpPwd.Form.rePassword.focus();
            }
            else {
                UpPwd.Form.btnSubmit.trigger('click');
            }
        }
    },

    bind: function () {
        UpPwd.Form.password.bind("focus", UpPwd.Password.onfocus);
        UpPwd.Form.password.bind("blur", UpPwd.Password.onblur);
        UpPwd.Form.password.bind("keydown", function (e) {
            $.enterSubmit(e, function () { UpPwd.Password.onkeydown(); });
        });
    }
};

UpPwd.RePassword = {
    check: function () {
        var rePassword = UpPwd.Form.rePassword.val();
        if (Utils.String(rePassword).isNullOrEmptyTrim()) {
            return 1;
        }

        var password = UpPwd.Form.password.val();
        if (password != rePassword) {
            return 2;
        }

        return 0;
    },

    clear: function () {
        UpPwd.Form.rePassword.val("");
        UpPwd.Form.rePassword.focus();
    },

    onfocus: function (e) {
        UpPwd.Form.focus(UpPwd.Form.rePassword);
    },
    onblur: function (e) {
        UpPwd.Form.blur(UpPwd.Form.rePassword);
    },

    onkeydown: function () {
        var rePasswordCheck = UpPwd.RePassword.check();
        if (rePasswordCheck > 0) {
            UpPwd.Form.rePassword.focus();
        } else {
            UpPwd.Form.btnSubmit.trigger('click');
        }
    },

    bind: function () {
        UpPwd.Form.rePassword.bind("focus", UpPwd.RePassword.onfocus);
        UpPwd.Form.rePassword.bind("blur", UpPwd.RePassword.onblur);
        UpPwd.Form.rePassword.bind("keydown", function (e) {
            $.enterSubmit(e, function () { UpPwd.RePassword.onkeydown(); });
        });
    }
};

UpPwd.BtnSubmit = {
    disable: function (obj) {
        $.disableButton(obj);
    },
    enable: function (obj) {
        $.enableButton(obj);
    },

    bind: function () {
        UpPwd.Form.btnSubmit.bind("click", function () {
            return UpPwd.onsubmit();
        });

        UpPwd.Form.btnSubmit.bind("keypress", function (e) {
            var event = e || window.event;
            if (event.keyCode == 13) {
                UpPwd.onsubmit();
            }
        });
    }
};

UpPwd.checkInput = function () {
    if (UpPwd.OldPassword.check()) {
        UpPwd.Form.error(UpPwd.Form.oldPassword);
        UpPwd.layui.layer.msg(UpPwd.Const.oldPassword.empty, { icon: 2 });
        UpPwd.Form.oldPassword.focus();
        return false;
    }

    var passwordCheck = UpPwd.Password.check();
    if (passwordCheck == 4) {
        UpPwd.Form.error(UpPwd.Form.password);
        UpPwd.layui.layer.msg(UpPwd.Const.password.empty, { icon: 2 });
        UpPwd.Form.password.focus();
        return false;
    }
    else if (passwordCheck > 0) {
        UpPwd.Form.error(UpPwd.Form.password);
        UpPwd.layui.layer.msg(UpPwd.Const.password.error, { icon: 2 });
        UpPwd.Form.password.focus();
        return false;
    }

    var rePasswordCheck = UpPwd.RePassword.check();
    if (rePasswordCheck == 1) {
        UpPwd.Form.error(UpPwd.Form.rePassword);
        UpPwd.layui.layer.msg(UpPwd.Const.rePassword.empty, { icon: 2 });
        UpPwd.Form.rePassword.focus();
        return false;
    }
    if (rePasswordCheck == 2) {
        UpPwd.Form.error(UpPwd.Form.rePassword);
        UpPwd.layui.layer.msg(UpPwd.Const.rePassword.error, { icon: 2 });
        UpPwd.Form.rePassword.focus();
        return false;
    }

    return true;
};

UpPwd.onsubmit = function () {
    if (!UpPwd.checkInput()) {
        return false;
    }

    UpPwd.BtnSubmit.disable(UpPwd.Form.btnSubmit);

    //请求参数
    var params = {
        oldPassword: UpPwd.Form.oldPassword.val(),
        password: UpPwd.Form.password.val(),
        rePassword: UpPwd.Form.rePassword.val()
    };

    var ajaxUrl = UpPwd.Const.url.doUpPwd;

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
                UpPwd.BtnSubmit.enable(UpPwd.Form.btnSubmit);
                UpPwd.RePassword.clear();
                UpPwd.Password.clear();
                UpPwd.OldPassword.clear();
                UpPwd.layui.layer.msg(data.msg, { icon: 2 });
            } else {
                UpPwd.BtnSubmit.enable(UpPwd.Form.btnSubmit);
                UpPwd.RePassword.clear();
                UpPwd.Password.clear();
                UpPwd.OldPassword.clear();
                UpPwd.layui.layer.msg(data.msg, { icon: 1 });
            }
        },
        error: function (result, status) {
            UpPwd.BtnSubmit.enable(UpPwd.Form.btnSubmit);

            if (status == 'timeout') {
                alert(UpPwd.Const.ajaxErr);
            } else if (result.responseText != "") {
                eval("exception = " + result.responseText);
                alert(exception.Message);
            }
        }
    });
};

UpPwd.bind = function () {
    layui.use('form', function () {
        UpPwd.layui.form = layui.form;
    });

    layui.use('layer', function () {
        UpPwd.layui.layer = layui.layer;
    });

    UpPwd.Form.inti();
    UpPwd.OldPassword.bind();
    UpPwd.Password.bind();
    UpPwd.RePassword.bind();
    UpPwd.BtnSubmit.bind();
};

/*
 *  窗体事件
 */
$(document).ready(function (e) {
    UpPwd.bind();
});
