//JQuery 扩展
$.extend({
    getKeyCode: function (e) {
        var evt = window.event || e;
        return evt.keyCode ? evt.keyCode : evt.which ? evt.which : evt.charCode;
    },

    enterSubmit: function (e, v) {
        if ($.getKeyCode(e) == 13) {
            if (typeof v == 'function') {
                v();
            } else if (typeof v == 'string') {
                $(v)[0].click();
            }
        }
    },

    ctrlEnterSubmit: function (e, v) {
        var evt = window.event || e;
        if (evt.ctrlKey && $.getKeyCode(evt) == 13) {
            if (typeof v == 'function') {
                v();
            } else if (typeof v == 'string') {
                $(v)[0].click();
            }
        }
    },

    jump: function (_aUrl) { $.jumpUrl(window, _aUrl); },
    jumpUrl: function (_aFrameObj, _aUrl, _aIsSetHistory) {
        try {
            var _location = (_aFrameObj.contentWindow || _aFrameObj).location;
            if (_aIsSetHistory) {
                _location.href = _aUrl;
            }
            else {
                _location.replace(_aUrl);
            }
        }
        catch (e) {
            _aFrameObj.src = _aUrl;
        }
    },

    /*
	 * 禁用提交按钮
	 */
    disableButton: function (obj) {
        obj.attr("disabled", true);
        obj.addClass("disabled");
    },

    enableButton: function (obj) {
        obj.attr("disabled", false);
        obj.removeClass("disabled");
    }
});