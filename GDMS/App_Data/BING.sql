/*
Navicat Oracle Data Transfer
Oracle Client Version : 11.2.0.1.0

Source Server         : 127.0.0.1-本机Oracle测试
Source Server Version : 110200
Source Host           : 127.0.0.1:1521
Source Schema         : BING

Target Server Type    : ORACLE
Target Server Version : 110200
File Encoding         : 65001

Date: 2018-07-19 16:48:36
*/


-- ----------------------------
-- Table structure for GDMS_DEV_MAIN
-- ----------------------------
DROP TABLE "BING"."GDMS_DEV_MAIN";
CREATE TABLE "BING"."GDMS_DEV_MAIN" (
"ID" NUMBER NOT NULL ,
"STN_ID" NUMBER NULL ,
"COUNT" NUMBER NULL ,
"SN" VARCHAR2(50 BYTE) NULL ,
"STYLE_ID" NUMBER NULL ,
"PROJECT_ID" NUMBER NULL ,
"DELIVERY_DATE" DATE NULL ,
"STATUS" CHAR(1 BYTE) NULL ,
"REMARK" VARCHAR2(50 BYTE) NULL ,
"USER_ID" NUMBER NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_DEV_MAIN" IS '设备主表 (位置ID、式样ID、项目ID)';

-- ----------------------------
-- Table structure for GDMS_DEV_MORE
-- ----------------------------
DROP TABLE "BING"."GDMS_DEV_MORE";
CREATE TABLE "BING"."GDMS_DEV_MORE" (
"DEV_ID" NUMBER NOT NULL ,
"ITEM" VARCHAR2(50 BYTE) NULL ,
"VALUE" VARCHAR2(100 BYTE) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_DEV_MORE" IS '设备副表';

-- ----------------------------
-- Table structure for GDMS_LANG
-- ----------------------------
DROP TABLE "BING"."GDMS_LANG";
CREATE TABLE "BING"."GDMS_LANG" (
"YE_MIAN_MING" NVARCHAR2(50) NOT NULL ,
"YU_ZHONG" NVARCHAR2(2) NOT NULL ,
"XU_HAO" NVARCHAR2(50) NOT NULL ,
"WEN_ZI" NVARCHAR2(50) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_LANG" IS '语言表';

-- ----------------------------
-- Table structure for GDMS_PROJECT
-- ----------------------------
DROP TABLE "BING"."GDMS_PROJECT";
CREATE TABLE "BING"."GDMS_PROJECT" (
"ID" NUMBER NOT NULL ,
"NAME" VARCHAR2(50 BYTE) NULL ,
"DETAIL" VARCHAR2(500 BYTE) NULL ,
"YEAR" VARCHAR2(4 BYTE) NULL ,
"SYSTEM_ID" NUMBER NULL ,
"USER_ID" NVARCHAR2(50) NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_PROJECT" IS '项目表';

-- ----------------------------
-- Table structure for GDMS_SITE
-- ----------------------------
DROP TABLE "BING"."GDMS_SITE";
CREATE TABLE "BING"."GDMS_SITE" (
"ID" NUMBER NOT NULL ,
"PARENT_ID" NUMBER NOT NULL ,
"SYSTEM_ID" NUMBER NOT NULL ,
"NAME" VARCHAR2(50 BYTE) NOT NULL ,
"REMARK" VARCHAR2(200 BYTE) NULL ,
"USER_ID" NUMBER NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_SITE" IS '地点表 (系统ID)';

-- ----------------------------
-- Table structure for GDMS_STN_MAIN
-- ----------------------------
DROP TABLE "BING"."GDMS_STN_MAIN";
CREATE TABLE "BING"."GDMS_STN_MAIN" (
"ID" NUMBER NOT NULL ,
"SITE_ID" NUMBER NULL ,
"NAME" VARCHAR2(50 BYTE) NOT NULL ,
"DETAIL" VARCHAR2(100 BYTE) NULL ,
"REMARK" VARCHAR2(200 BYTE) NULL ,
"STATUS" CHAR(1 BYTE) NULL ,
"USER_ID" NUMBER NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_STN_MAIN" IS '位置主表 (地点ID)';

-- ----------------------------
-- Table structure for GDMS_STN_MORE
-- ----------------------------
DROP TABLE "BING"."GDMS_STN_MORE";
CREATE TABLE "BING"."GDMS_STN_MORE" (
"STN_ID" NUMBER NOT NULL ,
"ITEM" VARCHAR2(50 BYTE) NOT NULL ,
"VALUE" VARCHAR2(100 BYTE) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_STN_MORE" IS '位置副表 (位置ID)';

-- ----------------------------
-- Table structure for GDMS_STYLE
-- ----------------------------
DROP TABLE "BING"."GDMS_STYLE";
CREATE TABLE "BING"."GDMS_STYLE" (
"ID" NUMBER NOT NULL ,
"NAME" VARCHAR2(50 BYTE) NULL ,
"DETAIL" VARCHAR2(1000 BYTE) NULL ,
"FILE_URL" VARCHAR2(100 BYTE) NULL ,
"FILE_TYPE" VARCHAR2(10 BYTE) NULL ,
"USER_ID" NUMBER NULL ,
"EDIT_DATE" DATE NULL ,
"TYPE_ID" NUMBER NULL ,
"SERVICE_YEAR" NUMBER NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_STYLE" IS '式样表 (类型ID)';

-- ----------------------------
-- Table structure for GDMS_SYSTEM
-- ----------------------------
DROP TABLE "BING"."GDMS_SYSTEM";
CREATE TABLE "BING"."GDMS_SYSTEM" (
"NAME" VARCHAR2(50 BYTE) NOT NULL ,
"ID" NUMBER NOT NULL ,
"REMARK" VARCHAR2(200 BYTE) NULL ,
"USER_ID" NUMBER NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_SYSTEM" IS '系统表';

-- ----------------------------
-- Table structure for GDMS_TYPE
-- ----------------------------
DROP TABLE "BING"."GDMS_TYPE";
CREATE TABLE "BING"."GDMS_TYPE" (
"ID" NUMBER NOT NULL ,
"SYSTEM_ID" NUMBER NULL ,
"NAME" VARCHAR2(50 BYTE) NULL ,
"USER_ID" NUMBER NULL ,
"EDIT_DATE" DATE NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_TYPE" IS '类型表 (系统ID)';

-- ----------------------------
-- Table structure for GDMS_USER
-- ----------------------------
DROP TABLE "BING"."GDMS_USER";
CREATE TABLE "BING"."GDMS_USER" (
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
COMMENT ON TABLE "BING"."GDMS_USER" IS '用户表';

-- ----------------------------
-- Table structure for GDMS_USER_SYSTEM
-- ----------------------------
DROP TABLE "BING"."GDMS_USER_SYSTEM";
CREATE TABLE "BING"."GDMS_USER_SYSTEM" (
"USER_ID" VARCHAR2(20 BYTE) NOT NULL ,
"SYSTEM_ID" NUMBER NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;
COMMENT ON TABLE "BING"."GDMS_USER_SYSTEM" IS '匹配 (用户ID - 系统ID)';

-- ----------------------------
-- Table structure for TXL_GUID_QUANXIAN
-- ----------------------------
DROP TABLE "BING"."TXL_GUID_QUANXIAN";
CREATE TABLE "BING"."TXL_GUID_QUANXIAN" (
"GUID" CHAR(36 BYTE) NOT NULL ,
"USER_ID" NVARCHAR2(50) NULL ,
"QUAN_XIAN" NUMBER NULL ,
"ZU_ID" NVARCHAR2(50) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- Table structure for TXL_JICHUSHUJU
-- ----------------------------
DROP TABLE "BING"."TXL_JICHUSHUJU";
CREATE TABLE "BING"."TXL_JICHUSHUJU" (
"GUID" CHAR(36 CHAR) NOT NULL ,
"XING_MING" NVARCHAR2(50) NOT NULL ,
"XIANG_MU" NVARCHAR2(100) NOT NULL ,
"NEI_RONG" NVARCHAR2(500) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- Table structure for TXL_USER
-- ----------------------------
DROP TABLE "BING"."TXL_USER";
CREATE TABLE "BING"."TXL_USER" (
"USER_ID" NVARCHAR2(50) NOT NULL ,
"USER_NAME" NVARCHAR2(50) NOT NULL ,
"PASS" NVARCHAR2(50) NOT NULL ,
"USER_TYPE" NUMBER(1) NOT NULL ,
"FAILED_LOGINS" NUMBER(1) NULL ,
"LAST_IP" NVARCHAR2(15) NULL ,
"EMAIL" NVARCHAR2(50) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- Table structure for TXL_USER_ZU
-- ----------------------------
DROP TABLE "BING"."TXL_USER_ZU";
CREATE TABLE "BING"."TXL_USER_ZU" (
"USER_ID" NVARCHAR2(50) NOT NULL ,
"ZU_ID" NVARCHAR2(50) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- Table structure for TXL_YUYAN
-- ----------------------------
DROP TABLE "BING"."TXL_YUYAN";
CREATE TABLE "BING"."TXL_YUYAN" (
"YE_MIAN_MING" NVARCHAR2(50) NOT NULL ,
"YU_ZHONG" NVARCHAR2(2) NOT NULL ,
"XU_HAO" NVARCHAR2(50) NOT NULL ,
"WEN_ZI" NVARCHAR2(50) NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- Table structure for TXL_ZU
-- ----------------------------
DROP TABLE "BING"."TXL_ZU";
CREATE TABLE "BING"."TXL_ZU" (
"ZU_ID" NVARCHAR2(50) NOT NULL ,
"PARENT_ID" NVARCHAR2(50) NULL ,
"ZU_NAME" NVARCHAR2(50) NOT NULL 
)
LOGGING
NOCOMPRESS
NOCACHE

;

-- ----------------------------
-- View structure for INTER_V
-- ----------------------------
CREATE OR REPLACE FORCE VIEW "BING"."INTER_V" AS 
select "CO1","CO2","CO3","CO4","CO5","CO6","CO7","CO8","CO9","CO10","CO11","CO12","CO13","CO14","CO15","CO16","CO17","CO18","CO19","CO20","CO21","CO22","CO23","CO24","CO25","CO26","CO27","CO28","CO29","CO30","CO31","CO32","CO33","CO34","CO35","CO36","CO37","CO38","CO39","CO40","CO41","CO42","CO43","CO44","CO45","CO46","CO47","CO48","CO49","CO50","CO51","CO52","CO53","CO54","CO55","CO56","CO57","CO58","CO59","CO60","CO61","CO62" from (
 select distinct y.* from a x join a y on X.CO39 = y.co5
 intersect
 --minus
 select distinct x.* from a x join a y on X.CO39 = y.co5
 );

-- ----------------------------
-- View structure for NEW_V
-- ----------------------------
CREATE OR REPLACE FORCE VIEW "BING"."NEW_V" AS 
select "CO1","CO2","CO3","CO4","CO5","CO6","CO7","CO8","CO9","CO10","CO11","CO12","CO13","CO14","CO15","CO16","CO17","CO18","CO19","CO20","CO21","CO22","CO23","CO24","CO25","CO26","CO27","CO28","CO29","CO30","CO31","CO32","CO33","CO34","CO35","CO36","CO37","CO38","CO39","CO40","CO41","CO42","CO43","CO44","CO45","CO46","CO47","CO48","CO49","CO50","CO51","CO52","CO53","CO54","CO55","CO56","CO57","CO58","CO59","CO60","CO61","CO62" from (
 select distinct x.* from a x join a y on X.CO39 = y.co5
 --intersect
 minus
 select distinct y.* from a x join a y on X.CO39 = y.co5
 );

-- ----------------------------
-- View structure for OLD_V
-- ----------------------------
CREATE OR REPLACE FORCE VIEW "BING"."OLD_V" AS 
select "CO1","CO2","CO3","CO4","CO5","CO6","CO7","CO8","CO9","CO10","CO11","CO12","CO13","CO14","CO15","CO16","CO17","CO18","CO19","CO20","CO21","CO22","CO23","CO24","CO25","CO26","CO27","CO28","CO29","CO30","CO31","CO32","CO33","CO34","CO35","CO36","CO37","CO38","CO39","CO40","CO41","CO42","CO43","CO44","CO45","CO46","CO47","CO48","CO49","CO50","CO51","CO52","CO53","CO54","CO55","CO56","CO57","CO58","CO59","CO60","CO61","CO62" from (
 select distinct y.* from a x join a y on X.CO39 = y.co5
 --intersect
 minus
 select distinct x.* from a x join a y on X.CO39 = y.co5
 );

-- ----------------------------
-- View structure for REST_V
-- ----------------------------
CREATE OR REPLACE FORCE VIEW "BING"."REST_V" AS 
select "CO1","CO2","CO3","CO4","CO5","CO6","CO7","CO8","CO9","CO10","CO11","CO12","CO13","CO14","CO15","CO16","CO17","CO18","CO19","CO20","CO21","CO22","CO23","CO24","CO25","CO26","CO27","CO28","CO29","CO30","CO31","CO32","CO33","CO34","CO35","CO36","CO37","CO38","CO39","CO40","CO41","CO42","CO43","CO44","CO45","CO46","CO47","CO48","CO49","CO50","CO51","CO52","CO53","CO54","CO55","CO56","CO57","CO58","CO59","CO60","CO61","CO62" from a
 minus
 select y."CO1",y."CO2",y."CO3",y."CO4",y."CO5",y."CO6",y."CO7",y."CO8",y."CO9",y."CO10",y."CO11",y."CO12",y."CO13",y."CO14",y."CO15",y."CO16",y."CO17",y."CO18",y."CO19",y."CO20",y."CO21",y."CO22",y."CO23",y."CO24",y."CO25",y."CO26",y."CO27",y."CO28",y."CO29",y."CO30",y."CO31",y."CO32",y."CO33",y."CO34",y."CO35",y."CO36",y."CO37",y."CO38",y."CO39",y."CO40",y."CO41",y."CO42",y."CO43",y."CO44",y."CO45",y."CO46",y."CO47",y."CO48",y."CO49",y."CO50",y."CO51",y."CO52",y."CO53",y."CO54",y."CO55",y."CO56",y."CO57",y."CO58",y."CO59",y."CO60",y."CO61",y."CO62" from a x join a y on X.CO39 = y.co5
 minus
 select x."CO1",x."CO2",x."CO3",x."CO4",x."CO5",x."CO6",x."CO7",x."CO8",x."CO9",x."CO10",x."CO11",x."CO12",x."CO13",x."CO14",x."CO15",x."CO16",x."CO17",x."CO18",x."CO19",x."CO20",x."CO21",x."CO22",x."CO23",x."CO24",x."CO25",x."CO26",x."CO27",x."CO28",x."CO29",x."CO30",x."CO31",x."CO32",x."CO33",x."CO34",x."CO35",x."CO36",x."CO37",x."CO38",x."CO39",x."CO40",x."CO41",x."CO42",x."CO43",x."CO44",x."CO45",x."CO46",x."CO47",x."CO48",x."CO49",x."CO50",x."CO51",x."CO52",x."CO53",x."CO54",x."CO55",x."CO56",x."CO57",x."CO58",x."CO59",x."CO60",x."CO61",x."CO62" from a x join a y on X.CO39 = y.co5;

-- ----------------------------
-- Sequence structure for GDMS_SYSTEM_ID_SEQ
-- ----------------------------
DROP SEQUENCE "BING"."GDMS_SYSTEM_ID_SEQ";
CREATE SEQUENCE "BING"."GDMS_SYSTEM_ID_SEQ"
 INCREMENT BY 1
 MINVALUE 1
 MAXVALUE 9999999999999999999999999999
 START WITH 21
 NOCACHE ;

-- ----------------------------
-- Indexes structure for table GDMS_DEV_MAIN
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_DEV_MAIN
-- ----------------------------
ALTER TABLE "BING"."GDMS_DEV_MAIN" ADD CHECK ("ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_DEV_MAIN
-- ----------------------------
ALTER TABLE "BING"."GDMS_DEV_MAIN" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_DEV_MORE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_DEV_MORE
-- ----------------------------
ALTER TABLE "BING"."GDMS_DEV_MORE" ADD CHECK ("DEV_ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_DEV_MORE
-- ----------------------------
ALTER TABLE "BING"."GDMS_DEV_MORE" ADD PRIMARY KEY ("DEV_ID");

-- ----------------------------
-- Indexes structure for table GDMS_LANG
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_LANG
-- ----------------------------
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "BING"."GDMS_LANG" ADD CHECK ("XU_HAO" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_LANG
-- ----------------------------
ALTER TABLE "BING"."GDMS_LANG" ADD PRIMARY KEY ("YE_MIAN_MING", "YU_ZHONG", "XU_HAO");

-- ----------------------------
-- Indexes structure for table GDMS_PROJECT
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_PROJECT
-- ----------------------------
ALTER TABLE "BING"."GDMS_PROJECT" ADD CHECK ("ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_PROJECT
-- ----------------------------
ALTER TABLE "BING"."GDMS_PROJECT" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_SITE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_SITE
-- ----------------------------
ALTER TABLE "BING"."GDMS_SITE" ADD CHECK ("ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_SITE" ADD CHECK ("PARENT_ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_SITE" ADD CHECK ("SYSTEM_ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_SITE" ADD CHECK ("NAME" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_SITE
-- ----------------------------
ALTER TABLE "BING"."GDMS_SITE" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_STN_MAIN
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_STN_MAIN
-- ----------------------------
ALTER TABLE "BING"."GDMS_STN_MAIN" ADD CHECK ("ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_STN_MAIN" ADD CHECK ("NAME" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_STN_MAIN
-- ----------------------------
ALTER TABLE "BING"."GDMS_STN_MAIN" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_STN_MORE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_STN_MORE
-- ----------------------------
ALTER TABLE "BING"."GDMS_STN_MORE" ADD CHECK ("STN_ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_STN_MORE" ADD CHECK ("ITEM" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_STN_MORE
-- ----------------------------
ALTER TABLE "BING"."GDMS_STN_MORE" ADD PRIMARY KEY ("STN_ID", "ITEM");

-- ----------------------------
-- Indexes structure for table GDMS_STYLE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_STYLE
-- ----------------------------
ALTER TABLE "BING"."GDMS_STYLE" ADD CHECK ("ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_STYLE
-- ----------------------------
ALTER TABLE "BING"."GDMS_STYLE" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_SYSTEM
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_SYSTEM
-- ----------------------------
ALTER TABLE "BING"."GDMS_SYSTEM" ADD CHECK ("ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_SYSTEM" ADD CHECK ("NAME" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_SYSTEM
-- ----------------------------
ALTER TABLE "BING"."GDMS_SYSTEM" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_TYPE
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_TYPE
-- ----------------------------
ALTER TABLE "BING"."GDMS_TYPE" ADD CHECK ("ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_TYPE
-- ----------------------------
ALTER TABLE "BING"."GDMS_TYPE" ADD PRIMARY KEY ("ID");

-- ----------------------------
-- Indexes structure for table GDMS_USER
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_USER
-- ----------------------------
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("USER_NAME" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("PASS" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("USER_TYPE" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("USER_NAME" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("PASS" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("USER_TYPE" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER" ADD CHECK ("FAILED_LOGINS" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_USER
-- ----------------------------
ALTER TABLE "BING"."GDMS_USER" ADD PRIMARY KEY ("USER_ID");

-- ----------------------------
-- Indexes structure for table GDMS_USER_SYSTEM
-- ----------------------------

-- ----------------------------
-- Checks structure for table GDMS_USER_SYSTEM
-- ----------------------------
ALTER TABLE "BING"."GDMS_USER_SYSTEM" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "BING"."GDMS_USER_SYSTEM" ADD CHECK ("SYSTEM_ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table GDMS_USER_SYSTEM
-- ----------------------------
ALTER TABLE "BING"."GDMS_USER_SYSTEM" ADD PRIMARY KEY ("USER_ID", "SYSTEM_ID");

-- ----------------------------
-- Indexes structure for table TXL_GUID_QUANXIAN
-- ----------------------------

-- ----------------------------
-- Checks structure for table TXL_GUID_QUANXIAN
-- ----------------------------
ALTER TABLE "BING"."TXL_GUID_QUANXIAN" ADD CHECK ("GUID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table TXL_GUID_QUANXIAN
-- ----------------------------
ALTER TABLE "BING"."TXL_GUID_QUANXIAN" ADD PRIMARY KEY ("GUID");

-- ----------------------------
-- Checks structure for table TXL_JICHUSHUJU
-- ----------------------------
ALTER TABLE "BING"."TXL_JICHUSHUJU" ADD CHECK ("GUID" IS NOT NULL);
ALTER TABLE "BING"."TXL_JICHUSHUJU" ADD CHECK ("XING_MING" IS NOT NULL);
ALTER TABLE "BING"."TXL_JICHUSHUJU" ADD CHECK ("XIANG_MU" IS NOT NULL);
ALTER TABLE "BING"."TXL_JICHUSHUJU" ADD CHECK ("NEI_RONG" IS NOT NULL);

-- ----------------------------
-- Indexes structure for table TXL_USER
-- ----------------------------

-- ----------------------------
-- Checks structure for table TXL_USER
-- ----------------------------
ALTER TABLE "BING"."TXL_USER" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "BING"."TXL_USER" ADD CHECK ("USER_NAME" IS NOT NULL);
ALTER TABLE "BING"."TXL_USER" ADD CHECK ("PASS" IS NOT NULL);
ALTER TABLE "BING"."TXL_USER" ADD CHECK ("USER_TYPE" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table TXL_USER
-- ----------------------------
ALTER TABLE "BING"."TXL_USER" ADD PRIMARY KEY ("USER_ID");

-- ----------------------------
-- Indexes structure for table TXL_USER_ZU
-- ----------------------------

-- ----------------------------
-- Checks structure for table TXL_USER_ZU
-- ----------------------------
ALTER TABLE "BING"."TXL_USER_ZU" ADD CHECK ("USER_ID" IS NOT NULL);
ALTER TABLE "BING"."TXL_USER_ZU" ADD CHECK ("ZU_ID" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table TXL_USER_ZU
-- ----------------------------
ALTER TABLE "BING"."TXL_USER_ZU" ADD PRIMARY KEY ("USER_ID", "ZU_ID");

-- ----------------------------
-- Indexes structure for table TXL_YUYAN
-- ----------------------------

-- ----------------------------
-- Checks structure for table TXL_YUYAN
-- ----------------------------
ALTER TABLE "BING"."TXL_YUYAN" ADD CHECK ("YE_MIAN_MING" IS NOT NULL);
ALTER TABLE "BING"."TXL_YUYAN" ADD CHECK ("YU_ZHONG" IS NOT NULL);
ALTER TABLE "BING"."TXL_YUYAN" ADD CHECK ("XU_HAO" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table TXL_YUYAN
-- ----------------------------
ALTER TABLE "BING"."TXL_YUYAN" ADD PRIMARY KEY ("YE_MIAN_MING", "YU_ZHONG", "XU_HAO");

-- ----------------------------
-- Indexes structure for table TXL_ZU
-- ----------------------------

-- ----------------------------
-- Checks structure for table TXL_ZU
-- ----------------------------
ALTER TABLE "BING"."TXL_ZU" ADD CHECK ("ZU_ID" IS NOT NULL);
ALTER TABLE "BING"."TXL_ZU" ADD CHECK ("ZU_NAME" IS NOT NULL);
ALTER TABLE "BING"."TXL_ZU" ADD CHECK ("ZU_ID" IS NOT NULL);
ALTER TABLE "BING"."TXL_ZU" ADD CHECK ("ZU_NAME" IS NOT NULL);

-- ----------------------------
-- Primary Key structure for table TXL_ZU
-- ----------------------------
ALTER TABLE "BING"."TXL_ZU" ADD PRIMARY KEY ("ZU_ID");
