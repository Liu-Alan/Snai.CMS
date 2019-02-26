CREATE DATABASE snai_cms CHARACTER SET utf8 COLLATE utf8_general_ci
;

USE snai_cms
;

CREATE TABLE admins(
	id INT AUTO_INCREMENT PRIMARY KEY,			-- 自增列需为主键
	user_name NVARCHAR(32) NOT NULL,
	`password` VARCHAR(32) NOT NULL,
	role_id INT NOT NULL,
	state TINYINT NOT NULL DEFAULT 1,			-- 1 启用，2 禁用
	create_time INT NOT NULL DEFAULT 0,
	update_time INT NOT NULL DEFAULT 0,
	last_logon_time INT NOT NULL DEFAULT 0,
	error_logon_time INT NOT NULL DEFAULT 0,	-- 错误开始时间
	error_logon_count INT NOT NULL DEFAULT 0,	-- 错误次数
	lock_time INT NOT NULL DEFAULT 0 			-- 锁定结束时间
)
;
						
ALTER TABLE admins ADD UNIQUE INDEX ix_admins_user_name(user_name)  		-- UNIQUE INDEX 唯一索引
;

-- password:snai2019
INSERT into admins(user_name,`password`,role_id,state,create_time)
VALUES('snai','55F6E397AB652A987D12914F1766DA58',1,1,1550937600)


CREATE TABLE modules(
	id INT AUTO_INCREMENT PRIMARY KEY,
	parent_id int not NULL DEFAULT 0,
	title NVARCHAR(32) not NULL,
	controller NVARCHAR(32) not NULL DEFAULT ''
	action NVARCHAR(32) not NULL DEFAULT ''
	state TINYINT NOT NULL DEFAULT 1			-- 1 启用，2 禁用
)
;

ALTER TABLE modules ADD UNIQUE INDEX ix_modules_parent_id_title(parent_id,title)  		
ALTER TABLE modules ADD INDEX ix_modules_controller_action(controller,action)  			
;

CREATE TABLE roles(
	id int AUTO_INCREMENT PRIMARY KEY,
	title NVARCHAR(32) not NULL,
	state TINYINT NOT NULL DEFAULT 1			-- 1 启用，2 禁用
)
;

ALTER TABLE roles ADD UNIQUE INDEX ix_roles_title(title)  		
;

CREATE TABLE role_right(
	role_id int not NULL,
	modules_id int not NULL
)

alter table role_right add primary key pk_role_right (role_id,modules_id)  		-- 主键