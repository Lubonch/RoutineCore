# Copilot Instructions - RoutineCore

Este archivo define el contexto de desarrollo, stack tecnológico, arquitectura y patrones de codificación de la solución **RoutineCore**, la réplica modernizada de los sistemas de Rutina Operativa.

## 📁 Estructura del Agente y Skills
Esta solución utiliza un agente de Copilot configurado localmente.
- **Directorio de Configuración:** [.agent](../.agent) (Ubicación relativa)
- **Índice de Skills:** [.agent/README.md](../.agent/README.md)
- **Inicialización de Sesiones:** [.agent/INIT.md](../.agent/INIT.md)

Al comenzar cada sesión, se debe invocar el checklist de inicialización descrito en [.agent/INIT.md](../.agent/INIT.md) para sincronizar las reglas de codificación y cargar los skills bajo demanda.

---

## 🛠️ Stack Tecnológico Reconocido

### 1️⃣ Frontend (SPA Desacoplada)
- **Framework:** Angular 18+ / 19 (Standalone Components, Routing nativo, RxJS, y gestión de estado moderno).
- **Herramientas de Configuración:** Vite / Esbuild como bundler local.
- **Distribución:** Totalmente desacoplada del Backend. En producción, la API de .NET 10 servirá la SPA estática mediante `MapFallbackToFile("index.html")`.

### 2️⃣ Backend (API Gateways y Servicios)
- **Framework:** ASP.NET Core (.NET 10).
- **ORM:** NHibernate (adaptado a .NET Core / .NET 10, con mapeo Fluent o por atributos).
- **Arquitectura de Base de Datos:** PostgreSQL.
- **Estructura del Proyecto:** Unificación de los endpoints en una única API modular para `/api/site/*` (Counter Site) y `/api/integration/*` (APIs de integración).

### 3️⃣ ActionCenter (Worker de Mensajería)
- **Tipo de Proyecto:** .NET 10 Worker Service.
- **Tecnología de Mensajería:** MassTransit con RabbitMQ.
- **Implementación de Plugins:** Los antiguos "plugins" se transforman en consumidores tipados (`IConsumer<TMessage>`) inyectados dinámicamente y estructurados de forma modular.

### 4️⃣ Seguridad y Roles
- **Autenticación:** Autenticación local mediante JWT (JSON Web Tokens).
- **Autorización:** Autorización basada en Roles (`[Authorize(Roles = "Operator,Admin,...")]`) y Claims de usuario, persistidos de forma sencilla en la base de datos principal, sin la complejidad heredada del sistema centralizado de seguridad externa.

---

## 📐 Patrones Arquitectónicos y Convenciones

### Convenciones de C# y .NET 10
- **Inyección de Dependencias:** Inyección obligatoria por constructor para todas las dependencias.
- **Naming:** Clases, Interfaces (`I[Interface]`), Records y Métodos en `PascalCase`. Variables locales y parámetros en `camelCase`.
- **Manejo de Errores:** Middleware global de captura de excepciones (`UseExceptionHandler`) para transformar errores en objetos `ProblemDetails`.
- **Acceso a Datos:** Repositorios de NHibernate inyectados mediante interfaces de repositorio por entidad, manteniendo transaccionalidad mediante patrones integrados (UoW - Unit of Work con NHibernate session por request).

### Convenciones de TypeScript y Angular
- **Componentes:** Componentes modernos Standalone sin módulos innecesarios.
- **Inyección:** Inyección utilizando la función nativa `inject(MyService)` o por constructor.
- **Naming:** Clases en `PascalCase`, selectores y nombres de archivos en `kebab-case` (ej. `user-list.component.ts`), y variables/funciones en `camelCase`.

---

## 🔄 Flujo de Trabajo (Workflow)
1. **Creación de Funcionalidades:** Se debe analizar el código base oculto de los proyectos originales dentro de `Proyectos base/Counter/src` antes de proponer implementaciones.
2. **Testing:** Adoptar TDD adaptado al stack tecnológico, utilizando xUnit y FluentAssertions para backend, y Jasmine/Karma o Jest para frontend.
3. **Commit y Merge:** Los commits deben usar prefijos semánticos (`feat:`, `fix:`, `refactor:`, `chore:`, `test:`).
