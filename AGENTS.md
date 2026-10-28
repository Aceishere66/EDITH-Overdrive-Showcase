# EDITH-Overdrive-Showcase — AI Agent Instructions

Apply repository-specific instructions and existing project constraints. This document adds the EDITH Dev Studio global Git branch lifecycle policy; security, data integrity and later explicit owner instructions take priority.

## Git branch lifecycle — mandatory EDITH policy

All agents (Codex, Antigravity, Jules, GitHub automation, etc.) must follow `docs/GIT_BRANCH_HYGIENE_POLICY.md` for every GitHub-related task.

- Minimize branch count; reuse an appropriate active branch when safe.
- After a merged PR, verify actual integration (including squash/rebase), no open-PR dependencies, no exclusive work, and no deployment/CI/worktree/release dependencies before deleting its remote source branch.
- Automatically remove **only** verified-safe `MERGED` branches. `OBSOLETE` or `REVIEW_REQUIRED` branches with exclusive work require technical review and owner approval.
- Never delete main/default/protected branches; do not force-push for cleanup, discard dirty work, or disrupt ongoing audits.
- Preserve branches with unresolved provenance/dependencies and explicitly report the blocker.
- End each GitHub activity with a **GIT BRANCH HYGIENE** summary: branch, PR, merge status, remote deletion SÌ/NO, retention reason, and obsolete/suspicious branches.
