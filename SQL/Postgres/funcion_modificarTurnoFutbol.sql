/*
	modificarTurnoFutbol
	Modifica un turno de fútbol
*/


CREATE OR REPLACE FUNCTION public.modificarTurnoFutbol(_id INT, _estado SMALLINT, _metodo_pago SMALLINT, _tipo_cesped SMALLINT, _hora DECIMAL (4, 2))
RETURNS TABLE(
		"status"		TEXT
)

LANGUAGE 'plpgsql'
AS $BODY$
begin
	
	UPDATE "TurnosFutbol"
	SET
	"estado" = _estado,
	"metodo_pago" = _metodo_pago,
	"tipo_cesped" = _tipo_cesped,
	"Hora" = _hora
	WHERE "id" = _id;
		
	RETURN QUERY 
			SELECT 'OK';
end;$BODY$;
