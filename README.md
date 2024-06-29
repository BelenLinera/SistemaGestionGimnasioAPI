SistemaGestionGimnasioApi

Pasos para levantar el proyecto

Paso 1: clonar el repositorio https://github.com/BelenLinera/SistemaGestionGimnasioAPI
Paso 2: crear una base de datos en mySql
Paso 3: entrar a visual studio y abrir el proyecto
Paso 4: configurar el appSetting.json y el launchSetting.json
Para configurar el appSetting.json:
en la parte de database hay que poner el nombre de la base de datos que creas en  mySql y en password, la contraseña que tenes en mySql
"ConnectionStrings": {
    "SystemGymDBConnectionString": "server=localhost;port=3306;database=sistema_gimnasio;user=root;password=123456Beli;"
  }
Para configurar el launchSetting.json
en la parte de localhost tenes que poner el mismo número que aparece en el archivo api.js en el front en esta linea ( baseURL: 'https://localhost:7254',)
 "applicationUrl": "https://localhost:7254;http://localhost:5106",
Paso 5: hacer una migración desde la consola administrador de paquetes, los comando sería
Add-migration initialMigration -DBContext SystemContext
update-Database
Paso 6: Configurar una variable de entorno de una cuenta SendGrid. En el caso de no tener una comunicarse con josedaniele493@gmail.com
Paso 7: Correr la aplicacion desde el boton SistemaGestionGimnasioApi


Tecnologías utilizadas:

C#
.NET
EntityFrameworkCore
AutoMapper
JWT (Authentication)
SendGrid
Swagger
NUnit
Moq
Microsoft.Net.Test.Sdk
MSTest

Frameworks:
AspNetCore
NETCore






