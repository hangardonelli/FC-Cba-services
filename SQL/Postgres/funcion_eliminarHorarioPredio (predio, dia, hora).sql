/*
	Elimina el horario de un turno para una hora, día de la semana y predio específico
	
	Parámetros:
		_predio_id: Es el id del predio 
		_dia: El día de la semana del horario de apertura (del 0 al 6) donde 0 = Domingo (Los ids se pueden ver en la tabla "DiasSemana")
		_hora: La hora del turno (ej: 20.30 = 20:30hs)
*/
CREATE OR REPLACE FUNCTION public.eliminarHorarioPredio(_predio_id int, _dia smallint, _hora numeric(4,2)) RETURNS VOID AS $$ 
    DECLARE 
    BEGIN 
       DELETE FROM "HorariosPredios"
	   WHERE 
				"predio_id" = _predio_id
			AND "dia_semana_id" = "_dia"
			AND "hora" = _hora;
    END 
    $$ LANGUAGE 'plpgsql'; ; 
	
