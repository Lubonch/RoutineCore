# Superpowers Agent Profile

You have superpowers.

This profile adapts Superpowers workflows for a strict single-flow execution environment.

## Core Rules

1. Prefer local skills in `.agent/skills/<skill-name>/SKILL.md`.
2. Execute one core task at a time with `task_boundary`.
3. Use `browser_subagent` only for browser automation tasks.
4. Track progress in chat. If temporary scratch notes are truly needed, keep them outside the repo in an ephemeral location and delete them before completion.
5. Do not create persistent documentation, tests, or Git integration actions unless the user explicitly requests them.
6. Keep changes scoped to the requested task and verify before completion claims.
7. Ignore `.agent/FOR_THE_HUMAN.md` unless the user explicitly asks to read or edit it. That file is for human maintainers, not for agent execution.

## Project Policy Overrides

- Persistent repo artifacts are opt-in. Create plans, PR guides, specs, or other documentation files only when the user explicitly asks for them.
- User-requested artifacts are allowed and should be kept in the repository when that is what the user asked for.
- Default scratch location is outside the repository, for example `%TEMP%/agent-scratch/<repo-name>/`.
- Do not run `git commit`, `git push`, create pull requests, merge branches, discard work, or perform other Git integration actions without explicit user instruction.
- Do not create tests or run tests unless the user explicitly requests that work.

## Tool Translation Contract

When source skills reference legacy tool names, map them to equivalent capabilities in the current platform. The exact tool names can vary by IDE, agent host, or CLI.

- Legacy assistant/platform names -> `the current agent`
- `Task` tool -> `browser_subagent` for browser tasks, otherwise sequential `task_boundary`

---

## Initialization Workflow

**At the start of every assistance session, execute in order:**

1. **Read global context:**
   - `.github/copilot-instructions.md` → Project context (stack, patterns, conventions)

2. **Read agent configuration:**
   - `.agent/AGENTS.md` → This file (Superpowers profile)

3. **Read skills index:**
   - `.agent/README.md` → Discover available skills

4. **Read initialization checklist:**
   - `.agent/INIT.md` → Verify all steps completed

5. **Load skills on demand:**
   - `.agent/skills/{skill-name}/SKILL.md` → Only when context matches

**Skill Auto-Detection Rules:**

| User Context | Skill to Load | File Path |
|-------------|---------------|-----------|
| Documenting feature/PR | writing-project-docs | `.agent/skills/writing-project-docs/SKILL.md` |
| Debugging/analyzing errors | systematic-debugging | `.agent/skills/systematic-debugging/SKILL.md` |
| Writing tests | test-driven-development | `.agent/skills/test-driven-development/SKILL.md` |
| Ready to commit/merge | verification-before-completion | `.agent/skills/verification-before-completion/SKILL.md` |

**Execution Priority:**
1. Skill rules (highest priority)
2. `.github/copilot-instructions.md` rules
3. General best practices (lowest priority)

**When in doubt:** Skills complement, not replace, project-specific rules in `copilot-instructions.md`.
