/*
A partir de las canchas conectadas (canchas padres e hijas) de un predio separadas por barra vertical (|), devuelve una tabla con todas las que se encuentran reservadas
Parámetros: 
	-listaCanchas: La lista de canchas conectadas (ejemplo, sea una cancha con id "1.1" y con dos canchas hijas), el parámetro 
					debería tener el valor "1.1|1|1.1.1|1.1.2"
	-idPredio: es el id del predio del cuál se desea averiguar las canchas
	-fechaTurno: La hora (DATE SIN HORA) del turno
	-horaTurno: El horario del turno
				
*/

CREATE OR REPLACE FUNCTION public.getReservadosFutbol(listaCanchas text, idPredio int, fechaTurno date, horaTurno decimal(10, 2))
RETURNS TABLE (id_cancha varchar)

LANGUAGE 'plpgsql'
AS $BODY$
begin

	RETURN QUERY 
		SELECT "cancha_id" FROM "TurnosFutbol" 
		WHERE 
				"predio_id" = idPredio
			AND "Fecha" = fechaTurno
			AND "Hora" = horaTurno
			AND	"cancha_id" IN	(SELECT UNNEST(string_to_array(listaCanchas, '|')));

 					

end;$BODY$;
