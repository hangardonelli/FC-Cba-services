/*
	Obtiene todos los mensajes de un turno
	
	Uso ejemplo:
	SELECT * FROM getMensajes(475);
*/



CREATE OR REPLACE FUNCTION public.getMensajes(_turno_id INTEGER)
RETURNS TABLE(
		"id" INTEGER,
		"turno_id" INTEGER,
		"contenido" TEXT,
		"autor" INTEGER,
		"fecha" TIMESTAMP
)

LANGUAGE 'plpgsql'
AS $BODY$
begin

	RETURN QUERY 
		SELECT * FROM "MensajesTurnos" t
		WHERE
				t.turno_id = _turno_id;

 
end;$BODY$;
