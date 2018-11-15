/*
Navicat Oracle Data Transfer
Oracle Client Version : 11.2.0.1.0

Source Server         : 127.0.0.1-本机Oracle测试
Source Server Version : 110200
Source Host           : 127.0.0.1:1521
Source Schema         : test

Target Server Type    : ORACLE
Target Server Version : 110200
File Encoding         : 65001

Date: 2018-11-15 11:29:57
*/


-- ----------------------------
-- Table structure for GDMS_DEV_MAIN
-- ----------------------------
CREATE TABLE "GDMS_DEV_MAIN" (
"ID" NUMBER NOT NULL ,
"STN_ID" NUMBER NULL ,
"COUNT" NUMBER NULL ,
"SN" VARCHAR2(50 BYTE) NULL ,
"STYLE_ID" NUMBER NULL ,
"PROJECT_ID" NUMBER NULL ,
"DELIVERY_DATE" DATE NULL ,
"STATUS" CHAR(1 BYTE) NULL ,
"REMARK" VARCHAR2(50 BYTE) NULL ,
"USER_ID" VARCHAR2(50 BYTE) NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_DEV_MAIN" IS '设备主表 (位置ID、式样ID、项目ID)';

-- ----------------------------
-- Records of GDMS_DEV_MAIN
-- ----------------------------
INSERT INTO "GDMS_DEV_MAIN" VALUES ('89', '2', '1', 'T003', '1', '1', TO_DATE('2018-11-06 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '3', '444', '1209435', TO_DATE('2018-11-06 07:55:56', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('85', null, '1', null, null, null, TO_DATE('2018-08-30 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), null, null, '1209435', TO_DATE('2018-08-30 11:03:11', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('26', null, '1', '00000000000000', '3', null, TO_DATE('2018-07-26 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '0', null, null, TO_DATE('2018-07-26 08:14:51', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('88', '2', '1', 'T002', '1', '1', TO_DATE('2018-11-06 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '2', '222', '1209435', TO_DATE('2018-11-06 07:55:48', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('25', null, '1', '99999999999999', '3', null, TO_DATE('2018-07-26 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '0', null, null, TO_DATE('2018-07-26 08:14:48', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('24', null, '1', '888888888888', '3', null, TO_DATE('2018-07-26 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '0', null, null, TO_DATE('2018-07-26 08:14:46', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('18', null, '1', '222222222222', '3', null, TO_DATE('2018-07-26 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '0', null, null, TO_DATE('2018-07-26 08:14:26', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('17', null, '1', '1111111111111', '3', null, TO_DATE('2018-07-26 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '0', null, null, TO_DATE('2018-07-26 08:14:21', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('23', null, '1', '777777777777', '3', null, TO_DATE('2018-07-26 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '0', null, null, TO_DATE('2018-07-26 08:14:43', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('87', '2', '1', 'T001', '1', '1', TO_DATE('2018-11-06 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '1', '123', '1209435', TO_DATE('2018-11-06 07:55:40', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_DEV_MAIN" VALUES ('21', null, '1', '55555555555555', '3', null, TO_DATE('2018-07-26 00:00:00', 'YYYY-MM-DD HH24:MI:SS'), '0', null, null, TO_DATE('2018-07-26 08:14:37', 'YYYY-MM-DD HH24:MI:SS'));

-- ----------------------------
-- Table structure for GDMS_DEV_MORE
-- ----------------------------
CREATE TABLE "GDMS_DEV_MORE" (
"DEV_ID" NUMBER NOT NULL ,
"ITEM" VARCHAR2(50 BYTE) NOT NULL ,
"VALUE" VARCHAR2(100 BYTE) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_DEV_MORE" IS '设备副表';

-- ----------------------------
-- Records of GDMS_DEV_MORE
-- ----------------------------
INSERT INTO "GDMS_DEV_MORE" VALUES ('21', 'IP', '10.37.94.129');
INSERT INTO "GDMS_DEV_MORE" VALUES ('82', '1', '2');
INSERT INTO "GDMS_DEV_MORE" VALUES ('21', 'CPU', '2.0');
INSERT INTO "GDMS_DEV_MORE" VALUES ('86', '3', '4');
INSERT INTO "GDMS_DEV_MORE" VALUES ('86', '6', '8');
INSERT INTO "GDMS_DEV_MORE" VALUES ('86', 'IP', '1111');
INSERT INTO "GDMS_DEV_MORE" VALUES ('69', '5', '6');
INSERT INTO "GDMS_DEV_MORE" VALUES ('72', '3', '4');
INSERT INTO "GDMS_DEV_MORE" VALUES ('83', '3', '4');
INSERT INTO "GDMS_DEV_MORE" VALUES ('72', '1', '2333');
INSERT INTO "GDMS_DEV_MORE" VALUES ('72', '特容易', '犹太人');
INSERT INTO "GDMS_DEV_MORE" VALUES ('67', '1', 's');
INSERT INTO "GDMS_DEV_MORE" VALUES ('67', '3', 'a');

-- ----------------------------
-- Table structure for GDMS_LANG
-- ----------------------------
CREATE TABLE "GDMS_LANG" (
"YE_MIAN_MING" NVARCHAR2(50) NOT NULL ,
"YU_ZHONG" NVARCHAR2(2) NOT NULL ,
"XU_HAO" NVARCHAR2(50) NOT NULL ,
"WEN_ZI" NVARCHAR2(50) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_LANG" IS '语言表';

-- ----------------------------
-- Records of GDMS_LANG
-- ----------------------------
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '设备管理系统', '设备管理系统');
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '密码', '密码');
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '用户名', '用户名');
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '登录', '登录');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '设备管理系统', 'GDMS');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '密码', 'Password');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '用户名', 'Username');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '登录', 'Login');
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '注册', '注册');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '注册', 'Register');
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '账号', '账号');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '账号', 'ID');
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '确认密码', '确认密码');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '确认密码', 'Confirm Password');
INSERT INTO "GDMS_LANG" VALUES ('account', 'CN', '暂未开放', '暂未开放');
INSERT INTO "GDMS_LANG" VALUES ('account', 'EN', '暂未开放', 'Not Open');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '设备管理系统', '设备管理系统');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '设备管理系统', 'GDMS');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '搜索', 'Search...');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '主页', '主页');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '搜索', '搜索...');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '主页', 'Home');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '控制台', '控制台');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '控制台', 'Dashboard');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '设备管理', '设备管理');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '设备管理', 'Management');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '设备图表', '设备图表');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '设备图表', 'Charts');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '设备监控', '设备监控');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '设备监控', 'Monitoring');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '系统管理', '系统管理');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '系统管理', 'System');
INSERT INTO "GDMS_LANG" VALUES ('index', 'CN', '基础管理', '基础管理');
INSERT INTO "GDMS_LANG" VALUES ('index', 'EN', '基础管理', 'Basic');

-- ----------------------------
-- Table structure for GDMS_PROJECT
-- ----------------------------
CREATE TABLE "GDMS_PROJECT" (
"ID" NUMBER NOT NULL ,
"NAME" VARCHAR2(50 BYTE) NOT NULL ,
"DETAIL" VARCHAR2(500 BYTE) NULL ,
"YEAR" VARCHAR2(7 BYTE) NULL ,
"SYSTEM_ID" NUMBER NOT NULL ,
"USER_ID" VARCHAR2(50 CHAR) NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_PROJECT" IS '项目表';

-- ----------------------------
-- Records of GDMS_PROJECT
-- ----------------------------
INSERT INTO "GDMS_PROJECT" VALUES ('1', 'MES改造', '一工厂MES改造项目', '2018-08', '1', '1209435', TO_DATE('2018-10-22 13:47:08', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_PROJECT" VALUES ('14', 'mes', null, '2018-08', '1', '1209435', TO_DATE('2018-08-30 15:11:39', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_PROJECT" VALUES ('11', '程序被撤销', null, '2018-08', '2', '1209435', TO_DATE('2018-08-30 14:15:00', 'YYYY-MM-DD HH24:MI:SS'));

-- ----------------------------
-- Table structure for GDMS_SITE
-- ----------------------------
CREATE TABLE "GDMS_SITE" (
"ID" NUMBER NOT NULL ,
"PARENT_ID" NUMBER NOT NULL ,
"SYSTEM_ID" NUMBER NOT NULL ,
"NAME" VARCHAR2(50 BYTE) NOT NULL ,
"REMARK" VARCHAR2(200 BYTE) NULL ,
"USER_ID" VARCHAR2(50 BYTE) NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_SITE" IS '地点表 (系统ID)';

-- ----------------------------
-- Records of GDMS_SITE
-- ----------------------------
INSERT INTO "GDMS_SITE" VALUES ('1', '0', '1', '手动阀', '手动阀', '1209435', TO_DATE('2018-08-31 11:10:21', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_SITE" VALUES ('4', '1', '1', '权威', '2', '1209435', TO_DATE('2018-09-03 10:54:27', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_SITE" VALUES ('3', '1', '1', '大发光火发', '风格', '1209435', TO_DATE('2018-10-24 14:28:48', 'YYYY-MM-DD HH24:MI:SS'));

-- ----------------------------
-- Table structure for GDMS_STN_MAIN
-- ----------------------------
CREATE TABLE "GDMS_STN_MAIN" (
"ID" NUMBER NOT NULL ,
"SITE_ID" NUMBER NULL ,
"NAME" VARCHAR2(50 BYTE) NOT NULL ,
"DETAIL" VARCHAR2(100 BYTE) NULL ,
"REMARK" VARCHAR2(200 BYTE) NULL ,
"STATUS" CHAR(1 BYTE) NULL ,
"USER_ID" VARCHAR2(50 BYTE) NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_STN_MAIN" IS '位置主表 (地点ID)';

-- ----------------------------
-- Records of GDMS_STN_MAIN
-- ----------------------------
INSERT INTO "GDMS_STN_MAIN" VALUES ('11', '4', '345', '567', '890', '1', '1209435', TO_DATE('2018-10-22 16:09:42', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_STN_MAIN" VALUES ('13', '3', '123', null, null, '0', '1209435', TO_DATE('2018-10-23 16:59:39', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_STN_MAIN" VALUES ('1', '3', '421LH', 'WBT工位', '无', '1', '1209435', TO_DATE('2018-10-22 16:08:43', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_STN_MAIN" VALUES ('2', '1', '421RH', 'WBT工位', '无', '1', null, TO_DATE('2018-06-28 14:58:38', 'YYYY-MM-DD HH24:MI:SS'));

-- ----------------------------
-- Table structure for GDMS_STN_MORE
-- ----------------------------
CREATE TABLE "GDMS_STN_MORE" (
"STN_ID" NUMBER NOT NULL ,
"ITEM" VARCHAR2(50 BYTE) NOT NULL ,
"VALUE" VARCHAR2(100 BYTE) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_STN_MORE" IS '位置副表 (位置ID)';

-- ----------------------------
-- Records of GDMS_STN_MORE
-- ----------------------------
INSERT INTO "GDMS_STN_MORE" VALUES ('1', 'IP', '10.37.81.100');
INSERT INTO "GDMS_STN_MORE" VALUES ('11', '1', '2');
INSERT INTO "GDMS_STN_MORE" VALUES ('11', '3', '4');
INSERT INTO "GDMS_STN_MORE" VALUES ('2', 'IP', '10.37.81.101');

-- ----------------------------
-- Table structure for GDMS_STYLE
-- ----------------------------
CREATE TABLE "GDMS_STYLE" (
"ID" NUMBER NOT NULL ,
"NAME" VARCHAR2(50 BYTE) NULL ,
"DETAIL" VARCHAR2(1000 BYTE) NULL ,
"FILE_URL" VARCHAR2(100 BYTE) NULL ,
"IMG_URL" VARCHAR2(100 BYTE) NULL ,
"USER_ID" VARCHAR2(50 BYTE) NULL ,
"EDIT_DATE" DATE NULL ,
"TYPE_ID" NUMBER NULL ,
"SERVICE_YEAR" NUMBER NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_STYLE" IS '式样表 (类型ID)';

-- ----------------------------
-- Records of GDMS_STYLE
-- ----------------------------
INSERT INTO "GDMS_STYLE" VALUES ('46', '1', '2', null, null, null, TO_DATE('2018-11-09 16:36:12', 'YYYY-MM-DD HH24:MI:SS'), '1', '3');
INSERT INTO "GDMS_STYLE" VALUES ('47', '1', '2', null, null, null, TO_DATE('2018-11-09 16:39:47', 'YYYY-MM-DD HH24:MI:SS'), '1', '3');
INSERT INTO "GDMS_STYLE" VALUES ('48', '1', '2', null, null, null, TO_DATE('2018-11-09 16:51:26', 'YYYY-MM-DD HH24:MI:SS'), '1', '3');
INSERT INTO "GDMS_STYLE" VALUES ('45', '1', '2', null, null, null, TO_DATE('2018-11-09 16:35:05', 'YYYY-MM-DD HH24:MI:SS'), '1', '3');
INSERT INTO "GDMS_STYLE" VALUES ('1', 'IPC-610MB', '研华工控机', null, null, null, TO_DATE('2018-06-28 14:06:15', 'YYYY-MM-DD HH24:MI:SS'), '1', '2');
INSERT INTO "GDMS_STYLE" VALUES ('2', 'P7003', '普印力P7003打印机', null, null, null, TO_DATE('2018-06-28 14:07:45', 'YYYY-MM-DD HH24:MI:SS'), '2', '5');
INSERT INTO "GDMS_STYLE" VALUES ('3', 'P8003', '普印力P8003打印机', null, null, null, TO_DATE('2018-06-28 14:08:06', 'YYYY-MM-DD HH24:MI:SS'), '2', '5');
INSERT INTO "GDMS_STYLE" VALUES ('44', '1', '2', null, null, null, TO_DATE('2018-11-09 15:04:42', 'YYYY-MM-DD HH24:MI:SS'), '1', '3');
INSERT INTO "GDMS_STYLE" VALUES ('41', '1', '2', null, null, null, TO_DATE('2018-11-09 15:02:31', 'YYYY-MM-DD HH24:MI:SS'), '1', '3');

-- ----------------------------
-- Table structure for GDMS_SYSTEM
-- ----------------------------
CREATE TABLE "GDMS_SYSTEM" (
"NAME" VARCHAR2(50 BYTE) NOT NULL ,
"ID" NUMBER NOT NULL ,
"REMARK" VARCHAR2(200 BYTE) NULL ,
"USER_ID" VARCHAR2(50 BYTE) NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_SYSTEM" IS '系统表';

-- ----------------------------
-- Records of GDMS_SYSTEM
-- ----------------------------
INSERT INTO "GDMS_SYSTEM" VALUES ('MES1', '1', '1工厂MES系统', null, TO_DATE('2018-06-28 14:01:38', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_SYSTEM" VALUES ('MES3', '3', '3工厂MES系统', null, TO_DATE('2018-06-28 14:02:08', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_SYSTEM" VALUES ('MES4', '4', '4工厂MES系统', null, TO_DATE('2018-06-28 14:02:11', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_SYSTEM" VALUES ('MES5', '5', '5工厂MES系统', null, TO_DATE('2018-06-28 14:02:13', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_SYSTEM" VALUES ('MES2', '2', '2工厂MES系统', null, TO_DATE('2018-06-28 14:02:15', 'YYYY-MM-DD HH24:MI:SS'));

-- ----------------------------
-- Table structure for GDMS_TYPE
-- ----------------------------
CREATE TABLE "GDMS_TYPE" (
"ID" NUMBER NOT NULL ,
"SYSTEM_ID" NUMBER NULL ,
"NAME" VARCHAR2(50 BYTE) NULL ,
"USER_ID" VARCHAR2(50 BYTE) NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_TYPE" IS '类型表 (系统ID)';

-- ----------------------------
-- Records of GDMS_TYPE
-- ----------------------------
INSERT INTO "GDMS_TYPE" VALUES ('1', '1', '工控机', '1209435', TO_DATE('2018-09-03 16:27:57', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO "GDMS_TYPE" VALUES ('2', '1', '序列打印机', '1209435', TO_DATE('2018-09-03 16:28:32', 'YYYY-MM-DD HH24:MI:SS'));

-- ----------------------------
-- Table structure for GDMS_USER
-- ----------------------------
CREATE TABLE "GDMS_USER" (
"USER_ID" NVARCHAR2(50) NOT NULL ,
"USER_NAME" NVARCHAR2(50) NOT NULL ,
"PASS" NVARCHAR2(50) NOT NULL ,
"USER_TYPE" NUMBER(1) NOT NULL ,
"FAILED_LOGINS" NUMBER(5) NOT NULL ,
"LAST_IP" NVARCHAR2(15) NULL ,
"EMAIL" NVARCHAR2(50) NULL ,
"LAST_LOGIN" DATE NULL ,
"JOIN_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_USER" IS '用户表';

-- ----------------------------
-- Records of GDMS_USER
-- ----------------------------
INSERT INTO "GDMS_USER" VALUES ('admin', '管理员', '487B9F81822B117FFEFE4DE1A38B1B27', '2', '0', '127.0.0.1', '9061@163.com', TO_DATE('2018-11-15 11:26:03', 'YYYY-MM-DD HH24:MI:SS'), TO_DATE('2018-07-19 15:45:17', 'YYYY-MM-DD HH24:MI:SS'));

-- ----------------------------
-- Table structure for GDMS_USER_SYSTEM
-- ----------------------------
CREATE TABLE "GDMS_USER_SYSTEM" (
"USER_ID" VARCHAR2(50 BYTE) NOT NULL ,
"SYSTEM_ID" NUMBER NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "GDMS_USER_SYSTEM" IS '匹配 (用户ID - 系统ID)';

-- ----------------------------
-- Records of GDMS_USER_SYSTEM
-- ----------------------------
INSERT INTO "GDMS_USER_SYSTEM" VALUES ('admin', '1');
INSERT INTO "GDMS_USER_SYSTEM" VALUES ('admin', '2');
INSERT INTO "GDMS_USER_SYSTEM" VALUES ('admin', '3');

-- ----------------------------
-- Sequence structure for GDMS_DEV_MAIN_SEQ
-- ----------------------------
CREATE SEQUENCE "GDMS_DEV_MAIN_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 90
 NOCACHE ;

-- ----------------------------
-- Sequence structure for GDMS_PROJECT_SEQ
-- ----------------------------
CREATE SEQUENCE "GDMS_PROJECT_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 15
 NOCACHE ;

-- ----------------------------
-- Sequence structure for GDMS_SITE_SEQ
-- ----------------------------
CREATE SEQUENCE "GDMS_SITE_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 5
 NOCACHE ;

-- ----------------------------
-- Sequence structure for GDMS_STN_MAIN_SEQ
-- ----------------------------
CREATE SEQUENCE "GDMS_STN_MAIN_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 14
 NOCACHE ;

-- ----------------------------
-- Sequence structure for GDMS_STYLE_SEQ
-- ----------------------------
CREATE SEQUENCE "GDMS_STYLE_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 49
 NOCACHE ;

-- ----------------------------
-- Sequence structure for GDMS_SYSTEM_ID_SEQ
-- ----------------------------
CREATE SEQUENCE "GDMS_SYSTEM_ID_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 21
 NOCACHE ;

-- ----------------------------
-- Sequence structure for GDMS_TYPE_SEQ
-- ----------------------------
CREATE SEQUENCE "GDMS_TYPE_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 6
 NOCACHE ;
 
 
-- ----------------------------
-- Function structure for DEVICE_ADD
-- ----------------------------
CREATE OR REPLACE FUNCTION "DEVICE_ADD" (
	stn_in VARCHAR2, 
	style_in VARCHAR2, 
	project_in VARCHAR2, 
	sn_in VARCHAR2, 
	delivery_in VARCHAR2, 
	status_in VARCHAR2, 
	remark_in VARCHAR2, 
	userId_in VARCHAR2
)
RETURN NUMBER IS
PRAGMA AUTONOMOUS_TRANSACTION;
devId NUMBER;

BEGIN
	INSERT INTO GDMS_DEV_MAIN 
		(ID,COUNT,STN_ID,STYLE_ID,PROJECT_ID,SN,DELIVERY_DATE,STATUS,REMARK,USER_ID,EDIT_DATE)
	VALUES(
		GDMS_DEV_MAIN_SEQ.nextVal,'1',stn_in,style_in,project_in,sn_in,to_date(delivery_in,'yyyy-mm-dd'),status_in,remark_in,userId_in,SYSDATE
	)
  RETURNING ID INTO devId;
	commit;
	RETURN devId;
	
END;


-- ----------------------------
-- Function structure for STN_ADD
-- ----------------------------
CREATE OR REPLACE FUNCTION "STN_ADD" (
	site_in VARCHAR2, 
	name_in VARCHAR2, 
	detail_in VARCHAR2, 
	status_in VARCHAR2, 
	remark_in VARCHAR2, 
	userId_in VARCHAR2
)
RETURN NUMBER IS
PRAGMA AUTONOMOUS_TRANSACTION;
stnId NUMBER;

BEGIN
	INSERT INTO GDMS_STN_MAIN 
		(ID,SITE_ID,NAME,DETAIL,REMARK,STATUS,USER_ID,EDIT_DATE)
	VALUES(
		GDMS_STN_MAIN_SEQ.nextVal,site_in,name_in,detail_in,remark_in,status_in,userId_in,SYSDATE
	)
  RETURNING ID INTO stnId;
	commit;
	RETURN stnId;
	
END;



-- ----------------------------
-- Function structure for STYLE_ADD
-- ----------------------------
CREATE OR REPLACE FUNCTION "STYLE_ADD" (
	name_in VARCHAR2, 
	detail_in VARCHAR2, 
	year_in VARCHAR2, 
	type_in VARCHAR2, 
	userId_in VARCHAR2
)
RETURN NUMBER IS
PRAGMA AUTONOMOUS_TRANSACTION;
styleId NUMBER;

BEGIN
	INSERT INTO GDMS_STYLE
		(ID,NAME,DETAIL,USER_ID,EDIT_DATE,TYPE_ID,SERVICE_YEAR)
	VALUES(
		GDMS_STYLE_SEQ.nextVal,name_in,detail_in,userId_in,SYSDATE,type_in,year_in
	)
  RETURNING ID INTO styleId;
	commit;
	RETURN styleId;
	
END;





-- ----------------------------
-- Indexes structure for table GDMS_DEV_MAIN
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_DEV_MAIN
-- ----------------------------
ALTER TABLE "GDMS_DEV_MAIN" ADD CHECK ("ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_DEV_MAIN
-- ----------------------------
ALTER TABLE "GDMS_DEV_MAIN" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Checks structure for table GDMS_DEV_MORE
-- ----------------------------
ALTER TABLE "GDMS_DEV_MORE" ADD CHECK ("DEV_ID" IS NOT NULL);
ALTER TABLE "GDMS_DEV_MORE" ADD CHECK ("ITEM" IS NOT NULL);

-- ----------------------------
-- Indexes structure for table GDMS_LANG
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_LANG
-- ----------------------------
ALTER TABLE "GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_LANG
-- ----------------------------
ALTER TABLE "GDMS_LANG" ADD PRIMARY KEY ("YE_MIAN_MING", "YU_ZHONG", "XU_HAO");

-- ----------------------------
-- Indexes structure for table GDMS_PROJECT
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_PROJECT
-- ----------------------------
ALTER TABLE "GDMS_PROJECT" ADD CHECK ("ID" IS NOT NULL);
ALTER TABLE "GDMS_PROJECT" ADD CHECK ("NAME" IS NOT NULL);
ALTER TABLE "GDMS_PROJECT" ADD CHECK ("SYSTEM_ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_PROJECT
-- ----------------------------
ALTER TABLE "GDMS_PROJECT" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_SITE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_SITE
-- ----------------------------
ALTER TABLE "GDMS_SITE" ADD CHECK ("ID" IS NOT NULL);
ALTER TABLE "GDMS_SITE" ADD CHECK ("PARENT_ID" IS NOT NULL);
ALTER TABLE "GDMS_SITE" ADD CHECK ("SYSTEM_ID" IS NOT NULL);
ALTER TABLE "GDMS_SITE" ADD CHECK ("NAME" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_SITE
-- ----------------------------
ALTER TABLE "GDMS_SITE" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_STN_MAIN
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_STN_MAIN
-- ----------------------------
ALTER TABLE "GDMS_STN_MAIN" ADD CHECK ("ID" IS NOT NULL);
ALTER TABLE "GDMS_STN_MAIN" ADD CHECK ("NAME" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_STN_MAIN
-- ----------------------------
ALTER TABLE "GDMS_STN_MAIN" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_STN_MORE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_STN_MORE
-- ----------------------------
ALTER TABLE "GDMS_STN_MORE" ADD CHECK ("STN_ID" IS NOT NULL);
ALTER TABLE "GDMS_STN_MORE" ADD CHECK ("ITEM" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_STN_MORE
-- ----------------------------
ALTER TABLE "GDMS_STN_MORE" ADD PRIMARY KEY ("STN_ID", "ITEM");

-- ----------------------------
-- Indexes structure for table GDMS_STYLE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_STYLE
-- ----------------------------
ALTER TABLE "GDMS_STYLE" ADD CHECK ("ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_STYLE
-- ----------------------------
ALTER TABLE "GDMS_STYLE" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_SYSTEM
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_SYSTEM
-- ----------------------------
ALTER TABLE "GDMS_SYSTEM" ADD CHECK ("ID" IS NOT NULL);
ALTER TABLE "GDMS_SYSTEM" ADD CHECK ("NAME" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_SYSTEM
-- ----------------------------
ALTER TABLE "GDMS_SYSTEM" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_TYPE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_TYPE
-- ----------------------------
ALTER TABLE "GDMS_TYPE" ADD CHECK ("ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_TYPE
-- ----------------------------
ALTER TABLE "GDMS_TYPE" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_USER
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_USER
-- ----------------------------
ALTER TABLE "GDMS_USER" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("USER_NAME" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("PASS" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("USER_TYPE" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("USER_NAME" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("PASS" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("USER_TYPE" IS NOT NULL);
ALTER TABLE "GDMS_USER" ADD CHECK ("FAILED_LOGINS" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_USER
-- ----------------------------
ALTER TABLE "GDMS_USER" ADD PRIMARY KEY ("USER_ID");

-- ----------------------------
-- Indexes structure for table GDMS_USER_SYSTEM
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_USER_SYSTEM
-- ----------------------------
ALTER TABLE "GDMS_USER_SYSTEM" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "GDMS_USER_SYSTEM" ADD CHECK ("SYSTEM_ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_USER_SYSTEM
-- ----------------------------
ALTER TABLE "GDMS_USER_SYSTEM" ADD PRIMARY KEY ("USER_ID", "SYSTEM_ID");
