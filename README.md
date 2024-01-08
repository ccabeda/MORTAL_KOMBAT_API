# CRUD API REST - MORTAL KOMBAT

CRUD hecho con C# y .NET (NetCore). Aplique patrones como DTO, Repository, IoC, Inyección de dependencias, además de validación de datos (Fluent Validation) y mapeos (Automapper). Como base de datos utilize MS SQL Server, y se versiono en Git.
Proximamente agregare una interfaz simple, ya que mi objetivo es enfocarme en el Back-end.



---
- [Instalacion](#instalación)
- [Endpoints](#endpoints-api)
- [Documentación Swagger](#documentación-swagger)
- [Personaje](#Personaje)
	- [Get All](#Get-All)
   	- [Get by Id](#Get-By-Id)
  - [Get by Name](#Get-By-Name)
  - [Create Personaje](#Create-Personaje)
  - [Update Personaje](#Update-Personaje)
  - [Delete Personaje](#Delete-Personaje)
   

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
En el archivo Program.cs, en la inyección de la base de datos, poner el mismo nombre que se utilizo para reemplazar Nombre_de_tu_conexión.

### Creá la Base de Datos con Entity
Para crear la base de datos, es mediante la migración de los datos de las tablas creadas en Entity Framework hacia MS SQL Server, ya que utilize first-code.
Para eso, deberan abrir la Consola del administrador de paquetes, que se encuentra clikeando en "Herramientas" y luego en "Administrador de paquetes NuGet"
En la consola, primero agregan la migración con el comando "Add-Migration" seguido del nombre que quieran darle.
Seguido, utilizan el comando "Update-Database" y ya les apareceran la tablas en la base de datos.

### Ejecutá la Aplicación
Una vez que hayas configurado la base de datos y guardado los cambios, podes ejecutar la aplicación, dandole al botón de "https" (en Visual Studio). Alli se te deberia abrir la interfaz de Swagger para porbar los EndPoints.

### Documentación Swagger

Para acceder a la documentación, una vez corrido el programa, ingrese a: https://localhost:{su_puerto}/swagger/index.html

## Endpoints API

## Personaje

### Get All

```http
  GET localhost:{su_puerto}/api/Personaje
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: https://localhost:7104/api/Personaje
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	200: Lista de todas las personas (DTO)  
	404: Error

### Get By Id

```http
  GET localhost:{su_puerto}/api/Personaje/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `id` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino. (DTO)  
	400 - 404: Error

### Get By Name

```http
  GET localhost:{su_puerto}/api/Personaje/nombre/{name}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Nombre`  | `string` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/nombre/{name}
- Metodo GET
- Parametros:
  	Nombre (URl)
- Respuesta:  
	200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino. (DTO)  
	400 - 404: Error

### Create Personaje

```http
  POST localhost:{su_puerto}/api/Personaje
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| Peronsaje | `PersonajeCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Personaje
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	200: Id, Nombre, ImagenURl, Alineación, Raza, Descripción, Estilo De Pelea, Armas, ClanId y ReinoId.  
	400, 404, 409: Error

### Update Personaje

```http
  PUT localhost:{su_puerto}/api/Personaje/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Personaje` | `PersonajeUpdateDto` | **Requerido** por body.  |


- URL: https://localhost:7104/api/Personaje/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino. (DTO).

  404: Error

### Delete Personaje

```http
  DELETE localhost:{su_puerto}/api/Personaje/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino. (DTO que se desea eliminar).

  404: Error