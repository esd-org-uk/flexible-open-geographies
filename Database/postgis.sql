CREATE TABLE public.area
(
  area_code character varying(10) NOT NULL,
  shape geometry NOT NULL,
  area_type_code character varying(50) NOT NULL,
  CONSTRAINT pk_area PRIMARY KEY (area_code, area_type_code) WITH (FILLFACTOR=80)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE public.area
  OWNER TO postgres;

CREATE INDEX ix_shape
  ON public.area
  USING gist
  (shape);
  
CREATE INDEX ix_area_type_code
  ON public.area
  (area_type_code);
  
CREATE INDEX ix_area_type_code_shape
  ON public.area
  USING gist
  (area_type_code, shape);
  
CREATE FUNCTION upsertAreaFromKml(content text, areacode text, typecode text) RETURNS void AS $$
DECLARE shp geometry;
BEGIN
	shp := ST_GeomFromKML(content);
	IF NOT (ST_IsValid(shp)) THEN
		shp := ST_MakeValid(shp);
	END IF;
	shp = ST_Force2D(shp);
	shp = ST_Buffer(shp, 0);
	UPDATE area SET shape = shp WHERE area_code = areacode AND area_type_code = typecode;
	INSERT INTO area (area_code, shape, area_type_code)
		SELECT areacode, shp, typecode
		WHERE NOT EXISTS (SELECT 1 FROM area WHERE area_code = areacode AND area_type_code = typecode);
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION upsertAreaFromGeoJson(content text, areacode text, typecode text) RETURNS void AS $$
DECLARE shp geometry;
BEGIN
	shp := ST_GeomFromGeoJSON(content);
	IF NOT (ST_IsValid(shp)) THEN
		shp := ST_MakeValid(shp);
	END IF;
	shp = ST_Force2D(shp);
	shp = ST_Buffer(shp, 0);
	UPDATE area SET shape = shp WHERE area_code = areacode AND area_type_code = typecode;
	INSERT INTO area (area_code, shape, area_type_code)
		SELECT areacode, shp, typecode
		WHERE NOT EXISTS (SELECT 1 FROM area WHERE area_code = areacode AND area_type_code = typecode);
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION upsertAreaFromChildAreas(childareas text, childtypecode text, areacode text, typecode text) RETURNS void AS $$
DECLARE
	shp geometry;
	codes varchar[];
BEGIN
	codes = string_to_array(childareas, ',');
	SELECT ST_Union(shape)
	FROM area
	WHERE area_code = ANY(codes)
	AND area_type_code = childtypecode
	INTO shp;
	IF NOT (ST_IsValid(shp)) THEN
		shp := ST_MakeValid(shp);
	END IF;
	shp = ST_Force2D(shp);
	shp = ST_Buffer(shp, 0);
	UPDATE area SET shape = shp WHERE area_code = areacode AND area_type_code = typecode;
	INSERT INTO area (area_code, shape, area_type_code)
		SELECT areacode, shp, typecode
		WHERE NOT EXISTS (SELECT 1 FROM area WHERE area_code = areacode AND area_type_code = typecode);
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION generateGeoJSON(areacodes text, typecode text) RETURNS text AS $$
DECLARE
	codes varchar[];
	geojson text;
BEGIN
	codes = string_to_array(areacodes, ',');
	SELECT ST_AsGeoJSON(ST_Multi(ST_Union(shape)))
	FROM area
	WHERE area_code = ANY(codes)
	AND area_type_code = typecode
	INTO geojson;
	RETURN geojson;
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION suggestAreas(areatypes text, childarea text, childtype text) RETURNS TABLE(typecode varchar(50), areacode varchar(10)) AS $$
DECLARE
	types varchar[];
BEGIN
	types = string_to_array(areatypes, ',');
	RETURN QUERY
	SELECT	s.area_type_code, s.area_code
	FROM	area a
	INNER JOIN area s ON ST_INTERSECTS(ST_Buffer(s.shape, 0), ST_Buffer(a.shape, 0))
	WHERE	s.area_type_code = ANY(types) AND a.area_code = childarea AND a.area_type_code = childtype;
END;
$$ LANGUAGE plpgsql;