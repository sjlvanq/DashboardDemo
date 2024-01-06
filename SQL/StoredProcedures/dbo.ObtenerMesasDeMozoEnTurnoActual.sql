CREATE PROCEDURE ObtenerMesasDeMozoEnTurnoActual
    @MozoId NVARCHAR(450)
AS
BEGIN
    DECLARE @HoraActual DATETIME2(7) = GETDATE();

    SELECT
        M.MesaId As MesaId,
        M.Nombre AS Mesa
    FROM
        DDAsignaciones A
    LEFT JOIN
        DDMesas M ON M.MesaId = A.MesaId
    LEFT JOIN
        AspNetUsers U ON A.MozoId = U.Id
    LEFT JOIN
        DDTurnos T ON A.TurnoId = T.TurnoId
    WHERE
        A.MozoId = @MozoId
        AND M.IsActive = 1
        AND T.HoraFin > @HoraActual
        AND T.HoraInicio < @HoraActual;
END