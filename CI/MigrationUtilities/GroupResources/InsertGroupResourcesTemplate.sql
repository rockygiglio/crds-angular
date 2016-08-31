-- Use a temporary stored proc to do the inserts and to report some status at the end, also handling rollback if necessary
DROP PROCEDURE IF EXISTS `SS_mysite`.`sp_insert_group_resources`;

DELIMITER $$
CREATE PROCEDURE `SS_mysite`.`sp_insert_group_resources`()
BEGIN
    DECLARE `_rollback` BOOL DEFAULT 0;
    DECLARE `@catId` INT DEFAULT 0;
    DECLARE `@imageId` INT DEFAULT 0;
    DECLARE `@pdfId` INT DEFAULT 0;
    DECLARE `@resourceId` INT DEFAULT 0;
    DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET `_rollback` = 1;
    
    START TRANSACTION;

    -- BEGIN: Generated SQL INSERTS go here
    SQL_INSERTS
    -- END: Generated SQL INSERTS go here

    IF `_rollback` THEN
        ROLLBACK;
    ELSE
        COMMIT;
    END IF;
END$$

DELIMITER ;

CALL `SS_mysite`.`sp_insert_group_resources`;

DROP PROCEDURE IF EXISTS `SS_mysite`.`sp_insert_group_resources`;