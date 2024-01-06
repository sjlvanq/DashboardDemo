CREATE PROCEDURE AsignarMozoAMesaEnTurno
    @TurnoId INT,
    @MesaId INT,
    @MozoDNI VARCHAR(12)
AS
BEGIN
    DECLARE @MozoId NVARCHAR(256);

    SELECT @MozoId = Id
    FROM AspNetUsers
    WHERE UserName = @MozoDNI;

    UPDATE DDAsignaciones
    SET MozoId = @MozoId
    WHERE TurnoId = @TurnoId AND MesaId = @MesaId;
END;