# CRUD API REST - MORTAL KOMBAT

CRUD hecho con C# y .NET (NetCore). Aplique patrones como DTO, Repository, IoC, Inyección de dependencias, además de validación de datos (Fluent Validation) y mapeos (Automapper). Como base de datos utilize MS SQL Server, y se versiono en Git.
Proximamente agregare una interfaz simple, ya que mi objetivo es enfocarme en el Back-end.



---
- [Instalacion](#instalación)
- [Endpoints](#endpoints-api)
- [Documentación Swagger](#documentación-swagger)
- [Personaje](#Personaje)
	- [Get All Personajes](#Get-All-Personajes)
   	- [Get Personaje by Id](#Get-Personaje-By-Id)
  - [Get Personaje by Name](#Get-Personaje-By-Name)
  - [Create Personaje](#Create-Personaje)
  - [Update Personaje](#Update-Personaje)
  - [Delete Personaje](#Delete-Personaje)
  - [Add Weapon To Personaje](#Add-Weapon-To-Personaje)
  - [Remove Weapon To Personaje](#Remove-Weapon-To-Personaje)
- [Clan](#Clan)
	- [Get All Clanes](#Get-All-Clanes)
   	- [Get Clan by Id](#Get-Clan-By-Id)
  - [Get Clan by Name](#Get-Clan-By-Name)
  - [Create Clan](#Create-Clan)
  - [Update Clan](#Update-Clan)
  - [Delete Clan](#Delete-Clan)
- [Reino](#Reino)
	- [Get All Reinos](#Get-All-Reinos)
   	- [Get Reino by Id](#Get-Reino-By-Id)
  - [Get Reino by Name](#Get-Reino-By-Name)
  - [Create Reino](#Create-Reino)
  - [Update Reino](#Update-Reino)
  - [Delete Reino](#Delete-Reino)
   
   

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

##  Personaje

### Get All Personajes

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
	200: Lista de todas los personajes (DTO)  
	404: Error

### Get Personaje By Id

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

### Get Personaje By Name

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

### Metodos Many-to-Many
- Al hacer una relación Many-to-Many entre Personaje y Arma, se creo un metodo para asociar un arma existente con un personaje existente.

  ### Add Weapon To Personaje

```http
  PUT localhost:{su_puerto}/api/Personaje/{id_personaje}/AddWeapon/{id_rama}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `id_personaje` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `id_arma` | `int`    | **Requerido** por URL.  |


- URL: https://localhost:7104/api/Personaje/{id_personaje}/AddWeapon/{id_rama}
- Metodo: PUT
- Parametros:
  Id del personaje (URL), Id del arma (URL)
- Respuesta:
	200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino.

  404: Error
  
    ### Remove Weapon To Personaje

```http
  PUT localhost:{su_puerto}/api/Personaje/{id_personaje}/RemoveWeapon/{id_rama}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `id_personaje` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `id_arma` | `int`    | **Requerido** por URL.  |


- URL: https://localhost:7104/api/Personaje/{id_personaje}/RemoveWeapon/{id_rama}
- Metodo: PUT
- Parametros:
  Id del personaje (URL), Id del arma (URL)
- Respuesta:
	200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino.

  400, 404: Error
  
  ## Clan

### Get All Clanes

```http
  GET localhost:{su_puerto}/api/Clan
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: https://localhost:7104/api/Clan
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	200: Lista de todos los clanes (DTO)  
	404: Error

### Get Clan By Id

```http
  GET localhost:{su_puerto}/api/Clan/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `id` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Clan/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	200: Id, Nombre, Descripción. (DTO)  
	400 - 404: Error

### Get Clan By Name

```http
  GET localhost:{su_puerto}/api/Clan/nombre/{name}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Nombre`  | `string` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Clan/nombre/{name}
- Metodo GET
- Parametros:
  	Nombre (URl)
- Respuesta:  
	200: Id, Nombre, Descripción. (DTO)  
	400 - 404: Error

### Create Clan

```http
  POST localhost:{su_puerto}/api/Clan
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| Peronsaje | `ClanCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Clan
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	200: Id, Nombre, Descripción
	400, 404, 409: Error

### Update Clan

```http
  PUT localhost:{su_puerto}/api/Clan/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Clan` | `ClanUpdateDto` | **Requerido** por body.  |


- URL: https://localhost:7104/api/Clan/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	200: Id, Nombre, Descripción. (DTO).

 404: Error

### Delete Clan

```http
  DELETE localhost:{su_puerto}/api/Clan/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Clan/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	200: Id, Nombre, Descripción. (DTO que se desea eliminar).

  404: Error

  ## Reino

### Get All Reinos

```http
  GET localhost:{su_puerto}/api/Reino
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: https://localhost:7104/api/Reino
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	200: Lista de todos los reinos (DTO)  
	404: Error

### Get By Id

```http
  GET localhost:{su_puerto}/api/Reino/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `id` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Reino/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	200: Id, Nombre, Descripción. (DTO)  
	400 - 404: Error

### Get Reino By Name

```http
  GET localhost:{su_puerto}/api/Reino/nombre/{name}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Nombre`  | `string` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Reino/nombre/{name}
- Metodo GET
- Parametros:
  	Nombre (URl)
- Respuesta:  
	200: Id, Nombre, Descripción. (DTO)  
	400 - 404: Error

### Create Reino

```http
  POST localhost:{su_puerto}/api/Reino
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| Peronsaje | `ReinoCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Reino
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	200: Id, Nombre, Descripción
	400, 404, 409: Error

### Update Reino

```http
  PUT localhost:{su_puerto}/api/Reino/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Clan` | `ReinoUpdateDto` | **Requerido** por body.  |


- URL: https://localhost:7104/api/Reino/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	200: Id, Nombre, Descripción. (DTO).

  404: Error

### Delete Reino

```http
  DELETE localhost:{su_puerto}/api/Reino/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Reino/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	200: Id, Nombre, Descripción. (DTO que se desea eliminar).

  404: Error
  
