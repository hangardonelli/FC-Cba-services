/*
	eliminarPreferencia(_predio_id, _preferencia_id)
	Elimina una preferencia a un predio determinado
*/
CREATE OR REPLACE FUNCTION public.eliminarPreferencia(_predio_id INTEGER, _preferencia_id INTEGER)
RETURNS TABLE(
		"status"		TEXT
)

LANGUAGE 'plpgsql'
AS $BODY$
begin
	
	DELETE FROM "PrediosPreferencias"
	WHERE
			"predio_id" = _predio_id
		AND	"preferencia_id" = _preferencia_id;
		
	RETURN QUERY 
			SELECT 'OK';
end;$BODY$;