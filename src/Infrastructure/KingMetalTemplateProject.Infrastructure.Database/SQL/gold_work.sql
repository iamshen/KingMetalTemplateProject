DROP TABLE IF EXISTS "gold_work"."gp_operate_record";
CREATE TABLE "gold_work"."gp_operate_record"
(
    "id"                  int8 NOT NULL,
    "operator_id"         TEXT COLLATE "pg_catalog"."default",
    "operator_user_name"  TEXT COLLATE "pg_catalog"."default",
    "operator_real_name"  TEXT COLLATE "pg_catalog"."default",
    "biz_type"            int2 NOT NULL,
    "actorId"             int8,
    "request_client_info" jsonb,
    "attach_data"         jsonb,
    "subject_id"          int8,
    "subject_remark"      TEXT COLLATE "pg_catalog"."default",
    "remark"              TEXT COLLATE "pg_catalog"."default",
    "operated_time"       int8 NOT NULL
) PARTITION BY LIST ( "biz_type" "pg_catalog"."int2_ops" );
CREATE TABLE "gold_work"."gp_operate_record_1" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '1' );
CREATE TABLE "gold_work"."gp_operate_record_2" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '2' );
CREATE TABLE "gold_work"."gp_operate_record_3" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '3' );
CREATE TABLE "gold_work"."gp_operate_record_4" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '4' );
CREATE TABLE "gold_work"."gp_operate_record_5" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '5' );
CREATE TABLE "gold_work"."gp_operate_record_6" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '6' );
CREATE TABLE "gold_work"."gp_operate_record_7" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '7' );
CREATE TABLE "gold_work"."gp_operate_record_8" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '8' );
CREATE TABLE "gold_work"."gp_operate_record_9" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '9' );
CREATE TABLE "gold_work"."gp_operate_record_10" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '10' );
CREATE TABLE "gold_work"."gp_operate_record_11" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '11' );
CREATE TABLE "gold_work"."gp_operate_record_12" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '12' );
CREATE TABLE "gold_work"."gp_operate_record_13" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '13' );
CREATE TABLE "gold_work"."gp_operate_record_14" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '14' );
CREATE TABLE "gold_work"."gp_operate_record_15" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '15' );
CREATE TABLE "gold_work"."gp_operate_record_16" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '16' );
CREATE TABLE "gold_work"."gp_operate_record_17" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '17' );
CREATE TABLE "gold_work"."gp_operate_record_18" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '18' );
CREATE TABLE "gold_work"."gp_operate_record_19" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '19' );
CREATE TABLE "gold_work"."gp_operate_record_20" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '20' );
CREATE TABLE "gold_work"."gp_operate_record_21" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '21' );
CREATE TABLE "gold_work"."gp_operate_record_22" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '22' );
CREATE TABLE "gold_work"."gp_operate_record_23" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '23' );
CREATE TABLE "gold_work"."gp_operate_record_24" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '24' );
CREATE TABLE "gold_work"."gp_operate_record_25" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '25' );
CREATE TABLE "gold_work"."gp_operate_record_26" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '26' );
CREATE TABLE "gold_work"."gp_operate_record_27" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '27' );
CREATE TABLE "gold_work"."gp_operate_record_28" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '28' );
CREATE TABLE "gold_work"."gp_operate_record_29" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '29' );
CREATE TABLE "gold_work"."gp_operate_record_30" PARTITION OF "gold_work"."gp_operate_record" FOR VALUES IN ( '30' );
COMMENT ON COLUMN "gold_work"."gp_operate_record"."id" IS 'Id';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."operator_id" IS '操作人Id';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."operator_user_name" IS '操作人用户名';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."operator_real_name" IS '操作人真实姓名';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."biz_type" IS '业务类型';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."actorId" IS 'ActorId';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."request_client_info" IS '客户端信息';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."attach_data" IS '附加数据';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."subject_id" IS '操作子项Id, 例如操作某个子项的Id';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."subject_remark" IS '操作子项说明, 例如操作某个子项的说明';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."remark" IS '备注';
COMMENT ON COLUMN "gold_work"."gp_operate_record"."operated_time" IS '操作时间';