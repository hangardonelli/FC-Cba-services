/*
	getMensajesFutbolByTurno(_turno_id)
	Obtiene todos los mensajes de un turno
*/
CREATE OR REPLACE FUNCTION public.getMensajesFutbolByTurno(_turno_id INTEGER)
RETURNS TABLE(
		"id" 			INTEGER,
		"turno_id" 		INTEGER,
		"contenido" 	TEXT,
		"autor"			INTEGER,
		"fecha" 		TIMESTAMP,
		"usuario_id"	INTEGER,
		"predio_id" 	INTEGER
)

LANGUAGE 'plpgsql'
AS $BODY$
begin

	RETURN QUERY 
		SELECT
			t.id, 					-- Id del mensaje
			t.turno_id,				-- Id del turno
			t.contenido,			-- Contenido del mensaje
			t.autor,				-- Autor del mensaje (usuario o predio)
			t.fecha,				-- Fecha del mensaje
			u.id AS "usuario_id",	-- Id del usuario
			p.id AS "predio_id"		-- Id del predio
			
		
		
		
		
		FROM "MensajesTurnos" t
		INNER JOIN "TurnosFutbol" tf
			ON tf.id = t.id
		INNER JOIN "Usuarios" u
			ON tf.usuario = u.id
		INNER JOIN "Predios" p
			ON tf.predio_id = p.id
			
		WHERE
				t.turno_id = _turno_id;

 
end;$BODY$;
