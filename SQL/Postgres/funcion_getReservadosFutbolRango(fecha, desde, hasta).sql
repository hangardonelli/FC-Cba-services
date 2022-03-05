
/*
	Obtiene todas las reservas en un rango horario indicado
	
	Uso ejemplo:
	SELECT * FROM getReservadosFutbolRango(CAST(CURRENT_TIMESTAMP AS DATE), 20.22, 20.23);
*/

CREATE OR REPLACE FUNCTION public.getReservadosFutbolRango(_fecha DATE, _horaDesde numeric, _horaHasta numeric)
RETURNS TABLE(
		"turno_id" INTEGER,
		"turno_fecha" DATE,
		"turno_predio_id" INTEGER,
		"turno_estado" SMALLINT,
		"turno_cancha_id" VARCHAR,
		"turno_hora" NUMERIC
)

LANGUAGE 'plpgsql'
AS $BODY$
begin

	RETURN QUERY 
		SELECT "id" 		AS "turno_id",
		"Fecha" 	AS "turno_fecha", 
		"predio_id" AS "turno_predio_id",
		"estado"	AS "turno_estado", 
		"cancha_id" AS "turno_cancha_id",
		"Hora"		AS "turno_hora"
		FROM "TurnosFutbol" tf
			WHERE 
					"Fecha" = (CAST(_fecha AS DATE))
				AND "Hora" BETWEEN _horaDesde AND _horaHasta;

 					

end;$BODY$;
