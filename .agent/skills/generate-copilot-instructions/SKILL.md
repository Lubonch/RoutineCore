---
name: generate-copilot-instructions
description: Use when creating or regenerating .github/copilot-instructions.md files for one or more project folders, especially when they must reference a shared ancestor .agent directory or bootstrap Copilot context in a new solution
---

# Generate Copilot Instructions

## Overview

Genera archivos `.github/copilot-instructions.md` por proyecto o solución usando el `.agent` compartido correcto.

**Esta skill debe ser autosuficiente.** Si existe un `copilot-instructions.md` local, úsalo solo como referencia. Si no existe, usa la plantilla `copilot-instructions-template.md` incluida en esta skill.

## When to Use

- El usuario pide crear, replicar o regenerar `copilot-instructions.md`
- Hay varias carpetas de proyecto bajo un directorio y deben apuntar a una `.agent` ancestro
- Se quiere bootstrapear contexto de Copilot en una solución nueva
- Existe un archivo base pero está demasiado atado a otro proyecto

No usar cuando:

- Solo hay que corregir una línea puntual en un archivo existente
- No existe `.agent` accesible y el usuario no pidió definir una nueva

## Workflow

1. Identificar el directorio objetivo.
2. Inventariar carpetas candidatas:
   - Prioridad 1: carpetas hijas que ya tengan `.github`
   - Prioridad 2: carpetas hijas con `.sln`, `*.csproj` u otro archivo principal de proyecto
   - Si no hay `.github`, preguntar si hay que crearlas antes de generar archivos
3. Resolver la `.agent` compartida:
   - Buscar la carpeta ancestro más cercana que contenga `.agent`
   - Calcular el path relativo desde cada carpeta de proyecto hasta esa `.agent`
   - Usar ese path exacto en el documento, por ejemplo `../.agent`
4. Recolectar información disponible antes de preguntar:
   - Nombre de solución/proyecto desde carpeta o `.sln`
   - Lenguaje, framework y tipo de aplicación desde archivos de proyecto
   - Convenciones desde docs, README, archivos existentes y `copilot-instructions.md` local si lo hubiera
   - Catálogo real de skills desde `.agent/README.md`
5. Preguntar solo la información faltante o ambigua.
6. Generar un `.github/copilot-instructions.md` por carpeta objetivo.
7. Verificar que cada archivo apunte a la `.agent` correcta y no deje placeholders sin resolver.

## Información que Debe Inferirse Primero

No preguntes estas cosas si ya están claras en el repositorio:

| Grupo | Fuentes a revisar primero |
|------|----------------------------|
| Identidad del proyecto | nombre de carpeta, `.sln`, README |
| Stack | `.csproj`, `packages.config`, `app.config`, `web.config`, `package.json` |
| Arquitectura | estructura de carpetas, nombres de capas, solution folders |
| Proveedor de repo/work items | documentación, URLs, archivos de build, textos existentes |
| Skills disponibles | `.agent/README.md`, `.agent/INIT.md` |

## Preguntas Obligatorias Si Falta Información

Agrupa preguntas cortas y concretas. Pedí solo lo que no pudiste confirmar.

- Nombre formal de la solución y propósito en una línea
- Rama principal y convención de branches
- Proveedor de repositorio y work items
- Base de datos principal si no es inferible
- Convenciones de naming, errores, logging o sesión si no hay evidencia suficiente
- Restricciones de seguridad, compliance o tooling específicas del equipo

## Reglas de Generación

- Preferí paths relativos reales como `../.agent/README.md` en lugar de `/.agent/README.md`
- Si un árbol tiene varios proyectos con distinta profundidad, calculá el path relativo por proyecto
- Si tomás un archivo local como base, eliminá o reemplazá cualquier dato que no aplique al proyecto destino
- No copies convenciones de otro proyecto sin evidencia o confirmación del usuario
- Si faltan datos no críticos y el usuario decide diferirlos, agregá una sección breve de `Información pendiente por confirmar`
- El documento final debe ser autocontenido: quien lo lea no tiene por qué conocer el origen del template

## Orden de Fuentes

1. `.github/copilot-instructions.md` dentro del árbol destino, si existe
2. `.agent/README.md`, `.agent/INIT.md`, `.agent/AGENTS.md`
3. Archivos de solución/proyecto y documentación local
4. `copilot-instructions-template.md` de esta skill

## Estándar de Salida

Cada archivo generado debe incluir, como mínimo:

- título y contexto breve del proyecto
- ubicación del `.agent` con paths relativos correctos
- secuencia de inicialización de Copilot
- guía de uso de skills o referencia explícita al índice de skills
- stack y arquitectura confirmados
- workflow de repositorio y documentación si está confirmado
- sección de información pendiente solo si el usuario decidió dejarla abierta

## Prompt Template

Esta skill incluye un template de mensaje inicial para reducir ida y vuelta con el usuario:

- `request-template.md` - prompt copy-paste para pedir la generacion de `copilot-instructions.md` con toda la informacion adicional util desde el primer mensaje

## Ejemplos de Preguntas Buenas

- `Confirmame el nombre formal de la solución y su propósito en una línea.`
- `Veo proyectos .NET Framework 4.5.1. ¿Querés que lo deje como stack oficial de la solución?`
- `¿La rama principal es main, master, develop u otra?`
- `¿Usan Azure DevOps, GitHub, GitLab u otra plataforma para repo y work items?`

## Errores Comunes

- Hardcodear `/.agent` en vez de calcular el path relativo
- Preguntar un cuestionario completo sin revisar `.sln`, proyectos y docs primero
- Generar archivos en carpetas sin `.github` cuando el usuario pidió limitarse a las que ya existen
- Dejar placeholders como `{{STACK}}` o `{{BRANCH}}` en el documento final
- Hacer que la skill dependa de un archivo externo al repositorio para funcionar