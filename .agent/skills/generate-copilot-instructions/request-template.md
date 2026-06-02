# Prompt Simple Para Chat

Usa este bloque tal cual en una ventana de chat y completa solo lo que ya sepas.

```text
Genera `.github/copilot-instructions.md` para todas las soluciones dentro de esta carpeta.

Tomá del repo todo lo que puedas inferir y preguntame solo lo minimo que falte.

Todas las soluciones dentro de la carpeta estan dentro del mismo repo

Datos que ya te paso:
- Alcance: varios proyectos
- Si falta `.github`: creala
- Si existe un instructions: no se debe crear uno nuevo
- `.agent`: inferila desde el repo
- Nombre formal: inferila desde cada solucion
- Proposito: inferila segun lo que haga el codigo
- Stack: inferila segun lo que haga el codigo
- Base de datos: inferila segun lo que haga el codigo
- Repo/workflow: <URL-del-repositorio> con rama principal <main|master|develop>, convencion ramas "<convencion>"
- Base opcional: no

Reglas:
- Usa paths relativos reales hacia `.agent`
- No copies reglas de otra solucion sin evidencia o sin mi confirmacion
- Si algo no se puede confirmar, dejalo en `Informacion pendiente por confirmar`
- Verifica que no queden placeholders sin resolver
```

## Ejemplo

```text
Genera `.github/copilot-instructions.md` para `Modulo-Pagos`.

Tomá del repo todo lo que puedas inferir y preguntame solo lo minimo que falte.

Datos que ya te paso:
- Alcance: solucion completa
- Si falta `.github`: creala
- `.agent`: inferila desde el repo
- Nombre formal: `Sistema de Pagos`
- Proposito: `API de procesamiento de pagos y conciliacion bancaria`
- Stack: `C#, .NET 8, ASP.NET Core Web API`
- Base de datos: `SQL Server`
- Repo/workflow: `Azure DevOps, rama main, branches feature/{descripcion}, Azure Boards`
- Convenciones clave: `Serilog para logging, configuracion por ambiente via appsettings`
- Base opcional: no

Reglas:
- Usa paths relativos reales hacia `.agent`
- Si algo no se puede confirmar, dejalo en `Informacion pendiente por confirmar`
- Verifica que no queden placeholders sin resolver
```