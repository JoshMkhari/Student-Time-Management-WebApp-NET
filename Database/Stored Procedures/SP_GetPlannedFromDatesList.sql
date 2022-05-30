CREATE PROCEDURE SP_GetPlannedFromDatesList
(
 @STORED_DATE DATE = ' ',
 @USERS_NAME VARCHAR (50) = ' '
)
AS
BEGIN
	SELECT PLANNED_ID FROM DATES_LIST
	WHERE @STORED_DATE = STORED_DATE
	AND @USERS_NAME = USERS_NAME
END

