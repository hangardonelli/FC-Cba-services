/*
	Crea el horario de un turno para una hora, día de la semana y predio específico
	
	Parámetros:
		_predio_id: Es el id del predio 
		_dia: El día de la semana del horario de apertura (del 0 al 6) donde 0 = Domingo (Los ids se pueden ver en la tabla "DiasSemana")
		_hora: La hora del turno (ej: 20.30 = 20:30hs)
		
		EJEMPLO:
		SELECT * FROM insertarHorarioPredio(1, CAST(2 AS SMALLINT), CAST(9.27 AS DECIMAL(4,2)));
*/
CREATE OR REPLACE FUNCTION public.insertarHorarioPredio(_predio_id int, _dia smallint, _hora numeric(4,2)) RETURNS VOID AS $$ 
    DECLARE 
    BEGIN 
      INSERT INTO "HorariosPredios"
	  VALUES(_predio_id, _dia, _hora);
    END 
    $$ LANGUAGE 'plpgsql'; 
	