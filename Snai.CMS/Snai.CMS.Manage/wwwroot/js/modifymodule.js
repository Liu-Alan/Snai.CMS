var MM = {};

MM.Const = {
    title: {
        empty: "请输入菜单名"
    },

    sort: {
        empty: "请输入排序值",
        error: "排序值必需为数字"
    },

    ajaxErr: "很抱歉，由于服务器繁忙，请您稍后再试",

    url: {
        doModifyModule: "/BackManage/ModifyModule",
        doModuleList: "/BackManage/ModuleList"
    }
};

MM.Form = {
    id: null,
    title: null,
    parentID: null,
    controller: null,
    action: null,
    sort: null,
    state1: null,
    state2: null,
    btnSubmit: null,

    inti: function () {
        this.id = $("#id");
        this.title = $("#title");
        this.parentID = $("#parentID");
        this.controller = $("#controller");
        this.action = $("#action");
        this.sort = $("#sort");
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

MM.layui = {
    form: null,
    layer: null
};

MM.Title = {
    check: function () {
        var strTitle = MM.Form.title.val();
        return Utils.String(strTitle).isNullOrEmptyTrim();
    },

    clear: function () {
        MM.Form.title.val("");
        MM.Form.title.focus();
    },

    onfocus: function () {
        MM.Form.focus(MM.Form.title);
    },
    onblur: function (e) {
        MM.Form.blur(MM.Form.title);
    },

    onkeydown: function () {
        if (this.check()) {
            MM.Form.title.focus();
        }
    },

    bind: function () {
        MM.Form.title.bind("focus", MM.Title.onfocus);
        MM.Form.title.bind("blur", MM.Title.onblur);

        MM.Form.title.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MM.Title.onkeydown(); });
        });
    }
};

MM.ParentID = {
    clear: function () {
        MM.Form.parentID.val(0);
    },
    select: function (value) {
        var parentID1 = -1;
        parentID1 = $("select[name='parentID'] option[value=" + value + "]").attr("data-parentID");
        if (parentID1 == 0) {
            MM.Form.controller.attr("disabled", true);
            MM.Form.action.attr("disabled", true);
            //MM.layui.form.render('select');
        }else{
            MM.Form.controller.attr("disabled", false);
            MM.Form.action.attr("disabled", false);
            //MM.layui.form.render('select');
        }
    }
};

MM.Controller = {
    clear: function () {
        MM.Form.controller.val("");
        MM.Form.controller.focus();
    },

    onfocus: function () {
        MM.Form.focus(MM.Form.controller);
    },
    onblur: function (e) {
        MM.Form.blur(MM.Form.controller);
    },

    onkeydown: function () {
        MM.Form.action.focus();
    },

    bind: function () {
        MM.Form.controller.bind("focus", MM.Controller.onfocus);
        MM.Form.controller.bind("blur", MM.Controller.onblur);
        MM.Form.controller.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MM.Controller.onkeydown(); });
        });
    }
};

MM.Action = {
    clear: function () {
        MM.Form.action.val("");
        MM.Form.action.focus();
    },

    onfocus: function (e) {
        MM.Form.focus(MM.Form.action);
    },
    onblur: function (e) {
        MM.Form.blur(MM.Form.action);
    },

    onkeydown: function () {
        MM.Form.sort.focus();
    },

    bind: function () {
        MM.Form.action.bind("focus", MM.Action.onfocus);
        MM.Form.action.bind("blur", MM.Action.onblur);
        MM.Form.action.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MM.Action.onkeydown(); });
        });
    }
};

MM.Sort = {
    check: function () {
        var sort = MM.Form.sort.val();
        if (Utils.String(sort).isNullOrEmptyTrim()) {
            return 1;
        }

        if (!Utils.Validator.NumberChars(sort)) {
            return 2;
        }

        return 0;
    },

    clear: function () {
        MM.Form.sort.val(0);
        MM.Form.sort.focus();
    },

    onfocus: function (e) {
        MM.Form.focus(MM.Form.sort);
    },
    onblur: function (e) {
        MM.Form.blur(MM.Form.sort);
    },

    onkeydown: function () {
        var sortCheck = MM.Sort.check();
        if (sortCheck > 0) {
            MM.Form.sort.focus();
        }
    },

    bind: function () {
        MM.Form.sort.bind("focus", MM.Sort.onfocus);
        MM.Form.sort.bind("blur", MM.Sort.onblur);
        MM.Form.sort.bind("keydown", function (e) {
            $.enterSubmit(e, function () { MM.Sort.onkeydown(); });
        });
    }
};

MM.State = {
    clear: function () {
        MM.Form.state1.attr("checked", true);
    }
};

MM.BtnSubmit = {
    disable: function (obj) {
        $.disableButton(obj);
    },
    enable: function (obj) {
        $.enableButton(obj);
    },

    bind: function () {
        MM.Form.btnSubmit.bind("click", function () {
            return MM.onsubmit();
        });

        MM.Form.btnSubmit.bind("keypress", function (e) {
            var event = e || window.event;
            if (event.keyCode == 13) {
                MM.onsubmit();
            }
        });
    }
};

MM.checkInput = function () {
    if (MM.Title.check()) {
        MM.Form.error(MM.Form.title);
        MM.layui.layer.msg(MM.Const.title.empty, { icon: 2 });
        MM.Form.title.focus();
        return false;
    }

    var sortCheck = MM.Sort.check();
    if (sortCheck == 1){
        MM.Form.error(MM.Form.sort);
        MM.layui.layer.msg(MM.Const.sort.empty, { icon: 2 });
        MM.Form.sort.focus();
        return false;
    }
    else if (sortCheck == 2) {
        MM.Form.error(MM.Form.sort);
        MM.layui.layer.msg(MM.Const.sort.error, { icon: 2 });
        MM.Form.sort.focus();
        return false;
    }

    return true;
};

MM.onsubmit = function () {
    if (!MM.checkInput()) {
        return false;
    }

    MM.BtnSubmit.disable(MM.Form.btnSubmit);

    //请求参数
    var params = {
        id: MM.Form.id.val(),
        title: MM.Form.title.val(),
        parentID: MM.Form.parentID.val(),
        controller: MM.Form.controller.val(),
        action: MM.Form.action.val(),
        sort: MM.Form.sort.val(),
        state: $("input[name='state']:checked").val()
    };

    var ajaxUrl = MM.Const.url.doModifyModule;

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
                MM.BtnSubmit.enable(MM.Form.btnSubmit);
                MM.layui.layer.msg(data.msg, { icon: 2 });
            } else {
                MM.layui.layer.msg(data.msg, { icon: 1 }, function () {
                    $.jump(MM.Const.url.doModuleList);
                });

            }
        },
        error: function (result, status) {
            MM.BtnSubmit.enable(MM.Form.btnSubmit);

            if (status == 'timeout') {
                alert(MM.Const.ajaxErr);
            } else if (result.responseText != "") {
                eval("exception = " + result.responseText);
                alert(exception.Message);
            }
        }
    });
};

MM.bind = function () {
    layui.use(['form', 'layer'], function () {
        MM.layui.form = layui.form;
        MM.layui.layer = layui.layer;

        MM.layui.form.on('radio(state)', function (data) {
            if (data.value == 1) {
                MM.Form.state1.attr("checked", true);
                MM.Form.state2.attr("checked", false);
            } else {
                MM.Form.state1.attr("checked", false);
                MM.Form.state2.attr("checked", true);
            }
        });

        MM.layui.form.on('select(parentID)', function (data) {
            MM.ParentID.select(data.value);
        });
    });

    MM.Form.inti();
    MM.Title.bind();
    MM.Controller.bind();
    MM.Action.bind();
    MM.Sort.bind();
    MM.BtnSubmit.bind();
    MM.ParentID.select(MM.Form.parentID.val());
};

/*
 *  窗体事件
 */
$(document).ready(function (e) {
    MM.bind();
});
