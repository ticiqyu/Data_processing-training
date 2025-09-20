CREATE SEQUENCE function_and_derivative_id_seq START WITH 1 INCREMENT BY 1;

CREATE TABLE function_and_derivative ( 
    id INTEGER PRIMARY KEY DEFAULT nextval('function_and_derivative_id_seq'),
    time TIMESTAMP,
    value NUMERIC
);


CREATE OR REPLACE FUNCTION random_timestamp(start_ts TIMESTAMP, end_ts TIMESTAMP)
RETURNS TIMESTAMP AS $$
BEGIN
  RETURN start_ts + (end_ts - start_ts) * random();
END;
$$ LANGUAGE plpgsql;


-- Вставка 100 случайных значений
DO $$
BEGIN
  FOR i IN 1..100 LOOP
    INSERT INTO function_and_derivative (time, value)
    VALUES (
      random_timestamp('2025-01-01 00:00:01', '2025-05-31 23:59:59'), 
      random() * 1000  
    );
  END LOOP;
END $$;


-- Проверка
SELECT * FROM function_and_derivative; 
