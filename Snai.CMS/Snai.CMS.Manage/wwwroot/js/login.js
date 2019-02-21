/**
 *	Login.js
 */

var Logon = {};

Logon.ErrorItem = {
    verifyCode: 0
};

Logon.Const = {
    accounts: {
        empty: "请输入您的游戏帐号",
        error: "您输入的游戏帐号不正确，请重新输入",
        defaultTips: "最近盗号猖獗，为了您的账号安全，请您先在最近一次登录的游戏大厅里设置保险箱密码！"
    },

    password: {
        empty: "请输入您的登录密码"
    },

    verifyCode: {
        empty: "请输入图片中的字符",
        error: "验证码输入错误",
        loading: "正在检查验证码,请稍候...",
        maxLen: 4
    },

    authCode: {
        empty: "请输入爱玩宝令上显示的动态码",
        error: "动态码输入错误",
        length: "请输入8位长度的动态码",
        loading: "正在检查动态码,请稍候...",
        maxLen: 8
    },
    SMSCode: {
        defaultTips: "最近盗号猖獗，为了您的账号安全，请验证您的密保手机！",
        empty: "请输入您收到的短信验证码",
        error: "短信验证码输入错误",
        mobileError: "请输入正确的密保手机",
        length: "请输入6位长度的动态码",
        loading: "正在检查短信验证码,请稍候...",
        maxLen: 6
    },

    InsurePass: {
        defaultTips: "最近盗号猖獗，为了您的账号安全，请验证您的保险箱密码！",
        empty: "请输入您的保险箱密码",
        error: "保险箱密码输入错误",
        loading: "正在验证保险箱密码,请稍候..."
    },

    ajaxErr: "很抱歉，由于服务器繁忙，请您稍后再试",

    url: {
        getservertime: "/handler/GetServerTime.cspx",
        dologon: "/handler/dologin.cspx",
        logonok: "/index.html",
        jumpurl: "",
        verifyCodeEquals: "/handler/validvcode.cspx",
        authCodeEquals: "/handler/validauthcode.cspx",
        doValidVerifyCode: "/handler/doValidSMSVerifyCode.cspx",
        sendVerifyCode: "/handler/SendSMSVerifyCode.cspx",

        doValidInsurePass: "/handler/doValidInsurePass.cspx"
    }
};

Logon.Utils = {
    urlEncodeBase64: function (str) {
        return Mo.Base64.encode(Mo.urlencode(str));
    }
};

Logon.Form = {
    accounts: null,
    password: null,
    autologin: null,
    button: null,
    gourl: null,

    divVerifyCode: null,
    verifyCode: null,
    picVerifyCode: null,
    btnVerifyCode: null,


    inti: function () {
        this.accounts = $("#accounts");
        this.password = $("#password");
        this.autologin = $("#autologin");
        this.button = $("#login");
        this.gourl = $("#gourl");

        this.divVerifyCode = $("#divVerifyCode")
        this.verifyCode = $("#verifyCode");
        this.picVerifyCode = $("#picVerifyCode");
        this.btnVerifyCode = $("#btnVerifyCode");
    },

    clear: function () {
        this.accounts.val("");
        this.password.val("");
    },

    focus: function (obj) {
        obj.select();
        obj.addClass("text_box_focus");
    },

    blur: function (obj) {
        obj.removeClass("text_box_focus");
        obj.removeClass("text_box_error");
    },

    error: function (obj) {
        obj.addClass("text_box_error");
    },

    icon: ["loginicon", "logincontext"],

    toArray: function (objs) {
        return new Array(objs[0], objs[1]);
    }
};

Logon.Accounts = {

    _cookieName: "fr_su",

    set: function () {
        var autologin = Logon.Form.autologin.attr("checked");
        var strAcc = Logon.Form.accounts.val();
        if (!this.check()) {
            if (autologin) {
                $.cookie(this._cookieName, strAcc, {
                    expires: 7
                });
            } else {
                $.cookie(this._cookieName, strAcc);
            }
        }
    },

    get: function () {
        var objAcc = $.cookie(this._cookieName);
        if (objAcc && !Mo.String(objAcc).isNullOrEmptyTrim()) {
            Logon.Form.accounts.val(objAcc);
            Logon.Form.autologin.attr("checked", "checked");
        }
    },

    clear: function () {
        var autologin = Logon.Form.autologin.attr("checked");
        if (!autologin) {
            Logon.Form.accounts.val("");
            $.cookie(this._cookieName, "", {
                expires: -1
            });
        }
    },

    check: function () {
        var strAcc = Logon.Form.accounts.val();
        return Mo.String(strAcc).isNullOrEmptyTrim();
    },

    onfocus: function () {
        Logon.Form.focus(Logon.Form.accounts);
    },
    onblur: function (e) {
        Logon.Form.blur(Logon.Form.accounts);
    },

    onkeydown: function () {
        if (this.check()) {
            Logon.Form.accounts.focus();
        } else {
            if (Logon.Password.check()) {
                Logon.Form.password.focus();
            }
            else {
                Logon.Form.button.trigger('click');
            }
        }
    },

    bind: function () {
        Logon.Form.accounts.bind("keydown", function (e) {
            $.enterSubmit(e, function () { Logon.Accounts.onkeydown(); });
        });

        Logon.Form.accounts.bind("focus", Logon.Accounts.onfocus);
        Logon.Form.accounts.bind("blur", Logon.Accounts.onblur);

        this.get();
        if (this.check()) {
            Logon.Form.accounts.focus();
        } else {
            Logon.Form.password.focus();
        }
    }
};

Logon.Password = {
    check: function () {
        var passwd = Logon.Form.password.val();
        return Mo.String(passwd).isNullOrEmptyTrim();
    },

    clear: function () {
        Logon.Form.password.val("");
        Logon.Form.password.focus();
    },

    onfocus: function () {
        Logon.Form.focus(Logon.Form.password);
    },
    onblur: function (e) {
        Logon.Form.blur(Logon.Form.password);
    },

    bind: function () {
        Logon.Form.password.bind("focus", Logon.Password.onfocus);
        Logon.Form.password.bind("blur", Logon.Password.onblur);
    }
};

Logon.VerifyCode = {
    isLogonClick: false,
    check: function () {
        if (Mo.Validator.EmptyTrim(Logon.Form.verifyCode.val())) {
            Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.verifyCode.empty);
            return false;
        }

        var len = Mo.String(Logon.Form.verifyCode.val()).byteLength();
        if (len != Logon.Const.verifyCode.maxLen) {
            Logon.Form.error(Logon.Form.verifyCode);
            Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.verifyCode.error);
            return false;
        }

        return true;
    },
    //刷新图片
    onrefresh: function () {
        var src = Logon.Form.picVerifyCode.attr("src");
        Logon.Form.picVerifyCode.attr("src", src + "&rnd=" + Math.random());
    },
    onfocus: function (e) {
        Logon.Form.focus(Logon.Form.verifyCode);
    },
    onblur: function (e) {
        Logon.Form.blur(Logon.Form.verifyCode);
        if (!Mo.Validator.EmptyTrim(Logon.Form.verifyCode.val())) {
            Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.loading, Logon.Const.verifyCode.loading);
            Mo.Function(Logon.VerifyCode.onquery).execute(200);
        } else {
            Logon.Form.verifyCode.val("");
            Logon.Form.error(Logon.Form.verifyCode);
            Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.verifyCode.empty);
        }
    },
    /*
	 * 是否需要验证码
	 */
    isVerifyCode: function () {
        return Logon.Form.divVerifyCode.is(":visible");
    },
    onquery: function (isAsync) {
        if (Logon.VerifyCode.isLogonClick && isAsync == undefined) {
            Logon.VerifyCode.isLogonClick = false;
            return;
        }
        var strCode = Logon.Form.verifyCode.val();
        if (Mo.Validator.EmptyTrim(strCode)) {
            Logon.Form.error(Logon.Form.verifyCode);
            Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.verifyCode.empty);
            Logon.ErrorItem.verifyCode = 0;
            return;
        }

        if (!Logon.VerifyCode.check()) {
            return;
        }

        if (isAsync == undefined) {
            isAsync = true;
        }

        $.ajax({
            async: isAsync,
            cache: false,
            type: "POST",
            dataType: "json",
            url: Logon.Const.url.verifyCodeEquals,
            data: {
                type: "Login",
                id: "",
                code: strCode
            },

            success: function (result) {
                if (result.Success) {
                    Logon.ErrorItem.verifyCode = 0;
                    Tips.hide(Logon.Form.toArray(Logon.Form.icon));
                } else {
                    if (result.Content == "抱歉，验证码错误，请刷新验证码！") {
                        Logon.VerifyCode.onrefresh();
                    }
                    Logon.ErrorItem.verifyCode = 1;
                    Logon.Form.error(Logon.Form.verifyCode);
                    Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.verifyCode.error);
                }
            },

            error: function (result, status) {
                if (status == 'timeout') {
                    Logon.Form.error(Logon.Form.verifyCode);
                    Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.ajaxErr);
                } else {
                    if (result.responseText != "") {
                        eval("exception = " + result.responseText);
                        alert(exception.Message);
                    }
                }
            }
        });
    },
    bind: function () {
        Logon.Form.verifyCode.bind("focus", Logon.VerifyCode.onfocus);
        Logon.Form.verifyCode.bind("blur", Logon.VerifyCode.onblur);
        Logon.Form.verifyCode.bind("keypress", function (e) {
            var event = e || window.event;
            if (event.keyCode == 13) {
                Logon.onsubmit();
            }
        });
    }
};

Logon.Button = {
    disable: function (obj) {
        obj.text("登录中...");
        $.disableButton(obj);
    },
    enable: function (obj) {
        obj.text("登 录");
        $.enableButton(obj);
    },
};

Logon.checkInput = function () {
    if (Logon.Accounts.check()) {
        //alert(Logon.Const.accounts.empty);
        Logon.Form.error(Logon.Form.accounts);
        Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.accounts.empty);
        Logon.Form.accounts.focus();
        return false;
    }

    if (Logon.Password.check()) {
        //alert(Logon.Const.password.empty);
        Logon.Form.error(Logon.Form.password);
        Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.password.empty);
        Logon.Form.password.focus();
        return false;
    }

    //验证码
    if (Logon.VerifyCode.isVerifyCode()) {
        if (!Logon.VerifyCode.check()) {
            Logon.Form.verifyCode.focus();

            if (Logon.ErrorItem.verifyCode == 1) {
                Logon.Form.verifyCode.unbind("focus");
                Logon.Form.error(Logon.Form.verifyCode);
                Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.verifyCode.error);
            }

            return false;
        }
        //同步再次验证
        Logon.VerifyCode.isLogonClick = true;
        Logon.VerifyCode.onquery(false);
        if (Logon.ErrorItem.verifyCode == 1) {
            Logon.Form.verifyCode.unbind("focus");
            Logon.Form.error(Logon.Form.verifyCode);
            Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, Logon.Const.verifyCode.error);
            Logon.VerifyCode.isLogonClick = false;
            return false;
        }
    }
    Logon.Accounts.set();

    return true;
};

// 手机安全令
Logon.AuthCode = {
    isAjaxing: false,
    submit: function (v, h, f) {
        var code = $.trim(h.find("#authcode").val());
        var msg = h.find("#authcodemsg");
        if (code.length == 0) {
            msg.text(Logon.Const.authCode.empty);
            return false;
        }
        if (code.length != Logon.Const.authCode.maxLen) {
            msg.text(Logon.Const.authCode.length);
            return false;
        }

        var _this = this, isSuccess = false;

        if (_this.isAjaxing) {
            msg.text(Logon.Const.ajaxErr);
            return false;
        }
        msg.text(Logon.Const.authCode.loading);
        _this.isAjaxing = true;
        $.ajax({
            async: false,
            cache: false,
            type: "POST",
            dataType: "json",
            url: Logon.Const.url.authCodeEquals,
            data: { code: code },
            success: function (result) {
                _this.isAjaxing = false;
                isSuccess = result.Success;
                if (result.Success) {
                    msg.text("");
                    $.jump(Logon.Const.url.jumpurl);
                } else {
                    msg.text(result.Content);
                }
            },

            error: function (result, status) {
                msg.text(Logon.Const.ajaxErr);
                _this.isAjaxing = false;
            }
        });

        return isSuccess;
    },
    show: function () {
        var option = {
            width: 350,
            height: 150,
            opacity: 0.5,
            title: "安全宝令动态码验证",
            showClose: true,
            submit: Logon.AuthCode.submit
        };
        var html = $("#codemodal").html();
        $.jBox(html, option);
    }
};

//保险箱密码
Logon.InsurePass = {
    icon: ["I1", "insurepassmsg"],
    lockTime: 0,
    loaded: false,
    isAjaxing: false,
    element: {
        btnDoValidInsurePass: $("#btnDoValidInsurePass"),
        insurepassmodal: $("#insurepassmodal"),
        insurepass: $("#insurepass")
    },
    tips: function (text, enumDate) {
        Tips.show(Logon.Form.toArray(this.icon), enumDate || Tips.enumDate.error, text);
    },
    show: function () {
        var _this = this;
        layer.open({
            type: 1, //page层
            area: ['430px', '310px'],
            skin: 'layui-layer-lan',
            offset: 'auto',
            title: '验证保险箱密码',
            shade: 0.1, //遮罩透明度
            moveType: 1, //拖拽风格，0是默认，1是传统拖动
            shift: 5, //0-6的动画形式，-1不开启
            content: _this.element.insurepassmodal,
            end: function () {
                Logon.Button.enable(Logon.Form.button);
            }
        });
        _this.element.insurepass.val("");
        _this.element.insurepass.focus();
        _this.tips(Logon.Const.InsurePass.defaultTips, Tips.enumDate.prompt);
        _this.lockTime = 0;
        if (!_this.loaded) {
            _this.loaded = true;
            _this.element.btnDoValidInsurePass.click(function () { _this.validInsurePass(); });
        }
    },
    validInsurePass: function () {
        var _this = this;
        var isSuccess = false;
        var pwd = $.trim(_this.element.insurepass.val());

        if (pwd.length == 0) {
            _this.tips(Logon.Const.InsurePass.empty);
            return false;
        }

        if (_this.isAjaxing) {
            _this.tips(Logon.Const.ajaxErr);
            return false;
        }
        _this.tips(Logon.Const.InsurePass.loading);
        _this.isAjaxing = true;
        $.ajax({
            async: false,
            cache: false,
            type: "POST",
            dataType: "json",
            url: Logon.Const.url.doValidInsurePass,
            data: { pwd: pwd },
            success: function (result) {
                _this.isAjaxing = false;
                isSuccess = result.Success;

                if (result.Success) {
                    _this.tips(result.Content, Tips.enumDate.prompt);
                    var backurl = Logon.Form.gourl.val();
                    backurl = Mo.String(backurl).isNullOrEmptyTrim() ? Logon.Const.url.logonok : backurl;
                    $.jump(backurl);
                }
                else {
                    _this.element.insurepass.focus();
                    _this.tips(result.Content);
                }
            },

            error: function (result, status) {
                _this.tips(Logon.Const.ajaxErr);
                _this.isAjaxing = false;
            }
        });

        return isSuccess;
    }
};

//禁止在网页登录
Logon.DisableLogon = {
    icon: ["I1", "disablelogonmsg"],
    element: {
        disablelogonmodal: $("#disablelogonmodal")
    },
    tips: function (text, enumDate) {
        Tips.show(Logon.Form.toArray(this.icon), enumDate || Tips.enumDate.error, text);
    },

    show: function () {
        var _this = this;
        layer.open({
            type: 1, //page层
            area: ['430px', '240px'],
            skin: 'layui-layer-lan',
            offset: 'auto',
            title: '登录提示',
            shade: 0.1, //遮罩透明度
            moveType: 1, //拖拽风格，0是默认，1是传统拖动
            shift: 5, //0-6的动画形式，-1不开启
            content: _this.element.disablelogonmodal,
            end: function () {
                Logon.Button.enable(Logon.Form.button);
            }
        });

        _this.tips(Logon.Const.accounts.defaultTips, Tips.enumDate.prompt);
    },
}

// 验证密保手机
Logon.SMSCode = {
    icon: ["I1", "sMSVerifyCodemsg"],
    lockTime: 0,
    loaded: false,
    element: {
        btnSendSMSCode: $("#btnSendSMSCode"),
        btnDoValidVerifyCode: $("#btnDoValidVerifyCode"),
        sMSVerifyCode: $("#sMSVerifyCode"),
        sMSCode: $("#sMSCode"),
        tipsProtectMobile: $("#tipsProtectMobile"),
        txtProtectMobile: $("#txtProtectMobile"),
        bind: $("#bind")
    },
    tips: function (text, enumDate) {
        Tips.show(Logon.Form.toArray(this.icon), enumDate || Tips.enumDate.error, text);
    },
    isAjaxing: false,
    validVerifyCode: function () {
        var _this = this;
        var isSuccess = false;
        var code = $.trim(_this.element.sMSCode.val());
        var mobile = ""// _this.element.txtProtectMobile.val();
        var bind = $.trim(_this.element.bind.val());
        /*
        if (Mo.String(mobile).isNullOrEmptyTrim() || !Mo.Validator.Mobile(mobile)) {
            _this.tips(Logon.Const.SMSCode.mobileError);
            return false;
        }
        */
        if (code.length == 0) {
            _this.tips(Logon.Const.SMSCode.empty);
            return false;
        }
        if (code.length != Logon.Const.SMSCode.maxLen) {
            _this.tips(Logon.Const.SMSCode.length);
            return false;
        }

        if (_this.isAjaxing) {
            _this.tips(Logon.Const.ajaxErr);
            return false;
        }
        _this.tips(Logon.Const.SMSCode.loading);
        _this.isAjaxing = true;
        $.ajax({
            async: false,
            cache: false,
            type: "POST",
            dataType: "json",
            url: Logon.Const.url.doValidVerifyCode,
            data: { code: code, bind: bind, mobile: mobile },
            success: function (result) {
                _this.isAjaxing = false;
                isSuccess = result.Success;

                if (result.Success) {
                    _this.tips(result.Content, Tips.enumDate.prompt);
                    var backurl = Logon.Form.gourl.val();
                    backurl = Mo.String(backurl).isNullOrEmptyTrim() ? Logon.Const.url.logonok : backurl;
                    $.jump(backurl);
                }
                else {
                    _this.tips(result.Content);
                    _this.element.sMSCode.focus();
                }
            },

            error: function (result, status) {
                _this.tips(Logon.Const.ajaxErr);
                _this.isAjaxing = false;
            }
        });

        return isSuccess;
    },
    sendVerifyCode: function () {
        var _this = this;
        var mobile = "";// _this.element.txtProtectMobile.val();
        /*
        if (Mo.String(mobile).isNullOrEmptyTrim() || !Mo.Validator.Mobile(mobile)) {
            _this.tips(Logon.Const.SMSCode.mobileError);
            return false;
        }
        */
        $.ajax({
            async: false,
            cache: false,
            type: "POST",
            dataType: "json",
            url: Logon.Const.url.sendVerifyCode,
            data: { mobile: mobile },
            success: function (result) {
                _this.isAjaxing = false;
                if (result.Success) {
                    layer.msg(result.Content);
                    _this.lockTime = 180;
                    _this.element.btnSendSMSCode.attr("disabled", "disabled");
                    var inter = window.setInterval(function () {
                        _this.lockTime = _this.lockTime - 1;
                        if (_this.lockTime <= 0) {
                            _this.element.btnSendSMSCode.removeAttr("disabled");
                            window.clearInterval(inter);
                            _this.element.btnSendSMSCode.text("获取验证码");
                        } else {
                            _this.element.btnSendSMSCode.text("等待" + _this.lockTime + "秒");
                        }
                    }, 1000);

                } else {
                    _this.tips(result.Content);
                }
            },
            error: function (result, status) {
                _this.tips(Logon.Const.ajaxErr);
                _this.isAjaxing = false;
            }
        });
    },
    show: function (mobile) {
        var _this = this;
        layer.open({
            type: 1, //page层
            area: ['430px', '310px'],
            skin: 'layui-layer-lan',
            offset: 'auto',
            title: '短信验证码',
            shade: 0.1, //遮罩透明度
            moveType: 1, //拖拽风格，0是默认，1是传统拖动
            shift: 5, //0-6的动画形式，-1不开启
            content: _this.element.sMSVerifyCode,
            end: function () {
                Logon.Button.enable(Logon.Form.button);
            }
        });
        _this.element.tipsProtectMobile.text(mobile);
        _this.element.sMSCode.val("");
        _this.element.sMSCode.focus();
        // _this.element.txtProtectMobile.val("");
        _this.tips(Logon.Const.SMSCode.defaultTips, Tips.enumDate.prompt);
        _this.lockTime = 0;
        if (!_this.loaded) {
            _this.loaded = true;
            _this.element.btnSendSMSCode.click(function () { _this.sendVerifyCode() });
            _this.element.btnDoValidVerifyCode.click(function () { _this.validVerifyCode(); });
        }
    },
};

Logon.onsubmit = function () {
    if (!Logon.checkInput()) {
        return false;
    }

    Logon.Button.disable(Logon.Form.button);

    var entry = null;
    $.ajax({
        url: Logon.Const.url.getservertime,
        type: "POST",
        cache: false,
        async: false,
        dataType: "json",
        data: {
            su: Logon.Utils.urlEncodeBase64(Logon.Form.accounts.val())
        },
        success: function (data, textStatus) {
            entry = data;
        },
        error: function () {
            alert(Logon.Const.ajaxErr);
        }
    });

    if (entry == null) {
        alert(Logon.Const.ajaxErr);
        Logon.Button.enable(Logon.Form.button);
        return false;
    }

    //请求参数
    var params = {
        retid: entry.retid,
        nonce: entry.nonce,
        servertime: entry.servertime,
        pcid: entry.pcid,

        su: Logon.Utils.urlEncodeBase64(Logon.Form.accounts.val()),
        sp: "",
        vd: ""
    };

    //加密密码
    var rsa = new RSAKey();
    rsa.setPublic(entry.pubKey, "10001");
    params.sp = rsa.encrypt(params.nonce + params.servertime + "\t" + Logon.Form.password.val());

    //取验证码
    if (Logon.VerifyCode.isVerifyCode()) {
        params.vd = Logon.Form.verifyCode.val();
    }

    var ajaxUrl = Logon.Const.url.dologon;
    //去向地址
    if (Logon.Form.gourl.val() != "") {
        ajaxUrl = ajaxUrl + "?gourl=" + Logon.Form.gourl.val();
    }

    //发送请求
    $.ajax({
        url: ajaxUrl,
        type: "POST",
        cache: false,
        async: true,
        dataType: "json",
        data: params,
        success: function (data, textStatus) {

            if (!data.Success) {
                Logon.Button.enable(Logon.Form.button);
                if (data.MessageID == 90) {
                    JBoxShowContent(Mo.format("抱歉！ {0} 您是{1}用户，请到{1}！", data.Content, data.EntityBag.Model.StationName)
                        , {
                            title: "站点转接", link: [[data.EntityBag.Model.StationDomain, Mo.format("去{0}", data.EntityBag.Model.StationName), "btn btn-link"]]
                            , stationid: data.EntityBag.Model.StationID
                        });
                } else {
                    //alert(data.Content);
                    Logon.Password.clear();
                    Tips.show(Logon.Form.toArray(Logon.Form.icon), Tips.enumDate.error, data.Content);

                    //显示验证码
                    if (data.Content == "抱歉，您输入的验证码错误了！" || data.Content == "抱歉，请输入验证码！") {
                        var code = data.EntityBag.Model
                        Logon.Form.verifyCode.attr("name", code.InputName);
                        if (code.DisableIme) {
                            Logon.Form.verifyCode.attr("style", "ime-mode:disabled;");
                        }
                        Logon.Form.error(Logon.Form.verifyCode);
                        Logon.Form.picVerifyCode.attr("src", code.ImageUrl);
                        Logon.Form.picVerifyCode.attr("alt", code.Tip);
                        Logon.Form.divVerifyCode.show();
                    }
                }
            } else {
                //Logon.Form.clear();
                Logon.Accounts.clear();
                Logon.Const.url.jumpurl = Mo.String(data.Content).isNullOrEmptyTrim() ? Logon.Const.url.logonok : data.Content;

                /*
                // 发送短信验证码
                if (data.EntityBag.IsSendSMS) {
                    Logon.SMSCode.show(data.EntityBag.Mobile);
                    return;
                }
                // 需要验证动态码
                if (data.EntityBag.BindMobileAuth) {
                    Logon.AuthCode.show();
                    return;
                }
                */

                //验证密保手机
                if (data.EntityBag.SecondValidType == 1) {
                    Logon.SMSCode.show(data.EntityBag.Mobile);
                    return;
                }

                //验证保险箱密码
                if (data.EntityBag.SecondValidType == 2) {
                    Logon.InsurePass.show();
                    return;
                }

                //提示不能在网站登录
                if (data.EntityBag.SecondValidType == -1) {
                    Logon.DisableLogon.show();
                    return;
                }

                $.jump(Logon.Const.url.jumpurl);
            }
        },
        error: function (result, status) {
            Logon.Button.enable(Logon.Form.button);
            Logon.VerifyCode.isLogonClick = false;

            if (status == 'timeout') {
                alert(Logon.Const.ajaxErr);
            } else if (result.responseText != "") {
                eval("exception = " + result.responseText);
                alert(exception.Message);
            }
        }
    });
};

Logon.bind = function () {
    layui.use('form', function () {
        var layuiForm = layui.form;
    });

    layui.use('layer', function () {
        var layer = layui.layer;
    });              

    Logon.Form.inti();
    Logon.Accounts.bind();
    Logon.Password.bind();
    Logon.VerifyCode.bind();

    Logon.Form.button.bind("click", function () {
        return Logon.onsubmit();
    });
    //$(document).bind('keydown', 'return', function () {
    //    Logon.Form.button.trigger('click');
    //});

    $("body").keydown(function (event) {
        var e = event || window.event; //兼容ie
        //keyCode=13是回车键 且 按钮没有聚焦
        if (e.keyCode == "13" && !window.buttonIsFocused) {

            var target = e.target || e.srcElment;//srcElment针对IE

            if (target.id == 'insurepass' || target.id == 'btnDoValidInsurePass') {
                Logon.InsurePass.element.btnDoValidInsurePass.click();
            }
            else if (target.id == 'sMSCode' || target.id == 'btnDoValidVerifyCode') {
                Logon.SMSCode.element.btnDoValidVerifyCode.click();

            } else {
                Logon.Form.button.click();
            }
        }
    });
};

/*
 *  窗体事件
 */
$(document).ready(function (e) {
    Logon.bind();
});
