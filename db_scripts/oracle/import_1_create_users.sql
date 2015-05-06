CREATE USER MAIN IDENTIFIED BY temppass DEFAULT TABLESPACE users TEMPORARY TABLESPACE temp
/

GRANT unlimited tablespace TO MAIN
/

GRANT connect, resource TO MAIN
/

CREATE USER PROD IDENTIFIED BY temppass DEFAULT TABLESPACE users TEMPORARY TABLESPACE temp
/

GRANT unlimited tablespace TO PROD
/

GRANT connect, resource TO PROD
/

EXIT;