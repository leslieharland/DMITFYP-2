USE dmitfypsystemdb
GO
DECLARE DelAllProcedures CURSOR
FOR
    SELECT name AS procedure_name 
    FROM sys.procedures;
OPEN DelAllProcedures
DECLARE @ProcName VARCHAR(100)
FETCH NEXT 
FROM DelAllProcedures
INTO @ProcName
WHILE @@FETCH_STATUS!=-1
BEGIN 
    DECLARE @command VARCHAR(100)
    SET @command=''
    SET @command=@command+'DROP PROCEDURE '+@ProcName
    --DROP PROCEDURE  @ProcName
    EXECUTE (@command)
    FETCH NEXT 
    FROM DelAllProcedures
    INTO @ProcName
END
CLOSE DelAllProcedures
DEALLOCATE DelAllProcedures