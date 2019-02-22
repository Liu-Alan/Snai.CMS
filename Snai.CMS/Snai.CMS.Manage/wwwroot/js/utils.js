var Utils = {};

// 字符串扩展
Utils.String = function (source) {
    //内部类
    var inti = function (source) {
        this.self = source;
        return this;
    };

    //扩展方法
    inti.prototype = {
        //字符串长度,一个汉字占两个字符
        byteLength: function () {
            var str = this.self;
            var len = str.replace(/[^\x00-\xff]/g, "**").length
            return len;
        },

        //去掉左右空白字符
        trim: function () {
            this.self = this.self.replace(/(^\s*)|(\s*$)/g, "");
            return this.self;
        },

        //是否为空值
        isNullOrEmpty: function () {
            return (this.self == null || this.self == "" || this.self.length == 0);
        },

        //去掉前后空格后 是否为空值
        isNullOrEmptyTrim: function () {
            var trimSelf = this.trim(this.self);
            return (trimSelf == null || trimSelf == "" || trimSelf.length == 0);
        }
    };

    //实例化类
    return new inti(source);
};