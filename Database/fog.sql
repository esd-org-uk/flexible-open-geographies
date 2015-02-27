CREATE DATABASE  IF NOT EXISTS `flexible_open_geographies` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `flexible_open_geographies`;

/*Make sure these are in the correct order so that foreign key constraints aren't violated*/
DROP TABLE IF EXISTS `area_resource`;
DROP TABLE IF EXISTS `area_type_resource`;
DROP TABLE IF EXISTS `area_alternate_label`;
DROP TABLE IF EXISTS `area_composition`;
DROP TABLE IF EXISTS `area_details`;
DROP TABLE IF EXISTS `area_type_alternate_label`;
DROP TABLE IF EXISTS `type_hierarchy`;
DROP TABLE IF EXISTS `area_type_group_member`;
DROP TABLE IF EXISTS `area_type`;
DROP TABLE IF EXISTS `nn_codes`;
DROP TABLE IF EXISTS `periods`;
DROP TABLE IF EXISTS `metric_type`;
DROP TABLE IF EXISTS `metric`;
DROP TABLE IF EXISTS `upload`;
DROP TABLE IF EXISTS `metric_aggregation`;
DROP TABLE IF EXISTS `metric_upload_permission_level`;
DROP TABLE IF EXISTS `user`;
DROP TABLE IF EXISTS `organisation`;

CREATE TABLE `metric_upload_permission_level` (
  `id` int(11) NOT NULL,
  `description` nvarchar(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `organisation` (
  `organisation_id` varchar(255) NOT NULL,
  `organisation_name` varchar(255) NOT NULL,
  PRIMARY KEY (`organisation_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `user` (
  `user_id` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `organisation_id` varchar(255) NOT NULL,
  `access_token` VARCHAR(255) NULL,
  `email` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`user_id`),
  CONSTRAINT `fk_user_organisation` FOREIGN KEY (`organisation_id`) REFERENCES `organisation` (`organisation_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_type` (
  `code` varchar(50) NOT NULL,
  `label` nvarchar(255) NOT NULL,
  `creator` varchar(255) NOT NULL,
  `create_date` datetime NOT NULL,
  `update_date` datetime DEFAULT NULL,
  `short_code` varchar(7) NOT NULL,
  `metric_upload_permission_level_id` int(11) NOT NULL,
  `external_link` text NULL,
  `is_group` bit(1) NULL,
  PRIMARY KEY (`code`),
  UNIQUE KEY (`label`),
  CONSTRAINT `fk_area_type_creator` FOREIGN KEY (`creator`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_area_type_permissions` FOREIGN KEY (`metric_upload_permission_level_id`) REFERENCES `metric_upload_permission_level` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_type_group_member` (
  `parent_code` varchar(50) NOT NULL,
  `child_code` varchar(50) NOT NULL,
  PRIMARY KEY (`parent_code`, `child_code`),
  KEY `ix_area_type_group_member_child` (`child_code`),
  CONSTRAINT `fk_area_type_group_member_parent` FOREIGN KEY (`parent_code`) REFERENCES `area_type` (`code`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_area_type_group_member_child` FOREIGN KEY (`child_code`) REFERENCES `area_type` (`code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `type_hierarchy` (
  `type_code` varchar(50) NOT NULL,
  `child_type_code` varchar(50) NOT NULL,
  `is_primary` bit(1) NOT NULL,
  `covers_whole` bit(1) NOT NULL,
  PRIMARY KEY (`type_code`,`child_type_code`),
  KEY `ix_type_hierarchy_child_type_type` (`child_type_code`, `type_code`),
  CONSTRAINT `fk_type_hierarchy_child_type` FOREIGN KEY (`child_type_code`) REFERENCES `area_type` (`code`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_type_hierarchy_type` FOREIGN KEY (`type_code`) REFERENCES `area_type` (`code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_details` (
  `id` int NOT NULL AUTO_INCREMENT,
  `code` varchar(10) NOT NULL,
  `label` varchar(255) NOT NULL,
  `type_code` varchar(50) NOT NULL,
  `kml` mediumtext,
  `creator` varchar(255) NOT NULL,
  `create_date` datetime NOT NULL,
  `update_date` datetime DEFAULT NULL,
  `colour` char(6) NULL,
  `requires_geometry_calculation` bit(1) NULL,
  `geometry_calculation_failed` bit(1) NULL,
  `external_link` text NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY (`type_code`, `code`),
  CONSTRAINT `fk_area_details_type` FOREIGN KEY (`type_code`) REFERENCES `area_type` (`code`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_area_details_creator` FOREIGN KEY (`creator`) REFERENCES `user` (`user_id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_composition` (
  `area_id` int NOT NULL,
  `child_area_id` int NOT NULL,
  PRIMARY KEY (`area_id`,`child_area_id`),
  KEY `ix_area_composition_child_area_area` (`child_area_id`, `area_id`),
  CONSTRAINT `fk_area_composition_area` FOREIGN KEY (`area_id`) REFERENCES `area_details` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_area_composition_child_area` FOREIGN KEY (`child_area_id`) REFERENCES `area_details` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `nn_codes` (
  `type_short_code` varchar(7) NOT NULL,
  `area_code` varchar(3) NOT NULL,
  PRIMARY KEY (`type_short_code`,`area_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_alternate_label` (
  `area_alternate_label_id` int(10) NOT NULL AUTO_INCREMENT,
  `area_id` int NOT NULL,
  `label` varchar(255) NOT NULL,
  PRIMARY KEY (`area_alternate_label_id`),
  CONSTRAINT `fk_area_alternate_label_area` FOREIGN KEY (`area_id`) REFERENCES `area_details` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_type_alternate_label` (
  `area_type_alternate_label_id` int(10) NOT NULL AUTO_INCREMENT,
  `type_code` varchar(50) NOT NULL,
  `label` varchar(255) NOT NULL,
  PRIMARY KEY (`area_type_alternate_label_id`),
  CONSTRAINT `fk_area_type_alternate_label_area_type` FOREIGN KEY (`type_code`) REFERENCES `area_type` (`code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_resource` (
  `area_resource_id` int(10) NOT NULL AUTO_INCREMENT,
  `area_id` int NOT NULL,
  `label` varchar(255) NOT NULL,
  `uri` text NOT NULL,
  PRIMARY KEY (`area_resource_id`),
  CONSTRAINT `fk_area_resource_area` FOREIGN KEY (`area_id`) REFERENCES `area_details` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `area_type_resource` (
  `area_type_resource_id` int(10) NOT NULL AUTO_INCREMENT,
  `type_code` varchar(50) NOT NULL,
  `label` varchar(255) NOT NULL,
  `uri` text NOT NULL,
  PRIMARY KEY (`area_type_resource_id`),
  CONSTRAINT `fk_area_type_resource_area` FOREIGN KEY (`type_code`) REFERENCES `area_type` (`code`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `periods` (
  `identifier` varchar(255) NOT NULL,
  `start` datetime NOT NULL,
  `end` datetime NOT NULL,
  `type` VARCHAR(255) NOT NULL,
  `label` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`identifier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `metric_type` (
  `identifier` INTEGER UNSIGNED NOT NULL,
  `label` varchar(255) NOT NULL,
  `period_type` varchar(255) NOT NULL,
  `period_start` datetime NOT NULL,
  `period_end` datetime NOT NULL,
  `output_precision` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `aggregatable_by_area` TINYINT(1) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY USING BTREE (`identifier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `metric` (
  `metric_type_identifier` varchar(255) NOT NULL,
  `period_identifier` varchar(255) NOT NULL,
  `area_identifier` varchar(255) NOT NULL,
  `type_code` VARCHAR(255) NOT NULL,
  `value` varchar(45) NOT NULL,
  PRIMARY KEY (`metric_type_identifier`, `period_identifier`, `area_identifier`, `type_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `upload` (
  `id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `csv` MEDIUMTEXT NOT NULL,
  `user_id` VARCHAR(512) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `metric_aggregation` (
  `metric_type_identifier` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `type_code` VARCHAR(255) NOT NULL,
  `is_aggregable` TINYINT UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY (`metric_type_identifier`, `type_code`)
) ENGINE = InnoDB DEFAULT CHARSET=utf8;

INSERT INTO `metric_upload_permission_level` (`id`, `description`)
VALUES (1, 'Creator');

INSERT INTO `metric_upload_permission_level` (`id`, `description`)
VALUES (2, 'Creator organisation');

INSERT INTO `metric_upload_permission_level` (`id`, `description`)
VALUES (3, 'Public');

CREATE VIEW ViewRDFExport AS
		SELECT CONCAT('<http://fog.id.esd.org.uk/',type_code,'/',code,'> <http://purl.org/dc/terms/identifier> "',code,'".') AS RDF, type_code, code
		FROM area_details
	UNION ALL
		SELECT CONCAT('<http://fog.id.esd.org.uk/',type_code,'/',code,'> <http://www.w3.org/2004/02/skos/core#prefLabel> "',`label`,'".') AS RDF, type_code, code
		FROM area_details
	UNION ALL
		SELECT CONCAT('<http://fog.id.esd.org.uk/',type_code,'/',code,'> <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://fog.id.esd.org.uk/',type_code,'>.') AS RDF, type_code, code
		FROM area_details
	UNION ALL
		SELECT CONCAT('<http://fog.id.esd.org.uk/',type_code,'/',code,'> <http://purl.org/dc/elements/1.1/relation> "http://beta.fog.esd.org.uk/areas/',CONVERT(id,char),'/kml".') AS RDF, type_code, code
		FROM area_details
	UNION ALL
		SELECT CONCAT('<', uri, '> <http://purl.org/dc/elements/1.1/relation> "http://fog.id.esd.org.uk/',type_code,'/',code,'".') AS RDF, type_code, code
		FROM area_resource
		INNER JOIN area_details ON area_details.id = area_resource.area_id
	UNION ALL
		SELECT CONCAT('<', uri, '> <http://purl.org/dc/elements/1.1/title> "',area_resource.`label`,'".') AS RDF, type_code, code
		FROM area_resource
		INNER JOIN area_details ON area_details.id = area_resource.area_id
	UNION ALL
		SELECT CONCAT('<http://fog.id.esd.org.uk/',code,'> <http://purl.org/dc/terms/identifier> "',code,'".') AS RDF, code as type_code, code
		FROM area_type
	UNION ALL
		SELECT CONCAT('<http://fog.id.esd.org.uk/',code,'> <http://www.w3.org/2004/02/skos/core#prefLabel> "',`label`,'".') AS RDF, code as type_code, code
		FROM area_type
	UNION ALL
		SELECT CONCAT('<http://fog.id.esd.org.uk/',code,'> <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> <http://fog.id.esd.org.uk>.') AS RDF, code as type_code, code
		FROM area_type
	ORDER BY code ASC;