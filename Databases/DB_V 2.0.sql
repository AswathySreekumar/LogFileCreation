***

CREATE TABLE IF NOT EXISTS `activitylog` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `ACTIVITYTYPE` varchar(60) NOT NULL,
  `ACTIVITY` varchar(800) NOT NULL,
  `RESULT` varchar(800) NOT NULL,
  `CREATED_USER` char(15) DEFAULT NULL,
  `CREATED_DATE` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=4742 DEFAULT CHARSET=latin1

***



CREATE DEFINER=`root`@`%` FUNCTION `DBUSER`() RETURNS char(15) CHARSET latin1
BEGIN
    RETURN left(user(),LOCATE('@', user()) -1);
END

***
CREATE DEFINER=`root`@`%` FUNCTION `NEXTBATCHID`(V_CODE CHAR(15)) RETURNS bigint(20) unsigned
BEGIN
DECLARE V_NEXTBATCHID BIGINT UNSIGNED;
DECLARE V_CNT INT;

    SELECT NEXT_BATCHID 
		INTO V_NEXTBATCHID
		FROM DBINFO
		WHERE CODE = V_CODE;

	IF V_NEXTBATCHID IS NULL THEN
		SET V_NEXTBATCHID = 1;
		SELECT COUNT(*)
		INTO V_CNT 
		FROM DBINFO WHERE CODE = V_CODE;
	
		IF V_CNT = 0 THEN
			INSERT INTO DBINFO
				(CODE,NEXT_BATCHID)
			VALUES
				(V_CODE,V_NEXTBATCHID);
		END IF;
	END IF;
	UPDATE DBINFO SET NEXT_BATCHID = NEXT_BATCHID + 1
			WHERE CODE = V_CODE;
	
    RETURN V_NEXTBATCHID;
END

***

CREATE DEFINER=`root`@`%` TRIGGER `TR_ACTIVITYLOG_INSERT` BEFORE INSERT ON `ACTIVITYLOG`
FOR EACH ROW
BEGIN
  SET NEW.CREATED_USER = DBUSER();
  SET NEW.CREATED_DATE = NOW();
END

***


CREATE TABLE IF NOT EXISTS `dbinfo` (
  `CODE` char(15) NOT NULL DEFAULT '',
  `NEXT_BATCHID` bigint(20) unsigned NOT NULL,
  `CREATED_USER` char(15) DEFAULT NULL,
  `CREATED_DATE` datetime DEFAULT NULL,
  `MODIFIED_USER` char(15) DEFAULT NULL,
  `MODIFIED_DATE` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`CODE`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1

***

CREATE TABLE IF NOT EXISTS `errorlog` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `BATCHID` bigint(20) unsigned NOT NULL,
  `OBJECTTYPE` varchar(60) NOT NULL,
  `OBJECTNAME` varchar(60) NOT NULL,
  `COMMANDTYPE` varchar(60) NOT NULL,
  `COMMANDSTRING` varchar(8000) DEFAULT NULL,
  `DBCODE` char(15) NOT NULL,
  `CREATED_USER` char(15) DEFAULT NULL,
  `CREATED_DATE` datetime DEFAULT NULL,
  `MODIFIED_USER` char(15) DEFAULT NULL,
  `MODIFIED_DATE` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2127 DEFAULT CHARSET=latin1

***

CREATE DEFINER=`root`@`localhost` TRIGGER `TR_ERRORLOG_INSERT` BEFORE INSERT ON `errorlog` FOR EACH ROW BEGIN
	SET NEW.CREATED_USER=DBUSER();
	SET NEW.CREATED_DATE=NOW();
END

***

CREATE TABLE IF NOT EXISTS `replicationinfo` (
  `CODE` char(15) NOT NULL,
  `FIRSTID` bigint(20) unsigned NOT NULL DEFAULT '0',
  `LASTCONNECTION` datetime DEFAULT NULL,
  `MINIMUMTIMEGAP` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`CODE`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1

***

CREATE TABLE IF NOT EXISTS `replicationlog` (
  `DBCODE` char(15) NOT NULL,
  `ID` bigint(20) unsigned NOT NULL,
  `BATCHID` bigint(20) unsigned NOT NULL,
  `OBJECTTYPE` varchar(60) NOT NULL,
  `OBJECTNAME` varchar(60) NOT NULL,
  `COMMANDTYPE` varchar(60) NOT NULL,
  `COMMANDSTRING` varchar(8000) DEFAULT NULL,
  `COMMANDSTATUS` enum('Pending','Success','Failed') DEFAULT 'Pending',
  `UPDATESTATUS` enum('Pending','Success','Failed') DEFAULT 'Pending',
  `REMARKS1` varchar(800) DEFAULT NULL,
  `REMARKS2` varchar(800) DEFAULT NULL,
  `SKIP` enum('Yes','No') DEFAULT 'No',
  `CREATED_USER` char(15) NOT NULL,
  `CREATED_DATE` datetime DEFAULT NULL,
  `MODIFIED_USER` char(15) NOT NULL,
  `MODIFIED_DATE` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`DBCODE`,`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1

***

CREATE TABLE IF NOT EXISTS `sqllog` (
  `ID` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `BATCHID` bigint(20) unsigned NOT NULL,
  `OBJECTTYPE` varchar(60) NOT NULL,
  `OBJECTNAME` varchar(60) NOT NULL,
  `COMMANDTYPE` varchar(60) NOT NULL,
  `COMMANDSTRING` varchar(8000) DEFAULT NULL,
  `DBCODE` char(15) NOT NULL,
  `CREATED_USER` char(15) DEFAULT NULL,
  `CREATED_DATE` datetime DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2127 DEFAULT CHARSET=latin1

***

CREATE DEFINER=`root`@`localhost` TRIGGER TR_SQLLOG_INSERT BEFORE INSERT ON SQLLOG
FOR EACH ROW 
BEGIN
  SET NEW.CREATED_USER = DBUSER();
  SET NEW.CREATED_DATE = NOW();
END

***


