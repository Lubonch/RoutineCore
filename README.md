# RoutineCore

**Sistema moderno de gestión de Rutina Operativa** — planificación de turnos, control de asistencia, registro de fichadas y administración de ausencias para equipos de producción.

Construido con una arquitectura limpia de capas sobre **.NET 10**, **Angular 18**, **NHibernate**, **PostgreSQL** y **RabbitMQ**.

---

## 📋 Tabla de Contenidos

- [Descripción General](#-descripción-general)
- [Arquitectura](#-arquitectura)
- [Stack Tecnológico](#-stack-tecnológico)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Modelo de Datos](#-modelo-de-datos)
- [API Endpoints](#-api-endpoints)
- [Requisitos Previos](#-requisitos-previos)
- [Instalación y Configuración](#-instalación-y-configuración)
- [Ejecución](#-ejecución)
- [Seguridad y Autenticación](#-seguridad-y-autenticación)
- [Mensajería y Eventos](#-mensajería-y-eventos)
- [Frontend (SPA)](#-frontend-spa)
- [Base de Datos](#-base-de-datos)
- [Convenciones de Código](#-convenciones-de-código)
- [Licencia](#-licencia)

---

## 📖 Descripción General

RoutineCore es una réplica modernizada de sistemas de Rutina Operativa, diseñada para:

- **Planificar turnos y horarios** de personal operativo asignado a tareas de producción.
- **Registrar fichadas** (punches) de entrada/salida mediante terminales biométricas o web.
- **Gestionar ausencias** con flujo de autorización y aprobación.
- **Publicar eventos de integración** hacia sistemas externos vía mensajería asincrónica.

El sistema está pensado para entornos de producción audiovisual, broadcasting y operaciones donde la planificación de personal es crítica.

---

## 🏗️ Arquitectura

RoutineCore sigue una **arquitectura de capas limpias (Clean Architecture)** con separación clara de responsabilidades:

```
┌─────────────────────────────────────────────────────────┐
│                   RoutineCore.Web                       │
│              (Angular 18 SPA - Frontend)                │
└──────────────────────┬──────────────────────────────────┘
                       │ HTTP / REST
┌──────────────────────▼──────────────────────────────────┐
│                   RoutineCore.Api                       │
│           (ASP.NET Core API - Controladores)            │
│         Swagger · JWT Auth · CORS · Static Files        │
└──────┬───────────────────────────────────┬──────────────┘
       │                                   │ MassTransit
┌──────▼──────────────┐          ┌─────────▼─────────────┐
│ RoutineCore.        │          │ RoutineCore.           │
│ Application         │          │ Dispatcher             │
│ (DTOs, Servicios,   │          │ (Worker Service)       │
│  Lógica de negocio) │          │ Consumers de RabbitMQ  │
└──────┬──────────────┘          └────────────────────────┘
       │
┌──────▼──────────────┐
│ RoutineCore.        │
│ Infrastructure      │
│ (NHibernate, Repos) │
└──────┬──────────────┘
       │
┌──────▼──────────────┐
│ RoutineCore.Domain  │
│ (Entidades,         │
│  Contratos, Repos)  │
└──────┬──────────────┘
       │
  ┌────▼────┐     ┌──────────┐
  │PostgreSQL│     │ RabbitMQ │
  └─────────┘     └──────────┘
```

### Flujo de datos

1. El **frontend Angular** se comunica con la API REST.
2. La **API** orquesta los servicios de aplicación, persiste datos vía NHibernate y publica eventos a RabbitMQ.
3. El **Dispatcher** (Worker Service) consume eventos de la cola y ejecuta lógica de integración con sistemas externos.

---

## 🛠️ Stack Tecnológico

| Capa | Tecnología | Versión |
|------|-----------|---------|
| **Frontend** | Angular (Standalone Components) | 18+ |
| **Backend API** | ASP.NET Core | .NET 10 |
| **ORM** | FluentNHibernate | Latest |
| **Base de Datos** | PostgreSQL | 14+ |
| **Autenticación** | JWT Bearer Tokens | — |
| **Mensajería** | MassTransit + RabbitMQ | 8.2.x |
| **Worker Service** | .NET 10 Worker | — |
| **API Docs** | Swagger / Swashbuckle | 6.6.x |
| **Bundler Frontend** | Angular CLI / Esbuild | — |

---

## 📁 Estructura del Proyecto

```
RoutineCore/
├── RoutineCore.sln                    # Solución de Visual Studio
├── sql/
│   └── setup_db.sql                   # Script DDL + seed data (PostgreSQL)
│
├── src/
│   ├── RoutineCore.Domain/            # Capa de Dominio
│   │   ├── Entities/                  # Entidades del negocio
│   │   │   ├── Employee.cs
│   │   │   ├── Punch.cs
│   │   │   ├── Schedule.cs
│   │   │   ├── Absence.cs
│   │   │   └── ProjectTask.cs
│   │   ├── Contracts/                 # Eventos de integración
│   │   │   └── IntegrationEvents.cs
│   │   └── Repositories/             # Interfaces de repositorios
│   │       ├── IRepository.cs         # Repositorio genérico base
│   │       ├── IEmployeeRepository.cs
│   │       ├── IPunchRepository.cs
│   │       ├── IScheduleRepository.cs
│   │       ├── IAbsenceRepository.cs
│   │       └── IProjectTaskRepository.cs
│   │
│   ├── RoutineCore.Infrastructure/    # Capa de Infraestructura
│   │   ├── NHibernateConfigurator.cs  # Setup de SessionFactory + SchemaUpdate
│   │   ├── Mappings/                  # Mapeos FluentNHibernate
│   │   │   ├── EmployeeMap.cs
│   │   │   ├── PunchMap.cs
│   │   │   ├── ScheduleMap.cs
│   │   │   ├── AbsenceMap.cs
│   │   │   └── ProjectTaskMap.cs
│   │   └── Repositories/             # Implementaciones de repositorios
│   │       ├── NHibernateRepository.cs  # Repositorio genérico
│   │       ├── EmployeeRepository.cs
│   │       ├── PunchRepository.cs
│   │       ├── ScheduleRepository.cs
│   │       ├── AbsenceRepository.cs
│   │       └── ProjectTaskRepository.cs
│   │
│   ├── RoutineCore.Application/       # Capa de Aplicación
│   │   ├── Dtos/
│   │   │   └── RoutineCoreDtos.cs     # Records: EmployeeDto, PunchDto, etc.
│   │   └── Services/
│   │       ├── Interfaces/            # Contratos de servicios
│   │       │   ├── IAuthService.cs
│   │       │   ├── IEmployeeService.cs
│   │       │   ├── IPunchService.cs
│   │       │   ├── IScheduleService.cs
│   │       │   ├── IAbsenceService.cs
│   │       │   ├── IProjectTaskService.cs
│   │       │   ├── IJwtTokenGenerator.cs
│   │       │   └── IEventsPublisher.cs
│   │       └── Implementations/       # Lógica de negocio
│   │           ├── AuthService.cs
│   │           ├── EmployeeService.cs
│   │           ├── PunchService.cs
│   │           ├── ScheduleService.cs
│   │           ├── AbsenceService.cs
│   │           ├── ProjectTaskService.cs
│   │           └── JwtTokenGenerator.cs
│   │
│   ├── RoutineCore.Api/               # API Web (Host principal)
│   │   ├── Program.cs                 # Configuración y pipeline
│   │   ├── appsettings.json           # Connection strings, JWT, RabbitMQ
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── EmployeeController.cs
│   │   │   ├── PunchController.cs
│   │   │   ├── ScheduleController.cs
│   │   │   ├── AbsenceController.cs
│   │   │   └── ProjectTaskController.cs
│   │   └── Services/
│   │       └── EventsPublisher.cs     # Publicador MassTransit
│   │
│   ├── RoutineCore.Dispatcher/        # Worker Service (Mensajería)
│   │   ├── Program.cs                 # Host de consumidores RabbitMQ
│   │   ├── appsettings.json
│   │   └── Consumers/
│   │       ├── PunchRegisteredConsumer.cs
│   │       └── AbsenceApprovedConsumer.cs
│   │
│   └── RoutineCore.Web/               # Frontend Angular SPA
│       ├── package.json
│       ├── tsconfig.json
│       └── src/
│           ├── main.ts                # Bootstrap Angular
│           └── app/
│               ├── app.component.ts   # Shell de la aplicación
│               ├── app.config.ts      # Providers (HttpClient, Router)
│               ├── app.routes.ts      # Rutas con lazy loading + guards
│               ├── services/
│               │   ├── auth.service.ts      # Estado de sesión (Signals)
│               │   └── auth.interceptor.ts  # Interceptor JWT
│               └── components/
│                   ├── login/               # Inicio de sesión
│                   ├── dashboard/           # Panel principal
│                   ├── punch/               # Registro de fichadas
│                   ├── schedule/            # Planificación de turnos
│                   └── absence-request/     # Solicitudes de ausencia
│
├── .github/
│   └── copilot-instructions.md        # Reglas de codificación para Copilot
│
└── .agent/                            # Configuración del agente AI
    ├── AGENTS.md
    ├── INIT.md
    ├── README.md
    └── skills/                        # Skills especializadas
```

---

## 💾 Modelo de Datos

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│  employees   │     │project_tasks │     │  schedules   │
├──────────────┤     ├──────────────┤     ├──────────────┤
│ id (PK)      │◄──┐ │ id (PK)      │◄──┐ │ id (PK)      │
│ name         │   │ │ description  │   │ │ employee_id  │──► employees
│ email (UQ)   │   │ │ project_code │   │ │ start_time   │
│ role         │   │ │ is_active    │   │ │ end_time     │
│ employee_code│   │ └──────────────┘   │ │project_task_id──► project_tasks
│ is_active    │   │                    │ │ status       │
└──────────────┘   │                    │ └──────────────┘
       ▲           │                    │
       │           │                    │
┌──────┴───────┐   │  ┌──────────────┐  │
│   punches    │   │  │  absences    │  │
├──────────────┤   │  ├──────────────┤  │
│ id (PK)      │   │  │ id (PK)      │  │
│ employee_id  │───┘  │ employee_id  │──┘
│ punch_time   │      │ start_date   │
│ direction    │      │ end_date     │
│ device_code  │      │ reason       │
│ processed    │      │ authorized   │
└──────────────┘      │ approved_by  │
                      └──────────────┘
```

### Entidades

| Entidad | Descripción |
|---------|------------|
| **Employee** | Personal operativo con código único, email, rol y estado activo/inactivo |
| **ProjectTask** | Tareas de producción asignables (ej: Sala de Control, Estudio de Noticias) |
| **Schedule** | Turnos planificados que vinculan empleados con tareas en franjas horarias |
| **Punch** | Fichadas de entrada/salida con timestamp, dirección y dispositivo |
| **Absence** | Solicitudes de ausencia con período, motivo y flujo de autorización |

### Roles del sistema

| Rol | Permisos |
|-----|---------|
| `Admin` | Acceso total: CRUD de empleados, turnos, fichadas, ausencias |
| `Operator` | Gestión de turnos y fichadas, registro de punches |
| `Manager` | Visualización general y aprobación de ausencias |

---

## 🔌 API Endpoints

Todos los endpoints están bajo el prefijo `/api/v1/`. La documentación interactiva está disponible en `/swagger` (solo en Development).

### Autenticación

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `POST` | `/api/v1/auth/login` | Iniciar sesión (retorna JWT + datos del empleado) | ❌ |

**Body de login:**
```json
{
  "email": "admin@routinecore.com",
  "employeeCode": "EMP001"
}
```

### Empleados

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/employees` | Listar todos los empleados | ✅ |
| `GET` | `/api/v1/employees/{id}` | Obtener empleado por ID | ✅ |
| `POST` | `/api/v1/employees` | Crear empleado | ✅ Admin |
| `PUT` | `/api/v1/employees/{id}` | Actualizar empleado | ✅ Admin |
| `DELETE` | `/api/v1/employees/{id}` | Eliminar empleado | ✅ Admin |

### Fichadas (Punches)

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `POST` | `/api/v1/punches/register` | Registrar fichada (In/Out) | ✅ |
| `GET` | `/api/v1/punches/unprocessed` | Obtener fichadas sin procesar | ✅ |
| `POST` | `/api/v1/punches/process` | Procesar fichadas pendientes | ✅ |

### Turnos (Schedules)

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/schedules` | Listar todos los turnos | ✅ |
| `GET` | `/api/v1/schedules/employee/{id}` | Turnos de un empleado | ✅ |
| `POST` | `/api/v1/schedules` | Crear turno | ✅ Admin/Operator |

### Ausencias

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/absences` | Listar todas las ausencias | ✅ |
| `GET` | `/api/v1/absences/employee/{id}` | Ausencias de un empleado | ✅ |
| `POST` | `/api/v1/absences` | Solicitar ausencia | ✅ |
| `PUT` | `/api/v1/absences/{id}/approve` | Aprobar ausencia | ✅ Admin/Manager |

### Tareas de Proyecto

| Método | Ruta | Descripción | Auth |
|--------|------|-------------|------|
| `GET` | `/api/v1/project-tasks` | Listar tareas de proyecto | ✅ |
| `POST` | `/api/v1/project-tasks` | Crear tarea | ✅ Admin |

---

## 📦 Requisitos Previos

Antes de ejecutar RoutineCore, asegúrate de tener instalados:

| Requisito | Versión mínima | Instalación |
|-----------|---------------|------------|
| **.NET SDK** | 10.0 | [dotnet.microsoft.com](https://dotnet.microsoft.com/download) |
| **Node.js** | 18+ | [nodejs.org](https://nodejs.org) |
| **PostgreSQL** | 14+ | [postgresql.org](https://www.postgresql.org/download/) |
| **RabbitMQ** | 3.12+ | [rabbitmq.com](https://www.rabbitmq.com/download.html) |

---

## ⚙️ Instalación y Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/Lubonch/RoutineCore.git
cd RoutineCore
```

### 2. Configurar PostgreSQL

Crear la base de datos y ejecutar el script de setup:

```bash
# Crear la base de datos
psql -U postgres -c "CREATE DATABASE routinecore;"

# Ejecutar el script DDL + seed data
psql -U postgres -d routinecore -f sql/setup_db.sql
```

> **Nota:** El ORM NHibernate también ejecuta `SchemaUpdate` automáticamente al iniciar la API, creando las tablas si no existen.

### 3. Configurar la conexión

Editar `src/RoutineCore.Api/appsettings.json` si tu PostgreSQL usa credenciales diferentes:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=routinecore;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Secret": "SuperSecretKeyWithAtLeast32BytesLength!",
    "Issuer": "RoutineCoreIssuer",
    "Audience": "RoutineCoreAudience"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Username": "guest",
    "Password": "guest"
  }
}
```

### 4. Restaurar dependencias

```bash
# Backend
dotnet restore RoutineCore.sln

# Frontend
cd src/RoutineCore.Web
npm install
```

---

## 🚀 Ejecución

### Backend (API)

```bash
cd src/RoutineCore.Api
dotnet run
```

La API estará disponible en `https://localhost:5001` (o el puerto configurado).  
Swagger UI: `https://localhost:5001/swagger`

### Frontend (Angular SPA)

```bash
cd src/RoutineCore.Web
npm start
```

La SPA estará disponible en `http://localhost:4200`.

### Dispatcher (Worker Service)

```bash
cd src/RoutineCore.Dispatcher
dotnet run
```

El worker se conectará a RabbitMQ y comenzará a consumir eventos.

### Credenciales de prueba (seed data)

| Email | Código | Rol |
|-------|--------|-----|
| `admin@routinecore.com` | `EMP001` | Admin |
| `operator@routinecore.com` | `EMP002` | Operator |
| `manager@routinecore.com` | `EMP003` | Manager |
| `supervisor@routinecore.com` | `EMP004` | Operator |

---

## 🔐 Seguridad y Autenticación

RoutineCore utiliza **JWT (JSON Web Tokens)** para autenticación stateless:

1. El usuario se autentica en `POST /api/v1/auth/login` con email + código de empleado.
2. La API retorna un token JWT firmado con HMAC-SHA256.
3. El frontend almacena el token en `localStorage` y lo envía como header `Authorization: Bearer <token>` en cada request.
4. Los endpoints protegidos validan el token y extraen claims de rol para autorización.

### Flujo de autenticación en el frontend

- **`AuthService`**: Maneja estado de sesión con Angular Signals (`currentUser`, `isAuthenticated`, `hasRole`).
- **`AuthInterceptor`**: Interceptor HTTP que adjunta el token JWT automáticamente.
- **Route Guards**: Protección de rutas con `canActivate` que redirige a `/login` si no hay sesión.

---

## 📡 Mensajería y Eventos

RoutineCore usa **MassTransit** sobre **RabbitMQ** para comunicación asincrónica entre servicios:

### Eventos publicados por la API

| Evento | Descripción | Datos |
|--------|-------------|-------|
| `PunchRegisteredEvent` | Se registró una nueva fichada | PunchId, EmployeeId, PunchTime, Direction, DeviceCode |
| `AbsenceApprovedEvent` | Se aprobó una ausencia | AbsenceId, EmployeeId, StartDate, EndDate, Reason, ApprovedBy |

### Consumers del Dispatcher

| Consumer | Cola | Función |
|----------|------|---------|
| `PunchRegisteredConsumer` | `pulsedispatcher-punches` | Procesa fichadas y las sincroniza con sistemas externos |
| `AbsenceApprovedConsumer` | `pulsedispatcher-absences` | Notifica ausencias aprobadas a sistemas de payroll/BI |

---

## 🖥️ Frontend (SPA)

La aplicación Angular utiliza **Standalone Components** (sin NgModules) y **Signals** para gestión de estado reactivo.

### Rutas

| Ruta | Componente | Guard | Descripción |
|------|-----------|-------|-------------|
| `/login` | `LoginComponent` | — | Inicio de sesión |
| `/dashboard` | `DashboardComponent` | Auth | Panel con métricas y resumen |
| `/punch` | `PunchComponent` | Auth | Registro de fichadas web |
| `/schedule` | `ScheduleComponent` | Auth | Planificación de turnos |
| `/absence` | `AbsenceRequestComponent` | Auth | Solicitudes de ausencia |

### Características

- **Lazy Loading**: Todos los componentes se cargan bajo demanda con `loadComponent()`.
- **Signals**: Estado reactivo moderno sin necesidad de servicios observables complejos.
- **Role-based UI**: Los elementos de gestión solo son visibles para Admin/Operator.
- **Responsive**: Layouts con CSS Grid y flexbox adaptables.

---

## 🗄️ Base de Datos

### NHibernate

El ORM está configurado con **FluentNHibernate** y mapeos tipados por clase:

- **SessionFactory**: Singleton registrado en DI, configurado en `NHibernateConfigurator.cs`.
- **ISession**: Scoped por request HTTP (Unit of Work pattern).
- **SchemaUpdate**: Ejecuta migraciones automáticas al inicio de la aplicación.

### Script de seed

El archivo `sql/setup_db.sql` contiene:

1. Creación de tablas (`employees`, `project_tasks`, `schedules`, `punches`, `absences`).
2. Truncado de datos existentes para desarrollo.
3. Datos de prueba: 4 empleados, 4 tareas, 3 turnos, 2 fichadas, 1 ausencia.

---

## 📏 Convenciones de Código

### C# / .NET

- Inyección de dependencias por constructor.
- Naming: `PascalCase` para clases, interfaces (`I[Name]`), records y métodos. `camelCase` para variables locales.
- DTOs definidos como `record` inmutables.
- Middleware global de excepciones con `ProblemDetails`.

### TypeScript / Angular

- Componentes Standalone sin módulos.
- Inyección con `inject()` nativo.
- Selectores y archivos en `kebab-case`.
- Variables y funciones en `camelCase`.

### Git

Commits con prefijos semánticos:

```
feat:     Nueva funcionalidad
fix:      Corrección de bug
refactor: Refactorización sin cambio funcional
chore:    Tareas de mantenimiento
test:     Tests
```

---

## 📄 Licencia

Este proyecto está licenciado bajo la [MIT License](LICENSE).

**Copyright © 2026 Lucas Eduardo Ramirez Bonch**