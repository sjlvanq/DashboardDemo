# Dashboard Demo.
Web API programada en ASP.NET / SQL Server como evaluación para selección de personal en una empresa que requiere desarrolladores de software. El FrontEnd se ha realizado en React.js y está disponible en el siguiente repositorio: https://github.com/sjlvanq/DashboardDemoUI.git

### La consigna ha sido: 

 >REALIZAR UN DASHBOARD QUE FILTRE LOS MENU'S, SEGUN EL ROL: MOZOS / GERENTE / CEO   
 >TABLAS DE SQL RELACIONALES: MENU / ROL / ROLEMPLEADO / TURNOS / MOZOS / MESAS / GERENTE / CEO / VENTA / DETALLE DE VENTA   
 >DEBERA TENER UN INICIO DE SESSION QUE FILTRE LOS MENU'S, SEGUN EL ROL.   
 >EL CEO PUEDE DAR DE ALTA GERENTE Y MOZOS , ASIGNANDO LOS ROLES. PUEDE VER LA LISTA DE GERENTES Y MOZOS, EDITARLOS, BORRARLOS.   
 >PUEDE BUSCAR EL HISTORIAL DE VENTAS, ELIGIENDO FECHA INICIO Y FIN.   
 >EL GERENTE PUEDE DAR DE ALTA, MOZOS, MESAS, TURNOS. ASIGNARLE TURNOS A LOS MOZOS. Y MESAS A LOS MOZOS. DEBE HABER MAS DE UN MOZO POR TURNO.   
 >EL MOZO SOLO PUEDE REALIZAR VENTAS.   
 >
 >UTILIZAR SQL SERVER PROCEDIMIENTOS ALMACENADOS, C#, API FETCH.   
 >PUBLICAR LA APLICACION EN CUALQUIER HOSTING GRATIS.   
 >ENVIAR 3 USUARIO DE INGRESO, PARA CADA ROL.
>

# Lógica de negocio
La solución presentada ofrece la siguiente lógica de negocios:
## 1. Usuarios
#### 1.1 Consideraciones generales
*    El personal será, a la vez, usuario del sistema.
*    La autorización de alta de usuarios sigue una lógica jerárquica lineal. (CEO puede crear GERENTE y MOZO; -> GERENTE puede crear MOZO; -> MOZO no puede crear usuarios).
#### 1.2 Implementación
*    La información referida al usuario en tanto usuario y personal (entidad y credenciales) utiliza unas mismas tablas.
*    La autorización de alta de usuarios está determinada por una propiedad del Rol del usuario/personal.
#### 1.3 Detalles
*    Se elaboró una implementación específica por herencia de las clases de Entity Framework Identity.
*    En la función de sembrado de la Base de datos se utiliza un tipo Enum para establecer HiercharyLevel junto con los nombres de Roles al crearlos.
*    No se utilizaron Claims por no expresar la consigna mayores requerimientos respecto al sistema de permisos y ser utilizado una única vez el HisercharyLevel del lado del cliente.
## 2. Turnos, Mesas y Mozos
#### 2.1 Consideraciones generales
*    En la asignación de Mesas y Mozos a Turnos se consideró el Turno como ámbito.
* * En un turno una mesa sólo puede tener asignada un mozo.
* * En un turno un mismo mozo puede estar asignado a distintas mesas.
#### 2.2 Implementación
*    Se sugieren asignaciones 'por defecto' al registrar un turno: se implementa en la asignación de mesas al turno como propiedad opcional de la mesa.
#### 2.3 Detalles
*    Se utiliza una tabla intermedia Asignaciones para gestionar las relaciones muchos a muchos de mozos <-----> turnos y mesas <-----> turnos.
*    La tabla Asignaciones tiene una clave secundaria compleja formada por la referencia a turno y mesa, de forma que se cumpla la condición de que una mesa no pueda tener más de un mozo asignado.
## 3. Ventas
#### 3.1 Consideraciones generales
*    Las ventas registran la fecha/hora en que fueron realizadas.
#### 3.2 Implementacion
*    Se ha considerado ociosa la referencia al turno en que se realizó la venta contando ya con las otras referencias mediante las cuales puede obtenerse.
*    En el modelo se utiliza un tipo de dato sql server decimal.

# Seguridad
## 1. Autenticación y autorización
### 1.1 Método de autenticación
*    Se utiliza JWT (JSON Web Token). RFC 7519.  
### 1.2 Códigos de error HTTP adecuados y consecuentes con la especificación RFC9110.
*    Se normaliza la respuesta ante errores basada en el objeto de respuesta ValidationProblemDetails (https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.validationproblemdetails?view=aspnetcore-8.0) devuelto por defecto en la validación automática. Se han creado clases para manejar distintos códigos de error.
### 1.3 Capa de validación intermedia basada en DTOs. 
* Se utiliza una arquitectura de DTOs (Data Transfer Objects) específicos para las solicitudes acotando las entidades con correspondencia en la base de datos.
### 1.4 Por hacer...
*    Se reconoce la necesidad de utilizar un almacén de claves.
