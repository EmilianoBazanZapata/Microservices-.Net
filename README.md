
# 🏗️ Clean Architecture Microservices con ASP.NET 6, RabbitMQ y Docker

Este es un proyecto educativo desarrollado como parte del curso **"Clean Architecture en Microservices con ASP.NET 8 RabbitMQ y Docker Containers | CQRS | IoC | Docker Containers"** de **Vazy Drez**. Este proyecto implementa microservicios siguiendo los principios de Clean Architecture y patrones como **CQRS**, **IoC**, y el uso de **RabbitMQ** para la comunicación entre microservicios.

---

## 📋 Características
- **Clean Architecture**: Separación clara de responsabilidades entre capas.
- **Microservicios**:
  - **Banking API**: Maneja la lógica de cuentas bancarias.
  - **Transfer API**: Procesa transferencias entre cuentas.
- **RabbitMQ**: Comunicación basada en eventos entre microservicios.
- **Supabase**: Base de datos PostgreSQL para almacenar información.
- **Azure App Services**: Despliegue en la nube.
- **CloudAMQP**: Instancia de RabbitMQ gestionada.

---

## 🛠️ Requisitos Previos
Antes de comenzar, asegúrate de tener lo siguiente instalado:
- **.NET 8 SDK**
- **Docker Desktop** (para ejecutar RabbitMQ y PostgreSQL localmente)
- **Postman** o cualquier cliente para probar APIs.
- **Git** para clonar el repositorio.

---

## 🚀 Configuración del Proyecto Localmente

### **1. Clona el Repositorio**
```bash
git clone https://github.com/tu-usuario/tu-repositorio.git
cd tu-repositorio
```

### **2. Configura RabbitMQ y PostgreSQL con Docker**
Si deseas ejecutar RabbitMQ y PostgreSQL localmente, utiliza este archivo `docker-compose.yml`:

```yaml
version: '3.8'
services:
  rabbitmq:
    image: rabbitmq:3-management
    container_name: rabbitmq
    ports:
      - "5672:5672" # RabbitMQ server
      - "15672:15672" # RabbitMQ management
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest

  postgres:
    image: postgres:13
    container_name: postgres
    ports:
      - "5432:5432"
    environment:
      POSTGRES_USER: supabase
      POSTGRES_PASSWORD: password
      POSTGRES_DB: microservicesdb
```

Ejecuta el comando:
```bash
docker-compose up -d
```

### **3. Configura las Cadenas de Conexión**
Edita los archivos `appsettings.json` en los microservicios:

- **Banking API (`BankingDbConnection`)**
```json
"ConnectionStrings": {
  "BankingDbConnection": "Host=localhost;Database=bankingdb;Username=user;Password=password"
}
```

- **Transfer API (`TransferDbConnection`)**
```json
"ConnectionStrings": {
  "TransferDbConnection": "Host=localhost;Database=transferdb;Username=user;Password=password"
}
```

- **RabbitMQ**
```json
"RabbitMqSettings": {
  "Host": "localhost",
  "Username": "guest",
  "Password": "guest"
}
```

### **4. Genera Migraciones y Actualiza la Base de Datos**
En cada microservicio (`Banking` y `Transfer`), ejecuta:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### **5. Ejecuta los Microservicios**
Desde la raíz de cada proyecto (`Banking.Api` y `Transfer.Api`), ejecuta:
```bash
dotnet run
```

Los microservicios estarán disponibles en:
- Banking API: `https://localhost:5162/swagger`
- Transfer API: `https://localhost:5001/swagger`

---

## 🧪 Pruebas
1. Usa Postman para probar los endpoints de los microservicios.
2. **Importante**: Para las pruebas entre la API de **Banking** y la API de **Transfer**, asegúrate de que el monto no exceda **999.99**, ya que existen limitaciones definidas en el esquema de la base de datos.
3. Simula transferencias desde la API de **Banking** hacia **Transfer**:
   - Envía transferencias utilizando eventos en RabbitMQ.

---

## 🛡️ Despliegue en la Nube
El proyecto está configurado para ser desplegado en:
- **Azure App Services**: Aloja los microservicios.
- **Supabase**: Base de datos PostgreSQL en la nube.
- **CloudAMQP**: RabbitMQ gestionado.

Consulta ante cualquier duda o adapta el proceso a tus necesidades.

---

## 📚 Recursos Relacionados
- **Curso de Vazy Drez**: [Enlace al curso](https://www.udemy.com/share/105NX43@IX5aIAOAPNIs9FacO-X_wDRU8F7tUNbp3o4oLNcqATu4L38JESFTaLiJLW6fMqxqbA==/)
- **Documentación de RabbitMQ**: [rabbitmq.com](https://www.rabbitmq.com/)
- **Documentación de Supabase**: [supabase.com](https://supabase.com/)
- **Documentación de ASP.NET Core**: [learn.microsoft.com](https://learn.microsoft.com/es-es/aspnet/core/)

---

## 👥 Contribuciones
Si encuentras algún problema o tienes ideas para mejorar este proyecto, no dudes en abrir un issue o realizar un pull request.

---

## 🏆 Créditos
Este proyecto fue desarrollado como parte del curso de **Vazy Drez** y sigue las mejores prácticas de Clean Architecture.
