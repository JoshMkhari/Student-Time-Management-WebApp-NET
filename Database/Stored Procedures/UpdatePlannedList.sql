CREATE PROCEDURE SP_UpdatePlannedList
(
 @PLANNED_ID INT = 0,
 @MODULES_ID VARCHAR(8) ='',
 @HOURS INT = 0
)
AS
BEGIN
	UPDATE PLANNED_LIST
	SET [HOURS] = @HOURS
	WHERE PLANNED_ID = @PLANNED_ID
	AND MODULES_ID = @MODULES_ID
END

