# CONTEXTO Y CONFIGURACION PARA GITHUB COPILOT
## {{PROJECT_NAME}}

{{PROJECT_SUMMARY}}

---

## CONFIGURACION DE GITHUB COPILOT SKILLS

> Los siguientes paths se expresan relativos a la carpeta raiz del proyecto o solucion.

### Ubicacion de Skills
- **Carpeta:** `{{AGENT_RELATIVE_PATH}}/skills/`
- **Indice:** `{{AGENT_RELATIVE_PATH}}/README.md` - Leer al inicio de cada sesion
- **Inicializacion:** `{{AGENT_RELATIVE_PATH}}/INIT.md` - Checklist de arranque
- **Perfil de agente:** `{{AGENT_RELATIVE_PATH}}/AGENTS.md` - Reglas de ejecucion

### Reglas de Aplicacion
- Aplicar skills automaticamente segun el contexto del usuario
- No pedir permiso para cargar una skill relevante
- Preferir siempre el catalogo real definido en `{{AGENT_RELATIVE_PATH}}/README.md`
- Combinar las skills con las reglas especificas de este proyecto

### Inicializacion Obligatoria
Al inicio de cada sesion de asistencia, leer:
1. Este archivo (`.github/copilot-instructions.md`)
2. `{{AGENT_RELATIVE_PATH}}/README.md`
3. `{{AGENT_RELATIVE_PATH}}/INIT.md`
4. `{{AGENT_RELATIVE_PATH}}/AGENTS.md`
5. La skill especifica bajo demanda cuando el contexto lo requiera

### Deteccion de Contexto -> Skill
Tomar como fuente de verdad el indice `{{AGENT_RELATIVE_PATH}}/README.md`. Si conviene destacar triggers frecuentes, usar solo skills realmente presentes en ese indice.

| Contexto del Usuario | Skill a Aplicar |
|---------------------|-----------------|
| Documentar una feature o PR | `writing-project-docs` |
| Crear o editar una skill | `writing-skills` |
| Generar o replicar `copilot-instructions.md` | `generate-copilot-instructions` |
| Analizar un error o una falla | `systematic-debugging` |
| Crear tests automatizados | `test-driven-development` |
| Preparar validacion de entrega o PR | `verification-before-completion` |

---

## INFORMACION DEL PROYECTO

### Resumen
- **Nombre:** `{{PROJECT_NAME}}`
- **Proposito:** {{PROJECT_PURPOSE}}

### Stack Tecnologico
- **Lenguaje principal:** {{PRIMARY_LANGUAGE}}
- **Framework / Runtime:** {{PRIMARY_FRAMEWORK}}
- **Arquitectura:** {{ARCHITECTURE_STYLE}}
- **Base de datos:** {{DATABASE_TECH}}
- **Tests:** {{TEST_STACK}}

### Estructura de Capas o Componentes
```text
{{LAYER_OVERVIEW}}
```

---

## WORKFLOW DE REPOSITORIO

### Repositorio
- **Proveedor:** {{REPO_PROVIDER}}
- **URL:** {{REPO_URL}}
- **Rama principal:** {{MAIN_BRANCH}}
- **Convencion de branches:** {{BRANCH_CONVENTION}}

### Work Items / Planeamiento
- **Herramienta:** {{WORK_ITEM_TOOL}}
- **Tipos principales:** {{WORK_ITEM_TYPES}}
- **Notas del proceso:** {{WORKFLOW_NOTES}}

---

## CONVENCIONES DE DESARROLLO

### Nomenclatura
{{NAMING_CONVENTIONS}}

### Manejo de Errores
{{ERROR_HANDLING_PATTERN}}

### Logging y Observabilidad
{{LOGGING_PATTERN}}

### Configuracion, Sesion o Secretos
{{CONFIG_AND_SESSION_PATTERN}}

---

## DOCUMENTACION Y VALIDACION

### Documentacion Esperada
{{DOCUMENTATION_CONVENTIONS}}

### Validaciones Antes de Cerrar una Tarea
{{DELIVERY_CHECKLIST}}

---

## INFORMACION PENDIENTE POR CONFIRMAR

{{OPEN_QUESTIONS}}

> Eliminar esta seccion si no queda informacion pendiente.