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

/********* 验证函数 *********/

Utils.Validator = {

    //对象是否为空
    Empty: function (o) {
        return typeof o == 'undefined' || o == '';
    },

    //对象是否为空
    EmptyTrim: function (o) {
        return typeof o == 'undefined' || o == '' || Utils.String(o).trim() == '';
    },

    //o1 是否与 o2 完全一致
    SameTwo: function (o1, o2) {
        return o1 === o2;
    },

    //o1 是否与 o2 完全一致
    SameTwoTrim: function (o1, o2) {
        return Utils.String(o1).trim() == Utils.String(o2).trim();
    },

    //包含字母
    ContainChars: function (str) {
        var _regex = /[A-Za-z]+/;
        return _regex.test(str);
    },

    //是否英文组合
    English: function (str) {
        var _regex = /^[A-Za-z]+$/;
        return _regex.test(str);
    },

    //是否数字组合
    NumberChars: function (str) {
        var _regex = /^\d+$/;
        return _regex.test(str);
    },

	/**
	 * 校验帐号密码
	 * str		密码字符串
	 *
	 * 返回值：
	 * 		0	密码正确
	 * 		1	包含空格 全角或半角
	 * 		2	字符长度不足
	 * 		4	密码为空
	 * 		5	单个字符重复出现
	 * 		6	递增或递减的数字或字母
	 * 		7	密码属于社会工程学中
     *      8   密码包含字母但不能是纯字母
	 */
    Password: function (str) {
        var minL = 6;
        var maxL = 100;

        //密码为空
        if (Utils.Validator.EmptyTrim(str)) {
            return 4;
        }

        //长度不足
        var len = Utils.String(str).byteLength();
        if (len < minL || len > maxL) {
            return 2;
        }

        //包含字母但不能是纯字母
        if (!Utils.Validator.ContainChars(str)) {
            return 8;
        }
        if (Utils.Validator.English(str)) {
            return 8;
        }

        //是否包含空格 或 其他全角字符
        var index;
        var charCount;
        var charId;
        for (index = 0; index < str.length; index++) {
            charId = str.charCodeAt(index);
            if (charId <= 32 || charId > 126) {
                charCount = 1;
                break;
            }
        }

        if (charCount == 1) {
            return 1;
        }

        //是否单个字符重复出现
        var firstCharId = str.charCodeAt(0);
        index = 0;
        charCount = 0;
        charId = 0;
        for (index = 1; index < str.length; index++) {
            charId = str.charCodeAt(index);
            if (firstCharId == charId) {
                charCount++;
            }
        }

        if ((charCount + 1) == str.length) {
            return 5;
        }

        //是否递增或递减的数字或字母
        index = 0;
        charCount = 0;
        if (Utils.Validator.NumberChars(str) || Utils.Validator.English(str)) {
            charId = str.charCodeAt(str.length - 1);

            //递减
            var vector = -1;
            if (firstCharId < charId) {
                //递增
                vector = 1;
            }

            while (index < str.length) {
                if (str.charCodeAt(index) + vector == str.charCodeAt(index + 1)) {
                    charCount++;
                } else {
                    break;
                }

                index++;
            }

            if ((charCount + 1) == str.length) {
                return 6;
            }
        }

        //是否是社会工程字典中的密码
        index = 0;
        charCount = 0;
        var lvDicts = new Array("asdfg", "asdfgh", "qwert", "qwerty", "zxcvb", "zxcvbn", "asdf", "qwer", "zxcv", "password", "passwd", "test", "woaini", "iloveyou", "woaiwojia", "521521", "5201314", "7758521", "1314520", "1314521", "520520", "201314", "211314", "7758258", "147258369", "159357", "12345", "123456", "1234567", "12345678", "123456789", "654321", "123123", "123321", "123abc");

        for (index = 0; index < lvDicts.length; index++) {
            if (str == lvDicts[index]) {
                return 7;
            }
        }

        return 0;
    }
};