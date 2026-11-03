# Entrega individual – Sprint 1
## Claudia Alvarado – Gestión de usuarios y pruebas
Proyecto: Sistema de Gestión y Control para la Pulpería El Buen Pastor.

### Contenido
- `GestionUsuarios/`: módulo C# WinForms para listar, registrar, editar y activar/desactivar usuarios.
- `GestionUsuarios/Usuario.cs`: modelo de usuario.
- `GestionUsuarios/UsuarioRepository.cs`: operaciones SQL Server mediante ADO.NET.
- `GestionUsuarios/PasswordHasher.cs`: hash PBKDF2 para no guardar contraseñas en texto plano.
- `GestionUsuarios/Permisos.cs`: verificación sencilla de permisos por rol.
- `GestionUsuarios/FormGestionUsuarios.cs`: formulario funcional construido por código.
- `GestionUsuarios/Program.cs` y `.csproj`: proyecto de demostración ejecutable.
- `sql/`: estructura mínima compatible de tablas `Roles` y `Usuarios`.
- `documentacion/`: casos de prueba y plantilla para registrar resultados reales.

### Requisitos
- Windows 10/11
- Visual Studio 2022
- .NET 8 SDK
- SQL Server Express o SQL Server
- Base de datos del proyecto

### Cómo ejecutar la demostración
1. Abrir `GestionUsuarios/GestionUsuarios.csproj` en Visual Studio.
2. Revisar `GestionUsuarios/UsuarioRepository.cs` y cambiar `ConnectionString` por el servidor y base de datos usados por el equipo.
3. Ejecutar primero `sql/01_tablas_compatibles.sql` solamente si el equipo aún no creó las tablas equivalentes. Si Amber ya creó `Roles` y `Usuarios`, NO ejecutar este script; comparar nombres y adaptar el repositorio a su esquema.
4. Ejecutar con F5. El formulario permite crear, editar, buscar y cambiar el estado de usuarios.

### Integración con el proyecto del equipo
El repositorio de equipo es:
https://github.com/ambermerlo-ux/Sistema-Pulperia-El-Buen-Pastor

Copiar la carpeta `GestionUsuarios` y la carpeta `documentacion` dentro del repositorio, conservando la estructura acordada por el equipo. No reemplazar archivos de otros integrantes.

### Importante antes de entregar
- La cadena de conexión es un ejemplo local y debe ajustarse.
- El script SQL es una estructura de compatibilidad; debe coincidir con el esquema definitivo de Amber.
- Los casos de prueba están preparados, pero deben ejecutarse contra la base real y completar el resultado observado. No marcar pruebas como aprobadas sin ejecutarlas.
- La autorización de acciones debe integrarse con el usuario autenticado por el módulo de inicio de sesión del equipo.
