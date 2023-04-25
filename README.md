# EquipmentMonitoringSystem
For the project to work, you need to add a functions to postgresql, which is given below:

```
CREATE OR REPLACE FUNCTION update_upmaintenance()
           RETURNS text AS
   $BODY$
        DECLARE
           val numeric;
        BEGIN 
        
       DELETE FROM public."UpcomingMaintenances";
WITH temp_table AS (
	SELECT "Maintenances"."Id" as "MainId"
    FROM "Equipments", "Maintenances"
    WHERE "Maintenances"."EquipmentId" = "Equipments"."Id" AND "Maintenances"."Status" = 'false' AND
	(CURRENT_DATE, CURRENT_DATE + integer '30') OVERLAPS ("Maintenances"."DateMaintenance", "Maintenances"."DateMaintenance")
	)
	INSERT INTO public."UpcomingMaintenances" ("MaintenancesID" )
	SELECT "MainId" FROM temp_table;
        
        val :=1;
		RETURN val;
        END;
   $BODY$
        LANGUAGE 'plpgsql' VOLATILE
```
AND
```
CREATE OR REPLACE FUNCTION update_nortifys()
           RETURNS text AS
   $BODY$
        DECLARE
           val numeric;
        BEGIN 
        
       DELETE FROM public."Nortifys";
WITH temp_table AS (
	SELECT 'Внеплановый ремонт!', "Maintenances"."Description", "Maintenances"."Id" as "MaintenancesId"
    FROM  "Maintenances"
    WHERE "Maintenances"."IsUnplanned" = 'true' )
	INSERT INTO public."Nortifys" ("Heding", "Description", "MaintenancesID") 
	SELECT * FROM temp_table;
        
        val :=1;
		RETURN val;
        END;
   $BODY$
        LANGUAGE 'plpgsql' VOLATILE
```
