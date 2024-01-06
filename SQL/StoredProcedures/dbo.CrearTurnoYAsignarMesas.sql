CREATE PROCEDURE CrearTurnoYAsignarMesas
    @HoraInicio DATETIME,
    @HoraFin DATETIME
AS
BEGIN
    DECLARE @NuevoTurnoId INT;
	BEGIN TRANSACTION;
	BEGIN TRY
		INSERT INTO DDTurnos (HoraInicio, HoraFin)
		VALUES (@HoraInicio, @HoraFin);

		SET @NuevoTurnoId = SCOPE_IDENTITY();
		
		INSERT INTO DDAsignaciones (TurnoId, MesaId)
		SELECT @NuevoTurnoId, MesaId
		FROM DDMesas
		WHERE AsignarANuevosTurnos = 1
		AND IsActive = 1;
		COMMIT
	END TRY
	BEGIN CATCH
		ROLLBACK;
	END CATCH
END;
