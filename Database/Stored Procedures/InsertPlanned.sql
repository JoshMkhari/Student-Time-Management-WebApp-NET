CREATE PROCEDURE SP_InsertPlanned
(
 @PLANNED_ID INT = 0
)
AS
BEGIN
INSERT INTO PLANNED (PLANNED_ID)
VALUES (@PLANNED_ID)
END
