# CRUD API REST - MORTAL KOMBAT
-- Para esta API se utilizó la version .NET 8.0.
CRUD hecho con C# y .NET (NetCore). Aplique patrones como DTO, Repository, IoC, Inyección de dependencias, además de validación de datos (Fluent Validation) y mapeos (Automapper). Como base de datos utilize MS SQL Server, y se versiono en Git.
Proximamente agregare una interfaz simple, ya que mi objetivo es enfocarme en el Back-end.


---
- [Instalacion](#Instalación)
- [Nugets](#NuGets)
- [Documentación Swagger](#Documentación-swagger)
- [Autentificación por token Jwt](#Autentificación-por-token-Jwt)
- [Endpoints](#Endpoints-api)
- [Personaje](#Personaje)
	- [Get All Personajes](#Get-All-Personajes)
   	- [Get Personaje by Id](#Get-Personaje-By-Id)
  - [Get Personaje by Name](#Get-Personaje-By-Name)
  - [Create Personaje](#Create-Personaje)
  - [Update Personaje](#Update-Personaje)
  - [Update Partial Personaje](#Update-Partial-Personaje)
  - [Delete Personaje](#Delete-Personaje)
  - [Add Weapon To Personaje](#Add-Weapon-To-Personaje)
  - [Remove Weapon To Personaje](#Remove-Weapon-To-Personaje)
  - [Add Style To Personaje](#Add-Style-To-Personaje)
  - [Remove Style To Personaje](#Remove-Style-To-Personaje)
- [Clan](#Clan)
	- [Get All Clanes](#Get-All-Clanes)
   	- [Get Clan by Id](#Get-Clan-By-Id)
  - [Get Clan by Name](#Get-Clan-By-Name)
  - [Create Clan](#Create-Clan)
  - [Update Clan](#Update-Clan)
  - [Update Partial Clan](#Update-Partial-Clan)
  - [Delete Clan](#Delete-Clan)
- [Reino](#Reino)
	- [Get All Reinos](#Get-All-Reinos)
   	- [Get Reino by Id](#Get-Reino-By-Id)
  - [Get Reino by Name](#Get-Reino-By-Name)
  - [Create Reino](#Create-Reino)
  - [Update Reino](#Update-Reino)
  - [Update Partial Reino](#Update-Partial-Reino)
  - [Delete Reino](#Delete-Reino)
- [Arma](#Arma)
	- [Get All Armas](#Get-All-Armas)
   	- [Get Arma by Id](#Get-Arma-By-Id)
  - [Get Arma by Name](#Get-Arma-By-Name)
  - [Create Arma](#Create-Arma)
  - [Update Arma](#Update-Arma)
  - [Update Partial Arma](#Update-Partial-Arma)
  - [Delete Arma](#Delete-Arma)
- [Estilo De Pelea](#Estilo-De-Pelea)
	- [Get All EstilosDePeleas](#Get-All-Estilos-De-Peleas)
   	- [Get EstiloDePelea by Id](#Get-Estilo-De-Pelea-By-Id)
  - [Get EstiloDePelea by Name](#Get-Estilo-De-Pelea-By-Name)
  - [Create EstiloDePelea](#Create-Estilo-De-Pelea)
  - [Update EstiloDePelea](#Update-Estilo-De-Pelea)
  - [Update Partial EstiloDePelea](#Update-Partial-Estilo-De-Pelea)
  - [Delete EstiloDePelea](#Delete-Estilo-De-Pelea)
- [Usuario](#Usuario)
	- [Get All Usuarios](#Get-All-Usuarios)
   	- [Get Usuario by Id](#Get-Usuario-By-Id)
  - [Get Usuario by Name](#Get-Usuario-By-Name)
  - [Create Usuario](#Create-Usuario)
  - [Update Usuario](#Update-Usuario)
  - [Update Partial Usuario](#Update-Partial-Usuario)
  - [Delete Usuario](#Delete-Usuario)
- [Rol](#Rol)
	- [Get All Roles](#Get-All-Roles)
   	- [Get Rol by Id](#Get-Rol-By-Id)
  - [Get Rol by Name](#Get-Rol-By-Name)
  - [Create Rol](#Create-Rol)
  - [Update Rol](#Update-Rol)
  - [Update Partial Rol](#Update-Partial-Rol)
  - [Delete Rol](#Delete-Rol)
 - [Login](#Login)
	- [Login Usuario](#Login-Usuario)


   
## Instalación


### Configuración y ejecución de la aplicación
Para configurar, instalar y ejecutar la aplicación siga estos pasos. Asegurate de tener .NET 8.0 y MS SQL Server.

### Cloná el repositorio
Primero, cloná este repositorio en tu máquina local usando el siguiente comando en tu terminal:

git clone https://github.com/ccabeda/MORTAL_KOMBAT_API

### Abrí el Proyecto en tu Entorno de Desarrollo (IDE)
Abrí tu entorno de desarrollo preferido (recomiendo Visual Studio). Navegá hasta la carpeta del proyecto que acabas de clonar y abrilo.

### Configurá la base de datos
En el archivo appsetting.json,modificar lo siguiente:

"ConnectionStrings": {
  "Nombre_de_tu_conexión": "Server=Nombre_de_tu_server;Database=Nombre_de_tu_Base_de_datos;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true"
   
Reemplazá Nombre_de_tu_conexión, Nombre_de_tu_server, Nombre_de_tu_Base_de_datos por los datos que quieras poner.
En el archivo Program.cs, en la inyección de la base de datos, poner el mismo nombre que se utilizó para reemplazar Nombre_de_tu_conexión.

### Creá la Base de Datos con Entity
Para crear la base de datos, es mediante la migración de los datos de las tablas creadas en Entity Framework hacia MS SQL Server, ya que utiliza first-code.
Para eso, deberan abrir la Consola del administrador de paquetes, que se encuentra cliqueando en "Herramientas" y luego en "Administrador de paquetes NuGet"
En la consola, primero agregan la migración con el comando "Add-Migration" seguido del nombre que quieran darle.
Seguido, utilizan el comando "Update-Database" y ya les aparecerán la tablas en la base de datos.

### Ejecutá la Aplicación
Una vez que hayas configurado la base de datos y guardado los cambios, podés ejecutar la aplicación, dándole al botón de "https" (en Visual Studio). Alli se te debería abrir la interfaz de Swagger para probar los EndPoints.

## NuGets

NuGets necesarias para esta API:
- AutoMapper -versión 13.0.0
- FluentValidationi -versón 11.9.0 
- FluentValidation.DependencyInjectionExtensions -versón 11.9.0
- Microsoft.AspNetCore.Authentication.JwtBearer -versón 8.0.1
- Microsoft.AspNetCore.Mvc.NewtonsoftJson -versón 8.0.1
- Microsoft.EntityFrameworkCore -versón 8.0.1
- Microsoft.EntityFrameworkCore.InMemory -versón 8.0.1
- Microsoft.EntityFrameworkCore.SqlServer -versón 8.0.1
- Microsoft.EntityFrameworkCore.Tools -versón 8.0.1
- Newtonsoft.Json -versón 13.0.3
- Swashbuckle.AspNetCore -versón 6.5.0

## Documentación Swagger

Para acceder a la documentación, una vez corrido el programa, ingrese a: https://localhost:{su_puerto}/swagger/index.html

## Autentificación por token Jwt

Se agregó la autentificación por token Jwt, que consiste en un login que devuelve un token Jwt unico por usuario. Una vez "iniciada sesión" hay diferentes tipos de roles (Super Administrador, Administrador y Público), y dependiendo el rango puedes acceder a diferentes
Endpoints. Todos los usuarios vienen por defecto con el rol Público. Se aclarará arriba de cada uno de los Endpoints el nivel necesario para utilizarlos.

Hay dos maneras de utilizar la API, con la interfaz Swagger, y con la aplicación Postman. A continuación, explicaré como utilizar la autentificación en cada uno.

- [Swagger](#Swagger)
- [Postman](#Postman)

## Swagger

Se añadió un botón de autorización arriba a la derecha, donde se deberá ingresar la palabra clave "Bearer" seguido del token que se recibe una vez iniciada sesión. Dependiendo el rol del usuario con el que inicies sesión, podrás acceder o no al Endpoint.

## Postman

Una vez registrado y con el token en tu poder, te diriges al Endpoint que deseas utilizar y en la parte de Headers, seleccionas Key = Authorization y en la parte de Value ingresas la palabra "Bearer" seguido del token. Dependiendo el rol del usuario con el que inicies sesión, podrás acceder o no al Endpoint.


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
	- 200: Lista de todas los personajes (DTO)
	- 404: Error

### Get Personaje By Id

```http
  GET localhost:{su_puerto}/api/Personaje/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino (DTO)  
	- 400 - 404: Error

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
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino (DTO)  
	- 400 - 404: Error

### Create Personaje

[Autorización: Super Administrador y Administrador]
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
	- 200: Id, Nombre, ImagenURl, Alineación, Raza, Descripción, ClanId y ReinoId (DTO) 
	- 400, 404, 409: Error

### Update Personaje

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Personaje/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Personaje` | `PersonajeUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Personaje/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:  
	- 200: Id, Nombre, ImagenURl, Alineación, Raza, Descripción, Clan y Reino (DTO)
 	-  400: Error

### Update Partial Personaje

[Autorización: Super Administrador y Administrador]
```http
  PATCH localhost:{su_puerto}/api/Personaje/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `JsonPatchDocument` | `PersonajeUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Personaje/{id}
- Metodo: PATCH
- Parametros:
  Id (URL), dato a actualizar (body)
- Respuesta:
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, ClanId y ReinoId (DTO)
	- 404: Error

### Delete Personaje

[Autorización: Super Administrador y Administrador]
```http
  DELETE localhost:{su_puerto}/api/Personaje/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino. (DTO que se desea eliminar)
	- 404: Error

### Metodos Many-to-Many
- Al hacer una relación Many-to-Many entre Personaje y Arma, se creo un metodo para asociar un arma existente con un personaje existente, y uno para eliminar una asociasión entre Arma y Personaje.

### Add Weapon To Personaje

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Personaje/{id_personaje}/AddWeapon/{id_rama}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_personaje` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_arma` | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id_personaje}/AddWeapon/{id_rama}
- Metodo: PUT
- Parametros:
  Id del personaje (URL), Id del arma (URL)
- Respuesta:
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino (DTO)
	- 404: Error
  
### Remove Weapon To Personaje

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Personaje/{id_personaje}/RemoveWeapon/{id_rama}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_personaje` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_arma` | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id_personaje}/RemoveWeapon/{id_rama}
- Metodo: PUT
- Parametros:
  Id del personaje (URL), Id del arma (URL)
- Respuesta:
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino (DTO)
	- 400, 404: Error

### Metodos Many-to-Many
- Al hacer una relación Many-to-Many entre Personaje y EstiloDePelea, se creo un metodo para asociar un arma existente con un personaje existente, y uno para eliminar una asociasión entre EstiloDePelea y Personaje.

### Add Style To Personaje

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Personaje/{id_personaje}/AddStyle/{id_estilo_de_pelea}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_personaje` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_estilo_de_pelea` | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id_personaje}/AddStyle/{id_estilo_de_pelea}
- Metodo: PUT
- Parametros:
  Id del personaje (URL), Id del estilo de pelea (URL)
- Respuesta:
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino (DTO)
	- 404: Error
  
### Remove Style To Personaje

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Personaje/{id_personaje}/RemoveStyle/{id_estilo_de_pelea}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_personaje` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id_estilo_de_pelea` | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Personaje/{id_personaje}/RemoveStyle/{id_estilo_de_pelea}
- Metodo: PUT
- Parametros:
  Id del personaje (URL), Id del estilo de pelea (URL)
- Respuesta:
	- 200: Id, Nombre, Alineación, Raza, Descripción, Estilo De Pelea, Armas, Clan y Reino (DTO)
	- 400, 404: Error
  
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
	- 200: Lista de todos los clanes (DTO) 
	- 404: Error

### Get Clan By Id

```http
  GET localhost:{su_puerto}/api/Clan/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Clan/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO) 
	- 400 - 404: Error

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
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

### Create Clan

[Autorización: Super Administrador y Administrador]
```http
  POST localhost:{su_puerto}/api/Clan
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Clan`| `ClanCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Clan
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)
	- 400, 404, 409: Error

### Update Clan

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Clan/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Clan` | `ClanUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Clan/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Update Partial Clan

[Autorización: Super Administrador y Administrador]
```http
  PATCH localhost:{su_puerto}/api/Clan/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `JsonPatchDocument` | `ClanUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Clan/{id}
- Metodo: PATCH
- Parametros:
  Id (URL), dato a actualizar (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Delete Clan

[Autorización: Super Administrador y Administrador]
```http
  DELETE localhost:{su_puerto}/api/Clan/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Clan/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO que se desea eliminar)
	- 404: Error

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
	- 200: Lista de todos los reinos (DTO)  
	- 404: Error

### Get By Id

```http
  GET localhost:{su_puerto}/api/Reino/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Reino/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

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
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

### Create Reino

[Autorización: Super Administrador y Administrador]
```http
  POST localhost:{su_puerto}/api/Reino
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Reino` | `ReinoCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Reino
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)
	- 400, 404, 409: Error

### Update Reino

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Reino/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Reino` | `ReinoUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Reino/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Update Partial Reino

[Autorización: Super Administrador y Administrador]
```http
  PATCH localhost:{su_puerto}/api/Reino/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `JsonPatchDocument` | `ReinoUpdateDto` | **Requerido** por body.  |


- URL: https://localhost:7104/api/Reino/{id}
- Metodo: PATCH
- Parametros:
  Id (URL), dato a actualizar (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Delete Reino

[Autorización: Super Administrador y Administrador]
```http
  DELETE localhost:{su_puerto}/api/Reino/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Reino/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO que se desea eliminar)
	- 404: Error

## Arma

### Get All Armas

```http
  GET localhost:{su_puerto}/api/Arma
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: https://localhost:7104/api/Arma
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	- 200: Lista de todos las armas (DTO)  
	- 404: Error

### Get Arma By Id

```http
  GET localhost:{su_puerto}/api/Arma/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Arma/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

### Get Arma By Name

```http
  GET localhost:{su_puerto}/api/Arma/nombre/{name}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Nombre`  | `string` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Arma/nombre/{name}
- Metodo GET
- Parametros:
  	Nombre (URl)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

### Create Arma

[Autorización: Super Administrador y Administrador]
```http
  POST localhost:{su_puerto}/api/Arma
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Arma` | `ArmaCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Arma
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)
	- 400, 404, 409: Error

### Update Arma

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/Arma/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Arma` | `ArmaUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Arma/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Update Partial Arma

[Autorización: Super Administrador y Administrador]
```http
  PATCH localhost:{su_puerto}/api/Arma/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `JsonPatchDocument` | `ArmaUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Arma/{id}
- Metodo: PATCH
- Parametros:
  Id (URL), dato a actualizar (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Delete Arma

[Autorización: Super Administrador y Administrador]
```http
  DELETE localhost:{su_puerto}/api/Arma/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Arma/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	- 200: Id, Nombre, Descripción (DTO que se desea eliminar)
	- 404: Error

## Estilo De Pelea

### Get All EstilosDePeleas

```http
  GET localhost:{su_puerto}/api/EstiloDePelea
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: https://localhost:7104/api/EstiloDePelea
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	- 200: Lista de todos lps estilos de pelea (DTO)  
	- 404: Error

### Get EstiloDePelea By Id

```http
  GET localhost:{su_puerto}/api/EstiloDePelea/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/EstiloDePelea/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

### Get EstiloDePelea By Name

```http
  GET localhost:{su_puerto}/api/EstiloDePelea/nombre/{name}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Nombre`  | `string` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/EstiloDePelea/nombre/{name}
- Metodo GET
- Parametros:
  	Nombre (URl)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

### Create EstiloDePelea

[Autorización: Super Administrador y Administrador]
```http
  POST localhost:{su_puerto}/api/EstiloDePelea
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `EstiloDePelea` | `EstiloDePeleaCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/EstiloDePelea
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)
	- 400, 404, 409: Error

### Update EstiloDePelea

[Autorización: Super Administrador y Administrador]
```http
  PUT localhost:{su_puerto}/api/EstiloDePelea/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `EstiloDePelea` | `EstiloDePeleaUpdateDto` | **Requerido** por body.  |


- URL: https://localhost:7104/api/EstiloDePelea/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Update Partial EstiloDePelea

[Autorización: Super Administrador y Administrador]
```http
  PATCH localhost:{su_puerto}/api/EstiloDePelea/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `JsonPatchDocument` | `EstiloDePeleaUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/EstiloDePelea/{id}
- Metodo: PATCH
- Parametros:
  Id (URL), dato a actualizar (body)
- Respuesta:
	- 200: Id, Nombre, Descripción. (DTO)
	- 404: Error

### Delete EstiloDePelea

[Autorización: Super Administrador y Administrador]
```http
  DELETE localhost:{su_puerto}/api/EstiloDePelea/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/EstiloDePelea/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	- 200: Id, Nombre, Descripción (DTO que se desea eliminar)
	- 404: Error
  
## Usuario

### Get All Usuarios

[Autorización: Super Administrador y Administrador]
```http
  GET localhost:{su_puerto}/api/Usuario
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: https://localhost:7104/api/Usuario
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	- 200: Lista de todos los usuarios (DTO)  
	- 404: Error

### Get Usuario By Id

[Autorización: Super Administrador y Administrador]
```http
  GET localhost:{su_puerto}/api/Usuario/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Usuario/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	- 200: Id, Nombre, Apellido, Mail, NombreDeUsuario, Rol (DTO)  
	- 400 - 404: Error

### Get Usuario By Name

[Autorización: Super Administrador y Administrador]
```http
  GET localhost:{su_puerto}/api/Usuario/nombreDeUsuario/{name}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Nombre`  | `string` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Usuario/nombre/{name}
- Metodo GET
- Parametros:
  	Nombre (URl)
- Respuesta:  
	- 200: Id, Nombre, Apellido, Mail, NombreDeUsuario, Rol (DTO)  
	- 400 - 404: Error

### Create Usuario

```http
  POST localhost:{su_puerto}/api/Usuario
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Usuario` | `UsuarioCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Usuario
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	- 200: Id, Nombre, Apellido, Mail, NombreDeUsuario (DTO)
	- 400, 404, 409: Error

### Update Usuario

[Autorización: Super Administrador]
```http
  PUT localhost:{su_puerto}/api/Usuario/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Usuario` | `UsuarioUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Usuario/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	- 200: Id, Nombre, Apellido, Mail, NombreDeUsuario (DTO)
	- 404: Error

### Update Partial Usuario

[Autorización: Super Administrador]
```http
  PATCH localhost:{su_puerto}/api/Usuario/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `JsonPatchDocument` | `UsuarioUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Usuario/{id}
- Metodo: PATCH
- Parametros:
  Id (URL), dato a actualizar (body)
- Respuesta:
	- 200: Id, Nombre, Apellido, Mail, NombreDeUsuario (DTO)
	- 404: Error

### Delete Usuario

[Autorización: Super Administrador]
```http
  DELETE localhost:{su_puerto}/api/Usuario/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Usuario/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	- 200: Id, Nombre, Apellido, Mail, NombreDeUsuario (DTO que se desea eliminar)
	- 404: Error

## Rol

### Get All Roles

[Autorización: Super Administrador]
```http
  GET localhost:{su_puerto}/api/Rol
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
|      |  | **No se requieren parametros**  |

- URL: https://localhost:7104/api/Rol
- Metodo GET
- Parametros:
	Ninguno
- Respuesta:
	- 200: Lista de todos los roles (DTO)  
	- 404: Error

### Get Rol By Id

[Autorización: Super Administrador]
```http
  GET localhost:{su_puerto}/api/Rol/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Rol/{id}
- Metodo GET
- Parametros:
  	Id (URL)
- Respuesta:  
	- 200: Id, Nombre,Descripción (DTO)  
	- 400 - 404: Error

### Get Rol By Name

[Autorización: Super Administrador]
```http
  GET localhost:{su_puerto}/api/Rol/nombre/{name}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Nombre`  | `string` | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Rol/nombre/{name}
- Metodo GET
- Parametros:
  	Nombre (URl)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)  
	- 400 - 404: Error

### Create Rol

[Autorización: Super Administrador]
```http
  POST localhost:{su_puerto}/api/Rol
```

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Rol` | `RolCreateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Rol
- Metodo: POST
- Parametros:
  	Datos personales en formato Json (body)
- Respuesta:  
	- 200: Id, Nombre, Descripción (DTO)
	- 400, 404, 409: Error

### Update Rol

[Autorización: Super Administrador]
```http
  PUT localhost:{su_puerto}/api/Rol/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Rol` | `RolUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Rol/{id}
- Metodo: PUT
- Parametros:
  Id (URL), datos personales en formato Json (body)
- Respuesta:
	- 200: Id, Nombre, Descripción (DTO)
	- 404: Error

### Update Partial Rol

[Autorización: Super Administrador]
```http
  PATCH localhost:{su_puerto}/api/Rol/{id}
```
| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `Id` | `int` | **Requerido** por URL.  |

| Parametro | Tipo     | Descripción              |
| :-------- | :------- | :------------------------- |
| `JsonPatchDocument` | `RolUpdateDto` | **Requerido** por body.  |

- URL: https://localhost:7104/api/Rol/{id}
- Metodo: PATCH
- Parametros:
  Id (URL), dato a actualizar (body)
- Respuesta:
	- 200: Id, Nombre, Descripción (DTO)
	- 404: Error


### Delete Rol

[Autorización: Super Administrador]
```http
  DELETE localhost:{su_puerto}/api/Rol/{id}
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Id`      | `int`    | **Requerido** por URL.  |

- URL: https://localhost:7104/api/Rol/{id}
- Metodo DELETE
- Parametros:
  Id (URL)
- Respuesta:
	- 200: Id, Nombre, Descripción (DTO que se desea eliminar)
	- 404: Error

##  Login

### Login Usuario

```http
  Post localhost:{su_puerto}/api/Login
```

| Parametro | Tipo     | Descripción                     |
| :-------- | :------- | :-------------------------------- |
| `Usuario`      | `Login`    | **Requerido** por URL.  |


- URL: https://localhost:7104/api/Login
- Metodo POST
- Parametros:
	datos de inicio de sesión en formato Json (body)
- Respuesta:
	- 200: Token  
	- 400: Error
