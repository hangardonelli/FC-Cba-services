/*
	Modifica el horario de apertura y cierre para un día de la semana de un predio
	
	Parametros: 
		_predio_id: El id del predio
		_dia: El día de la semana del horario de apertura (del 0 al 6) donde 0 = Domingo (Los ids se pueden ver en la tabla "DiasSemana")
		_desde: Horario de apertura, ej: 20.30 = 20:30hs
		_hasta: Horario de cierre, ej: 22.45 = 22:45hs
		
		EJEMPLO:
			Cambiar el horario de apertura a 10:00hs, el de cierre a 23:20hs en el día 2 (Martes) para el predio con ID 1:
			SELECT * FROM actualizarHorarioPredio(1, CAST(2 AS SMALLINT), CAST(10 AS NUMERIC(4,2)), CAST(23.20 AS NUMERIC(4,2)));
*/
CREATE OR REPLACE FUNCTION public.actualizarHorarioPredio(_predio_id int, _dia smallint, _desde numeric(4,2), _hasta numeric(4,2)) RETURNS VOID AS $$ 
    DECLARE 
    BEGIN 
        UPDATE "HorariosPredios" SET "hora_desde" = _desde, "hora_hasta" = _hasta WHERE "predio_id" = _predio_id AND "dia_semana_id" = _dia; 
        IF NOT FOUND THEN 
        INSERT INTO "HorariosPredios" VALUEs (_predio_id, _dia, _desde, _hasta); 
        END IF; 
    END; 
    $$ LANGUAGE 'plpgsql'; 
	
