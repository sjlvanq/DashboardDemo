CREATE PROCEDURE ObtenerMesasDeTurno
    @TurnoId INT
AS
BEGIN
    SELECT
        M.MesaId AS MesaId,
        M.Nombre AS Mesa,
        U.UserName AS Mozo
    FROM
        DDAsignaciones A 
    LEFT JOIN
        DDMesas M ON M.MesaId = A.MesaId
    LEFT JOIN
        AspNetUsers U ON A.MozoId = U.Id
    WHERE
        M.IsActive = 1
        AND (A.TurnoId = @TurnoId OR A.TurnoId IS NULL)
END
