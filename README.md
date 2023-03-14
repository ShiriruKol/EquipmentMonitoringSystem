# EquipmentMonitoringSystem
For the project to work, you need to add a function to postgresql, which is given below:

```
CREATE OR REPLACE FUNCTION update_upmaintenance()
           RETURNS text AS
   $BODY$
        DECLARE
           val numeric;
        BEGIN 
        
       DELETE FROM public."UpcomingMaintenances";
WITH temp_table AS (
	SELECT "Equipments"."Name", "Maintenances"."Name", "Maintenances"."DateMaintenance"
    FROM "Equipments", "Maintenances"
    WHERE "Maintenances"."EquipmentId" = "Equipments"."Id" AND 
	(CURRENT_DATE, CURRENT_DATE + integer '20') OVERLAPS ("Maintenances"."DateMaintenance", "Maintenances"."DateMaintenance")
	)
INSERT INTO public."UpcomingMaintenances" ("EquipmentName", "MaintenanceName", "DateMaintenance") 
SELECT * FROM temp_table;
        
        
        val :=1;
		RETURN val;
        END;
   $BODY$
        LANGUAGE 'plpgsql' VOLATILE
```
