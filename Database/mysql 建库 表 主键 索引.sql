CREATE DATABASE snai_cms CHARACTER SET utf8 COLLATE utf8_general_ci
;

USE snai_cms
;

CREATE TABLE admin(
	id INT AUTO_INCREMENT PRIMARY KEY,			-- 自增列需为主键
	user_name NVARCHAR(32) NOT NULL,
	'password' NVARCHAR(32) NOT NULL,
	state TINYINT NOT NULL DEFAULT 1,			-- 1 正常，2 禁用
	role_id INT NOT NULL,
	create_time INT NOT NULL DEFAULT 0,
	last_logon_time INT NOT NULL DEFAULT 0,
	error_logon_time INT NOT NULL DEFAULT 0,
	error_logon_count INT NOT NULL DEFAULT 0,		-- 错误次数
	lock_time INT NOT NULL DEFAULT 0 			-- 锁定开始时间
)
;

-- alter table student add primary key pk_student (id)  	
ALTER TABLE admin ADD UNIQUE INDEX ix_admin_user_name(user_name)  		-- UNIQUE INDEX 唯一索引
;