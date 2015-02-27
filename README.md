<h1>Introduction</h1>
<p>Flexible open geographies is a web application for defining areas and assigning metrics to them. Areas are defined using either KML, GeoJSON, or as a set of previously defined areas.<p>

<h2>Projects</h2>
<h5>Esd.FlexibleOpenGeographies.Web</h5>
<p>An ASP.NET MVC web project and the main project for the site.</p>

<h5>Esd.FlexibleOpenGeographies</h5>
<p>Contains business logic.</p>

<h5>Esd.FlexibleOpenGeographies.Data</h5>
<p>Data layer using Entity Framework.</p>

<h5>Esd.FlexibleOpenGeographies.SignIn</h5>
<p>Contracts and classes to use as a basis for your own sign-in implementation.</p>

<h5>Esd.FlexibleOpenGeographies.Loader</h5>
<p>Loads data. The executable accepts space delimited parameters of one or more of "geometry", "kml", and "aggregate".</p>
<p>geometry is only required when setting up data. It finds areas with KML set and creates the shape in the PostgreSQL database</p>
<p>kml is also only required when setting up data. It reads the KML from an external source based on the area code. You will need to amend Esd.FlexibleOpenGeographies.KmlReader.KmlStringForCode so that it reads the external resources you require.</p>
<p>aggregate should run constantly and I run it as a scheduled task repeating once per minute. It creates the shape in PostgreSQL when an area is defined as a collection of other areas.</p>
<p>When loading initial data you can create all the areas first using insert statements on the MySQL database. Load the KML using the kml parameter. When finished load the PostgreSQL data using the geometry parameter.</p>

<h5>Esd.FlexibleOpenGeographies.Fuseki.Updater</h5>
<p>This loads data into Fuseki automatically from the area and area type tables.</p>

<h5>Esd.FlexibleOpenGeographies.Importer</h5>
<p>This populates the metric types, and period tables with data from the esd web services</p>

<h5>Esd.FlexibleOpenGeographies.Service</h5>
<p>This handles the import of metrics in the background</p>

<h5>Esd.FlexibleOpenGeographies.Service.Functionality</h5>
<p>Contains the actual functionality of the Esd.FlexibleOpenGeographies.Service for importing metrics.</p>

<h5>Esd.FlexibleOpenGeographies.Web.Services</h5>
<p>The FOG REST web services which provides a cut down version of the more complete esd web serivces.</p>

<h2>Languages, frameworks, databases, etc</h2>
<p>Code is written in C# using Ninject for dependency injection and Entity Framework as an ORM.</p>
<p>Client script is JavaScript often using jQuery.</p>
<p>Bootstrap is used for much of the CSS.</p>
<p>A MySQL database holds most of the information about the areas and the metrics. A PostgreSQL database holds all the geographic information about the areas.</p>
<p>Maps are drawn using OpenLayers 3 either directly using KML/GeoJSON or via GeoServer.</p>

<h2>Web.config</h2>
<p>Configuration for the application is held in the Web.config file in Esd.FlexibleOpenGeographies.Web. Values you may wish to change when configuring the application are:</p>
<ul>
<li><strong>FogConnection:</strong> The connection string for the MySQL database.</li>
<li><strong>PostgisConnection:</strong> The connection string for the PostgreSQL database.</li>
<li><strong>GeoServerUrl:</strong> The location of your GeoServer.</li>
<li><strong>UserProvider:</strong> The class in the format <em>classname, namespace</em> implementing IUserProvider which is used for authentication.</li>
<li><strong>SubHeader:</strong> The text below the main header on each page.</li>
<li><strong>GoogleAnalyticsCode:</strong> Your google analytics code to use on the page. Leave blank to not use google analytics.</li>
<li><strong>HasSignIn:</strong> "true" if you want to implement some sort of sign-in functionality. Any other value otherwise.</li>
<li><strong>DisablePermissions:</strong> "true" if you want to hide the option to set permission levels. All newly created area types will be set to public.</li>
</ul>

<h2>Implementing sign-in</h2>
<p>To implement custom sign-in create a class implementing Esd.FlexibleOpenGeographies.UserProvider.IUserProvider. The following members need to be implemented:</p>
<ul>
<li><strong>CreateUser</strong> Add the user and organisation to the database.</li>
<li><strong>SignOut</strong> Redirect to the sign-out page.</li>
<li><strong>AuthenticationCheck</strong> Return false if no authentication required. Otherwise perform whatever action is required (e.g. redirect to sign-in page) and return true.</li>
</ul>
<p>If the method signatures do not match the data you require then please fork and modify.</p>
<p>If OAuth is being used you can use the OAuthManager class provided. You will need to implement IOAuthSettingsProvider to configure it.</p>
<p>If you write custom implementations of IUserProvider and/or IOAuthSettingsProvider modify NinjectWebCommon (in App_Start of the web project) to use your implementations.</p>

<h2>Creating databases</h2>
<p>The database schema creation scripts are in the Database folder. This will set up the database schemas and any reference data.</p>
<h4>PostgreSQL</h4>
<p>Firstly install <a href="http://www.postgresql.org/">PostgreSQL</a> and <a href="http://postgis.net/">PostGIS</a>. Manually create a database and a user with id "postgres". Then run the script postgis.sql against this database. Update Web.config with the new connection details.</p>
<h4>MySQL</h4>
<p>Make sure you have <a href="http://www.mysql.com/">MySQL</a> installed on your server. Run fog.sql to create the database and initial schema. Update Web.config with the new connection details.</p>

<h2>Creating GeoServer</h2>
<p>Install <a href="http://geoserver.org/">GeoServer</a> and change Web.config to set the location.</p>
<p>In the GeoServer web admin page create a new store of type PostGIS and entering the connection details of the PostgreSQL database you created earlier. All other values can be left as default.</p>
<p>Create a new layer for a SQL view and name it areas_for_type. The SQL statement will be:</p>

```
SELECT ST_COLLECT(shape) AS areas FROM area WHERE area_type_code = '%typecode%'
```

<p>Click "Guess parameters from SQL" and the parameter will be guessed correctly. The attribute will be name: areas, type: Geometry, SRID: 4326. Everything else in the layer can be left as default.</p>

<h2>Initial areas</h2>
<p>If you have KML available and a small number of areas the easiest way to create your initial areas is through the application itself. Create the type first, then create your areas by uploading KML.</p>
<p>If you are manually creating your areas (for example because you don't have KML) you'll need to add your records to the MySQL table "area_details", then the PostgreSQL table "area". The shape column will need to be populated using a PostGIS function (see the Geometry Constructors section of <a href="http://www.postgis.us/downloads/postgis21_cheatsheet.pdf">this document</a>). Alternatively use <a href="http://suite.opengeo.org/4.1/dataadmin/pgGettingStarted/pgshapeloader.html">pgShapeLoader</a> to load the shapes.</p>
