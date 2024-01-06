CREATE PROCEDURE ObtenerMesasNoAsignadas
    @TurnoId INT
AS
BEGIN
    SELECT M.MesaId as MesaId, M.Nombre as Mesa
    FROM DDMesas M
    LEFT JOIN DDAsignaciones a ON M.MesaId = A.MesaId 
    AND A.TurnoId = @TurnoId
    WHERE A.MesaId IS NULL
    AND M.IsActive = 1;
END;

