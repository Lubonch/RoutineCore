# GitHub Copilot Skills

Esta carpeta contiene skills especializadas para GitHub Copilot que mejoran la asistencia en desarrollo.

## 📚 Skills Disponibles

### 🧭 using-superpowers
**Archivo:** `.agent/skills/using-superpowers/SKILL.md`  
**Propósito:** Establece cómo encontrar y usar skills; requiere cargar el skill relevante antes de responder  
**Triggers:**
- Al inicio de cualquier conversación o tarea
- Antes de tomar cualquier acción de implementación

**Características:**
- Fuerza la detección de skills antes de actuar
- Define prioridad entre skills cuando varios aplican
- Previene responder sin revisar si existe un skill relevante

---

### 🧠 brainstorming
**Archivo:** `.agent/skills/brainstorming/SKILL.md`  
**Propósito:** Exploración de ideas y diseño antes de implementar  
**Triggers:**
- Usuario quiere crear una feature, componente o funcionalidad
- Antes de cualquier trabajo creativo o de diseño

**Características:**
- HARD-GATE: no se implementa nada sin diseño aprobado
- Preguntas de a una por vez, múltiple opción preferida
- Propone 2-3 enfoques con tradeoffs antes de decidir

---

### 📋 writing-plans
**Archivo:** `.agent/skills/writing-plans/SKILL.md`  
**Propósito:** Crear planes de implementación escritos con tareas granulares  
**Triggers:**
- Usuario solicita un plan escrito o artefacto de plan
- Tarea multi-paso que requiere planificación explícita

**Características:**
- Tareas de 2-5 minutos cada una
- Paths exactos de archivos, comandos copy-paste
- Encabezado estándar con goal, arquitectura y stack

---

### ⚙️ executing-plans
**Archivo:** `.agent/skills/executing-plans/SKILL.md`  
**Propósito:** Ejecutar un plan escrito existente en modo single-flow  
**Triggers:**
- Hay un plan escrito y debe ejecutarse
- Usuario pide ejecutar el plan de a una tarea por vez

**Características:**
- Batches de 3 tareas con checkpoint entre batches
- Revisión crítica del plan antes de empezar
- No ofrece modos alternativos de ejecución

---

### 🔁 single-flow-task-execution
**Archivo:** `.agent/skills/single-flow-task-execution/SKILL.md`  
**Propósito:** Ejecución secuencial con revisión en dos etapas por tarea  
**Triggers:**
- Planes con tareas independientes en la sesión actual
- Múltiples problemas a resolver secuencialmente

**Características:**
- Una tarea activa a la vez
- Revisión de conformidad con spec primero, luego calidad de código
- Sin subagentes paralelos de codificación

---

### 📝 writing-project-docs
**Archivo:** `.agent/skills/writing-project-docs/SKILL.md`  
**Propósito:** Documentación concisa y práctica de features y PRs  
**Triggers:**
- Usuario solicita documentar una feature
- Usuario pregunta "cómo documentar esto"
- Usuario menciona "crear guía de testing"

**Características:**
- Documentos de ~150-200 líneas (no extensos)
- Formato: Tablas + comandos copy-paste
- Legible en 5-10 minutos
- 7 escenarios de prueba estándar: Carga, Modificación, Autorización, Ya Autorizado, Edge Cases, Rollback, Re-carga

---

### 🧭 generate-copilot-instructions
**Archivo:** `.agent/skills/generate-copilot-instructions/SKILL.md`  
**Propósito:** Generar `.github/copilot-instructions.md` por proyecto o solución con referencia relativa correcta a una carpeta `.agent` compartida  
**Triggers:**
- Usuario pide crear o replicar `copilot-instructions.md`
- Usuario quiere bootstrapear Copilot en una solución nueva
- Usuario necesita apuntar desde subcarpetas a una `.agent` ancestro

**Características:**
- Reusa una base local si existe; si no, usa plantilla interna
- Calcula el path relativo hacia `.agent` por proyecto
- Pide solo la información que no puede inferir del repositorio
- Puede usarse en otras soluciones sin requerir conocimiento del proyecto de origen

---

### � systematic-debugging
**Archivo:** `.agent/skills/systematic-debugging/SKILL.md`  
**Propósito:** Debugging metódico con trazabilidad completa  
**Triggers:**
- Usuario reporta un error
- Usuario pide analizar logs
- Usuario pregunta "por qué falla esto"

**Características:**
- Enfoque sistemático: Síntomas → Hipótesis → Verificación
- Trazabilidad completa del análisis
- Recomendaciones de logging
- Checklist de validación

---

### 🧪 test-driven-development
**Archivo:** `.agent/skills/test-driven-development/SKILL.md`  
**Propósito:** Patrones TDD y mejores prácticas de testing  
**Triggers:**
- Usuario solicita crear tests
- Usuario pregunta "cómo testear esto"
- Usuario menciona unit tests, integration tests

**Características:**
- Ciclo Red-Green-Refactor
- Patterns de testing adaptados al stack del proyecto
- Cobertura de edge cases
- Nomenclatura estándar de tests

---

### ✅ verification-before-completion
**Archivo:** `.agent/skills/verification-before-completion/SKILL.md`  
**Propósito:** Validación exhaustiva pre-commit/pre-merge  
**Triggers:**
- Usuario está listo para commit
- Usuario pregunta "qué validar antes de merge"
- Usuario menciona "checklist de PR"

**Características:**
- Checklist de validación técnica
- Verificación de estándares del proyecto
- Review de seguridad básica
- Validación de documentación

---

## 🔄 Cómo Funcionan los Skills

### Para GitHub Copilot:
1. Leer `.github/copilot-instructions.md` (contexto global del proyecto)
2. Leer `.agent/README.md` (este archivo - índice de skills)
3. Detectar contexto de la pregunta del usuario
4. Cargar y aplicar el skill correspondiente automáticamente
5. Seguir las reglas definidas en el SKILL.md SIN pedir permiso

### Para Desarrolladores:
- **No necesitas** mencionar el skill explícitamente
- Copilot detecta el contexto y lo aplica automáticamente
- Ejemplo: Si pides documentar algo, se aplica `writing-project-docs`

---

## 📁 Estructura de Carpetas

``
.agent/
├── README.md                          ← Este archivo (índice)
├── AGENTS.md                          ← Perfil Superpowers
├── INIT.md                            ← Checklist de inicialización
├── FOR_THE_HUMAN.md                   ← Instrucciones de mantenimiento
├── INSTALL.md                         ← Guía de instalación
├── task.md                            ← Template de tracking
├── skills/
│   ├── using-superpowers/SKILL.md
│   ├── brainstorming/SKILL.md
│   ├── writing-plans/SKILL.md
│   ├── executing-plans/SKILL.md
│   ├── single-flow-task-execution/SKILL.md
│   ├── writing-project-docs/SKILL.md
│   ├── writing-skills/SKILL.md
│   ├── generate-copilot-instructions/
│   │   ├── SKILL.md
│   │   ├── copilot-instructions-template.md
│   │   └── request-template.md
│   ├── systematic-debugging/SKILL.md
│   ├── test-driven-development/SKILL.md
│   ├── verification-before-completion/SKILL.md
│   ├── requesting-code-review/SKILL.md
│   ├── receiving-code-review/SKILL.md
│   ├── using-git-worktrees/SKILL.md
│   └── finishing-a-development-branch/SKILL.md
└── workflows/
    ├── brainstorm.md
    ├── write-plan.md
    └── execute-plan.md
``

---

## 🛠️ Mantenimiento

### Agregar un Nuevo Skill
1. Crear carpeta en `.agent/skills/{nombre-skill}/`
2. Crear `SKILL.md` con:
   - Propósito claro
   - Triggers (cuándo aplicar)
   - Reglas específicas
   - Ejemplos
3. Actualizar este README.md
4. Commit en la branch de desarrollo activa

### Modificar un Skill Existente
1. Editar `.agent/skills/{nombre-skill}/SKILL.md`
2. Testear con Copilot
3. Actualizar este README si cambian triggers o propósito

---

## 🎯 Principios de los Skills

1. **Auto-detección:** Copilot aplica skills automáticamente según contexto
2. **Sin fricción:** El desarrollador NO debe mencionar el skill
3. **Accionables:** Skills producen outputs concretos y útiles
4. **Consistentes:** Todos siguen estándares del proyecto
5. **Documentados:** Cada skill explica claramente su propósito

---

## 📖 Referencias

- **Contexto global del proyecto:** `.github/copilot-instructions.md`
- **Perfil de agente:** `.agent/AGENTS.md`
- **Para humanos mantenedores:** `.agent/FOR_THE_HUMAN.md`

---

**Mantenedor:** Equipo del proyecto
