CREATE PROCEDURE ObtenerHistorialDeVentas
    @IntervaloInicio DATETIME2(7),
	@IntervaloFin DATETIME2(7)
AS
BEGIN
    SELECT
		V.FechaHoraVenta AS FechaHoraVenta, 
		V.Monto AS Monto,
        Ma.Nombre As Mesa,
        Mo.UserName AS Mozo
    FROM
        DDVentas V
    LEFT JOIN
        DDMesas Ma ON V.MesaId = Ma.MesaId
    LEFT JOIN
        AspNetUsers Mo ON V.MozoId = Mo.Id
    WHERE
			V.FechaHoraVenta > @IntervaloInicio
        AND V.FechaHoraVenta < @IntervaloFin;
END
