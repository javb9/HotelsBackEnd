# HotelsBackEnd

Este es el backend para la gestión de hoteles, desarrollado con .NET. Proporciona una API para manejar reservaciones, clientes y habitaciones.

## Características

- Gestión de hoteles
- Gestión de habitaciones
- Validación de disponibilidad de fechas
- Gestion de reservas

## Requisitos

- .NET 6.0 o superior
- SQL Server

## Configuración

1. Clona el repositorio:
    ```bash
    git clone https://github.com/javb9/HotelsBackEnd.git
    cd HotelsBackEnd
    ```

2. Configura la cadena de conexión a la base de datos en `appsettings.json`:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=.;Database=HotelsDB;Trusted_Connection=True;"
    }
    ```

3. Aplica las migraciones a la base de datos:
    ```bash
    dotnet ef database update
    ```

4. Ejecuta la aplicación:
    ```bash
    dotnet run
    ```

## Endpoints

### Administracion de Hotel

-  POST  /api/HotelManagement/CreateHotel        - Creacion de hotel
-  PUT   /api/HotelManagement/UpdateHotel        - Actualizacion de hotel 
-  PATCH /api/HotelManagement/UpdateStatusHotel  - Activacion o desactivacion de hotel
-  POST  /api/HotelManagement/CreateRoom         - Creacion de habitacion
-  PUT   /api/HotelManagement/UpdateRoom         - Actualizacion de habitacion 
-  PATCH /api/HotelManagement/UpdateStatusRoom   - Activacion o desactivacion de habitacion

### Reservacion de Hotel

-  GET   /api/HotelReservation/AvailableRooms             -  Obtiene habitaciones disponibles segun filtros especificados 
-  POST  /api/HotelReservation/CreateReservation          -  Crea una reservacion
-  GET   /api/HotelReservation/Reservations               -  Lista las reservaciones realizadas
-  PATCH /api/HotelReservation/UpdateStatusReservation    - Activacion o desactivacion de reservas realizadas
## Arquitectura

Este proyecto sigue los principios de la Arquitectura Orientada al Dominio (DDD) y utiliza la Inyección de Dependencia para gestionar las dependencias entre los componentes. La estructura del proyecto está organizada de la siguiente manera:

- **Domain**: Contiene las Modelos, Enumeradores, e interfaces.
- **Application**: Contiene los Servicios de aplicacion, DTO'S y contexto de la BD usando EF (Entity framework).
- **Infrastructure**: Contiene las implementaciones de las interfaces y otro servicio de infraestructura, como lo es el envio de correo.
- **API**: Contiene los controladores y la configuración de la API.
