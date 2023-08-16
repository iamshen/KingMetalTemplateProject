-- Orleans 数据库集群表 多合一SQL
-- （PostgreSQL-Main.sql 、PostgreSQL-Clustering.sql、PostgreSQL-Persistence.sql、PostgreSQL-Reminders.sql）
-- 以下脚本执行在 gold_cluster 模式下。

-- requires Postgres 9.5 (or perhaps higher)
/*
Implementation notes:
 
1) The general idea is that data is read and written through Orleans specific queries.
Orleans operates on column names and types when reading and on parameter names and types when writing.
 
2) The implementations *must* preserve input and output names and types. Orleans uses these parameters to reads query results by name and type.
Vendor and deployment specific tuning is allowed and contributions are encouraged as long as the interface contract
is maintained.
 
3) The implementation across vendor specific scripts *should* preserve the constraint names. This simplifies troubleshooting
by virtue of uniform naming across concrete implementations.
 
5) ETag for Orleans is an opaque column that represents a unique version. The type of its actual implementation
is not important as long as it represents a unique version. In this implementation we use integers for versioning
 
6) For the sake of being explicit and removing ambiguity, Orleans expects some queries to return either TRUE as >0 value
or FALSE as =0 value. That is, affected rows or such does not matter. If an error is raised or an exception is thrown
the query *must* ensure the entire transaction is rolled back and may either return FALSE or propagate the exception.
Orleans handles exception as a failure and will retry.
 
7) The implementation follows the Extended Orleans membership protocol. For more information, see at:
https://docs.microsoft.com/dotnet/orleans/implementation/cluster-management
https://github.com/dotnet/orleans/blob/main/src/Orleans.Core/SystemTargetInterfaces/IMembershipTable.cs
*/


-- This table defines Orleans operational queries. Orleans uses these to manage its operations,
-- these are the only queries Orleans issues to the database.
-- These can be redefined (e.g. to provide non-destructive updates) provided the stated interface principles hold.
CREATE TABLE OrleansQuery
(
    QueryKey  varchar(64)   NOT NULL,
    QueryText varchar(8000) NOT NULL,

    CONSTRAINT OrleansQuery_Key PRIMARY KEY (QueryKey)
);


-- PostgreSQL-Clustering.sql
-- PostgreSQL-Clustering.sql
-- PostgreSQL-Clustering.sql

-- For each deployment, there will be only one (active) membership version table version column which will be updated periodically.
CREATE TABLE OrleansMembershipVersionTable
(
    DeploymentId varchar(150)   NOT NULL,
    Timestamp    timestamptz(3) NOT NULL DEFAULT now(),
    Version      integer        NOT NULL DEFAULT 0,

    CONSTRAINT PK_OrleansMembershipVersionTable_DeploymentId PRIMARY KEY (DeploymentId)
);

-- Every silo instance has a row in the membership table.
CREATE TABLE OrleansMembershipTable
(
    DeploymentId varchar(150)   NOT NULL,
    Address      varchar(45)    NOT NULL,
    Port         integer        NOT NULL,
    Generation   integer        NOT NULL,
    SiloName     varchar(150)   NOT NULL,
    HostName     varchar(150)   NOT NULL,
    Status       integer        NOT NULL,
    ProxyPort    integer        NULL,
    SuspectTimes varchar(8000)  NULL,
    StartTime    timestamptz(3) NOT NULL,
    IAmAliveTime timestamptz(3) NOT NULL,

    CONSTRAINT PK_MembershipTable_DeploymentId PRIMARY KEY (DeploymentId, Address, Port, Generation),
    CONSTRAINT FK_MembershipTable_MembershipVersionTable_DeploymentId FOREIGN KEY (DeploymentId) REFERENCES OrleansMembershipVersionTable (DeploymentId)
);

CREATE FUNCTION update_i_am_alive_time(
    deployment_id OrleansMembershipTable.DeploymentId%TYPE,
    address_arg OrleansMembershipTable.Address%TYPE,
    port_arg OrleansMembershipTable.Port%TYPE,
    generation_arg OrleansMembershipTable.Generation%TYPE,
    i_am_alive_time OrleansMembershipTable.IAmAliveTime%TYPE)
    RETURNS void AS
$func$
BEGIN
    -- This is expected to never fail by Orleans, so return value
-- is not needed nor is it checked.
    UPDATE OrleansMembershipTable as d
    SET IAmAliveTime = i_am_alive_time
    WHERE d.DeploymentId = deployment_id
      AND deployment_id IS NOT NULL
      AND d.Address = address_arg
      AND address_arg IS NOT NULL
      AND d.Port = port_arg
      AND port_arg IS NOT NULL
      AND d.Generation = generation_arg
      AND generation_arg IS NOT NULL;
END
$func$ LANGUAGE plpgsql;

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('UpdateIAmAlivetimeKey', '
-- This is expected to never fail by Orleans, so return value
-- is not needed nor is it checked.
SELECT * from update_i_am_alive_time(
@DeploymentId,
@Address,
@Port,
@Generation,
@IAmAliveTime
);
');

CREATE FUNCTION insert_membership_version(
    DeploymentIdArg OrleansMembershipTable.DeploymentId%TYPE
)
    RETURNS TABLE
            (
                row_count integer
            )
AS
$func$
DECLARE
    RowCountVar int := 0;
BEGIN

    BEGIN

        INSERT INTO OrleansMembershipVersionTable
        (DeploymentId)
        SELECT DeploymentIdArg
        ON CONFLICT (DeploymentId) DO NOTHING;

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        ASSERT RowCountVar <> 0, 'no rows affected, rollback';

        RETURN QUERY SELECT RowCountVar;
    EXCEPTION
        WHEN assert_failure THEN
    RETURN QUERY SELECT RowCountVar;
END; END
$func$ LANGUAGE plpgsql;

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('InsertMembershipVersionKey', '
SELECT * FROM insert_membership_version(
@DeploymentId
);
');

CREATE FUNCTION insert_membership(
    DeploymentIdArg OrleansMembershipTable.DeploymentId%TYPE,
    AddressArg OrleansMembershipTable.Address%TYPE,
    PortArg OrleansMembershipTable.Port%TYPE,
    GenerationArg OrleansMembershipTable.Generation%TYPE,
    SiloNameArg OrleansMembershipTable.SiloName%TYPE,
    HostNameArg OrleansMembershipTable.HostName%TYPE,
    StatusArg OrleansMembershipTable.Status%TYPE,
    ProxyPortArg OrleansMembershipTable.ProxyPort%TYPE,
    StartTimeArg OrleansMembershipTable.StartTime%TYPE,
    IAmAliveTimeArg OrleansMembershipTable.IAmAliveTime%TYPE,
    VersionArg OrleansMembershipVersionTable.Version%TYPE)
    RETURNS TABLE
            (
                row_count integer
            )
AS
$func$
DECLARE
    RowCountVar int := 0;
BEGIN

    BEGIN
        INSERT INTO OrleansMembershipTable
        (DeploymentId,
         Address,
         Port,
         Generation,
         SiloName,
         HostName,
         Status,
         ProxyPort,
         StartTime,
         IAmAliveTime)
        SELECT DeploymentIdArg,
               AddressArg,
               PortArg,
               GenerationArg,
               SiloNameArg,
               HostNameArg,
               StatusArg,
               ProxyPortArg,
               StartTimeArg,
               IAmAliveTimeArg
        ON CONFLICT (DeploymentId, Address, Port, Generation) DO
            NOTHING;


        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        UPDATE OrleansMembershipVersionTable
        SET Timestamp = now(),
            Version   = Version + 1
        WHERE DeploymentId = DeploymentIdArg
          AND DeploymentIdArg IS NOT NULL
          AND Version = VersionArg
          AND VersionArg IS NOT NULL
          AND RowCountVar > 0;

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        ASSERT RowCountVar <> 0, 'no rows affected, rollback';


        RETURN QUERY SELECT RowCountVar;
    EXCEPTION
        WHEN assert_failure THEN
    RETURN QUERY SELECT RowCountVar;
END; END
$func$ LANGUAGE plpgsql;

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('InsertMembershipKey', '
SELECT * FROM insert_membership(
@DeploymentId,
@Address,
@Port,
@Generation,
@SiloName,
@HostName,
@Status,
@ProxyPort,
@StartTime,
@IAmAliveTime,
@Version
);
');

CREATE FUNCTION update_membership(
    DeploymentIdArg OrleansMembershipTable.DeploymentId%TYPE,
    AddressArg OrleansMembershipTable.Address%TYPE,
    PortArg OrleansMembershipTable.Port%TYPE,
    GenerationArg OrleansMembershipTable.Generation%TYPE,
    StatusArg OrleansMembershipTable.Status%TYPE,
    SuspectTimesArg OrleansMembershipTable.SuspectTimes%TYPE,
    IAmAliveTimeArg OrleansMembershipTable.IAmAliveTime%TYPE,
    VersionArg OrleansMembershipVersionTable.Version%TYPE
)
    RETURNS TABLE
            (
                row_count integer
            )
AS
$func$
DECLARE
    RowCountVar int := 0;
BEGIN

    BEGIN

        UPDATE OrleansMembershipVersionTable
        SET Timestamp = now(),
            Version   = Version + 1
        WHERE DeploymentId = DeploymentIdArg
          AND DeploymentIdArg IS NOT NULL
          AND Version = VersionArg
          AND VersionArg IS NOT NULL;


        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        UPDATE OrleansMembershipTable
        SET Status       = StatusArg,
            SuspectTimes = SuspectTimesArg,
            IAmAliveTime = IAmAliveTimeArg
        WHERE DeploymentId = DeploymentIdArg
          AND DeploymentIdArg IS NOT NULL
          AND Address = AddressArg
          AND AddressArg IS NOT NULL
          AND Port = PortArg
          AND PortArg IS NOT NULL
          AND Generation = GenerationArg
          AND GenerationArg IS NOT NULL
          AND RowCountVar > 0;


        GET DIAGNOSTICS RowCountVar = ROW_COUNT;

        ASSERT RowCountVar <> 0, 'no rows affected, rollback';


        RETURN QUERY SELECT RowCountVar;
    EXCEPTION
        WHEN assert_failure THEN
    RETURN QUERY SELECT RowCountVar;
END; END
$func$ LANGUAGE plpgsql;

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('UpdateMembershipKey', '
SELECT * FROM update_membership(
@DeploymentId,
@Address,
@Port,
@Generation,
@Status,
@SuspectTimes,
@IAmAliveTime,
@Version
);
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('MembershipReadRowKey', '
SELECT
v.DeploymentId,
m.Address,
m.Port,
m.Generation,
m.SiloName,
m.HostName,
m.Status,
m.ProxyPort,
m.SuspectTimes,
m.StartTime,
m.IAmAliveTime,
v.Version
FROM
OrleansMembershipVersionTable v
-- This ensures the version table will returned even if there is no matching membership row.
LEFT OUTER JOIN OrleansMembershipTable m ON v.DeploymentId = m.DeploymentId
AND Address = @Address AND @Address IS NOT NULL
AND Port = @Port AND @Port IS NOT NULL
AND Generation = @Generation AND @Generation IS NOT NULL
WHERE
v.DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('MembershipReadAllKey', '
SELECT
v.DeploymentId,
m.Address,
m.Port,
m.Generation,
m.SiloName,
m.HostName,
m.Status,
m.ProxyPort,
m.SuspectTimes,
m.StartTime,
m.IAmAliveTime,
v.Version
FROM
OrleansMembershipVersionTable v LEFT OUTER JOIN OrleansMembershipTable m
ON v.DeploymentId = m.DeploymentId
WHERE
v.DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('DeleteMembershipTableEntriesKey', '
DELETE FROM OrleansMembershipTable
WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
DELETE FROM OrleansMembershipVersionTable
WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('GatewaysQueryKey', '
SELECT
Address,
ProxyPort,
Generation
FROM
OrleansMembershipTable
WHERE
DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
AND Status = @Status AND @Status IS NOT NULL
AND ProxyPort > 0;
');

-- PostgreSQL-Persistence.sql
-- PostgreSQL-Persistence.sql
-- PostgreSQL-Persistence.sql

CREATE TABLE OrleansStorage
(
    grainidhash            integer                     NOT NULL,
    grainidn0              bigint                      NOT NULL,
    grainidn1              bigint                      NOT NULL,
    graintypehash          integer                     NOT NULL,
    graintypestring        character varying(512)      NOT NULL,
    grainidextensionstring character varying(512),
    serviceid              character varying(150)      NOT NULL,
    payloadbinary          bytea,
    payloadxml             xml,
    payloadjson            text,
    modifiedon             timestamp without time zone NOT NULL,
    version                integer
);

CREATE INDEX ix_orleansstorage
    ON orleansstorage USING btree
        (grainidhash, graintypehash);

CREATE OR REPLACE FUNCTION writetostorage(
    _grainidhash integer,
    _grainidn0 bigint,
    _grainidn1 bigint,
    _graintypehash integer,
    _graintypestring character varying,
    _grainidextensionstring character varying,
    _serviceid character varying,
    _grainstateversion integer,
    _payloadbinary bytea,
    _payloadjson text,
    _payloadxml xml)
    RETURNS TABLE
            (
                newgrainstateversion integer
            )
    LANGUAGE 'plpgsql'
AS
$function$
DECLARE
    _newGrainStateVersion integer := _GrainStateVersion;
    RowCountVar           integer := 0;

BEGIN

    -- Grain state is not null, so the state must have been read from the storage before.
-- Let's try to update it.
--
-- When Orleans is running in normal, non-split state, there will
-- be only one grain with the given ID and type combination only. This
-- grain saves states mostly serially if Orleans guarantees are upheld. Even
-- if not, the updates should work correctly due to version number.
--
-- In split brain situations there can be a situation where there are two or more
-- grains with the given ID and type combination. When they try to INSERT
-- concurrently, the table needs to be locked pessimistically before one of
-- the grains gets @GrainStateVersion = 1 in return and the other grains will fail
-- to update storage. The following arrangement is made to reduce locking in normal operation.
--
-- If the version number explicitly returned is still the same, Orleans interprets it so the update did not succeed
-- and throws an InconsistentStateException.
--
-- See further information at https://docs.microsoft.com/dotnet/orleans/grains/grain-persistence.
    IF _GrainStateVersion IS NOT NULL
    THEN
        UPDATE OrleansStorage
        SET PayloadBinary = _PayloadBinary,
            PayloadJson   = _PayloadJson,
            PayloadXml    = _PayloadXml,
            ModifiedOn    = (now() at time zone 'utc'),
            Version       = Version + 1

        WHERE GrainIdHash = _GrainIdHash
          AND _GrainIdHash IS NOT NULL
          AND GrainTypeHash = _GrainTypeHash
          AND _GrainTypeHash IS NOT NULL
          AND GrainIdN0 = _GrainIdN0
          AND _GrainIdN0 IS NOT NULL
          AND GrainIdN1 = _GrainIdN1
          AND _GrainIdN1 IS NOT NULL
          AND GrainTypeString = _GrainTypeString
          AND _GrainTypeString IS NOT NULL
          AND ((_GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND
                GrainIdExtensionString = _GrainIdExtensionString) OR
               _GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
          AND ServiceId = _ServiceId
          AND _ServiceId IS NOT NULL
          AND Version IS NOT NULL
          AND Version = _GrainStateVersion
          AND _GrainStateVersion IS NOT NULL;

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;
        IF RowCountVar > 0
        THEN
            _newGrainStateVersion := _GrainStateVersion + 1;
        END IF;
    END IF;

    -- The grain state has not been read. The following locks rather pessimistically
-- to ensure only one INSERT succeeds.
    IF _GrainStateVersion IS NULL
    THEN
        INSERT INTO OrleansStorage
        (GrainIdHash,
         GrainIdN0,
         GrainIdN1,
         GrainTypeHash,
         GrainTypeString,
         GrainIdExtensionString,
         ServiceId,
         PayloadBinary,
         PayloadJson,
         PayloadXml,
         ModifiedOn,
         Version)
        SELECT _GrainIdHash,
               _GrainIdN0,
               _GrainIdN1,
               _GrainTypeHash,
               _GrainTypeString,
               _GrainIdExtensionString,
               _ServiceId,
               _PayloadBinary,
               _PayloadJson,
               _PayloadXml,
               (now() at time zone 'utc'),
               1
        WHERE NOT EXISTS
                  (
-- There should not be any version of this grain state.
                      SELECT 1
                      FROM OrleansStorage
                      WHERE GrainIdHash = _GrainIdHash
                        AND _GrainIdHash IS NOT NULL
                        AND GrainTypeHash = _GrainTypeHash
                        AND _GrainTypeHash IS NOT NULL
                        AND GrainIdN0 = _GrainIdN0
                        AND _GrainIdN0 IS NOT NULL
                        AND GrainIdN1 = _GrainIdN1
                        AND _GrainIdN1 IS NOT NULL
                        AND GrainTypeString = _GrainTypeString
                        AND _GrainTypeString IS NOT NULL
                        AND ((_GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND
                              GrainIdExtensionString = _GrainIdExtensionString) OR
                             _GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
                        AND ServiceId = _ServiceId
                        AND _ServiceId IS NOT NULL);

        GET DIAGNOSTICS RowCountVar = ROW_COUNT;
        IF RowCountVar > 0
        THEN
            _newGrainStateVersion := 1;
        END IF;
    END IF;

    RETURN QUERY SELECT _newGrainStateVersion AS NewGrainStateVersion;
END

$function$;

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('WriteToStorageKey', '
 
select * from WriteToStorage(@GrainIdHash, @GrainIdN0, @GrainIdN1, @GrainTypeHash, @GrainTypeString, @GrainIdExtensionString, @ServiceId, @GrainStateVersion, @PayloadBinary, @PayloadJson, CAST(@PayloadXml AS xml));
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('ReadFromStorageKey', '
SELECT
PayloadBinary,
PayloadXml,
PayloadJson,
(now() at time zone ''utc''),
Version
FROM
OrleansStorage
WHERE
GrainIdHash = @GrainIdHash
AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
AND GrainIdN0 = @GrainIdN0 AND @GrainIdN0 IS NOT NULL
AND GrainIdN1 = @GrainIdN1 AND @GrainIdN1 IS NOT NULL
AND GrainTypeString = @GrainTypeString AND GrainTypeString IS NOT NULL
AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('ClearStorageKey', '
UPDATE OrleansStorage
SET
PayloadBinary = NULL,
PayloadJson = NULL,
PayloadXml = NULL,
Version = Version + 1
WHERE
GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
AND GrainIdN0 = @GrainIdN0 AND @GrainIdN0 IS NOT NULL
AND GrainIdN1 = @GrainIdN1 AND @GrainIdN1 IS NOT NULL
AND GrainTypeString = @GrainTypeString AND @GrainTypeString IS NOT NULL
AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
AND Version IS NOT NULL AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL
Returning Version as NewGrainStateVersion
');



-- PostgreSQL-Reminders.sql
-- PostgreSQL-Reminders.sql
-- PostgreSQL-Reminders.sql

-- Orleans Reminders table - https://docs.microsoft.com/dotnet/orleans/grains/timers-and-reminders
CREATE TABLE OrleansRemindersTable
(
    ServiceId    varchar(150)   NOT NULL,
    GrainId      varchar(150)   NOT NULL,
    ReminderName varchar(150)   NOT NULL,
    StartTime    timestamptz(3) NOT NULL,
    Period       bigint         NOT NULL,
    GrainHash    integer        NOT NULL,
    Version      integer        NOT NULL,

    CONSTRAINT PK_RemindersTable_ServiceId_GrainId_ReminderName PRIMARY KEY (ServiceId, GrainId, ReminderName)
);

CREATE FUNCTION upsert_reminder_row(
    ServiceIdArg OrleansRemindersTable.ServiceId%TYPE,
    GrainIdArg OrleansRemindersTable.GrainId%TYPE,
    ReminderNameArg OrleansRemindersTable.ReminderName%TYPE,
    StartTimeArg OrleansRemindersTable.StartTime%TYPE,
    PeriodArg OrleansRemindersTable.Period%TYPE,
    GrainHashArg OrleansRemindersTable.GrainHash%TYPE
)
    RETURNS TABLE
            (
                version integer
            )
AS
$func$
DECLARE
    VersionVar int := 0;
BEGIN

    INSERT INTO OrleansRemindersTable
    (ServiceId,
     GrainId,
     ReminderName,
     StartTime,
     Period,
     GrainHash,
     Version)
    SELECT ServiceIdArg,
           GrainIdArg,
           ReminderNameArg,
           StartTimeArg,
           PeriodArg,
           GrainHashArg,
           0
    ON CONFLICT (ServiceId, GrainId, ReminderName)
        DO UPDATE SET StartTime = excluded.StartTime,
                      Period    = excluded.Period,
                      GrainHash = excluded.GrainHash,
                      Version   = OrleansRemindersTable.Version + 1
    RETURNING
        OrleansRemindersTable.Version INTO STRICT VersionVar;

    RETURN QUERY SELECT VersionVar AS versionr;

END
$func$ LANGUAGE plpgsql;

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('UpsertReminderRowKey', '
SELECT * FROM upsert_reminder_row(
@ServiceId,
@GrainId,
@ReminderName,
@StartTime,
@Period,
@GrainHash
);
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('ReadReminderRowsKey', '
SELECT
GrainId,
ReminderName,
StartTime,
Period,
Version
FROM OrleansRemindersTable
WHERE
ServiceId = @ServiceId AND @ServiceId IS NOT NULL
AND GrainId = @GrainId AND @GrainId IS NOT NULL;
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('ReadReminderRowKey', '
SELECT
GrainId,
ReminderName,
StartTime,
Period,
Version
FROM OrleansRemindersTable
WHERE
ServiceId = @ServiceId AND @ServiceId IS NOT NULL
AND GrainId = @GrainId AND @GrainId IS NOT NULL
AND ReminderName = @ReminderName AND @ReminderName IS NOT NULL;
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('ReadRangeRows1Key', '
SELECT
GrainId,
ReminderName,
StartTime,
Period,
Version
FROM OrleansRemindersTable
WHERE
ServiceId = @ServiceId AND @ServiceId IS NOT NULL
AND GrainHash > @BeginHash AND @BeginHash IS NOT NULL
AND GrainHash <= @EndHash AND @EndHash IS NOT NULL;
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('ReadRangeRows2Key', '
SELECT
GrainId,
ReminderName,
StartTime,
Period,
Version
FROM OrleansRemindersTable
WHERE
ServiceId = @ServiceId AND @ServiceId IS NOT NULL
AND ((GrainHash > @BeginHash AND @BeginHash IS NOT NULL)
OR (GrainHash <= @EndHash AND @EndHash IS NOT NULL));
');

CREATE FUNCTION delete_reminder_row(
    ServiceIdArg OrleansRemindersTable.ServiceId%TYPE,
    GrainIdArg OrleansRemindersTable.GrainId%TYPE,
    ReminderNameArg OrleansRemindersTable.ReminderName%TYPE,
    VersionArg OrleansRemindersTable.Version%TYPE
)
    RETURNS TABLE
            (
                row_count integer
            )
AS
$func$
DECLARE
    RowCountVar int := 0;
BEGIN


    DELETE
    FROM OrleansRemindersTable
    WHERE ServiceId = ServiceIdArg
      AND ServiceIdArg IS NOT NULL
      AND GrainId = GrainIdArg
      AND GrainIdArg IS NOT NULL
      AND ReminderName = ReminderNameArg
      AND ReminderNameArg IS NOT NULL
      AND Version = VersionArg
      AND VersionArg IS NOT NULL;

    GET DIAGNOSTICS RowCountVar = ROW_COUNT;

    RETURN QUERY SELECT RowCountVar;

END
$func$ LANGUAGE plpgsql;

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('DeleteReminderRowKey', '
SELECT * FROM delete_reminder_row(
@ServiceId,
@GrainId,
@ReminderName,
@Version
);
');

INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES ('DeleteReminderRowsKey', '
DELETE FROM OrleansRemindersTable
WHERE
ServiceId = @ServiceId AND @ServiceId IS NOT NULL;
');
