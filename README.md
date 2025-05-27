# Proyecto Examen API .NET

## Descripción

Esta es una **API REST** desarrollada en .NET 6/7 para gestionar **Cursos** y **Estudiantes**. Cada estudiante puede estar inscrito en un único curso.

## Estructura del proyecto

``` plaintext
Examen
└── api
    ├── Controllers
    │   ├── CourseController.cs
    │   └── StudentController.cs
    ├── Data
    │   └── ApplicationDBContext.cs
    ├── Dtos
    │   ├── Course
    │   │   ├── CreateCourseDto.cs
    │   │   ├── UpdateCourseDto.cs
    │   │   └── CourseDto.cs
    │   └── Student
    │       ├── CreateStudentDto.cs
    │       ├── UpdateStudentDto.cs
    │       └── StudentDto.cs
    ├── Mappers
    │   ├── CourseMapper.cs
    │   └── StudentMapper.cs
    ├── Models
    │   ├── Course.cs
    │   └── Student.cs
    ├── Migrations
    ├── UploadedImages                # Carpeta para imágenes de cursos
    ├── appsettings.json
    └── Program.cs
```

## Requisitos previos

- [.NET 6.0 SDK o superior](https://dotnet.microsoft.com/download)
- SQL Server (puede ser local o remoto) escuchando en el puerto configurado (por defecto `1433`)
- Herramienta de EF Core CLI:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Configuración de la cadena de conexión

En el archivo `appsettings.json` define tu cadena bajo `ConnectionStrings:DefaultConnection`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=Examen;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Se acalara que esta es la conexion local la computadora en la que se instalo sql

Reemplaza `Server=` y `Database=` según tu entorno.

## Migraciones y base de datos

1. Abre la terminal en la carpeta del proyecto que contiene el archivo `.csproj` (e.g., `Examen/api`).
2. Genera la migración inicial:
  
   ```bash
   dotnet ef migrations add Initial
   ```

  > Crea el script para crear las tablas **Courses** y **Students**.
  
3. Aplica la migración y crea la base de datos:
  
   ```bash
   dotnet ef database update
   ```

Al ejecutar el comando anterior, EF Core:

- Creará la base de datos `Examen` si no existe.
- Ejecutará las migraciones pendientes.

## Ejecución de la API

Durante desarrollo, utiliza **hot reload**:

```bash
cd Examen/api
dotnet watch run
```

O bien, sin hot reload:

```bash
dotnet run
```

La aplicación escuchará por defecto en:

```plaintext
https://localhost:5001
http://localhost:5000
```

## Swagger UI

Para explorar y probar la API automáticamente:

1. Abre en tu navegador:

```plaintext
https://localhost:5001/swagger
```

2. Verás todos los endpoints expuestos y podrás hacer llamadas de prueba.

## Endpoints disponibles

### Cursos (Course)

| Método | Ruta               | Descripción                   |
| ------ | ------------------ | ----------------------------- |
| GET    | `/api/course`      | Listar todos los cursos       |
| GET    | `/api/course/{id}` | Obtener un curso por su ID    |
| POST   | `/api/course`      | Crear un nuevo curso          |
| PUT    | `/api/course/{id}` | Actualizar un curso existente |
| DELETE | `/api/course/{id}` | Eliminar un curso por su ID   |

#### Ejemplo de body para POST/PUT Course

```json
{
  "name": "Curso de Inglés",
  "description": "Nivel básico de inglés conversacional",
  "imageUrl": "https://localhost:5001/uploads/ingles.jpg",
  "schedule": "Mar, Jue 9:00-11:00",
  "professor": "Ana López"
}
```

### Estudiantes (Student)

| Método | Ruta                | Descripción                        |
| ------ | ------------------- | ---------------------------------- |
| GET    | `/api/student`      | Listar todos los estudiantes       |
| GET    | `/api/student/{id}` | Obtener un estudiante por su ID    |
| POST   | `/api/student`      | Crear un nuevo estudiante          |
| PUT    | `/api/student/{id}` | Actualizar un estudiante existente |
| DELETE | `/api/student/{id}` | Eliminar un estudiante por su ID   |

#### Ejemplo de body para POST/PUT Student

```json
{
  "name": "Juan Pérez",
  "email": "juan.perez@example.com",
  "phone": "87654321",
  "courseId": 1
}
```

## Archivos estáticos (imágenes)

- Se utiliza la carpeta `UploadedImages` para almacenar imágenes de cursos.
- Las imágenes se exponen en la ruta `/uploads`, por ejemplo:

  ```plaintext
  https://localhost:5001/uploads/ingles.jpg
  ```

> Asegúrate de que la sección en `Program.cs` para servir archivos estáticos está habilitada:
>
> ```csharp
> app.UseStaticFiles(new StaticFileOptions
> {
>     FileProvider = new PhysicalFileProvider(
>         Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages")
>     ),
>     RequestPath = "/uploads"
> });
> ```

## Notas adicionales

- **Autenticación**: Actualmente no se ha implementado. Puedes agregar JWT o identidad más adelante.
- **Validaciones**: Se usan atributos `DataAnnotations`. Para validaciones más avanzadas, considera FluentValidation.
- **Errores**: Los controladores devuelven códigos HTTP adecuados (400, 404, 201, etc.).
