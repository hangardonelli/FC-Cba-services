/*
	insertarPreferencia(_predio_id, _preferencia_id)
	Inserta una preferencia a un predio determinado
*/
CREATE OR REPLACE FUNCTION public.insertarPreferencia(_predio_id INTEGER, _preferencia_id INTEGER)
RETURNS TABLE(
		"status"		TEXT
)

LANGUAGE 'plpgsql'
AS $BODY$
begin
	
	INSERT INTO "PrediosPreferencias"
	VALUES (_predio_id, _preferencia_id);
		
	RETURN QUERY 
			SELECT 'OK';
end;$BODY$;
