# CRUD API REST - MORTAL KOMBAT

CRUD hecho con C# y .NET (NetCore). Aplique patrones como DTO, Repository, IoC, Inyección de dependencias, además de validación de datos (Fluent Validation) y mapeos (Automapper). Como base de datos utilize MS SQL Server, y se versiono en Git.
Proximamente agregare una interfaz simple, ya que mi objetivo es enfocarme en el Back-end.



---
- [Instalacion](#instalación)
- [Documentación Swagger](#documentación-swagger)
- [Endpoints](#endpoints-api)
- [Personaje](#Personaje)
	- [DELETE ALL](#delete-all)

## Instalación

### Configuración y ejecución de la aplicación
Para configurar, instalar y ejecutar la aplicación siga estos pasos. Asegurate de tener .NET 8.0 y MS SQL Server.

### Cloná el repositorio
Primero, cloná este repositorio en tu máquina local usando el siguiente comando en tu terminal:

git clone https://github.com/ccabeda/MORTAL_KOMBAT_API

### Abrí el Proyecto en tu Entorno de Desarrollo (IDE)
Abrí tu entorno de desarrollo preferido (recomiendo Visual Studio). Navegá hasta la carpeta del proyecto que acabas de clonar y ábrilo.

### Configurá la base de datos
En el archivo appsetting.json,modificar lo siguiente:

"ConnectionStrings": {
  "Nombre_de_tu_conexión": "Server=Nombre_de_tu_server;Database=Nombre_de_tu_Base_de_datos;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true"
   
Reemplazá Nombre_de_tu_conexión, Nombre_de_tu_server, Nombre_de_tu_Base_de_datos por los datos que quieras poner.
Por último, en el archivo Program.cs, en la inyección de la base de datos, poner el mismo nombre que se utilizo para reemplazar Nombre_de_tu_conexión.

### Creá la Base de Datos
Abrí tu cliente de SQL Server para crear la base de datos con el nombre que especificaste en la URL anterior.

### Ejecutá la Aplicación
Una vez que hayas configurado la base de datos y guardado los cambios, podes ejecutar la aplicación, dandole al botón de "Https" (en Visual Studio). Alli se te deberia abrir la interfaz de Swagger para porbar los EndPoints.

## Endpoints API

## Personaje

### Get all

```http
  GET htpp://localhost:{su_puerto}/api/v1/employees/
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: htpp://https://localhost:7104/api/Personaje
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	200: Lista de todas las personas (DTO)  
	404: excepcion 

Ejemplo por Postman

![github-small](https://imgbox.com/tKfdPWdw)
