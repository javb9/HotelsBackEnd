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

- URL SWAGGER http://hotelsback.somee.com/swagger/index.html (en la publicacion se permitio que se visualizara el swagger por efectos de pruebas, de igual manera la API es consumible con POSTMAN)

### Administracion de Hotel

-  POST  http://hotelsback.somee.com/api/HotelManagement/CreateHotel        - Creacion de hotel
-  PUT   http://hotelsback.somee.com/api/HotelManagement/UpdateHotel        - Actualizacion de hotel 
-  PATCH http://hotelsback.somee.com/api/HotelManagement/UpdateStatusHotel  - Activacion o desactivacion de hotel
-  POST  http://hotelsback.somee.com/api/HotelManagement/CreateRoom         - Creacion de habitacion
-  PUT   http://hotelsback.somee.com/api/HotelManagement/UpdateRoom         - Actualizacion de habitacion 
-  PATCH http://hotelsback.somee.com/api/HotelManagement/UpdateStatusRoom   - Activacion o desactivacion de habitacion

### Reservacion de Hotel

-  GET   http://hotelsback.somee.com/api/HotelReservation/AvailableRooms             -  Obtiene habitaciones disponibles segun filtros especificados 
-  POST  http://hotelsback.somee.com/api/HotelReservation/CreateReservation          -  Crea una reservacion
-  GET   http://hotelsback.somee.com/api/HotelReservation/Reservations               -  Lista las reservaciones realizadas
-  PATCH http://hotelsback.somee.com/api/HotelReservation/UpdateStatusReservation    - Activacion o desactivacion de reservas realizadas

## Arquitectura

Este proyecto sigue los principios de la Arquitectura Orientada al Dominio (DDD) y utiliza la Inyección de Dependencia para gestionar las dependencias entre los componentes. La estructura del proyecto está organizada de la siguiente manera:

- **Domain**: Contiene las Modelos, Enumeradores, e interfaces.
- **Application**: Contiene los Servicios de aplicacion, DTO'S y contexto de la BD usando EF (Entity framework).
- **Infrastructure**: Contiene las implementaciones de las interfaces y otro servicio de infraestructura, como lo es el envio de correo.
- **API**: Contiene los controladores y la configuración de la API.
